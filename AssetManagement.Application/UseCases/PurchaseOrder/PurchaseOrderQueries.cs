using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.PurchaseOrder
{
    public record GetAllPurchaseOrdersQuery : IRequest<IEnumerable<PurchaseOrderDto>>;
    public record GetPurchaseOrderByIdQuery(int Id) : IRequest<PurchaseOrderDto>;

    public class PurchaseOrderQueryHandler : IRequestHandler<GetAllPurchaseOrdersQuery, IEnumerable<PurchaseOrderDto>>,
        IRequestHandler<GetPurchaseOrderByIdQuery, PurchaseOrderDto>
    {
        private readonly IRepository<Domain.Entities.PurchaseOrder> _repository;
        private readonly IMapper _mapper;

        public PurchaseOrderQueryHandler(IRepository<Domain.Entities.PurchaseOrder> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PurchaseOrderDto>> Handle(GetAllPurchaseOrdersQuery request, CancellationToken cancellationToken)
        {
            var purchaseOrders = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<PurchaseOrderDto>>(purchaseOrders);
        }

        public async Task<PurchaseOrderDto> Handle(GetPurchaseOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _repository.GetByIdAsync(request.Id);
            if (purchaseOrder == null)
                throw new KeyNotFoundException($"PurchaseOrder with ID {request.Id} not found.");
            return _mapper.Map<PurchaseOrderDto>(purchaseOrder);
        }
    }
}