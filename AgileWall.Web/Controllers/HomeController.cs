using System.Web.Mvc;

namespace AgileWall.Web.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet, AllowAnonymous]
        public ViewResult Index()
        {
            return View();
        }
    }
}