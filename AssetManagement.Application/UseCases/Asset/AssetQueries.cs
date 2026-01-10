using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Asset
{
    public record GetAssetsByLocationQuery(int LocationId) : IRequest<IEnumerable<AssetDto>>;
    public record GetAssetsByCategoryQuery(int CategoryId) : IRequest<IEnumerable<AssetDto>>;
    public record GetAssetsBySourceQuery(int SourceId, string SourceType) : IRequest<IEnumerable<AssetDto>>;
    public record GetAssetsByVendorQuery(int VendorId) : IRequest<IEnumerable<AssetDto>>;
    public record GetAssetsByAcquisitionDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<IEnumerable<AssetDto>>;
    public record GetLowStockAssetsQuery(int Threshold) : IRequest<IEnumerable<AssetDto>>;
    public record GetAllAssetsQuery(string UserId) : IRequest<IEnumerable<AssetDto>>;
    public class GetAssetByIdQuery : IRequest<AssetDto>
    {
        public int Id { get; }
        public GetAssetByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class AssetQueryHandler :
        IRequestHandler<GetAssetsByLocationQuery, IEnumerable<AssetDto>>,
        IRequestHandler<GetAssetsByCategoryQuery, IEnumerable<AssetDto>>,
        IRequestHandler<GetAssetsBySourceQuery, IEnumerable<AssetDto>>,
        IRequestHandler<GetAssetsByVendorQuery, IEnumerable<AssetDto>>,
        IRequestHandler<GetAssetsByAcquisitionDateRangeQuery, IEnumerable<AssetDto>>,
        IRequestHandler<GetLowStockAssetsQuery, IEnumerable<AssetDto>>,
        IRequestHandler<GetAllAssetsQuery, IEnumerable<AssetDto>>,
        IRequestHandler<GetAssetByIdQuery, AssetDto>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AssetQueryHandler(IAssetRepository assetRepository, IIdentityService identityService, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AssetDto>> Handle(GetAssetsByLocationQuery request, CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetAssetsByLocationAsync(request.LocationId);
            return _mapper.Map<IEnumerable<AssetDto>>(assets);
        }

        public async Task<IEnumerable<AssetDto>> Handle(GetAssetsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetAssetsByCategoryAsync(request.CategoryId);
            return _mapper.Map<IEnumerable<AssetDto>>(assets);
        }

        public async Task<IEnumerable<AssetDto>> Handle(GetAssetsBySourceQuery request, CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetAssetsBySourceAsync(request.SourceId, request.SourceType);
            return _mapper.Map<IEnumerable<AssetDto>>(assets);
        }

        public async Task<IEnumerable<AssetDto>> Handle(GetAssetsByVendorQuery request, CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetAssetsByVendorAsync(request.VendorId);
            return _mapper.Map<IEnumerable<AssetDto>>(assets);
        }

        public async Task<IEnumerable<AssetDto>> Handle(GetAssetsByAcquisitionDateRangeQuery request, CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetAssetsByAcquisitionDateRangeAsync(request.StartDate, request.EndDate);
            return _mapper.Map<IEnumerable<AssetDto>>(assets);
        }

        public async Task<IEnumerable<AssetDto>> Handle(GetLowStockAssetsQuery request,
            CancellationToken cancellationToken)
        {
            var assets = await _assetRepository.GetLowStockAssetsAsync(request.Threshold);
            return _mapper.Map<IEnumerable<AssetDto>>(assets);
        }

        public async Task<IEnumerable<AssetDto>> Handle(GetAllAssetsQuery request, CancellationToken cancellationToken)
        {
            var allowedLocations = await _identityService.GetAllowedLocationIdsAsync(request.UserId);
            var assets = await _assetRepository.GetAllAssetsAsync(allowedLocations);
            return _mapper.Map<IEnumerable<AssetDto>>(assets);
        }

        public async Task<AssetDto> Handle(GetAssetByIdQuery request, CancellationToken cancellationToken)
        {
            var asset = await _assetRepository.GetByIdAsync(request.Id);
            if (asset == null)
                throw new KeyNotFoundException($"Asset with ID {request.Id} not found.");
            return _mapper.Map<AssetDto>(asset);
        }
    }
}