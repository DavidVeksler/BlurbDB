using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using E1Blurbs.Data;
using EntityFramework.Patterns;

namespace E1Blurbs.DataAccess.Repositories
{
    public class BlurbRepository : Repository<Blurb>
    {
        public BlurbRepository(IObjectSetFactory objectSetFactory) : base(objectSetFactory)
        {
        }
        
        public Blurb Add(int blurbId, int areaId, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new Exception("Description required");
            }

            var duplicate = Find(f => f.CategoryId == areaId && f.Description == description).ToList();

            if (duplicate.Any())
            {
                throw new Exception("This is a duplicate of blurb #" + duplicate.First().BlurbId);
            }

            Blurb blurb = new Blurb()
            {
                CategoryId = areaId,
                Description = description,
                DefaultText = description, // TODO
                BlurbId = blurbId,
                //ProductId = productId,
                //Task_id = 0,
                //WordCount = description.Split(Convert.ToChar(" ")).Length,
                //Status_id = (int)BlurbStatus.Translated,
                //Instructions = "",
                InsertDate = DateTime.Now,
                //SaveDate = DateTime.Now,
                //UpdateDate = DateTime.Now
            };
            
            Insert(blurb);

            return blurb;
        }

        public void Add(int blurbId, int areaId, string description, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
