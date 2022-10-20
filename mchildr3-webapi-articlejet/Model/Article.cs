namespace mchildr3_webapi_articlejet.Model
{
    public class Article
    {
        public int ArticleID { get; set; }
        public string? Title { get; set; }
        public DateTime PostDate { get; set; }
        public string? Summary { get; set; }
        public string? Link { get; set; }
        public string? OwnerGuid { get; set; }

    }
}
