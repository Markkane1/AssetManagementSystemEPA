using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Project
{
    public record GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>;
    public record GetProjectByIdQuery(int Id) : IRequest<ProjectDto>;

    public class ProjectQueryHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>,
                                       IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IRepository<Domain.Entities.Project> _repository;
        private readonly IMapper _mapper;

        public ProjectQueryHandler(IRepository<Domain.Entities.Project> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id);
            if (project == null)
                throw new KeyNotFoundException($"Project with ID {request.Id} not found.");
            return _mapper.Map<ProjectDto>(project);
        }
    }
}