namespace AgileWall.Web.Controllers
{
    using System.Web.Mvc;

    public class BaseController : Controller
    {

        public ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}