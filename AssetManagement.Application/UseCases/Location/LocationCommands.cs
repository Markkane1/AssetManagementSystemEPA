using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Location
{
    public record CreateLocationCommand(LocationDto Location) : IRequest<int>;
    public record UpdateLocationCommand(LocationDto Location) : IRequest<Unit>;
    public record DeleteLocationCommand(int Id) : IRequest<Unit>;

    public class LocationCommandHandler : IRequestHandler<CreateLocationCommand, int>,
                                          IRequestHandler<UpdateLocationCommand, Unit>,
                                          IRequestHandler<DeleteLocationCommand, Unit>
    {
        private readonly IRepository<Domain.Entities.Location> _repository;
        private readonly IMapper _mapper;

        public LocationCommandHandler(IRepository<Domain.Entities.Location> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            var location = _mapper.Map<Domain.Entities.Location>(request.Location);
            await _repository.AddAsync(location);
            return location.Id;
        }

        public async Task<Unit> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await _repository.GetByIdAsync(request.Location.Id);
            if (location == null)
                throw new KeyNotFoundException($"Location with ID {request.Location.Id} not found.");
            _mapper.Map(request.Location, location);
            await _repository.UpdateAsync(location);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            var location = await _repository.GetByIdAsync(request.Id);
            if (location == null)
                throw new KeyNotFoundException($"Location with ID {request.Id} not found.");
            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}