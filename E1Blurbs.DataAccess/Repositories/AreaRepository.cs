using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using E1Blurbs.Data;
using EntityFramework.Patterns;

namespace E1Areas.DataAccess.Repositories
{
    public class AreaRepository : Repository<Category>
    {
        public AreaRepository(IObjectSetFactory objectSetFactory)
            : base(objectSetFactory)
        {
        }


        //public List<Translation> GetTranslations(int AreaId)
        //{
        //    //var translationRepo = new Repository<Translation>(adp);


        //}
        public void Add(int areaId, string name, int parentCategoryId, int productId)
        {

            var area = new Category
                {
                    CategoryId = areaId,
                    CategoryName = name,
                    ParentCategoryId = parentCategoryId,
                    ProductId = productId,

                    InsertDate = DateTime.Now,
                    SaveDate = DateTime.Now,
                    UpdateDate = DateTime.Now,

                    // legacy values:

                    //ValidFromDate = DateTime.Now,
                    //ValidToDate = DateTime.Now.AddYears(100),
                    //DateRangeType = "",
                    //Desc = "",
                    //Code = areaId.ToString(),
                    //AreaProductMapping_id = areaId,
                };

            Insert(area);

        }

       
    }
}
