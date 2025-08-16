using Course.Application.Dtos.LectureDto;
using FluentValidation;

namespace Course.Application.Slices.Lectures.Queries.GetLectureById
{
    public record GetLectureByIdQuery(Guid Id) : IQuery<LectureResponse>;

    public class GetLectureByIdValidator : AbstractValidator<GetLectureByIdQuery>
    {
        public GetLectureByIdValidator()
        {
            RuleFor(q => q.Id)
                .NotEmpty()
                .WithMessage("Lecture Id is required");
        }
    }

    public class GetLectureByIdQueryHandler(ILectureService lectureService)
        : IQueryHandler<GetLectureByIdQuery, LectureResponse>
    {
        public async Task<LectureResponse> Handle(GetLectureByIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Lecture, bool>> filter = l => l.Id == request.Id;
            return await lectureService.GetLectureByIdAsync(filter);
        }
    }
}
