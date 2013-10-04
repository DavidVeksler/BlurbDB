using System.Linq;
using System.Web.Mvc;
using E1Blurbs.DataAccess;

namespace E1Blurbs.Web.UI.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            string test = "";

            var uow = new BlurbDbUnitofWork();

            test = uow.Blurbs.Count().ToString();

            return Content(test);
        }
    }
}