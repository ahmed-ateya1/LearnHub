using Course.Application.Dtos.LectureDto;
using FluentValidation;

namespace Course.Application.Slices.Lectures.Command.AddLecture
{
    public record AddLectureCommand(LectureAddRequest Request):ICommand<LectureResponse>;
    public class AddLectureValidator: AbstractValidator<AddLectureCommand>
    {
        public AddLectureValidator()
        {
            RuleFor(l => l.Request.Title)
                .NotEmpty()
                .WithMessage("Title is Required!");
            RuleFor(l => l.Request.Video)
                .NotNull()
                .WithMessage("must upload video");

            RuleFor(l => l.Request.SectionId)
                .NotEmpty()
                .WithMessage("Section is required");
        }
    }
    public class AddLectureCommandHandler(ILectureService lectureService) 
        : ICommandHandler<AddLectureCommand, LectureResponse>
    {
        public async Task<LectureResponse> Handle(AddLectureCommand request, CancellationToken cancellationToken)
        {
            return await lectureService.AddLectureAsync(request.Request);
        }
    }
}
