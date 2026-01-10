using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.MaintenanceRecord
{
    public record GetAllMaintenanceRecordsQuery(string UserId) : IRequest<IEnumerable<MaintenanceRecordDto>>;
    public record GetMaintenanceRecordByIdQuery(int Id) : IRequest<MaintenanceRecordDto>;

    public class MaintenanceRecordQueryHandler : IRequestHandler<GetAllMaintenanceRecordsQuery, IEnumerable<MaintenanceRecordDto>>,
        IRequestHandler<GetMaintenanceRecordByIdQuery, MaintenanceRecordDto>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public MaintenanceRecordQueryHandler(IAssetRepository assetRepository, IIdentityService identityService, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MaintenanceRecordDto>> Handle(GetAllMaintenanceRecordsQuery request, CancellationToken cancellationToken)
        {
            var allowedLocations = await _identityService.GetAllowedLocationIdsAsync(request.UserId);
            var maintenanceRecords = await _assetRepository.GetAllMaintenanceRecordsAsync(allowedLocations);
            return _mapper.Map<IEnumerable<MaintenanceRecordDto>>(maintenanceRecords);
        }

        public async Task<MaintenanceRecordDto> Handle(GetMaintenanceRecordByIdQuery request, CancellationToken cancellationToken)
        {
            var maintenanceRecord = await _assetRepository.GetMaintenanceRecordByIdAsync(request.Id);
            return _mapper.Map<MaintenanceRecordDto>(maintenanceRecord);
        }
    }
}