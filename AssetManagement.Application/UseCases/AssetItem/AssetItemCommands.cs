using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.AssetItem
{
    public record CreateAssetItemCommand(AssetItemDto AssetItem) : IRequest<int>;
    public record UpdateAssetItemCommand(AssetItemDto AssetItem) : IRequest<Unit>;
    public record DeleteAssetItemCommand(int Id) : IRequest<Unit>;

    public class AssetItemCommandHandler : IRequestHandler<CreateAssetItemCommand, int>,
                                           IRequestHandler<UpdateAssetItemCommand, Unit>,
                                           IRequestHandler<DeleteAssetItemCommand, Unit>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public AssetItemCommandHandler(IAssetRepository assetRepository, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAssetItemCommand request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.AssetItem.AssetId);
            if (asset == null)
                throw new KeyNotFoundException($"Asset with ID {request.AssetItem.AssetId} not found.");

            var assetItem = _mapper.Map<Domain.Entities.AssetItem>(request.AssetItem);

            // This implicitly updates UntrackedQuantity and validates invariant
            asset.AddAssetItem(assetItem);

            await _assetRepository.AddAssetItemAsync(assetItem);
            await _assetRepository.UpdateAsync(asset); // Save the updated UntrackedQuantity

            return assetItem.Id;
        }

        public async Task<Unit> Handle(UpdateAssetItemCommand request, CancellationToken cancellationToken)
        {
            var assetItem = await _assetRepository.GetAssetItemByIdAsync(request.AssetItem.Id);
            if (assetItem == null)
                throw new KeyNotFoundException($"AssetItem with ID {request.AssetItem.Id} not found.");
            _mapper.Map(request.AssetItem, assetItem);
            await _assetRepository.UpdateAssetItemAsync(assetItem);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteAssetItemCommand request, CancellationToken cancellationToken)
        {
            var assetItem = await _assetRepository.GetAssetItemByIdAsync(request.Id);
            if (assetItem == null)
                throw new KeyNotFoundException($"AssetItem with ID {request.Id} not found.");
            await _assetRepository.DeleteAssetItemAsync(request.Id);
            return Unit.Value;
        }
    }
}