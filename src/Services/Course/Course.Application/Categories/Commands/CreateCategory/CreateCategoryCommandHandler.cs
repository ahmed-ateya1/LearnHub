using Course.Application.Dtos.CategoryDto;
using FluentValidation;

namespace Course.Application.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(CategoryAddRequest Category):ICommand<CategoryResponse>;

    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Category.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");
            RuleFor(x => x.Category.BaseCategoryId)
                .Must(BeValidBaseCategoryId).WithMessage("Base category ID must be a valid GUID or null.");
        }
        private bool BeValidBaseCategoryId(Guid? baseCategoryId)
        {
            return baseCategoryId == null || baseCategoryId != Guid.Empty;
        }
    }
    public class CreateCategoryCommandHandler(ICategoryService categoryService)
        : ICommandHandler<CreateCategoryCommand, CategoryResponse>
    {
        public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            return await categoryService.CreateCategoryAsync(request.Category);
        }
    }
}
