using System.Linq;
using AgileWall.Domain.Conract.RequestDto;
using AgileWall.Domain.Entity;
using AgileWall.Domain.Repo;
using AgileWall.Domain.Service;
using AgileWall.Utils;
using NUnit.Framework;

namespace AgileWall.Test
{
    [TestFixture]
    public class OrganizationServiceTests : BaseTests
    {
        [SetUp]
        public void Setup()
        {
            _userRepo = new BaseRepo<User>(dbName);
            _orgRepo = new BaseRepo<Organization>(dbName);

            _organizationService = new OrganizationService(_orgRepo, _userRepo);
        }

        [Test]
        public void can_create_organization_and_return_id()
        {
            ClearCollections();

            var orgId = _organizationService.CreateOrganization(DtoNewOrganizationRequestDto);

            Assert.IsNotNullOrEmpty(orgId);

            var org = _organizationService.GetOrganizationByIdOrUrlName(new ItemRequestDto { Text = orgId });

            Assert.IsNotNull(org);
            Assert.AreEqual(org.Name, DtoNewOrganizationRequestDto.OrganizationName);
        }

        [Test]
        public void when_can_not_create_return_null()
        {
            ClearCollections();

            var orgId = _organizationService.CreateOrganization(DtoNewOrganizationRequestDto);

            Assert.IsNotNullOrEmpty(orgId);

            var org = _organizationService.GetOrganizationByIdOrUrlName(new ItemRequestDto { Text = orgId });

            Assert.IsNotNull(org);
            Assert.AreEqual(org.Name, DtoNewOrganizationRequestDto.OrganizationName);

            orgId = _organizationService.CreateOrganization(DtoNewOrganizationRequestDto);
            Assert.IsNull(orgId);
        }

        [Test]
        public void when_creating_some_values_are_critical_and_must_be_setted()
        {
            ClearCollections();

            var dto = DtoNewOrganizationRequestDto;

            var orgId = _organizationService.CreateOrganization(dto);
            Assert.IsNotNullOrEmpty(orgId);

            var org = _organizationService.GetOrganizationByIdOrUrlName(new ItemRequestDto { Text = orgId });
            Assert.IsNotNull(org);

            Assert.AreEqual(org.Name, dto.OrganizationName);
            Assert.AreEqual(org.NameLowered, dto.OrganizationName.ToLowerInvariant());
            Assert.IsNotNullOrEmpty(dto.OrganizationUrlName);

            var user = _userRepo.AsQueryable().FirstOrDefault(x => x.Email == dto.UserEmail);
            Assert.IsNotNull(user);

            Assert.IsTrue(BCrypt.Net.BCrypt.Verify(dto.UserPassword, user.PasswordHash));

            Assert.AreEqual(user.OrganizationId, orgId);
            Assert.AreEqual(user.FirstName, dto.UserFirstName);
            Assert.AreEqual(user.LastName, dto.UserLastName);
            Assert.AreEqual(user.Name, string.Format("{0} {1}", dto.UserFirstName, dto.UserLastName));
            Assert.AreEqual(user.NameLowered, string.Format("{0} {1}", dto.UserFirstName, dto.UserLastName).ToLowerInvariant());
            Assert.AreEqual(user.Initial, string.Format("{0}{1}", dto.UserFirstName[0], dto.UserLastName[0]));

            Assert.AreEqual(org.UserId, user.IdStr);
            Assert.AreEqual(org.Groups, Consts.DefaultGroups);

            Assert.True(org.Users.Any(x => x.UserId == user.IdStr));
            Assert.Contains(string.Format(Consts.OrgRoleStringFormat, org.IdStr), user.Roles);
            Assert.Contains(string.Format(Consts.OrgAdminRoleStringFormat, org.IdStr), user.Roles);

            CollectionAssert.AllItemsAreUnique(org.Groups);
        }

        [Test]
        public void can_get_organization_by_id_and_by_urlname()
        {
            ClearCollections();

            var dto = DtoNewOrganizationRequestDto;

            var orgId = _organizationService.CreateOrganization(dto);

            Assert.IsNotNullOrEmpty(orgId);

            var org = _organizationService.GetOrganizationByIdOrUrlName(new ItemRequestDto { Text = orgId });

            Assert.IsNotNull(org);
            Assert.AreEqual(org.Name, dto.OrganizationName);

            org = _organizationService.GetOrganizationByIdOrUrlName(new ItemRequestDto { Text = org.NameUrl });

            Assert.IsNotNull(org);
            Assert.AreEqual(org.Name, dto.OrganizationName);
            Assert.AreEqual(orgId, org.IdStr);
        }
    }
}
