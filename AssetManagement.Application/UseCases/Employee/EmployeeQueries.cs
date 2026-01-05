using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Employee
{
    public record GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>;
    public record GetEmployeeByIdQuery(int Id) : IRequest<EmployeeDto>;

    public class EmployeeQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>,
        IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {request.Id} not found.");
            return _mapper.Map<EmployeeDto>(employee);
        }
    }
}