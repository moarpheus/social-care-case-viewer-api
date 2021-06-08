using System;
using SocialCareCaseViewerApi.V1.Boundary.Requests;
using SocialCareCaseViewerApi.V1.Boundary.Response;
using SocialCareCaseViewerApi.V1.Infrastructure;

namespace SocialCareCaseViewerApi.V1.UseCase.Interfaces
{
    public interface IFormSubmissionsUseCase
    {
        (CaseSubmissionResponse, CaseSubmission) ExecutePost(CreateCaseSubmissionRequest request);

        CaseSubmissionResponse ExecuteGetById(Guid submissionId);

        CaseSubmissionResponse UpdateAnswers(Guid submissionId, string stepId,
            UpdateFormSubmissionAnswersRequest request);
    }
}
