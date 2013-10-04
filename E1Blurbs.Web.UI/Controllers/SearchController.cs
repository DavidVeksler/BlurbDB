using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using E1Blurbs.DataAccess;
using E1Blurbs.Web.UI.Models;

namespace E1Blurbs.Web.UI.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        private readonly BlurbDbUnitofWork uow;

        public SearchController()
        {
            uow = new BlurbDbUnitofWork();
        }

        /// <summary>
        ///     Search Blurbs
        ///     http://localhost:13894/Search/SearchBlurbs?searchTerm=banana
        ///     Results are grouped by area name
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public JsonResult SearchBlurbs(string searchTerm, int productId)
        {
            //uow.BlurbRepository.FindBlurbs(searchTerm, productId);

            int blurbId;

            int.TryParse(searchTerm, NumberStyles.Integer, null, out blurbId);

            var blurbs =
                uow.BlurbRepository.Find(
                    f => (f.Description.Contains(searchTerm) || f.BlurbId == blurbId) && (productId == 0|| f.Category.ProductId == productId))
                    .OrderByDescending(b=> b.BlurbId)
                    .Take(100);

            var areasIds = blurbs.Select(s => s.CategoryId).Distinct();

            var areas =
                uow.AreaRepository.Find(f => areasIds.Contains(f.CategoryId))
                    .Select(s => new {s.CategoryId, s.CategoryName})
                    .ToLookup(s => s.CategoryId, s => s.CategoryName)
                    .ToList();

            //var results = new Dictionary<string, BlurbModel[]>();
            var results = new List<AreaSearchResult>();

            areas.ForEach(area =>
            {
                List<BlurbModel> blurbsForArea =
                    blurbs.Where(s => s.CategoryId == area.Key)
                        .Select(
                            s =>
                                new BlurbModel
                                {
                                    BlurbId = s.BlurbId,
                                    Description = s.Description,
                                    AreaId = (int) s.CategoryId
                                })
                        .ToList();
                //results.Add(area.First(), blurbsForArea.ToArray());

                results.Add(new AreaSearchResult {BlurbName = area.First(), Blurbs = blurbsForArea.ToArray()});
            });

            //var results = blurbs.Select(s => new { BlurbId = s.BlurbId, Description = s.Description, AreaId = s.CategoryId, AreaName = "TEST" });

            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public class AreaSearchResult
        {
            public string BlurbName;

            public BlurbModel[] Blurbs;
        }
    }
}