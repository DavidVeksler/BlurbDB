using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using E1Blurbs.Data;
using E1Blurbs.DataAccess;
using E1Blurbs.Web.UI.Models;

namespace E1Blurbs.Web.UI.Controllers
{
    public class AreaController : ApiController
    {
        private readonly BlurbDbUnitofWork uow;

        public AreaController()
        {
            uow = new BlurbDbUnitofWork();
        }

        // GET api/<controller>
        public IEnumerable<AreaModel> Get(int productId, int ParentCategoryId = 0)
        {

            var areasForProduct =
                uow.AreaRepository.Find(f => f.ProductId == productId);

            if (ParentCategoryId == 0)
            {
                areasForProduct = areasForProduct.Where(f => f.ParentCategoryId == 0 || f.ParentCategoryId == null);
            }
            else
            {
                areasForProduct = areasForProduct.Where(f => f.ParentCategoryId == ParentCategoryId);

            }
            

            return
                areasForProduct.Select(s => new AreaModel
                {
                    AreaId = s.CategoryId,
                    Name = s.CategoryName,
                    ParentCategoryId = s.ParentCategoryId.GetValueOrDefault(),
                    //productId = s.productId
                });
        }


        // POST api/<controller>
        public AreaModel Post([FromBody] AreaModel areaModel)
        {
            // todo: fix new area creation: parent area id
            //areaModel.AreaId = uow.GetMaxId(typeof (Category));

            uow.AreaRepository.Add(areaModel.AreaId, areaModel.Name, areaModel.ParentCategoryId, areaModel.productId);
            uow.SaveChanges();

            return areaModel;
        }


        // PUT api/<controller>/5
        public void Put(int id, [FromBody] AreaModel areaModel)
        {
            var area = uow.AreaRepository.Single(s => s.CategoryId == id);
            area.CategoryName = areaModel.Name;
            area.ParentCategoryId = areaModel.ParentCategoryId;

            uow.SaveChanges();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            var area = uow.AreaRepository.Single(s => s.CategoryId == id);

            uow.AreaRepository.Delete(area);

            uow.SaveChanges();
        }
    }
}