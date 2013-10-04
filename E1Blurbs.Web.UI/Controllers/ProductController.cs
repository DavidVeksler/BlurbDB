using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using E1Blurbs.DataAccess;
using E1Blurbs.Web.UI.Models;

namespace E1Blurbs.Web.UI.Controllers
{
//    http://localhost:13894/api/Product/
//http://localhost:13894/api/Area/?productId=1
//http://localhost:13894/api/Blurbs/?productId=2&areaId=300030
//http://localhost:13894/api/Translation/?blurbId=300001

    public class ProductController : ApiController
    {
        private readonly BlurbDbUnitofWork uow;

        public ProductController()
        {
            uow = new BlurbDbUnitofWork();
        }

        // GET api/<controller>
        public IEnumerable<ProductModel> Get()
        {
            var products = uow.ProductRepository.GetAll();

            return
                products.Select(
                    s => new ProductModel {Name = s.Name, Code = s.ProductId.ToString()});
        }


        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}