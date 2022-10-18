namespace mchildr3_webapi_articlejet.DataTransfer
{
    public class UserDto
    {
        public string? Email { get; set; }
        public string? UserPassword { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool ValidUserDto { get; set; }
    }
}
