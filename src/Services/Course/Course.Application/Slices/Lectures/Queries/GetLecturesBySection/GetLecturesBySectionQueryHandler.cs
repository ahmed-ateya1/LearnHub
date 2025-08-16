using Course.Application.Dtos.LectureDto;
using Course.Application.ServicContract;
using Course.Domain.Models;
using FluentValidation;
using System.Linq.Expressions;

namespace Course.Application.Slices.Lectures.Queries.GetLecturesBySection
{
    public record GetLecturesBySectionQuery(Guid SectionId) : IQuery<IEnumerable<LectureResponse>>;

    public class GetLecturesBySectionValidator : AbstractValidator<GetLecturesBySectionQuery>
    {
        public GetLecturesBySectionValidator()
        {
            RuleFor(q => q.SectionId)
                .NotEmpty()
                .WithMessage("Section Id is required");
        }
    }

    public class GetLecturesBySectionQueryHandler(ILectureService lectureService)
        : IQueryHandler<GetLecturesBySectionQuery, IEnumerable<LectureResponse>>
    {
        public async Task<IEnumerable<LectureResponse>> Handle(GetLecturesBySectionQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Lecture, bool>> filter = l => l.SectionId == request.SectionId;
            return await lectureService.GetLecturesByAsync(filter);
        }
    }
}