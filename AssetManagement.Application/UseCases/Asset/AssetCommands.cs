using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
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
            var asset = _mapper.Map<Domain.Entities.Asset>(request.Asset);
            await _assetRepository.AddAsync(asset);
            return asset.Id;
        }

        public async Task<Unit> Handle(UpdateAssetCommand request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.Asset.Id);
            if (asset == null)
                throw new KeyNotFoundException($"Asset with ID {request.Asset.Id} not found.");
            _mapper.Map(request.Asset, asset);
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