using System.Collections.Generic;
using System.Linq;
using SocialCareCaseViewerApi.V1.Boundary.Requests;
using SocialCareCaseViewerApi.V1.Boundary.Response;
using SocialCareCaseViewerApi.V1.Factories;
using SocialCareCaseViewerApi.V1.Gateways;
using SocialCareCaseViewerApi.V1.Infrastructure;
using SocialCareCaseViewerApi.V1.UseCase.Interfaces;

namespace SocialCareCaseViewerApi.V1.UseCase
{
    public class TeamsUseCase : ITeamsUseCase
    {
        private readonly IDatabaseGateway _databaseGateway;

        public TeamsUseCase(IDatabaseGateway databaseGateway)
        {
            _databaseGateway = databaseGateway;
        }

        public ListTeamsResponse ExecuteGet(GetTeamsRequest request)
        {
            if (request.Id != null)
            {
                var teamFoundWithId = _databaseGateway.GetTeamByTeamId(request.Id ?? 0);

                if (teamFoundWithId == null)
                {
                    return new ListTeamsResponse() {Teams = new List<TeamResponse>()};
                }

                var teams = new List<Team> {teamFoundWithId};
                return new ListTeamsResponse() { Teams = teams.Select(team => team.ToDomain().ToResponse()).ToList()};
            }

            if (request.Name != null)
            {
                var teamFoundWithName = _databaseGateway.GetTeamByTeamName(request.Name);

                if (teamFoundWithName == null)
                {
                    return new ListTeamsResponse() {Teams = new List<TeamResponse>()};
                }

                var teams = new List<Team> {teamFoundWithName};
                return new ListTeamsResponse() { Teams = teams.Select(team => team.ToDomain().ToResponse()).ToList()};
            }

            if (request.ContextFlag != null)
            {
                var teams = _databaseGateway.GetTeamsByTeamContextFlag(request.ContextFlag);
                return new ListTeamsResponse() { Teams = teams.Select(team => team.ToDomain().ToResponse()).ToList()};
            }

            return new ListTeamsResponse() {Teams = new List<TeamResponse>()};
        }

        public TeamResponse ExecutePost(CreateTeamRequest request)
        {

            // todo check if team name already exists, if so throw an error

            var team = _databaseGateway.CreateTeam(request);

            return team.ToDomain().ToResponse();
        }
    }
}
