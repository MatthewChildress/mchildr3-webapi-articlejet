/// File User.cs
/// Name: Matthew Childress
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Project 2
namespace mchildr3_webapi_articlejet.Model
{
    /// <summary>
    /// This class matches the database table User for CRUD operations
    /// </summary>
    public class User
    {
        public string? Guid { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Boolean IsActive { get; set; }
        public int LevelID { get; set; }
    }
}
