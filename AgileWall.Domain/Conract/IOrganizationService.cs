using AgileWall.Domain.Conract.RequestDto;
using AgileWall.Domain.Entity;

namespace AgileWall.Domain.Conract
{
    public interface IOrganizationService
    {
        string CreateOrganization(NewOrganizationRequestDto dto);
        Organization GetOrganizationByIdOrUrlName(ItemRequestDto dto);
    }
}


