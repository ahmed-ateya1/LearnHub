using BuildingBlocks.CQRS;
using Enrollement.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Enrollement.API.Enrollement.DeleteEnrollement
{
    public record DeleteEnrollementCommand(Guid EnrollementId) : ICommand<bool>;
    public class DeleteEnrollementCommandHandler(EnrollementDbContext db)
        : ICommandHandler<DeleteEnrollementCommand, bool>
    {
        public async Task<bool> Handle(DeleteEnrollementCommand request, CancellationToken cancellationToken)
        {
            var enrollement = await db.Enrollements
                .FirstOrDefaultAsync(e => e.Id == request.EnrollementId);
            if (enrollement == null)
            {
                return false;
            }
            db.Enrollements.Remove(enrollement);
            await db.SaveChangesAsync();
            return true;
        }
    }
}
