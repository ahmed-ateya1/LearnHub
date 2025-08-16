using Course.Application.Dtos.LectureDto;

namespace Course.Application.Slices.Lectures.Queries.GetLectures
{
    public record GetLecturesQuery() : IQuery<IEnumerable<LectureResponse>>;

    public class GetLecturesQueryHandler(ILectureService lectureService)
        : IQueryHandler<GetLecturesQuery, IEnumerable<LectureResponse>>
    {
        public async Task<IEnumerable<LectureResponse>> Handle(GetLecturesQuery request, CancellationToken cancellationToken)
        {
            return await lectureService.GetLecturesByAsync();
        }
    }
}