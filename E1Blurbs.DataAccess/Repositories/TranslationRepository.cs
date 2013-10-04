using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using E1Blurbs.Data;
using EntityFramework.Patterns;

namespace E1Blurbs.DataAccess.Repositories
{
    public class TranslationRepository : Repository<Translation>, IRepository<Translation>
    {
        public TranslationRepository(IObjectSetFactory objectSetFactory)
            : base(objectSetFactory)
        {
        }


        public List<Translation> GetTranslations(int blurbId)
        {
            return Find(s => s.BlurbId == blurbId).ToList();
        }

        public Translation Add(int translationId, int blurbId, string cultureCode, int productId, string content, int cultureId)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new Exception("Error: you must enter the translated text");
            }

            var translation = new Translation();
            {
                translation.TranslationId = translationId;
                translation.BlurbId = blurbId;
                translation.Text = content;
                translation.LanguageCode = cultureCode;
                translation.CultureId = cultureId;
                

                //translation.CultureCode = cultureCode;
                //translation.productId = productId;
                //translation.Text = content;

                //translation.Status_id = (int)BlurbStatus.Translated;
                //translation.WordCount = 0;

                translation.InsertDate = DateTime.Now;
             //   translation.SaveDate = DateTime.Now;
             //   translation.UpdateDate = DateTime.Now;
            }


            Insert(translation);

            return translation;
        }
    }
}
