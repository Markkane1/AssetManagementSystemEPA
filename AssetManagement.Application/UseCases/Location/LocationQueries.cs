using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Location
{
    public record GetAllLocationsQuery : IRequest<IEnumerable<LocationDto>>;
    public record GetLocationByIdQuery(int Id) : IRequest<LocationDto>;

    public class LocationQueryHandler : IRequestHandler<GetAllLocationsQuery, IEnumerable<LocationDto>>,
        IRequestHandler<GetLocationByIdQuery, LocationDto>
    {
        private readonly IRepository<Domain.Entities.Location> _repository;
        private readonly IMapper _mapper;

        public LocationQueryHandler(IRepository<Domain.Entities.Location> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LocationDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<LocationDto>>(locations);
        }

        public async Task<LocationDto> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var location = await _repository.GetByIdAsync(request.Id);
            if (location == null)
                throw new KeyNotFoundException($"Location with ID {request.Id} not found.");
            return _mapper.Map<LocationDto>(location);
        }
    }
}