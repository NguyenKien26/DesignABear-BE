using AutoMapper;
using Microsoft.Extensions.Logging;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using DesignABear.Services.Configurations;

namespace DesignABear.Services.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<PaymentService> _logger;
        private readonly BusinessSettings _businessSettings;

        public PaymentService(
            IUnitOfWork uow, 
            IMapper mapper, 
            IInventoryService inventoryService,
            ILogger<PaymentService> logger, 
            IOptions<BusinessSettings> businessSettings)
        {
            _uow = uow;
            _mapper = mapper;
            _inventoryService = inventoryService;
            _logger = logger;
            _businessSettings = businessSettings.Value;
        }

        public async Task<IEnumerable<PaymentResponse>> GetByOrderIdAsync(Guid orderId)
        {
            var payments = await _uow.Payments.GetByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<PaymentResponse>>(payments);
        }

        public async Task<PaymentResponse?> GetByIdAsync(Guid paymentId)
        {
            var payment = await _uow.Payments.GetByIdAsync(paymentId);
            if (payment == null) throw new NotFoundException("Payment", paymentId);
            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task<PaymentResponse?> GetByExternalTxnIdAsync(string externalTxnId)
        {
            var payment = await _uow.Payments.GetByExternalTxnIdAsync(externalTxnId);
            if (payment == null) throw new NotFoundException("Payment", externalTxnId);
            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task<PaymentResponse> CreateAsync(CreatePaymentRequest request)
        {
            var payment = _mapper.Map<Payment>(request);
            payment.PaymentId = Guid.NewGuid();
            payment.CreatedAt = DateTime.UtcNow;
            payment.Status = PaymentStatusConstants.Pending;
            
            await _uow.Payments.AddAsync(payment);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task UpdateAsync(Guid paymentId, UpdatePaymentRequest request)
        {
            var payment = await _uow.Payments.GetByIdAsync(paymentId);
            if (payment == null) throw new NotFoundException("Payment", paymentId);
            
            _mapper.Map(request, payment);
            
            if (request.Status == PaymentStatusConstants.Captured && payment.PaidAt == null)
            {
                payment.PaidAt = DateTime.UtcNow;
            }
            
            _uow.Payments.Update(payment);
            await _uow.SaveChangesAsync();
        }

        public async Task HandleWebhookAsync(string externalTxnId, string rawPayload, string? statusOverride = null)
        {
            _logger.LogInformation("HandleWebhookAsync called for externalTxnId: {ExternalTxnId}, statusOverride: {StatusOverride}", externalTxnId, statusOverride);
            await _uow.ExecuteInTransactionAsync(async () =>
            {
                _logger.LogInformation("Processing webhook/confirmation for externalTxnId: {ExternalTxnId}", externalTxnId);
                var payment = await _uow.Payments.GetByExternalTxnIdAsync(externalTxnId);
                
                if (payment is null)
                {
                    _logger.LogWarning("Payment not found for externalTxnId: {ExternalTxnId}", externalTxnId);
                    return;
                }

                // Idempotency check: If payment is already captured, don't re-process
                if (payment.Status == PaymentStatusConstants.Captured)
                {
                    _logger.LogInformation("Payment {PaymentId} already processed (CAPTURED). Skipping.", payment.PaymentId);
                    return;
                }

                payment.WebhookRaw = rawPayload;

                // Handle Cancellation
                if (string.Equals(statusOverride, OrderStatusConstants.Cancelled, StringComparison.OrdinalIgnoreCase) || 
                    string.Equals(statusOverride, "CANCELLED", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("Marking payment {PaymentId} and order {OrderId} as CANCELLED", payment.PaymentId, payment.OrderId);
                    payment.Status = PaymentStatusConstants.Cancelled;
                    _uow.Payments.Update(payment);

                    var orderToCancel = await _uow.Orders.GetByIdAsync(payment.OrderId);
                    if (orderToCancel != null && 
                        orderToCancel.Status != OrderStatusConstants.Paid && 
                        orderToCancel.Status != OrderStatusConstants.Completed &&
                        orderToCancel.Status != OrderStatusConstants.Cancelled)
                    {
                        orderToCancel.Status = OrderStatusConstants.Cancelled;
                        orderToCancel.UpdatedAt = DateTime.UtcNow;
                        _uow.Orders.Update(orderToCancel);
                        _logger.LogInformation("Order {OrderId} status successfully updated to CANCELLED", payment.OrderId);
                    }
                    else if (orderToCancel == null)
                    {
                        _logger.LogWarning("Order {OrderId} not found for cancellation", payment.OrderId);
                    }
                    
                    await _uow.SaveChangesAsync();
                    return;
                }

                payment.Status = PaymentStatusConstants.Captured;
                payment.PaidAt = DateTime.UtcNow;

                _uow.Payments.Update(payment);

                _logger.LogInformation("Fetching order details for OrderId: {OrderId}", payment.OrderId);
                var order = await _uow.Orders.GetWithDetailsAsync(payment.OrderId);
                
                if (order != null && order.Status != OrderStatusConstants.Paid)
                {
                    // Guard: If order is Cancelled or Rejected, do NOT mark as Paid
                    // This prevents late webhooks from reviving a cancelled order and avoids inventory inconsistencies
                    if (order.Status == OrderStatusConstants.Cancelled || order.Status == OrderStatusConstants.Rejected)
                    {
                        _logger.LogWarning("Late webhook received for {Status} Order {OrderNumber}. Skipping status update to PAID.", order.Status, order.OrderNumber);
                        return;
                    }

                    _logger.LogInformation("Updating Order {OrderNumber} status to PAID", order.OrderNumber);
                    order.Status = OrderStatusConstants.Paid;
                    order.UpdatedAt = DateTime.UtcNow;
                    _uow.Orders.Update(order);

                    var warehouseId = _businessSettings.WarehouseGuid;

                    _logger.LogInformation("Found {Count} order items to process for TotalSales and Inventory update", order.OrderItems.Count);
                    
                    foreach (var item in order.OrderItems)
                    {
                        var hasAiProcessor = ResolveHasAiProcessor(item);
                        // 1. Confirm Inventory Deduction for Main Variant
                        if (item.VariantId.HasValue)
                        {
                            _logger.LogInformation("Confirming inventory deduction for Variant {VariantId}, Qty: {Quantity}", item.VariantId.Value, item.Quantity);
                            await _inventoryService.ConfirmDeductionAsync(item.VariantId.Value, false, item.Quantity, warehouseId);
                        }

                        // 2. Confirm Inventory Deduction for Build Components (Accessories)
                        if (item.BuildId.HasValue && item.Build != null)
                        {
                            _logger.LogInformation("Confirming inventory deduction for components of build: {BuildId}", item.BuildId);
                            foreach (var component in item.Build.BuildComponents ?? Enumerable.Empty<BuildComponent>())
                            {
                                var identityId = component.OptionVariantId ?? component.OptionProductId;
                                _logger.LogInformation("Confirming inventory deduction for Component {IdentityId}, Qty: {Quantity}", identityId, (item.Quantity * component.Quantity));
                                await _inventoryService.ConfirmDeductionAsync(identityId, true, item.Quantity * component.Quantity, warehouseId);
                            }
                        }

                        // 3. Update TotalSales for each product
                        Guid? productId = null;
                        if (item.Variant != null)
                        {
                            productId = item.Variant.ProductId;
                        }
                        else if (item.Build != null)
                        {
                            productId = item.Build.BaseProductId;
                        }

                        if (productId.HasValue)
                        {
                            _logger.LogInformation("Incrementing TotalSales for ProductId: {ProductId} by {Quantity}", productId.Value, item.Quantity);
                            var product = await _uow.Products.GetByIdAsync(productId.Value);
                            if (product != null)
                            {
                                product.TotalSales += item.Quantity;
                                product.UpdatedAt = DateTime.UtcNow;
                                _uow.Products.Update(product);
                            }
                        }

                        // 4. Update TotalSales and Deduct Inventory for AI-PROCESSOR if item has it
                        if (hasAiProcessor)
                        {
                            var chipProductId = _businessSettings.AiProcessorGuid;
                            _logger.LogInformation("Confirming inventory deduction for AI-PROCESSOR (ID: {ChipProductId}) by {Quantity}", chipProductId, item.Quantity);
                            
                            // Deduction (Identity is the Product Guid, treated as accessory)
                            await _inventoryService.ConfirmDeductionAsync(chipProductId, true, item.Quantity, warehouseId);

                            var chipProduct = await _uow.Products.GetByIdAsync(chipProductId);
                            if (chipProduct != null)
                            {
                                chipProduct.TotalSales += item.Quantity;
                                chipProduct.UpdatedAt = DateTime.UtcNow;
                                _uow.Products.Update(chipProduct);
                            }
                        }

                        // 5. Update TotalSales for all accessories within a custom build
                        if (item.Build != null)
                        {
                            foreach (var component in item.Build.BuildComponents)
                            {
                                var componentProduct = await _uow.Products.GetByIdAsync(component.OptionProductId);
                                if (componentProduct != null)
                                {
                                    _logger.LogInformation("Incrementing TotalSales for Component Product: {ProductId} by {Quantity}", componentProduct.ProductId, (item.Quantity * component.Quantity));
                                    componentProduct.TotalSales += (item.Quantity * component.Quantity);
                                    componentProduct.UpdatedAt = DateTime.UtcNow;
                                    _uow.Products.Update(componentProduct);
                                }
                            }
                        }

                        // 6. Update existing ProductionJobs status or create if missing
                        if (item.ProductionJobs != null && item.ProductionJobs.Any())
                        {
                            foreach (var job in item.ProductionJobs.Where(j => j.Status == ProductionJobStatusConstants.PendingPayment))
                            {
                                job.Status = ProductionJobStatusConstants.InQueue;
                                job.HasSmartChip = hasAiProcessor;
                                job.EspStatus = hasAiProcessor ? ProductionStatus.InQueue : ProductionStatus.Completed;
                                job.UpdatedAt = DateTime.UtcNow;
                                _uow.ProductionJobs.Update(job);
                            }
                        }
                        else
                        {
                            // Get base assembly cost for commission snapshots
                            decimal baseAssemblyCost = item.Variant?.AssemblyCost ?? item.Build?.BaseVariant?.AssemblyCost ?? 0m;
                            
                            for (int i = 0; i < item.Quantity; i++)
                            {
                                await _uow.ProductionJobs.AddAsync(new ProductionJob
                                {
                                    JobId = Guid.NewGuid(),
                                    OrderItemId = item.OrderItemId,
                                    BuildId = item.BuildId,
                                    SerialNumber = $"SN-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                                    Status = ProductionStatus.InQueue, // Simplified back to InQueue
                                    HasSmartChip = hasAiProcessor,
                                    EspStatus = hasAiProcessor ? ProductionStatus.InQueue : ProductionStatus.Completed, // Auto-complete if no chip
                                    Priority = PriorityConstants.Normal,
                                    UpdatedAt = DateTime.UtcNow,
                                    PrintData = JsonSerializer.Serialize(new { personalization = item.PersonalizationSnapshot, aiProcessor = hasAiProcessor }),
                                    
                                    // Commissions
                                    TotalAssemblyCost = baseAssemblyCost,
                                    CraftsmanCommission = baseAssemblyCost * 0.7m,
                                    QCCommission = baseAssemblyCost * 0.3m
                                });
                            }
                        }
                    }
                }
                else if (order == null)
                {
                    _logger.LogWarning("Order not found for Payment: {PaymentId}", payment.PaymentId);
                }
                else
                {
                    _logger.LogInformation("Order {OrderNumber} already PAID. Skipping Inventory/TotalSales update.", order.OrderNumber);
                }

                await _uow.SaveChangesAsync();
            });
        }

        private bool ResolveHasAiProcessor(OrderItem item)
        {
            if (item.IncludesSmartChip || item.Build?.IncludesSmartChip == true)
            {
                return true;
            }

            if (item.Build?.BuildComponents == null)
            {
                return false;
            }

            return item.Build.BuildComponents.Any(c =>
                c.OptionProductId == _businessSettings.AiProcessorGuid ||
                string.Equals(c.OptionProduct?.Sku, "CORE-ESP32-AI", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(c.OptionProduct?.Name, "AI Processor", StringComparison.OrdinalIgnoreCase));
        }

    }
}
