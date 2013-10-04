using System.Linq;
using System.Web.Mvc;
using E1Blurbs.DataAccess;

namespace E1Blurbs.Web.UI.Controllers
{
    public class LookupController : Controller
    {
        private readonly BlurbDbUnitofWork uow;

        public LookupController()
        {
            uow = new BlurbDbUnitofWork();
        }

        //  http://localhost:13894/lookup/Cultures/?productId=1

        public JsonResult Cultures(int productId = 4)
        {
            return
                Json(
                    uow.ProductCulture_lnk.Where(p => p.ProductId == productId)
                        .ToList()
                        .Select(s => new {Code = s.Culture.CultureCode, Name= s.Culture.Description}), JsonRequestBehavior.AllowGet);
        }
    }
}