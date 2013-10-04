using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using E1Blurbs.Data;
using E1Blurbs.DataAccess;
using E1Blurbs.Web.UI.Models;
using WebGrease.Css.Extensions;

namespace E1Blurbs.Web.UI.Controllers
{
    public class BlurbsController : ApiController
    {
        private readonly BlurbDbUnitofWork uow;

        public BlurbsController()
        {
            uow = new BlurbDbUnitofWork();
        }


        // GET api/<controller>
        public IEnumerable<BlurbModel> Get(string productId, int areaId)
        {
            var blurbs = uow.BlurbRepository.Find(f => f.CategoryId == areaId);

            var blurbModels =
                blurbs.Select(
                    s => new BlurbModel
                    {
                        BlurbId = s.BlurbId,
                        Description = s.Description,
                        AreaId = s.CategoryId,
                        Languages = s.Translations.Select(c => c.Culture.CultureCode).ToArray()
                    }).OrderBy(b=> b.BlurbId);

            return blurbModels;
        }

        public BlurbModel Get(int id)
        {
            var blurbs = uow.BlurbRepository.Find(f => f.BlurbId == id);

            return
                blurbs.Select(
                    s => new BlurbModel
                    {
                        BlurbId = s.BlurbId,
                        Description = s.Description,
                        AreaId = s.CategoryId,
                        productId = s.Category.ProductId.ToString()
                    }).FirstOrDefault();
        }


        // POST api/<controller>
        public BlurbModel Post([FromBody] BlurbModel blurbModel)
        {
            //blurbModel.BlurbId = uow.GetMaxId(typeof(Blurb));

            var blurb = uow.BlurbRepository.Add(blurbModel.BlurbId, blurbModel.AreaId, blurbModel.Description);

            uow.SaveChanges();

            blurbModel.BlurbId = blurb.BlurbId;

            return blurbModel;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] BlurbModel blurbModel)
        {
            var Blurb = uow.BlurbRepository.Single(s => s.BlurbId == id);
            Blurb.Description = blurbModel.Description;
            Blurb.DefaultText = blurbModel.Description;
            Blurb.CategoryId = blurbModel.AreaId;

            uow.SaveChanges();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            var blurb = uow.BlurbRepository.Single(b => b.BlurbId == id);
            var trans = uow.TranslationRepository.Find(t => t.BlurbId == id);
            foreach (var t in trans)
            {
                uow.TranslationRepository.Delete(t);
            }
            uow.BlurbRepository.Delete(blurb);

            uow.SaveChanges();
        }
    }
}