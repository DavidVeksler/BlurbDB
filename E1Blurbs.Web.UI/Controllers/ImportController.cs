using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E1Blurbs.DataAccess;
using E1Blurbs.Domain.ImportExport;

namespace E1Blurbs.Web.UI.Controllers
{
    public class ImportController : Controller
    {
        private readonly BlurbDbUnitofWork uow;

        public ImportController()
        {
            uow = new BlurbDbUnitofWork();
        }

        //
        // GET: /Import/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExportBlubs(int ParentCategoryId = 300003)
        {
            BlurbExporter exporter = new BlurbExporter(uow);

            string description = "";

            var package = exporter.GenerateExcel(ParentCategoryId, out description);

            //Write it back to the client
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            Response.AddHeader("content-disposition",
                String.Format("attachment;  filename=BlurbExport_{0}.xlsx", description));
            Response.BinaryWrite(package.GetAsByteArray());

            return Content(null);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ImportBlubs(HttpPostedFileBase file, int ParentCategoryId)
        {
            try
            {
                BlurbImporter importer = new BlurbImporter(uow);

                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    string status = importer.ProcessImportedBlurbs(file.InputStream, ParentCategoryId);

                    return Json(status);
                }
                return Json("Error: no file provided");
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return Json(ex.ToString());
            }
        }
    }
}