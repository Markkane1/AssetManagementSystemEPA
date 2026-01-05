using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Directorate
{
    public record GetAllDirectoratesQuery : IRequest<IEnumerable<DirectorateDto>>;
    public record GetDirectorateByIdQuery(int Id) : IRequest<DirectorateDto>;

    public class DirectorateQueryHandler : IRequestHandler<GetAllDirectoratesQuery, IEnumerable<DirectorateDto>>,
        IRequestHandler<GetDirectorateByIdQuery, DirectorateDto>
    {
        private readonly IRepository<Domain.Entities.Directorate> _repository;
        private readonly IMapper _mapper;

        public DirectorateQueryHandler(IRepository<Domain.Entities.Directorate> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DirectorateDto>> Handle(GetAllDirectoratesQuery request, CancellationToken cancellationToken)
        {
            var directorates = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DirectorateDto>>(directorates);
        }

        public async Task<DirectorateDto> Handle(GetDirectorateByIdQuery request, CancellationToken cancellationToken)
        {
            var directorate = await _repository.GetByIdAsync(request.Id);
            if (directorate == null)
                throw new KeyNotFoundException($"Directorate with ID {request.Id} not found.");
            return _mapper.Map<DirectorateDto>(directorate);
        }
    }
}