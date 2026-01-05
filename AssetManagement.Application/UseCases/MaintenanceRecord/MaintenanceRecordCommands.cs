using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.MaintenanceRecord
{
    public record CreateMaintenanceRecordCommand(MaintenanceRecordDto MaintenanceRecord) : IRequest<int>;
    public record UpdateMaintenanceRecordCommand(MaintenanceRecordDto MaintenanceRecord) : IRequest<Unit>;
    public record DeleteMaintenanceRecordCommand(int Id) : IRequest<Unit>;

    public class MaintenanceRecordCommandHandler : IRequestHandler<CreateMaintenanceRecordCommand, int>,
                                                   IRequestHandler<UpdateMaintenanceRecordCommand, Unit>,
                                                   IRequestHandler<DeleteMaintenanceRecordCommand, Unit>
    {
        private readonly IRepository<Domain.Entities.MaintenanceRecord> _repository;
        private readonly IMapper _mapper;

        public MaintenanceRecordCommandHandler(IRepository<Domain.Entities.MaintenanceRecord> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateMaintenanceRecordCommand request, CancellationToken cancellationToken)
        {
            var maintenanceRecord = _mapper.Map<Domain.Entities.MaintenanceRecord>(request.MaintenanceRecord);
            await _repository.AddAsync(maintenanceRecord);
            return maintenanceRecord.Id;
        }

        public async Task<Unit> Handle(UpdateMaintenanceRecordCommand request, CancellationToken cancellationToken)
        {
            var maintenanceRecord = await _repository.GetByIdAsync(request.MaintenanceRecord.Id);
            if (maintenanceRecord == null)
                throw new KeyNotFoundException($"MaintenanceRecord with ID {request.MaintenanceRecord.Id} not found.");
            _mapper.Map(request.MaintenanceRecord, maintenanceRecord);
            await _repository.UpdateAsync(maintenanceRecord);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteMaintenanceRecordCommand request, CancellationToken cancellationToken)
        {
            var maintenanceRecord = await _repository.GetByIdAsync(request.Id);
            if (maintenanceRecord == null)
                throw new KeyNotFoundException($"MaintenanceRecord with ID {request.Id} not found.");
            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}