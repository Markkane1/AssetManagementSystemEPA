using AssetManagement.Application.DTOs;
using AssetManagement.Application.Interfaces;
using AutoMapper;
using MediatR;

namespace AssetManagement.Application.UseCases.Category
{
    public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto>;

    public class CategoryQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>,
        IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IRepository<Domain.Entities.Category> _repository;
        private readonly IMapper _mapper;

        public CategoryQueryHandler(IRepository<Domain.Entities.Category> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var category = await _repository.GetByIdAsync(request.Id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {request.Id} not found.");
            return _mapper.Map<CategoryDto>(category);
        }
    }
}