using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Directorate
{
    public record CreateDirectorateCommand(DirectorateDto Directorate) : IRequest<int>;
    public record UpdateDirectorateCommand(DirectorateDto Directorate) : IRequest<Unit>;
    public record DeleteDirectorateCommand(int Id) : IRequest<Unit>;

    public class DirectorateCommandHandler : IRequestHandler<CreateDirectorateCommand, int>,
                                            IRequestHandler<UpdateDirectorateCommand, Unit>,
                                            IRequestHandler<DeleteDirectorateCommand, Unit>
    {
        private readonly IRepository<Domain.Entities.Directorate> _repository;
        private readonly IMapper _mapper;

        public DirectorateCommandHandler(IRepository<Domain.Entities.Directorate> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateDirectorateCommand request, CancellationToken cancellationToken)
        {
            var directorate = _mapper.Map<Domain.Entities.Directorate>(request.Directorate);
            await _repository.AddAsync(directorate);
            return directorate.Id;
        }

        public async Task<Unit> Handle(UpdateDirectorateCommand request, CancellationToken cancellationToken)
        {
            var directorate = await _repository.GetByIdAsync(request.Directorate.Id);
            if (directorate == null)
                throw new KeyNotFoundException($"Directorate with ID {request.Directorate.Id} not found.");
            _mapper.Map(request.Directorate, directorate);
            await _repository.UpdateAsync(directorate);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteDirectorateCommand request, CancellationToken cancellationToken)
        {
            var directorate = await _repository.GetByIdAsync(request.Id);
            if (directorate == null)
                throw new KeyNotFoundException($"Directorate with ID {request.Id} not found.");
            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}