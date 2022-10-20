using mchildr3_webapi_articlejet.DataTransfer;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

/// File: BusLogLayer.cs
/// Name: Dave Blair
/// Class: CITC 1317
/// Semester: Fall 2022
/// Project: Article Jet
namespace mchildr3_webapi_articlejet.BusinessLayer
{
    /// <summary>
    /// This class is a static helper class
    /// </summary>
    public class BusLogLayer
    {
        /// <summary>
        /// Validate a Rating object checking for something in each field
        /// and the rating is between 0.0 and 5.0
        /// </summary>
        /// <param name="rating"></param>
        /// <returns>Rating</returns>
        public static RatingDto ValidateRating(RatingDto rating)
        {
            rating.ValidRating = true;
            if (rating.ArticleID == 0)
            {
                rating.ValidRating = false;
            }

            if (rating.UserID.Length == 0)
            {
                rating.UserID = "User ID is not valid";
                rating.ValidRating = false;
            }

            if (rating.UserRating < 0.0 || rating.UserRating > 5.0)
            {
                rating.UserRating = 0.0F;
                rating.ValidRating = false;
            }

            return rating;
        }

        /// <summary>
        /// Validate an ArticleDto object and check fields to match database
        /// but I am not checking the date field for two reasons, one is that
        /// it can never be null and two, we might want to set the date to 
        /// a future date
        /// </summary>
        /// <param name="articleDto"></param>
        /// <returns>ArticleDto</returns>
        public static ArticleDto ValidateArticleDto(ArticleDto articleDto)
        {
            articleDto.ValidArticleDto = true;
            if (articleDto.Title.Length == 0 || articleDto.Title.Length > 256)
            {
                articleDto.Title = "Title must be between 1 and 256 characters.";
                articleDto.ValidArticleDto = false;
            }

            if (articleDto.Summary.Length == 0 || articleDto.Summary.Length > 65000)
            {
                articleDto.Summary = "Summary must be no longer than 65000 characters";
                articleDto.ValidArticleDto = false;
            }

            if (articleDto.Link.Length == 0 || articleDto.Link.Length > 256)
            {
                articleDto.Link = "Link must be no longer than 256 characters.";
                articleDto.ValidArticleDto = false;
            }

            return articleDto;
        }

        /// <summary>
        /// Check fields for length for 0 and a length
        /// Also check email and password
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>UserDto</returns>
        public static UserDto ValidateUserDto(UserDto userDto)
        {
            userDto.ValidUserDto = true;

            if (!IsValidEmail(userDto.Email) || userDto.Email.Length > 50 || userDto.Email.Length == 0)
            {
                userDto.Email = "Not a valid email.";
                userDto.ValidUserDto = false;
            }

            if (userDto.Email.Length > 50)
            {
                userDto.Email = "Email over 50 characters in length.";
                userDto.ValidUserDto = false;
            }

            if (userDto.FirstName.Length > 50 || userDto.FirstName.Length == 0)
            {
                userDto.FirstName = "First name can not be over 50 characters.";
                userDto.ValidUserDto = false;
            }

            if (userDto.LastName.Length > 50 || userDto.LastName.Length == 0)
            {
                userDto.LastName = "Last name can not be over 50 characters.";
                userDto.ValidUserDto = false;
            }

            if (!isValidPassword(userDto.UserPassword) || userDto.UserPassword.Length > 50 || userDto.UserPassword.Length == 0)
            {
                userDto.UserPassword = "Password must be at least 8 characters, contain a number, and at least one uppercase letter";
                userDto.ValidUserDto = false;
            }

            return userDto;
        }

        /// <summary>
        /// User regex to validate email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>bool</returns>
        private static bool IsValidEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|edu)$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Validate a password, must have 8 chars, an uppercase, and a number
        /// </summary>
        /// <param name="password"></param>
        /// <returns>bool</returns>
        private static bool isValidPassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+"); // Must match any number 1 through 9, + means 1 or more times
            var hasUpperChar = new Regex(@"[A-Z]+"); // Must match any UPPERCASE letter from A through Z, + means 1 or more times
            var hasMinimum8Chars = new Regex(@".{8,}"); // the dot is a wildcard, the { } says to match the provious char at least 8 times to n number

            var isValidated = hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password);
            return isValidated;
        }

        /// <summary>
        /// Create a hash from the string input. Can be sha256 sha384 sha512 Md2 Md4 Md5
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="input"></param>
        /// <returns>string</returns>
        public static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        } // end GetHash

    }
}

