using DesignABear.Services.Common;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using DesignABear.Repositories.Models;

namespace DesignABear.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IPayOSService _payOSService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            IPaymentService paymentService, 
            IPayOSService payOSService, 
            IOrderService orderService,
            IConfiguration configuration, 
            ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _payOSService = payOSService;
            _orderService = orderService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var payment = await _paymentService.GetByIdAsync(id);
            return HandleResult(Result<object>.Success(payment));
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            var payments = await _paymentService.GetByOrderIdAsync(orderId);
            return HandleResult(Result<object>.Success(payments));
        }

        [HttpGet("external/{externalTxnId}")]
        public async Task<IActionResult> GetByExternalTxnId(string externalTxnId)
        {
            var payment = await _paymentService.GetByExternalTxnIdAsync(externalTxnId);
            return HandleResult(Result<object>.Success(payment));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
        {
            var payment = await _paymentService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = payment.PaymentId }, Result<object>.Success(payment));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePaymentRequest request)
        {
            await _paymentService.UpdateAsync(id, request);
            return HandleResult(Result.Success());
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePayOSPaymentRequest request)
        {
            if (!ModelState.IsValid || request == null || request.Amount <= 0)
            {
                _logger.LogWarning("Invalid model state for CreatePayment request.");
                return HandleResult(Result<object>.Failure(new Error("BadRequest", "Invalid request data. Amount is required.")));
            }

            // Layer 2 Hard Check: Validate stock one last time before creating payment link
            try 
            {
                await _orderService.ValidateOrderStockAsync(request.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Layer 2 stock check failed for OrderId {OrderId}: {Message}", request.OrderId, ex.Message);
                return HandleResult(Result<object>.Failure(new Error("InventoryError", ex.Message)));
            }

            // Layer 3 Hard Check: Validate that the payment amount matches the order total in DB
            var dbOrder = await _orderService.GetByIdAsync(request.OrderId);
            if (dbOrder == null)
            {
                return HandleResult(Result<object>.Failure(new Error("OrderNotFound", "Order not found.")));
            }

            if (dbOrder.GrandTotal != request.Amount)
            {
                _logger.LogWarning("Payment amount mismatch for OrderId {OrderId}. DB: {DbTotal}, Request: {ReqAmount}", request.OrderId, dbOrder.GrandTotal, request.Amount);
                return HandleResult(Result<object>.Failure(new Error("AmountMismatch", $"Payment amount {request.Amount} does not match order total {dbOrder.GrandTotal} Khoản thanh toán {request.Amount} không khớp với tổng đơn hàng {dbOrder.GrandTotal}.")));
            }

            // Guard: Prevent payment creation for orders that are already Cancelled or Rejected
            if (dbOrder.Status == OrderStatusConstants.Cancelled || dbOrder.Status == OrderStatusConstants.Rejected)
            {
                _logger.LogWarning("Attempted to create payment for {Status} order {OrderId}", dbOrder.Status, request.OrderId);
                return HandleResult(Result<object>.Failure(new Error("InvalidOrderStatus", $"Cannot create payment for an order with status {dbOrder.Status}. Đơn hàng đã b {dbOrder.Status}, không thể thực hiện thanh toán.")));
            }

            long orderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var item = new ItemData(request.ItemName, request.Quantity, (int)request.Amount);
            var items = new List<ItemData> { item };

            var paymentData = new PaymentData(
                orderCode,
                (int)request.Amount,
                request.Description,
                items,
                _configuration["RedirectUrls:CancelUrl"] ?? "http://localhost:3000/cancel",
                _configuration["RedirectUrls:SuccessUrl"] ?? "http://localhost:3000/success"
            );

            CreatePaymentResult result = await _payOSService.CreatePaymentLink(paymentData);

            var paymentRequest = new CreatePaymentRequest
            {
                OrderId = request.OrderId,
                Amount = dbOrder.GrandTotal, // Use DB total to be safe
                PaymentMethod = "PayOS",
                TransactionId = orderCode.ToString()
            };

            await _paymentService.CreateAsync(paymentRequest);

            _logger.LogInformation("Payment link created successfully for OrderId {OrderId} with order code {OrderCode}", request.OrderId, orderCode);
            return HandleResult(Result<object>.Success(new { CheckoutUrl = result.checkoutUrl }));
        }

        [HttpGet("confirm-payment/{orderCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPayment(long orderCode)
        {
            _logger.LogInformation("ConfirmPayment triggered for orderCode: {OrderCode}", orderCode);
            try
            {
                PaymentLinkInformation paymentInfo = await _payOSService.GetPaymentLinkInformation(orderCode);
                _logger.LogInformation("PayOS status for orderCode {OrderCode}: {Status}", orderCode, paymentInfo.status);
                
                bool isPaid = string.Equals(paymentInfo.status, OrderStatusConstants.Paid, StringComparison.OrdinalIgnoreCase) || 
                              string.Equals(paymentInfo.status, "PAID", StringComparison.OrdinalIgnoreCase);
                
                bool isCancelled = string.Equals(paymentInfo.status, "CANCELLED", StringComparison.OrdinalIgnoreCase) || 
                                   string.Equals(paymentInfo.status, "EXPIRED", StringComparison.OrdinalIgnoreCase);

                if (isPaid)
                {
                    await _paymentService.HandleWebhookAsync(orderCode.ToString(), "{ \"status\": \"MANUAL_CONFIRMED\" }");
                    return HandleResult(Result<object>.Success(new { Message = "Payment confirmed and order updated.", Status = paymentInfo.status }));
                }
                else if (isCancelled)
                {
                    await _paymentService.HandleWebhookAsync(orderCode.ToString(), $"{{ \"status\": \"MANUAL_{paymentInfo.status}_CONFIRMED\" }}", "CANCELLED");
                    return HandleResult(Result<object>.Success(new { Message = $"Payment {paymentInfo.status.ToLower()} and order updated.", Status = paymentInfo.status }));
                }
                
                return HandleResult(Result<object>.Success(new { Message = "Payment not completed.", Status = paymentInfo.status }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming payment for order {OrderCode}", orderCode);
                return HandleResult(Result<object>.Failure(new Error("Error", ex.Message)));
            }
        }

        [HttpPost("payos-webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> HandleWebhook([FromBody] PayOSWebhookEnvelope body)
        {
            _logger.LogInformation("Webhook received: {@Body}", body);

            var d = body?.data;
            if (d == null)
            {
                _logger.LogInformation("Webhook: missing data (likely ping) -> OK");
                return HandleResult(Result.Success());
            }

            if (d.orderCode == 123)
            {
                _logger.LogInformation("Webhook: ignore sample ping (orderCode=123) -> OK");
                return HandleResult(Result.Success());
            }

            bool isSuccess =
                string.Equals(body?.code, "00", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(d.code, "00", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(d.status, OrderStatusConstants.Paid, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(d.status, "PAID", StringComparison.OrdinalIgnoreCase);

            bool isCancelled = string.Equals(d.status, "CANCELLED", StringComparison.OrdinalIgnoreCase);

            if (!isSuccess && !isCancelled)
            {
                _logger.LogInformation("Webhook: neither success nor cancel (code/desc: {Code}/{Desc}, status: {Status}) -> OK", d.code, d.desc, d.status);
                return HandleResult(Result.Success());
            }

            long orderCode = d.orderCode;
            decimal amount = d.amount;

            var payment = await _paymentService.GetByExternalTxnIdAsync(orderCode.ToString());

            if (payment == null)
            {
                _logger.LogWarning("Webhook: payment not found for order {OrderCode} -> OK", orderCode);
                return HandleResult(Result.Success());
            }

            // Using HandleWebhookAsync logic from our generic PaymentService
            await _paymentService.HandleWebhookAsync(
                orderCode.ToString(), 
                body != null ? System.Text.Json.JsonSerializer.Serialize(body) : "{ \"status\": \"WEBHOOK_RECEIVED\" }",
                isCancelled ? "CANCELLED" : null
            );

            _logger.LogInformation(
                "Webhook OK: order {OrderCode}; amount {Amount} processed",
                orderCode, amount
            );

            return HandleResult(Result.Success());
        }
    }

    public class CreatePayOSPaymentRequest
    {
        public Guid OrderId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class PayOSWebhookEnvelope
    {
        public string? code { get; set; }
        public string? desc { get; set; }
        public PayOSWebhookData? data { get; set; }
    }

    public class PayOSWebhookData
    {
        public long orderCode { get; set; }
        public long amount { get; set; }
        public string? description { get; set; }
        public string? code { get; set; }
        public string? desc { get; set; }
        public string? status { get; set; }
    }
}
