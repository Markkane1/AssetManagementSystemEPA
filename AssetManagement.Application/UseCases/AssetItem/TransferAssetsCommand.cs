using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.UseCases.AssetItem
{
    public record TransferAssetsCommand(int[] AssetItemIds, int TargetLocationId) : IRequest<Unit>;

    public class TransferAssetsCommandHandler : IRequestHandler<TransferAssetsCommand, Unit>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IRepository<Domain.Entities.Location> _locationRepository;
        private readonly IRepository<Domain.Entities.TransferHistory> _transferHistoryRepository;
        private readonly IMapper _mapper;

        public TransferAssetsCommandHandler(
            IAssetRepository assetRepository,
            IRepository<Domain.Entities.Location> locationRepository,
            IRepository<Domain.Entities.TransferHistory> transferHistoryRepository,
            IMapper mapper)
        {
            _assetRepository = assetRepository;
            _locationRepository = locationRepository;
            _transferHistoryRepository = transferHistoryRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(TransferAssetsCommand request, CancellationToken cancellationToken)
        {
            var targetLocation = await _locationRepository.GetByIdAsync(request.TargetLocationId);
            if (targetLocation == null)
                throw new KeyNotFoundException($"Target location with ID {request.TargetLocationId} not found.");

            var assetItems = new List<Domain.Entities.AssetItem>();
            var assetIds = new HashSet<int>();
            foreach (var id in request.AssetItemIds)
            {
                var assetItem = await _assetRepository.GetAssetItemByIdAsync(id);
                if (assetItem == null)
                    throw new KeyNotFoundException($"AssetItem with ID {id} not found.");
                if (assetItem.LocationId == request.TargetLocationId)
                    throw new InvalidOperationException($"AssetItem with ID {id} is already at location {request.TargetLocationId}.");
                if (assetItem.AssignmentStatus == AssignmentStatus.Assigned)
                    throw new InvalidOperationException($"AssetItem with ID {id} is currently assigned and cannot be transferred.");
                assetItems.Add(assetItem);
                assetIds.Add(assetItem.AssetId);
            }

            foreach (var assetId in assetIds)
            {
                var asset = await _assetRepository.GetByIdAsync(assetId);
                if (asset == null)
                    throw new KeyNotFoundException($"Asset with ID {assetId} not found.");
                var itemsToTransfer = assetItems.Count(ai => ai.AssetId == assetId);
                if (itemsToTransfer > asset.Quantity)
                    throw new InvalidOperationException($"Insufficient quantity for Asset ID {assetId}. Available: {asset.Quantity}, Requested: {itemsToTransfer}.");
            }

            foreach (var assetItem in assetItems)
            {
                var oldLocationId = assetItem.LocationId;
                assetItem.LocationId = request.TargetLocationId;
                await _assetRepository.UpdateAssetItemAsync(assetItem);

                var transferHistory = new Domain.Entities.TransferHistory
                {
                    AssetItemId = assetItem.Id,
                    FromLocationId = oldLocationId,
                    ToLocationId = request.TargetLocationId,
                    TransferDate = DateTime.UtcNow
                };
                await _transferHistoryRepository.AddAsync(transferHistory);

                var asset = await _assetRepository.GetByIdAsync(assetItem.AssetId);
                asset.UpdateQuantity(asset.Quantity - 1);
                if (asset.Quantity == 0)
                    await _assetRepository.DeleteAsync(assetItem.AssetId);
                else
                    await _assetRepository.UpdateAsync(asset);
            }

            return Unit.Value;
        }
    }
}