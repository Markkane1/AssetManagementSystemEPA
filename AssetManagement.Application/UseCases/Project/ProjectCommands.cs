using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Project
{
    public record CreateProjectCommand(ProjectDto Project) : IRequest<int>;
    public record UpdateProjectCommand(ProjectDto Project) : IRequest<Unit>;
    public record DeleteProjectCommand(int Id) : IRequest<Unit>;

    public class ProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>,
                                         IRequestHandler<UpdateProjectCommand, Unit>,
                                         IRequestHandler<DeleteProjectCommand, Unit>
    {
        private readonly IRepository<Domain.Entities.Project> _repository;
        private readonly IMapper _mapper;

        public ProjectCommandHandler(IRepository<Domain.Entities.Project> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = _mapper.Map<Domain.Entities.Project>(request.Project);
            await _repository.AddAsync(project);
            return project.Id;
        }

        public async Task<Unit> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Project.Id);
            if (project == null)
                throw new KeyNotFoundException($"Project with ID {request.Project.Id} not found.");
            _mapper.Map(request.Project, project);
            await _repository.UpdateAsync(project);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id);
            if (project == null)
                throw new KeyNotFoundException($"Project with ID {request.Id} not found.");
            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}