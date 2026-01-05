using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Vendor
{
    public record GetAllVendorsQuery : IRequest<IEnumerable<VendorDto>>;
    public record GetVendorByIdQuery(int Id) : IRequest<VendorDto>;

    public class VendorQueryHandler : IRequestHandler<GetAllVendorsQuery, IEnumerable<VendorDto>>,
        IRequestHandler<GetVendorByIdQuery, VendorDto>
    {
        private readonly IRepository<AssetManagement.Domain.Entities.Vendor> _repository;
        private readonly IMapper _mapper;

        public VendorQueryHandler(IRepository<Domain.Entities.Vendor> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VendorDto>> Handle(GetAllVendorsQuery request, CancellationToken cancellationToken)
        {
            var vendors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<VendorDto>>(vendors);
        }

        public async Task<VendorDto> Handle(GetVendorByIdQuery request, CancellationToken cancellationToken)
        {
            var vendor = await _repository.GetByIdAsync(request.Id);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with ID {request.Id} not found.");
            return _mapper.Map<VendorDto>(vendor);
        }
    }
}