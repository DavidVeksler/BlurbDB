using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Text;
using E1Areas.DataAccess.Repositories;
using E1Blurbs.Data;
using E1Blurbs.DataAccess.Repositories;
using EntityFramework.Patterns;

namespace E1Blurbs.DataAccess
{
    public class BlurbDbUnitofWork : BlurbDBEntities
    {
        public BlurbDBEntities context { get; set; }

        public BlurbRepository BlurbRepository { get; set; }
        public Repository<Product> ProductRepository { get; set; }
        public AreaRepository AreaRepository { get; set; }
        public TranslationRepository TranslationRepository { get; set; }
        public Repository<Culture> Cultures { get; set; }

        private readonly DbContextAdapter adp;

        public BlurbDbUnitofWork()
        {
            Database.SetInitializer(new System.Data.Entity.CreateDatabaseIfNotExists<DbContext>());

            context = new BlurbDBEntities();

            adp = new DbContextAdapter(context);

            BlurbRepository = new BlurbRepository(adp);
            ProductRepository = new Repository<Product>(adp);
            AreaRepository = new AreaRepository(adp);
            TranslationRepository = new TranslationRepository(adp);
            Cultures = new Repository<Culture>(adp);
        }



        public override int SaveChanges()
        {
            int changes = 0;
            changes += adp.Context.SaveChanges();
            return changes + base.SaveChanges();
        }

        public int GetMaxId(Type type)
        {
            int maxId;

            switch (type.Name)
            {
                case "Category":
                    maxId = context.Categories.OrderByDescending(f => f.CategoryId).FirstOrDefault().CategoryId + 1;
                    break;
                case "Blurb":
                    maxId = context.Blurbs.OrderByDescending(f => f.BlurbId).FirstOrDefault().BlurbId + 1;
                    break;
                case "Translation":
                    maxId = context.Translations.OrderByDescending(f => f.TranslationId).FirstOrDefault().TranslationId + 1; ;
                    break;
                default:
                    throw new Exception("Unsupported type");

            }
            return maxId;
        }
    }
}
