using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.MaintenanceRecord
{
    public record GetAllMaintenanceRecordsQuery : IRequest<IEnumerable<MaintenanceRecordDto>>;
    public record GetMaintenanceRecordByIdQuery(int Id) : IRequest<MaintenanceRecordDto>;

    public class MaintenanceRecordQueryHandler : IRequestHandler<GetAllMaintenanceRecordsQuery, IEnumerable<MaintenanceRecordDto>>,
        IRequestHandler<GetMaintenanceRecordByIdQuery, MaintenanceRecordDto>
    {
        private readonly IRepository<Domain.Entities.MaintenanceRecord> _repository;
        private readonly IMapper _mapper;

        public MaintenanceRecordQueryHandler(IRepository<Domain.Entities.MaintenanceRecord> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MaintenanceRecordDto>> Handle(GetAllMaintenanceRecordsQuery request, CancellationToken cancellationToken)
        {
            var maintenanceRecords = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MaintenanceRecordDto>>(maintenanceRecords);
        }

        public async Task<MaintenanceRecordDto> Handle(GetMaintenanceRecordByIdQuery request, CancellationToken cancellationToken)
        {
            var maintenanceRecord = await _repository.GetByIdAsync(request.Id);
            if (maintenanceRecord == null)
                throw new KeyNotFoundException($"MaintenanceRecord with ID {request.Id} not found.");
            return _mapper.Map<MaintenanceRecordDto>(maintenanceRecord);
        }
    }
}