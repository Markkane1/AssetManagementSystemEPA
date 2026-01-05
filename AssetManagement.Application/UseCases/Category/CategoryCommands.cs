using System.Threading;
using System.Threading.Tasks;
using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Category
{
    public record CreateCategoryCommand(CategoryDto Category) : IRequest<int>;
    public record UpdateCategoryCommand(CategoryDto Category) : IRequest<Unit>;
    public record DeleteCategoryCommand(int Id) : IRequest<Unit>;

    public class CategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>,
                                         IRequestHandler<UpdateCategoryCommand, Unit>,
                                         IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IRepository<Domain.Entities.Category> _repository;
        private readonly IMapper _mapper;

        public CategoryCommandHandler(IRepository<Domain.Entities.Category> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Domain.Entities.Category>(request.Category);
            await _repository.AddAsync(category);
            return category.Id;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Category.Id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {request.Category.Id} not found.");
            _mapper.Map(request.Category, category);
            await _repository.UpdateAsync(category);
            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            await _repository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}