using SocialCareCaseViewerApi.V1.Boundary.Requests;
using SocialCareCaseViewerApi.V1.Boundary.Response;

namespace SocialCareCaseViewerApi.V1.UseCase.Interfaces
{
    public interface IWarningNoteUseCase
    {
        PostWarningNoteResponse ExecutePost(PostWarningNoteRequest request);
        ListWarningNotesResponse ExecuteGet(long personId);
    }
}
