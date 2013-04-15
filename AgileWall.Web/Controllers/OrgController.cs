using System.Web.Mvc;
using AgileWall.Domain.Conract;
using AgileWall.Domain.Conract.RequestDto;
using AgileWall.Utils;
using AgileWall.Web.Properties;

namespace AgileWall.Web.Controllers
{
    public class OrgController : BaseController
    {
        private readonly IOrganizationService _organizationService;

        public OrgController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet, AllowAnonymous]
        public ViewResult New()
        {
            return View(new NewOrganizationRequestDto());
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public ActionResult New(NewOrganizationRequestDto model)
        {
            if (!string.IsNullOrEmpty(model.UserEmail)
                && !string.IsNullOrEmpty(model.OrganizationName)
                && !string.IsNullOrEmpty(model.OrganizationUrlName)
                && !string.IsNullOrEmpty(model.UserFirstName)
                && !string.IsNullOrEmpty(model.UserLastName)
                && !string.IsNullOrEmpty(model.UserPassword)
                && model.UserEmail.IsEmail())
            {
                var orgId = _organizationService.CreateOrganization(model);
                if (!string.IsNullOrEmpty(orgId))
                {
                    return Redirect("/org/wall");
                }
            }

            ViewBag.Msg = Texts.NewOrganizationMissingField;

            return View(model);
        }
    }
}