using System.Linq;
using System.Web.Mvc;
using E1Blurbs.DataAccess;
using EntityFramework.Patterns;

namespace E1Blurbs.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

         private readonly BlurbDbUnitofWork uow;

         public HomeController()
        {
            uow = new BlurbDbUnitofWork();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sync()
        {
            return View("SyncView");
        }

        [OutputCache(Duration = 6000, VaryByParam = "none")]
        public ActionResult Summary()
        {
            int totalBlurbs = uow.context.Blurbs.Count();
            int totalTranslations = uow.context.Translations.Count();
            int products = uow.context.Products.Count();
            int categories = uow.context.Categories.Count();

            return Content(string.Format("{0} blurbs, {1} translations for {3} categories in {2} products.",totalBlurbs, totalTranslations, products, categories));
        }
    }
}