using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Web.Mvc;
using E1Blurbs.DataAccess;

namespace E1Blurbs.Web.UI.Controllers
{
    public class SyncController : Controller
    {
        private readonly BlurbDbUnitofWork uow;

        public SyncController()
        {
            uow = new BlurbDbUnitofWork();
        }

        //
        // GET: /Sync/

        public ActionResult Index()
        {
            return View();
        }

        private SqlConnection GetSqlConnection()
        {
            var cn = uow.Database.Connection.ConnectionString;
            SqlConnection connection = new SqlConnection(cn);
            return connection;
        }

        [HttpPost]
        public ActionResult SyncBlurbsToDev()
        {
            RedirectDebug();

            Response.Write("Executing SyncBlurbsDevSSIS " + "<br>");
            Response.Flush();

            var connection = GetSqlConnection();

            using (connection)
            {
                connection.Open();

                var sqlCommand = new SqlCommand("SyncBlurbsDevSSIS", connection) {CommandTimeout = 999};

                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SqlString data = reader.GetSqlString(0);

                        if (!data.IsNull)
                        {
                            Response.Write(data + "<br>");
                            Response.Flush();
                        }
                    }
                }
            }
            return Content("OK");
        }


        [HttpPost]
        public ActionResult SyncBlurbsToLive()
        {
            RedirectDebug();

            Response.Write("Executing SyncBlurbsDevSSIS " + "<br>");
            Response.Flush();

            var connection = GetSqlConnection();

            using (connection)
            {
                connection.Open();

                var sqlCommand = new SqlCommand("SyncBlurbsLiveSSIS", connection) {CommandTimeout = 999};

                SqlDataReader reader = sqlCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Response.Write(reader.GetString(0) + "<br>");
                        Response.Flush();
                    }
                }
            }
            return Content("OK");
        }

        private void RedirectDebug()
        {
            var writer = new HttpResponseLogWriter(Response);
            Debug.Listeners.Add(new TextWriterTraceListener(writer));
        }
    }
}