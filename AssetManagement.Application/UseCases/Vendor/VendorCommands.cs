using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;
using AssetManagement.Domain.Entities;

namespace AssetManagement.Application.UseCases.Vendor
{
    public record CreateVendorCommand(VendorDto Vendor) : IRequest<int>;
    public record UpdateVendorCommand(VendorDto Vendor) : IRequest<Unit>;
    public record DeleteVendorCommand(int Id) : IRequest<Unit>;

    public class VendorCommandHandler : IRequestHandler<CreateVendorCommand, int>,
                                        IRequestHandler<UpdateVendorCommand, Unit>,
                                        IRequestHandler<DeleteVendorCommand, Unit>
    {
        private readonly IRepository<Domain.Entities.Vendor> _repository;
        private readonly IMapper _mapper;

        public VendorCommandHandler(IRepository<Domain.Entities.Vendor> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = _mapper.Map<Domain.Entities.Vendor>(request.Vendor);
            await _repository.AddAsync(vendor);
            return vendor.Id;
        }

        public async Task<Unit> Handle(UpdateVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _repository.GetByIdAsync(request.Vendor.Id);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with ID {request.Vendor.Id} not found.");
            _mapper.Map(request.Vendor, vendor);
            await _repository.UpdateAsync(vendor);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
        {
            var vendor = await _repository.GetByIdAsync(request.Id);
            if (vendor == null)
                throw new KeyNotFoundException($"Vendor with ID {request.Id} not found.");
            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}