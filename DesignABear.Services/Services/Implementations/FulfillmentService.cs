using AutoMapper;
using DesignABear.Repositories.Models;
using DesignABear.Repositories.UnitOfWork;
using DesignABear.Services.DTOs.Requests;
using DesignABear.Services.DTOs.Responses;
using DesignABear.Services.Exceptions;
using DesignABear.Services.Services.Interfaces;

namespace DesignABear.Services.Services.Implementations
{
    public class FulfillmentService : IFulfillmentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public FulfillmentService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FulfillmentResponse>> GetByOrderIdAsync(Guid orderId)
        {
            var fulfillments = await _uow.Fulfillments.GetByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<FulfillmentResponse>>(fulfillments);
        }

        public async Task<FulfillmentResponse?> GetByIdAsync(Guid fulfillmentId)
        {
            var fulfillment = await _uow.Fulfillments.GetByIdAsync(fulfillmentId);
            if (fulfillment == null) throw new NotFoundException("Fulfillment", fulfillmentId);
            return _mapper.Map<FulfillmentResponse>(fulfillment);
        }

        public async Task<FulfillmentResponse> CreateAsync(CreateFulfillmentRequest request)
        {
            var fulfillment = _mapper.Map<Fulfillment>(request);
            fulfillment.FulfillmentId = Guid.NewGuid();
            fulfillment.CreatedAt = DateTime.UtcNow;
            fulfillment.Status = "PENDING";
            
            await _uow.Fulfillments.AddAsync(fulfillment);
            await _uow.SaveChangesAsync();
            
            return _mapper.Map<FulfillmentResponse>(fulfillment);
        }

        public async Task UpdateAsync(Guid fulfillmentId, UpdateFulfillmentRequest request)
        {
            var fulfillment = await _uow.Fulfillments.GetByIdAsync(fulfillmentId);
            if (fulfillment == null) throw new NotFoundException("Fulfillment", fulfillmentId);
            
            _mapper.Map(request, fulfillment);
            
            if (request.Status == "COMPLETED" && fulfillment.ShippedAt == null)
            {
                fulfillment.ShippedAt = DateTime.UtcNow;
            }
            
            _uow.Fulfillments.Update(fulfillment);
            await _uow.SaveChangesAsync();
        }
    }
}
