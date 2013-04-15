namespace AgileWall.Test
{
    using Domain.Conract;
    using Domain.Conract.RequestDto;
    using Domain.Entity;
    using Domain.Repo;

    public class BaseTests
    {
        protected const string dbName = "AgileWallTestDB";

        protected IBaseRepo<User> _userRepo;
        protected IBaseRepo<Organization> _orgRepo;

        protected IOrganizationService _organizationService;

        protected void ClearCollections()
        {
            _userRepo.DeleteAll();
            _orgRepo.DeleteAll();
        }

        protected NewOrganizationRequestDto DtoNewOrganizationRequestDto
        {
            get
            {
                return new NewOrganizationRequestDto
                {
                    OrganizationName = "Collade",
                    OrganizationUrlName = "collade",
                    UserEmail = "hserdarb@gmail.com",
                    UserFirstName = "Serdar",
                    UserLastName = "Büyüktemiz",
                    UserPassword = "password"
                };
            }
        }
    }
}
