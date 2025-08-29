using BuildingBlocks.CQRS;
using Enrollement.API.Data;

namespace Enrollement.API.Enrollement.AddEnrollement
{

    public record class AddEnrollementCommand(Guid CourseId, Guid StudentId) : ICommand<Guid>;
    public class AddEnrollementCommandHandler(EnrollementDbContext db)
        : ICommandHandler<AddEnrollementCommand, Guid>
    {
        public async Task<Guid> Handle(AddEnrollementCommand request, CancellationToken cancellationToken)
        {
            var enrollement = new Models.Enrollement
            {
                Id = Guid.NewGuid(),
                CourseId = request.CourseId,
                StudentId = request.StudentId,
                EnrollementDate = DateTime.UtcNow
            };

            await db.Enrollements.AddAsync(enrollement);

            await db.SaveChangesAsync(cancellationToken);

            return request.CourseId;

        }
    }
}
