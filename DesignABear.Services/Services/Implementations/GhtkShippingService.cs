using DesignABear.Repositories.Integrations.Ghtk.Models;
using DesignABear.Repositories.Repositories.Interfaces;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.Common;
using DesignABear.Services.Configurations;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DesignABear.Services.Services.Implementations
{
    public class GhtkShippingService : IGhtkShippingService
    {
        private readonly IGhtkRepository _ghtkRepository;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GhtkShippingService> _logger;
        private readonly GhtkSettings _ghtkSettings;

        public GhtkShippingService(
            IGhtkRepository ghtkRepository,
            IUnitOfWork uow,
            IOptions<GhtkSettings> ghtkSettings,
            ILogger<GhtkShippingService> logger)
        {
            _ghtkRepository = ghtkRepository;
            _uow = uow;
            _logger = logger;
            _ghtkSettings = ghtkSettings.Value;
        }

        public async Task<Result<GhtkFeeResponse>> GetShippingFeeAsync(CalculateGhtkFeeRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (request.Weight <= 0)
                {
                    return Result<GhtkFeeResponse>.Failure(new Error("GHTK.InvalidWeight", "Weight must be greater than 0."));
                }

                if (string.IsNullOrWhiteSpace(request.Province) || string.IsNullOrWhiteSpace(request.District))
                {
                    return Result<GhtkFeeResponse>.Failure(new Error("GHTK.InvalidAddress", "Missing required destination address fields."));
                }

                var transportType = request.Transport?.Trim().ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(transportType) && transportType != "road" && transportType != "fly")
                {
                    transportType = "road";
                }

                var ghtkRequest = new GhtkFeeRequest
                {
                    PickProvince = _ghtkSettings.PickProvince,
                    PickDistrict = _ghtkSettings.PickDistrict,
                    PickWard = _ghtkSettings.PickWard,
                    PickAddress = _ghtkSettings.PickAddress,
                    Province = request.Province.Trim(),
                    District = request.District.Trim(),
                    Ward = string.IsNullOrWhiteSpace(request.Ward) ? null : request.Ward.Trim(),
                    Address = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim(),
                    Weight = request.Weight,
                    Value = request.Value,
                    Transport = transportType
                };

                var response = await _ghtkRepository.CalculateFeeAsync(ghtkRequest, cancellationToken);
                return Result<GhtkFeeResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating GHTK fee");
                return Result<GhtkFeeResponse>.Failure(new Error("GHTK.ServiceError", ex.Message));
            }
        }

        public async Task<Result<GhtkFeeResponse>> CalculateFeeForCartAsync(Guid cartId, Guid addressId, string? transport = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var address = await _uow.Addresses.GetByIdAsync(addressId);
                if (address == null)
                {
                    return Result<GhtkFeeResponse>.Failure(new Error("Address.NotFound", "Shipping address not found."));
                }

                var cart = await _uow.Carts.GetWithItemsAsync(cartId);
                if (cart == null || !cart.CartItems.Any())
                {
                    return Result<GhtkFeeResponse>.Failure(new Error("Cart.Empty", "Cart is empty."));
                }

                float totalWeight = 0;
                decimal subtotal = 0;

                foreach (var item in cart.CartItems)
                {
                    if (item.BuildId.HasValue && item.Build != null)
                    {
                        totalWeight += (item.Build.TotalWeightGram ?? 500f) * item.Quantity;
                    }
                    else if (item.Product != null)
                    {
                        var weight = item.Product.Variants.FirstOrDefault()?.WeightGram ?? 500f;
                        totalWeight += (weight == 0 ? 500f : weight) * item.Quantity;
                    }
                    else
                    {
                        totalWeight += 500f * item.Quantity;
                    }

                    var price = item.Product?.Variants.FirstOrDefault()?.Price ?? 0;
                    subtotal += (item.UnitPriceSnapshot ?? price) * item.Quantity;
                }

                var calculateRequest = new CalculateGhtkFeeRequest
                {
                    Province = address.State ?? address.City ?? "",
                    District = address.City ?? "",
                    Ward = address.Line2,
                    Address = address.Line1,
                    Weight = (int)totalWeight,
                    Value = (int)subtotal,
                    Transport = transport
                };

                return await GetShippingFeeAsync(calculateRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating GHTK fee for cart {CartId}", cartId);
                return Result<GhtkFeeResponse>.Failure(new Error("GHTK.CartCalculationError", ex.Message));
            }
        }

        public async Task<Result<GhtkFeeResponse>> CalculateFeeForOrderAsync(Guid orderId, string? transport = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var order = await _uow.Orders.GetWithDetailsAsync(orderId);
                if (order == null)
                {
                    return Result<GhtkFeeResponse>.Failure(new Error("Order.NotFound", "Order not found."));
                }

                if (!order.ShippingAddressId.HasValue)
                {
                    return Result<GhtkFeeResponse>.Failure(new Error("Order.NoShippingAddress", "Order does not have a shipping address."));
                }

                var address = order.ShippingAddress ?? await _uow.Addresses.GetByIdAsync(order.ShippingAddressId.Value);
                if (address == null)
                {
                    return Result<GhtkFeeResponse>.Failure(new Error("Address.NotFound", "Shipping address not found."));
                }

                float totalWeight = 0;
                decimal subtotal = 0;

                foreach (var item in order.OrderItems)
                {
                    float itemWeight = item.WeightSnapshot ?? 0;

                    if (itemWeight == 0)
                    {
                        if (item.BuildId.HasValue && item.Build != null)
                        {
                            itemWeight = item.Build.TotalWeightGram ?? 500f;
                        }
                        else if (item.Product != null)
                        {
                            var weight = item.Product.Variants.FirstOrDefault()?.WeightGram ?? 500f;
                            itemWeight = weight == 0 ? 500f : weight;
                        }
                        else
                        {
                            itemWeight = 500f;
                        }
                    }

                    totalWeight += itemWeight * item.Quantity;
                    subtotal += item.UnitPrice * item.Quantity;
                }

                var calculateRequest = new CalculateGhtkFeeRequest
                {
                    Province = address.State ?? address.City ?? "",
                    District = address.City ?? "",
                    Ward = address.Line2,
                    Address = address.Line1,
                    Weight = (int)Math.Ceiling((double)totalWeight),
                    Value = (int)subtotal,
                    Transport = transport
                };

                return await GetShippingFeeAsync(calculateRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating GHTK fee for order {OrderId}", orderId);
                return Result<GhtkFeeResponse>.Failure(new Error("GHTK.OrderCalculationError", ex.Message));
            }
        }

        public async Task<Result<GhtkSubmitOrderResponse>> SubmitExpressOrderAsync(SubmitGhtkExpressOrderRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (request.Products == null || request.Products.Count == 0)
                {
                    return Result<GhtkSubmitOrderResponse>.Failure(new Error("GHTK.InvalidProducts", "Products is required."));
                }

                var transportType = request.Transport?.Trim().ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(transportType) && transportType != "road" && transportType != "fly")
                {
                    transportType = "road";
                }

                var totalWeight = request.Products.Sum(x => x.Weight * x.Quantity);

                var ghtkRequest = new GhtkSubmitOrderRequest
                {
                    Products = request.Products.Select(x => new GhtkSubmitOrderProduct
                    {
                        Name = x.Name.Trim(),
                        Weight = x.Weight / 1000.0, // GHTK expects KG for product weight in V1
                        Quantity = x.Quantity,
                        ProductCode = x.ProductCode
                    }).ToList(),
                    Order = new GhtkSubmitOrderData
                    {
                        Id = request.OrderId.Trim(),
                        PickName = _ghtkSettings.PickName,
                        PickAddress = _ghtkSettings.PickAddress ?? "",
                        PickProvince = _ghtkSettings.PickProvince,
                        PickDistrict = _ghtkSettings.PickDistrict,
                        PickWard = _ghtkSettings.PickWard,
                        PickTel = _ghtkSettings.PickTel,
                        Name = request.CustomerName.Trim(),
                        Tel = request.CustomerPhone.Trim(),
                        Address = request.CustomerAddress.Trim(),
                        Province = request.CustomerProvince.Trim(),
                        District = request.CustomerDistrict.Trim(),
                        Ward = string.IsNullOrWhiteSpace(request.CustomerWard) ? null : request.CustomerWard.Trim(),
                        Hamlet = request.Hamlet,
                        PickMoney = 0,
                        Value = request.Value,
                        Transport = transportType,
                        Note = request.Note,
                        IsFreeShip = 1,
                        PickOption = request.PickOption,
                        DeliverOption = request.DeliverOption,
                        WeightOption = request.WeightOption,
                        Weight = (int)Math.Ceiling((double)totalWeight) // GHTK expects Grams for total weight in V1
                    }
                };

                var response = await _ghtkRepository.SubmitExpressOrderAsync(ghtkRequest, cancellationToken);
                return Result<GhtkSubmitOrderResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting GHTK express order");
                return Result<GhtkSubmitOrderResponse>.Failure(new Error("GHTK.SubmitOrderExpressError", ex.Message));
            }
        }

        public async Task<Result<GhtkPrintLabelFile>> DownloadPrintLabelAsync(string label, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(label))
                {
                    return Result<GhtkPrintLabelFile>.Failure(new Error("GHTK.InvalidLabel", "Label is required."));
                }

                var response = await _ghtkRepository.DownloadPrintLabelAsync(label.Trim(), cancellationToken);
                return Result<GhtkPrintLabelFile>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading GHTK print label {Label}", label);
                return Result<GhtkPrintLabelFile>.Failure(new Error("GHTK.PrintLabelError", ex.Message));
            }
        }

        public async Task<Result<GhtkTrackingStatusResponse>> GetTrackingStatusAsync(string label, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(label))
                {
                    return Result<GhtkTrackingStatusResponse>.Failure(new Error("GHTK.InvalidLabel", "Label is required."));
                }

                var response = await _ghtkRepository.GetTrackingStatusAsync(label.Trim(), cancellationToken);
                return Result<GhtkTrackingStatusResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking GHTK order {Label}", label);
                return Result<GhtkTrackingStatusResponse>.Failure(new Error("GHTK.TrackingStatusError", ex.Message));
            }
        }
    }
}