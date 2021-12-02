using System;
using System.Collections.Generic;
using Bogus;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using SocialCareCaseViewerApi.Tests.V1.Helpers;
using SocialCareCaseViewerApi.V1.Factories;
using SocialCareCaseViewerApi.V1.Gateways;
using SocialCareCaseViewerApi.V1.Gateways.Interfaces;
using SocialCareCaseViewerApi.V1.Helpers;
using SocialCareCaseViewerApi.V1.Infrastructure;


#nullable enable
namespace SocialCareCaseViewerApi.Tests.V1.Gateways.MashReferralGatewayTests
{
    [TestFixture]
    public class GetReferralUsingIdTests : DatabaseTests
    {
        private Mock<IMongoGateway> _mongoGateway = null!;
        private Mock<ISystemTime> _systemTime = null!;
        private IMashReferralGateway _mashReferralGateway = null!;
        private const string CollectionName = "mash-referrals";

        private readonly Faker _faker = new Faker();

        [SetUp]
        public void Setup()
        {
            _mongoGateway = new Mock<IMongoGateway>();
            _systemTime = new Mock<ISystemTime>();
            _mashReferralGateway = new MashReferralGateway(_mongoGateway.Object, _systemTime.Object, DatabaseContext);
        }

        [Test]
        public void GetReferralUsingIdReturnsDomainMashReferral()
        {
            var referral = new MashReferral
            {
                Id = ObjectId.Parse(_faker.Random.String2(24, "0123456789abcdef")),
                Referrer = "Police - red",
                CreatedAt = DateTime.Now.AddHours(-3),
                RequestedSupport = "Safeguarding",
                Clients = new List<string> { "Sally Samuels" },
                Stage = "Contact",
                ReferralDocumentURI = "hardcoded-referral-1-URI"
            };

            _mongoGateway
                .Setup(x => x.LoadRecordById<MashReferral>(CollectionName, referral.Id))
                .Returns(referral);

            var response = _mashReferralGateway.GetReferralUsingId(referral.Id.ToString());

            response.Should().BeEquivalentTo(referral.ToDomain());
        }

        [Test]
        public void GetReferralFromPostgresUsingIdReturnsDomainMashReferral()
        {
            const long referralId = 1L;
            var referral = MashReferralHelper.SaveMashReferralToDatabase(DatabaseContext, mashReferralId: referralId);

            for (var i = 0; i < 8; i++)
            {
                MashResidentHelper.SaveMashResidentToDatabase(DatabaseContext, referralId);
            }

            var response = _mashReferralGateway.GetReferralUsingId_2(referral.Id);

            response.Should().BeEquivalentTo(referral.ToDomain(),
            options =>
                {
                    options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000)).WhenTypeIs<DateTime>();
                    return options;
                });
        }

        [Test]
        public void GetReferralUsingIdReturnsNullIfNoMashReferralFound()
        {
            var nonExistentId = _faker.Random.String2(24, "0123456789abcdef");

            _mongoGateway.Setup(x => x.LoadRecordById<MashReferral>(CollectionName, ObjectId.Parse(nonExistentId)));

            var response = _mashReferralGateway.GetReferralUsingId(nonExistentId);

            response.Should().BeNull();
        }

        [Test]
        public void GetReferralFromPostgresUsingIdReturnsNullIfNoMashReferralFound()
        {
            const long nonExistentId = 123L;

            var response = _mashReferralGateway.GetReferralUsingId_2(nonExistentId);

            response.Should().BeNull();
        }
    }
}
