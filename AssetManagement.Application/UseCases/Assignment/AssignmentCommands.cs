using System;
using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;
using AssetManagement.Domain.Enums;

namespace AssetManagement.Application.UseCases.Assignment
{
    public record ReturnAssetItemCommand(int AssetItemId, DateTime ReturnDate) : IRequest<Unit>;
    public record ReassignAssetItemCommand(int AssignmentId, int NewEmployeeId, DateTime AssignmentDate) : IRequest<Unit>;

    public record CreateAssignmentCommand(AssignmentDto Assignment) : IRequest<int>;
    public record UpdateAssignmentCommand(AssignmentDto Assignment) : IRequest<Unit>;
    public record DeleteAssignmentCommand(int Id) : IRequest<Unit>;

    public class AssignmentCommandHandler :
        IRequestHandler<ReturnAssetItemCommand, Unit>,
        IRequestHandler<ReassignAssetItemCommand, Unit>,
        IRequestHandler<CreateAssignmentCommand, int>,
        IRequestHandler<UpdateAssignmentCommand, Unit>,
        IRequestHandler<DeleteAssignmentCommand, Unit>
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IRepository<Domain.Entities.Assignment> _assignmentRepository;
        private readonly IMapper _mapper;

        public AssignmentCommandHandler(
            IAssetRepository assetRepository,
            IRepository<Domain.Entities.Assignment> assignmentRepository,
            IMapper mapper)
        {
            _assetRepository = assetRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(ReturnAssetItemCommand request, CancellationToken cancellationToken)
        {
            var assetItem = await _assetRepository.GetAssetItemByIdAsync(request.AssetItemId);
            if (assetItem == null)
                throw new KeyNotFoundException($"AssetItem with ID {request.AssetItemId} not found.");

            // Bad performance but sticking to existing pattern of fetching all for now, or use Where if Repos allows
            // Assuming _assignmentRepository.GetAllAsync() is what we have. 
            // Ideally we should add GetActiveAssignmentByAssetId logic.
            var assignments = await _assignmentRepository.GetAllAsync();
            var activeAssignment = assignments.FirstOrDefault(a => a.AssetItemId == request.AssetItemId && a.ReturnDate == null);

            if (activeAssignment == null)
                throw new InvalidOperationException($"No active assignment found for AssetItem ID {request.AssetItemId}.");

            activeAssignment.ReturnDate = request.ReturnDate;
            activeAssignment.AssignmentStatus = AssignmentStatus.Returned; // Updating status on Assignment

            await _assignmentRepository.UpdateAsync(activeAssignment);
            // No need to update assetItem.AssignmentStatus

            // However, we might need to update AssetItem state (Functional/etc) if logic requires, but AssetItem.Status is different.

            return Unit.Value;
        }

        public async Task<Unit> Handle(ReassignAssetItemCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.AssignmentId);
            if (assignment == null)
                throw new KeyNotFoundException($"Assignment with ID {request.AssignmentId} not found.");

            var assetItem = await _assetRepository.GetAssetItemByIdAsync(assignment.AssetItemId);
            if (assetItem == null)
                throw new KeyNotFoundException($"AssetItem with ID {assignment.AssetItemId} not found.");

            assignment.ReturnDate = DateTime.UtcNow;
            assignment.AssignmentStatus = AssignmentStatus.Returned;
            await _assignmentRepository.UpdateAsync(assignment);

            var newAssignment = new Domain.Entities.Assignment
            {
                AssetItemId = assignment.AssetItemId,
                EmployeeId = request.NewEmployeeId,
                AssignmentDate = request.AssignmentDate,
                AssignmentStatus = AssignmentStatus.Assigned
            };
            await _assignmentRepository.AddAsync(newAssignment);
            // No need to update assetItem.AssignmentStatus

            return Unit.Value;
        }

        public async Task<int> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assetItem = await _assetRepository.GetAssetItemByIdAsync(request.Assignment.AssetItemId);
            if (assetItem == null)
                throw new KeyNotFoundException($"AssetItem with ID {request.Assignment.AssetItemId} not found.");

            if (assetItem.Status == ItemStatus.NotRepairable)
                throw new InvalidOperationException("Cannot assign a non-repairable asset item.");

            var assignments = await _assignmentRepository.GetAllAsync();
            var activeAssignment = assignments.FirstOrDefault(a => a.AssetItemId == request.Assignment.AssetItemId && a.ReturnDate == null);

            if (activeAssignment != null)
                throw new InvalidOperationException($"AssetItem with ID {request.Assignment.AssetItemId} is already assigned.");

            var assignment = _mapper.Map<Domain.Entities.Assignment>(request.Assignment);
            assignment.AssignmentDate = DateTime.UtcNow;
            assignment.AssignmentStatus = AssignmentStatus.Assigned;

            await _assignmentRepository.AddAsync(assignment);
            // No need to update assetItem.AssignmentStatus

            return assignment.Id;
        }


        //public async Task<int> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        //{
        //    var assetItem = await _assetRepository.GetAssetItemByIdAsync(request.Assignment.AssetItemId);
        //    if (assetItem == null)
        //        throw new KeyNotFoundException($"AssetItem with ID {request.Assignment.AssetItemId} not found.");

        //    if (assetItem.Status == ItemStatus.NotRepairable)
        //        throw new InvalidOperationException("Cannot assign a non-repairable asset item.");

        //    if (assetItem.AssignmentStatus != AssignmentStatus.Available)
        //        throw new InvalidOperationException($"AssetItem with ID {request.Assignment.AssetItemId} is not available for assignment.");

        //    var assignment = _mapper.Map<Domain.Entities.Assignment>(request.Assignment);
        //    assignment.AssignmentDate = DateTime.UtcNow; // Ensure consistent UTC time
        //    await _assignmentRepository.AddAsync(assignment);

        //    assetItem.AssignmentStatus = AssignmentStatus.Assigned;
        //    await _assetRepository.UpdateAssetItemAsync(assetItem);

        //    return assignment.Id;
        //}

        //public async Task<int> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        //{
        //    var assignment = _mapper.Map<Domain.Entities.Assignment>(request.Assignment);
        //    await _assignmentRepository.AddAsync(assignment);
        //    return assignment.Id;
        //}

        public async Task<Unit> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.Assignment.Id);
            if (assignment == null)
                throw new KeyNotFoundException($"Assignment with ID {request.Assignment.Id} not found.");
            _mapper.Map(request.Assignment, assignment);
            await _assignmentRepository.UpdateAsync(assignment);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.Id);
            if (assignment == null)
                throw new KeyNotFoundException($"Assignment with ID {request.Id} not found.");
            await _assignmentRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}