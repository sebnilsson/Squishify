using System.Web.Mvc;

namespace Squishify.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToActionPermanent("Js");
        }

        public ActionResult Js()
        {
            return View();
        }

        public ActionResult Css()
        {
            return View();
        }
    }
}