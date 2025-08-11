using Course.Application.Dtos.CategoryDto;

namespace Course.Application.Categories.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryResponse>;
    public class GetCategoryByIdQueryHandler (ICategoryService categoryService)
        : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
    {
        public async Task<CategoryResponse> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await categoryService.GetCategoryByAsync(x=>x.Id == request.Id);
        }
    }
}
