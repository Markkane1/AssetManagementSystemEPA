using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AssetManagement.Domain.ValueObjects;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Asset
{
    public record CreateAssetCommand(AssetDto Asset) : IRequest<int>;
    public record UpdateAssetCommand(AssetDto Asset) : IRequest<Unit>;
    public record DeleteAssetCommand(int Id) : IRequest<Unit>;

    public class AssetCommandHandler : IRequestHandler<CreateAssetCommand, int>,
                                       IRequestHandler<UpdateAssetCommand, Unit>,
                                       IRequestHandler<DeleteAssetCommand, Unit>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public AssetCommandHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAssetCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Asset;
            var price = Money.Create(dto.Price ?? 0, "USD");

            var asset = new Domain.Entities.Asset(
                dto.Name,
                dto.AssetCode,
                dto.CategoryId,
                dto.PurchaseDate ?? DateTime.UtcNow, // Mapping PurchaseDate to AcquisitionDate or similar
                price,
                dto.Quantity,
                dto.UntrackedQuantity,
                dto.Manufacturer,
                dto.Model,
                dto.Specification,
                dto.WarrantyEndDate,
                "System", // Should ideally be from user context
                DateTime.UtcNow
            );

            if (dto.VendorId.HasValue) asset.SetVendor(dto.VendorId.Value);
            if (dto.PurchaseOrderId.HasValue) asset.SetPurchaseOrder(dto.PurchaseOrderId.Value);

            await _assetRepository.AddAsync(asset);
            return asset.Id;
        }

        public async Task<Unit> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.Asset.Id);
            if (asset == null)
                throw new KeyNotFoundException($"Asset with ID {request.Asset.Id} not found.");

            var dto = request.Asset;
            asset.UpdateDetails(
                dto.Name,
                dto.AssetCode,
                dto.Manufacturer,
                dto.Model,
                dto.Specification,
                dto.PurchaseDate ?? asset.AcquisitionDate,
                dto.WarrantyEndDate,
                "System",
                DateTime.UtcNow
            );

            if (dto.VendorId != asset.VendorId) asset.SetVendor(dto.VendorId);
            if (dto.PurchaseOrderId != asset.PurchaseOrderId) asset.SetPurchaseOrder(dto.PurchaseOrderId);

            await _assetRepository.UpdateAsync(asset);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.Id);
            if (asset == null)
                throw new KeyNotFoundException($"Asset with ID {request.Id} not found.");
            await _assetRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}