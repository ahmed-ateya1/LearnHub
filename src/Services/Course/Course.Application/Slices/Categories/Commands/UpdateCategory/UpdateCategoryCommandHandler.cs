using Course.Application.Dtos.CategoryDto;
using FluentValidation;

namespace Course.Application.Slices.Categories.Commands.UpdateCategory

{
    public record UpdateCategoryCommand(CategoryUpdateRequest Category) : ICommand<CategoryResponse>;

    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.Category.Id).NotEqual(Guid.Empty).WithMessage("Category ID cannot be empty.");
            RuleFor(x => x.Category.Name).NotEmpty().WithMessage("Category name cannot be empty.");
            RuleFor(x => x.Category.BaseCategoryId)
                .NotEmpty().WithMessage("Base category ID cannot be empty.")
                .When(x => x.Category.BaseCategoryId.HasValue);
        }
    }
    internal class UpdateCategoryCommandHandler(ICategoryService categoryService)
        : ICommandHandler<UpdateCategoryCommand, CategoryResponse>
    {
        public async Task<CategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            return await categoryService.UpdateCategoryAsync(request.Category);
        }
    }
}
