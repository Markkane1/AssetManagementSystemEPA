using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.AssetItem
{
    public record GetAllAssetItemsQuery(string UserId) : IRequest<IEnumerable<AssetItemDto>>;
    public record GetAssetItemByIdQuery(int Id) : IRequest<AssetItemDto>;

    public class AssetItemQueryHandler : IRequestHandler<GetAllAssetItemsQuery, IEnumerable<AssetItemDto>>,
        IRequestHandler<GetAssetItemByIdQuery, AssetItemDto>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AssetItemQueryHandler(IAssetRepository assetRepository, IIdentityService identityService, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AssetItemDto>> Handle(GetAllAssetItemsQuery request, CancellationToken cancellationToken)
        {
            var allowedLocations = await _identityService.GetAllowedLocationIdsAsync(request.UserId);
            var assetItems = await _assetRepository.GetAllAssetItemsAsync(allowedLocations);
            return _mapper.Map<IEnumerable<AssetItemDto>>(assetItems);
        }

        public async Task<AssetItemDto> Handle(GetAssetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var assetItem = await _assetRepository.GetAssetItemByIdAsync(request.Id);
            if (assetItem == null)
                throw new KeyNotFoundException($"AssetItem with ID {request.Id} not found.");
            return _mapper.Map<AssetItemDto>(assetItem);
        }
    }
}