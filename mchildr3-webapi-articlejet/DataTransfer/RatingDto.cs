namespace mchildr3_webapi_articlejet.DataTransfer
{
    public class RatingDto
    {
        /// <summary>
        /// Class Properties, entity to match database table Rating
        /// </summary>
        public int ArticleID { get; set; }
        public string? UserID { get; set; }
        public bool ValidRating { get; internal set; }
        public double UserRating { get; internal set; }

        /// <summary>
        /// Output properties for testing, mostly
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return "Article ID: " + ArticleID + " User ID: " + UserID;
        }
    }
}
