namespace AgileWall.Domain.Conract.RequestDto
{
    public class NewOrganizationRequestDto
    {
        public string OrganizationName { get; set; }
        public string OrganizationUrlName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }
}