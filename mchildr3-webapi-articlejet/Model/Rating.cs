namespace mchildr3_webapi_articlejet.Model
{
    public class Rating
    {
        public int ArticleID { get; set; }
        public string? UserID { get; set; }
        public float UserRating { get; set; }
        public bool ValidRating { get; set; }

    }
}
