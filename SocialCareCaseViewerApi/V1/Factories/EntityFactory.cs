using SocialCareCaseViewerApi.V1.Domain;
using SocialCareCaseViewerApi.V1.Infrastructure;

namespace SocialCareCaseViewerApi.V1.Factories
{
    public static class EntityFactory
    {
        public static Entity ToDomain(this Person databaseEntity)
        {
            //TODO: Map the rest of the fields in the domain object.
            // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings

            return new Entity
            {
                Id = databaseEntity.Id
                //CreatedAt = databaseEntity.CreatedAt,
            };
        }
    }
}
