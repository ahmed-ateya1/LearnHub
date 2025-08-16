namespace Course.Application.Slices.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : ICommand<bool>;
    public class DeleteCategoryCommandHandler(ICategoryService categoryService)
        : ICommandHandler<DeleteCategoryCommand, bool>
    {
        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            return await categoryService.DeleteCategoryAsync(request.Id);
        }
    }
}
