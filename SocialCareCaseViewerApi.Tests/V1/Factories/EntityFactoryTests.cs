using System;
using System.Collections.Generic;
using System.Dynamic;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SocialCareCaseViewerApi.V1.Boundary.Requests;
using SocialCareCaseViewerApi.V1.Domain;
using SocialCareCaseViewerApi.V1.Factories;
using SocialCareCaseViewerApi.V1.Infrastructure;
using dbPhoneNumber = SocialCareCaseViewerApi.V1.Infrastructure.PhoneNumber;
using dbTeam = SocialCareCaseViewerApi.V1.Infrastructure.Team;
using dbWarningNote = SocialCareCaseViewerApi.V1.Infrastructure.WarningNote;
using dbWorker = SocialCareCaseViewerApi.V1.Infrastructure.Worker;
using PhoneNumber = SocialCareCaseViewerApi.V1.Domain.PhoneNumber;
using Team = SocialCareCaseViewerApi.V1.Domain.Team;
using WarningNote = SocialCareCaseViewerApi.V1.Domain.WarningNote;
using Worker = SocialCareCaseViewerApi.V1.Domain.Worker;
using dbAddress = SocialCareCaseViewerApi.V1.Infrastructure.Address;
using SocialCareCaseViewerApi.Tests.V1.Helpers;

namespace SocialCareCaseViewerApi.Tests.V1.Factories
{
    [TestFixture]
    public class EntityFactoryTests
    {
        private Faker _faker;

        [SetUp]
        public void SetUp()
        {
            _faker = new Faker();
        }

        #region ToDomain
        [Test]
        public void CanMapWorkerFromInfrastructureToDomainWithoutTeamDetails()
        {
            var email = _faker.Internet.Email();
            var firstName = _faker.Name.FirstName();
            var lastName = _faker.Name.LastName();
            var id = 1;
            var role = _faker.Random.Word();
            int allocationCount = 1;

            var dbWorker = new dbWorker()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Id = id,
                Role = role,
                Allocations = new List<AllocationSet>() {
                    new AllocationSet() { Id = 1, PersonId = 2, CaseStatus = "Open" },
                    new AllocationSet() { Id = 2, PersonId = 3, CaseStatus = "Closed" }
                }
            };

            var expectedResponse = new Worker()
            {
                FirstName = firstName,
                LastName = lastName,
                Id = id,
                AllocationCount = allocationCount,
                Email = email,
                Role = role,
                Teams = null
            };

            dbWorker.ToDomain(false).Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void CanMapWorkerFromInfrastructureToDomainWithTeamDetails()
        {
            var email = _faker.Internet.Email();
            var firstName = _faker.Name.FirstName();
            var lastName = _faker.Name.LastName();
            var id = 1;
            var role = _faker.Random.Word();
            int allocationCount = 1; //open allocations

            var dbWorker = new dbWorker()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Id = id,
                Role = role,
                Allocations = new List<AllocationSet>() {
                    new AllocationSet() { Id = 1, PersonId = 2, CaseStatus = "Closed" },
                    new AllocationSet() { Id = 2, PersonId = 3, CaseStatus = "Open" }
                },
                WorkerTeams = new List<WorkerTeam>()
                {
                    new WorkerTeam() { Id = 1 , Team = new dbTeam() { Id = 1, Name = "Team 1", Context = "C" } },
                    new WorkerTeam() { Id = 2 , Team = new dbTeam() { Id = 2, Name = "Team 2", Context = "C" } },
                }
            };

            var expectedResponse = new Worker()
            {
                FirstName = firstName,
                LastName = lastName,
                Id = id,
                AllocationCount = allocationCount,
                Email = email,
                Role = role,
                Teams = new List<Team>()
                {
                    new Team() { Id = 1, Name = "Team 1"},
                    new Team() { Id = 2, Name = "Team 2"}
                }
            };

            dbWorker.ToDomain(true).Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void CanMapTeamFromInfrastrcutureToDomain()
        {
            var id = _faker.Random.Number();
            var name = _faker.Name.ToString();
            var context = "a";

            var dbTeam = new dbTeam()
            {
                Id = id,
                Name = name,
                Context = context
            };

            var exptectedResponse = new Team()
            {
                Id = id,
                Name = name
            };

            dbTeam.ToDomain().Should().BeEquivalentTo(exptectedResponse);
        }

        [Test]
        public void CanMapWarningNoteFromDatabaseEntityToDomainObject()
        {
            long number = _faker.Random.Number();
            var dt = DateTime.Now;
            var text = _faker.Random.String();

            var dbWarningNote = new dbWarningNote
            {
                Id = number,
                PersonId = number,
                StartDate = dt,
                EndDate = dt,
                DisclosedWithIndividual = true,
                DisclosedDetails = text,
                Notes = text,
                ReviewDate = dt,
                NextReviewDate = dt,
                NoteType = text,
                Status = text,
                DisclosedDate = dt,
                DisclosedHow = text,
                WarningNarrative = text,
                ManagerName = text,
                DiscussedWithManagerDate = dt,
                CreatedBy = text
            };

            var expectedResponse = new WarningNote
            {
                Id = number,
                PersonId = number,
                StartDate = dt,
                EndDate = dt,
                DisclosedWithIndividual = true,
                DisclosedDetails = text,
                Notes = text,
                ReviewDate = dt,
                NextReviewDate = dt,
                NoteType = text,
                Status = text,
                DisclosedDate = dt,
                DisclosedHow = text,
                WarningNarrative = text,
                ManagerName = text,
                DiscussedWithManagerDate = dt,
                CreatedBy = text,
                WarningNoteReviews = new List<WarningNoteReview>()
            };

            var response = dbWarningNote.ToDomain();

            response.Should().BeOfType<WarningNote>();
            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void CanMapPostWarningNoteRequestToDatabaseObject()
        {
            long number = _faker.Random.Number();
            var dt = DateTime.Now;
            var text = _faker.Random.String();

            var request = new PostWarningNoteRequest
            {
                PersonId = number,
                StartDate = dt,
                EndDate = dt,
                DisclosedWithIndividual = true,
                DisclosedDetails = text,
                Notes = text,
                ReviewDate = dt,
                NextReviewDate = dt,
                NoteType = text,
                DisclosedDate = dt,
                DisclosedHow = text,
                WarningNarrative = text,
                ManagerName = text,
                DiscussedWithManagerDate = dt,
                CreatedBy = text
            };

            var expectedResponse = new dbWarningNote
            {
                PersonId = number,
                StartDate = dt,
                EndDate = dt,
                DisclosedWithIndividual = true,
                DisclosedDetails = text,
                Notes = text,
                ReviewDate = dt,
                NextReviewDate = dt,
                NoteType = text,
                Status = "open",
                DisclosedDate = dt,
                DisclosedHow = text,
                WarningNarrative = text,
                ManagerName = text,
                DiscussedWithManagerDate = dt,
                CreatedBy = text
            };

            var response = request.ToDatabaseEntity();

            response.Should().BeOfType<dbWarningNote>();
            response.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void CanMapPhoneNumberFromDatabaseEntityToDomainObject()
        {
            dbPhoneNumber phoneNumber = DatabaseGatewayHelper.CreatePhoneNumberEntity(_faker.Random.Long());

            var expectedResponse = new PhoneNumber()
            {
                Number = phoneNumber.Number,
                Type = phoneNumber.Type
            };

            phoneNumber.ToDomain().Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void CanMapPersonOtherNameFromDatabaseObjectToDomainObject()
        {
            PersonOtherName otherName = DatabaseGatewayHelper.CreatePersonOtherNameDatabaseEntity(_faker.Random.Long());

            var expectedResponse = new OtherName()
            {
                FirstName = otherName.FirstName,
                LastName = otherName.LastName
            };

            otherName.ToDomain().Should().BeEquivalentTo(expectedResponse);
        }

        #endregion
        #region ToEntity
        [Test]
        public void CanMapCreateAllocationRequestDomainObjectToDatabaseEntity()
        {
            var personId = _faker.Random.Long();
            var createdBy = _faker.Internet.Email();
            var workerId = _faker.Random.Number();
            var dt = DateTime.Now;
            var caseStatus = "Open";

            var allocationRequest = new CreateAllocationRequest()
            {
                MosaicId = personId,
                CreatedBy = createdBy,
                AllocatedWorkerId = workerId
            };

            var expectedResponse = new AllocationSet()
            {
                PersonId = personId,
                WorkerId = workerId,
                AllocationStartDate = dt,
                CaseStatus = caseStatus,
                CreatedBy = createdBy
            };

            allocationRequest.ToEntity(workerId, dt, caseStatus).Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public void CanMapOtherNameFromDomainToInfrastructure()
        {
            var firstName = _faker.Name.FirstName();
            var lastName = _faker.Name.LastName();
            var createdBy = _faker.Internet.Email();

            var domainOtherName = new OtherName()
            {
                FirstName = firstName,
                LastName = lastName
            };

            long personId = 123;

            var infrastructureOtherName = new PersonOtherName()
            {
                FirstName = firstName,
                LastName = lastName,
                PersonId = personId,
                CreatedBy = createdBy
            };

            domainOtherName.ToEntity(personId, createdBy).Should().BeEquivalentTo(infrastructureOtherName);
        }

        [Test]
        public void CanMapPhoneNumberFromDomainToInfrastructure()
        {
            string phoneNumber = "12345678";
            string phoneNumberType = "Mobile";
            long personId = 123;
            string createdBy = _faker.Internet.Email();

            var domainNumber = new PhoneNumber()
            {
                Number = phoneNumber,
                Type = phoneNumberType
            };

            var infrastructurePhoneNumber = new dbPhoneNumber()
            {
                Number = phoneNumber.ToString(),
                PersonId = personId,
                Type = phoneNumberType,
                CreatedBy = createdBy
            };

            domainNumber.ToEntity(personId, createdBy).Should().BeEquivalentTo(infrastructurePhoneNumber);
        }

        [Test]
        public void CanMapDbAddressToAddressDomain()
        {
            dbAddress address = DatabaseGatewayHelper.CreateAddressDatabaseEntity();

            AddressDomain expectedAddressDomain = new AddressDomain()
            {
                Address = address.AddressLines,
                Postcode = address.PostCode,
                Uprn = address.Uprn
            };

            EntityFactory.DbAddressToAddressDomain(address).Should().BeEquivalentTo(expectedAddressDomain);

        }

        #endregion
    }
}
