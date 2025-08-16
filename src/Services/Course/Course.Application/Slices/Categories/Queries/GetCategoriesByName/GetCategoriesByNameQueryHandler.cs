using Course.Application.Dtos.CategoryDto;

namespace Course.Application.Slices.Categories.Queries.GetCategoriesByName
{
    public record GetCategoriesByNameQuery(string Name) : IQuery<IEnumerable<CategoryResponse>>;
    internal class GetCategoriesByNameQueryHandler (ICategoryService categoryService)
        : IQueryHandler<GetCategoriesByNameQuery, IEnumerable<CategoryResponse>>
    {
        public async Task<IEnumerable<CategoryResponse>> Handle(GetCategoriesByNameQuery request, CancellationToken cancellationToken)
        {
            return await categoryService.GetAllCategoriesAsync(x => x.Name.ToUpper().Contains(request.Name.ToUpper()));
        }
    }
}
