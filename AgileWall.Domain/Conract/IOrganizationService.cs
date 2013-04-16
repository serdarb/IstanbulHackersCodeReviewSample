using AgileWall.Domain.Conract.RequestDto;
using AgileWall.Domain.Entity;

namespace AgileWall.Domain.Conract
{
    public interface IOrganizationService
    {
        string CreateOrganization(NewOrganizationRequestDto dto);
        string CreateOrganizationWithUser(NewOrganizationRequestDto dto);

        Organization GetOrganizationById(string id);
        Organization GetOrganizationBySlug(string slug);
    }
}


