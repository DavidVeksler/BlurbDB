namespace E1Blurbs.Web.UI.Models
{
    public class TranslationModel
    {
        public int BlurbId { get; set; }

        public string CultureCode { get; set; }

        public string Content { get; set; }

        public int TranslationId { get; set; }

        public int productId { get; set; }
    }
}