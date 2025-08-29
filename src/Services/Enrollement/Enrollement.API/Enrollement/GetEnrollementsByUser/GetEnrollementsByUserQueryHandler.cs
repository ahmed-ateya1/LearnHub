using BuildingBlocks.CQRS;
using Enrollement.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Enrollement.API.Enrollement.GetEnrollementsByUser
{
    public record class GetEnrollementsByUserQuery(Guid UserId) : IQuery<IEnumerable<Models.Enrollement>>;
    public class GetEnrollementsByUserQueryHandler(EnrollementDbContext db)
        : IQueryHandler<GetEnrollementsByUserQuery, IEnumerable<Models.Enrollement>>
    {
        public async Task<IEnumerable<Models.Enrollement>> Handle(GetEnrollementsByUserQuery request, CancellationToken cancellationToken)
        {
            var enrollements = await db.Enrollements
                .Where(e => e.StudentId == request.UserId)
                .ToListAsync();

            if (enrollements == null || !enrollements.Any())
            {
                return Enumerable.Empty<Models.Enrollement>();
            }

            return enrollements;

        }
    }
}
