using Course.Application.Dtos.LectureDto;
using FluentValidation;

namespace Course.Application.Slices.Lectures.Command.UpdateLecture
{
    public record UpdateLectureCommand(LectureUpdateRequest Request) : ICommand<LectureResponse>;

    public class UpdateLectureValidator : AbstractValidator<UpdateLectureCommand>
    {
        public UpdateLectureValidator()
        {
            RuleFor(l => l.Request.Id)
                .NotEmpty()
                .WithMessage("Lecture Id is required");

            RuleFor(l => l.Request.Title)
                .NotEmpty()
                .WithMessage("Title is required!");

            RuleFor(l => l.Request.SectionId)
                .NotEmpty()
                .WithMessage("Section is required");
        }
    }

    public class UpdateLectureCommandHandler(ILectureService lectureService)
        : ICommandHandler<UpdateLectureCommand, LectureResponse>
    {
        public async Task<LectureResponse> Handle(UpdateLectureCommand request, CancellationToken cancellationToken)
        {
            return await lectureService.UpdateLectureAsync(request.Request);
        }
    }
}
