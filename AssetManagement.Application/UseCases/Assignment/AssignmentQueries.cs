using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Assignment
{
    public record GetAssignmentsByEmployeeQuery(int EmployeeId) : IRequest<IEnumerable<AssignmentDto>>;
    public record GetAssignmentsByAssetItemQuery(int AssetItemId) : IRequest<IEnumerable<AssignmentDto>>;
    public record GetAllAssignmentHistoryForEmployeeQuery(int EmployeeId) : IRequest<IEnumerable<AssignmentDto>>;
    public record GetAssignmentHistoryForAssetQuery(int AssetItemId) : IRequest<IEnumerable<AssignmentDto>>;
    public record GetAssignmentsByDateRangeQuery(DateTime StartDate, DateTime EndDate) : IRequest<IEnumerable<AssignmentDto>>;
    public record GetUnassignedAssetItemsQuery : IRequest<IEnumerable<AssetItemDto>>;
    public record GetOverdueAssignmentsQuery : IRequest<IEnumerable<AssignmentDto>>;

    public class GetAllAssignmentsQuery : IRequest<IEnumerable<AssignmentDto>> { }

    public class GetAssignmentByIdQuery : IRequest<AssignmentDto>
    {
        public int Id { get; }
        public GetAssignmentByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class AssignmentQueryHandler :
        IRequestHandler<GetAssignmentsByEmployeeQuery, IEnumerable<AssignmentDto>>,
        IRequestHandler<GetAssignmentsByAssetItemQuery, IEnumerable<AssignmentDto>>,
        IRequestHandler<GetAllAssignmentHistoryForEmployeeQuery, IEnumerable<AssignmentDto>>,
        IRequestHandler<GetAssignmentHistoryForAssetQuery, IEnumerable<AssignmentDto>>,
        IRequestHandler<GetAssignmentsByDateRangeQuery, IEnumerable<AssignmentDto>>,
        IRequestHandler<GetUnassignedAssetItemsQuery, IEnumerable<AssetItemDto>>,
        IRequestHandler<GetOverdueAssignmentsQuery, IEnumerable<AssignmentDto>>,
        IRequestHandler<GetAllAssignmentsQuery, IEnumerable<AssignmentDto>>,
        IRequestHandler<GetAssignmentByIdQuery, AssignmentDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public AssignmentQueryHandler(IEmployeeRepository employeeRepository, IAssetRepository assetRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _assetRepository = assetRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AssignmentDto>> Handle(GetAllAssignmentsQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _employeeRepository.GetAllAsync();
            var allAssignments = assignments.SelectMany(e => e.Assignments);
            return _mapper.Map<IEnumerable<AssignmentDto>>(allAssignments);
        }

        public async Task<AssignmentDto> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _employeeRepository.GetAllAsync();
            var assignment = assignments.SelectMany(e => e.Assignments)
                .FirstOrDefault(a => a.Id == request.Id);
            if (assignment == null)
                throw new KeyNotFoundException($"Assignment with ID {request.Id} not found.");
            return _mapper.Map<AssignmentDto>(assignment);
        }
        public async Task<IEnumerable<AssignmentDto>> Handle(GetAssignmentsByEmployeeQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _employeeRepository.GetAssignmentsByEmployeeAsync(request.EmployeeId);
            return _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
        }

        public async Task<IEnumerable<AssignmentDto>> Handle(GetAssignmentsByAssetItemQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _employeeRepository.GetAllAsync(); // Get all assignments
            var filteredAssignments = assignments
                .SelectMany(e => e.Assignments) // Flatten assignments from employees
                .Where(a => a.AssetItemId == request.AssetItemId && a.ReturnDate == null);
            return _mapper.Map<IEnumerable<AssignmentDto>>(filteredAssignments);
        }

        public async Task<IEnumerable<AssignmentDto>> Handle(GetAllAssignmentHistoryForEmployeeQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _employeeRepository.GetAllAssignmentHistoryForEmployeeAsync(request.EmployeeId);
            return _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
        }

        public async Task<IEnumerable<AssignmentDto>> Handle(GetAssignmentHistoryForAssetQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _employeeRepository.GetAllAsync(); // Get all assignments
            var filteredAssignments = assignments
                .SelectMany(e => e.Assignments)
                .Where(a => a.AssetItemId == request.AssetItemId);
            return _mapper.Map<IEnumerable<AssignmentDto>>(filteredAssignments);
        }

        public async Task<IEnumerable<AssignmentDto>> Handle(GetAssignmentsByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _employeeRepository.GetAssignmentsByDateRangeAsync(request.StartDate, request.EndDate);
            return _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
        }

        public async Task<IEnumerable<AssetItemDto>> Handle(GetUnassignedAssetItemsQuery request, CancellationToken cancellationToken)
        {
            var assetItems = await _assetRepository.GetUnassignedAssetItemsAsync();
            return _mapper.Map<IEnumerable<AssetItemDto>>(assetItems);
        }

        public async Task<IEnumerable<AssignmentDto>> Handle(GetOverdueAssignmentsQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _employeeRepository.GetOverdueAssignmentsAsync();
            return _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
        }
    }
}