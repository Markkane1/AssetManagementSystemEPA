using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.PurchaseOrder
{
    public record CreatePurchaseOrderCommand(PurchaseOrderDto PurchaseOrder) : IRequest<int>;
    public record UpdatePurchaseOrderCommand(PurchaseOrderDto PurchaseOrder) : IRequest<Unit>;
    public record DeletePurchaseOrderCommand(int Id) : IRequest<Unit>;

    public class PurchaseOrderCommandHandler : IRequestHandler<CreatePurchaseOrderCommand, int>,
                                               IRequestHandler<UpdatePurchaseOrderCommand, Unit>,
                                               IRequestHandler<DeletePurchaseOrderCommand, Unit>
    {
        private readonly IRepository<Domain.Entities.PurchaseOrder> _repository;
        private readonly IMapper _mapper;

        public PurchaseOrderCommandHandler(IRepository<Domain.Entities.PurchaseOrder> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = _mapper.Map<Domain.Entities.PurchaseOrder>(request.PurchaseOrder);
            await _repository.AddAsync(purchaseOrder);
            return purchaseOrder.Id;
        }

        public async Task<Unit> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _repository.GetByIdAsync(request.PurchaseOrder.Id);
            if (purchaseOrder == null)
                throw new KeyNotFoundException($"PurchaseOrder with ID {request.PurchaseOrder.Id} not found.");
            _mapper.Map(request.PurchaseOrder, purchaseOrder);
            await _repository.UpdateAsync(purchaseOrder);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _repository.GetByIdAsync(request.Id);
            if (purchaseOrder == null)
                throw new KeyNotFoundException($"PurchaseOrder with ID {request.Id} not found.");
            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}