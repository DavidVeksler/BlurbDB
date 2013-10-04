using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Services.Protocols;
using E1Blurbs.Data;
using E1Blurbs.DataAccess;
using E1Blurbs.Web.UI.Models;

namespace E1Blurbs.Web.UI.Controllers
{
    public class TranslationController : ApiController
    {
        private readonly BlurbDbUnitofWork uow;

        public TranslationController()
        {
            uow = new BlurbDbUnitofWork();
        }

        // GET api/<controller>
        public IEnumerable<TranslationModel> Get(int blurbId)
        {
            var translations = uow.TranslationRepository.Find(f => f.BlurbId == blurbId);

            return
                translations.Select(
                    s => new TranslationModel
                    {
                        BlurbId = s.BlurbId,
                        CultureCode = s.Culture.CultureCode,
                        Content = s.Text,
                        TranslationId = s.TranslationId
                    });
        }


        // POST api/<controller>
        public TranslationModel Post([FromBody] TranslationModel translationModel)
        {
            if (string.IsNullOrWhiteSpace(translationModel.CultureCode))
            {
                //throw new Exception("Language required");

                var culture =
                    uow.BlurbRepository.First(b => b.BlurbId == translationModel.BlurbId)
                        .Category.Product.ProductCulture_lnk.First(c => c.Culture.CultureCode != "en-US")
                        .Culture;

                translationModel.CultureCode = culture.CultureCode;

            }

            int cultureId = uow.Cultures.First(c => c.CultureCode == translationModel.CultureCode).CultureId;

            var translation = uow.TranslationRepository.Add(translationModel.TranslationId, translationModel.BlurbId,
                translationModel.CultureCode, translationModel.productId, translationModel.Content, cultureId);

            uow.SaveChanges();

            translationModel.TranslationId = translation.TranslationId;

            return translationModel;
        }


        // PUT api/<controller>/5
        public void Put(int id, [FromBody] TranslationModel translationModel)
        {
            if (string.IsNullOrWhiteSpace(translationModel.Content))
            {
                throw new Exception("Error: you must enter the translated text");
            }

            var Translation = uow.TranslationRepository.Single(s => s.TranslationId == id);
            Translation.Text = translationModel.Content;
            uow.SaveChanges();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            var Translation = uow.TranslationRepository.Single(s => s.TranslationId == id);

            uow.TranslationRepository.Delete(Translation);

            uow.SaveChanges();
        }
    }
}