using System.Collections.Generic;

namespace E1Blurbs.Web.UI.Models
{
    public class BlurbModel
    {
        public int BlurbId { get; set; }

        public string Description { get; set; }

        public int AreaId { get; set; }

        public string productId { get; set; }

        public string[] Languages { get; set; }
    }
}