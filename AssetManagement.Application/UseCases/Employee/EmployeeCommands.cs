using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Employee
{
    public record CreateEmployeeCommand(EmployeeDto Employee) : IRequest<int>;
    public record UpdateEmployeeCommand(EmployeeDto Employee) : IRequest<Unit>;
    public record DeleteEmployeeCommand(int Id) : IRequest<Unit>;

    public class EmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>,
                                          IRequestHandler<UpdateEmployeeCommand, Unit>,
                                          IRequestHandler<DeleteEmployeeCommand, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeCommandHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<Domain.Entities.Employee>(request.Employee);
            await _employeeRepository.AddAsync(employee);
            return employee.Id;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Employee.Id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {request.Employee.Id} not found.");
            _mapper.Map(request.Employee, employee);
            await _employeeRepository.UpdateAsync(employee);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(request.Id);
            if (employee == null)
                throw new KeyNotFoundException($"Employee with ID {request.Id} not found.");
            await _employeeRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}