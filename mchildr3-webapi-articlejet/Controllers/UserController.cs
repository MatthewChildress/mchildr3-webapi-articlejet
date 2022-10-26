using mchildr3_webapi_articlejet.BusinessLayer;
using mchildr3_webapi_articlejet.Data;
using mchildr3_webapi_articlejet.DataTransfer;
using mchildr3_webapi_articlejet.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace mchildr3_webapi_articlejet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // ex.GET: api/v1/user/status/22541e71-ceba-42c9-8908-eee3792543d1 -- working
        /// <summary>
        /// GET request that checks a user's active status in database after having
        /// the user GUID validated
        /// </summary>
        /// <param name="key">user GUID</param>
        /// <returns>returns active status if key is correct or bad request if not</returns>
        [HttpGet("status/{key}")]
        public ActionResult<string> GetAUserStatus(string key)
        {

            DataLayer dl = new DataLayer();
            var userResults = dl.GetAUserByGUIDAsync(key);

            // if user is retrieved from database
            if (userResults.Result != null)
            {
                // if user is active
                if (userResults.Result.IsActive == true)
                {
                    return Ok("User is active");
                }
                // if user is inactive
                else
                {
                    return Ok("User in inactive");
                }
            }
            // bad key
            else
            {
                return BadRequest("bad user key");
            }
        }

        // ex. POST api/v1/user/ -- working
        /// <summary>
        /// POST request from user to be added. The request validates the JSON 
        /// body information and generates a GUID and SHA256 Password for user
        /// </summary>
        /// <param name="userDto">JSON body info that matches UserDto</param>
        /// <returns>User info if successful or bad requests if validation fails or input is incorrect</returns>
        [HttpPost]
        public ActionResult<string> PostUserRequest([FromBody] UserDto userDto)
        {
            DataLayer dl = new DataLayer();
            // if JSON is not empty
            if (userDto != null)
            {
                // validates JSON
                if (BusLogLayer.ValidateUserDto(userDto).ValidUserDto)
                {
                    // creates GUID and SHA256 Password for user in database
                    SHA256 sha256Hash = SHA256.Create();
                    var hashPass = BusLogLayer.GetHash(sha256Hash, userDto.UserPassword);
                    var guid = Guid.NewGuid().ToString();

                    // new user object to be stored in database
                    User user = new User();
                    user.Guid = guid;
                    user.Email = userDto.Email;
                    user.Password = hashPass;
                    user.FirstName = userDto.FirstName;
                    user.LastName = userDto.LastName;
                    user.IsActive = false;
                    user.LevelID = 4;

                    var results = dl.PostANewUserAsync(user);
                    // if post is successful and user is not a duplicate
                    if (results.Result > 0)
                    {
                        return Ok(userDto);
                    }
                    // user is a duplicate
                    else
                    {
                        return BadRequest("Not successful adding user to the database.");
                    }
                }
                // JSON was incorrect
                else
                {
                    return BadRequest(userDto);
                }
            }
            // no JSON
            else
            {
                return BadRequest("no data");
            }
        }

        // ex. POST api/v1/user/f2161cae-5fe4-49d6-b61a-73203d94bdf7/true -- working
        /// <summary>
        /// POST request where a valid admin key can add a new user and determine their active stauts
        /// </summary>
        /// <param name="key">admin GUID</param>
        /// <param name="userDto">JSON body information</param>
        /// <returns>User info if successful with selected active status, or bad requests of validation fails or input is incorrect.</returns>
        [HttpPost("{adminKey}/{status}")]
        public ActionResult<string> PostNewUserActiveStatus(string adminKey, bool status, [FromBody] UserDto userDto)
        {
            // admin key is 1 or 2
            DataLayer dl = new DataLayer();
            var keyResults = dl.GetAUserByGUIDAsync(adminKey);

            // checks that admin key is in system and the appropriate level
            if (keyResults.Result != null && (keyResults.Result.LevelID == 1 || keyResults.Result.LevelID == 2))
            {
                // checks admin key is active
                if (keyResults.Result.IsActive == true)
                {
                    // validates JSON 
                    if (BusLogLayer.ValidateUserDto(userDto).ValidUserDto)
                    {
                        // creates GUID and SHA256 Password for user in database
                        SHA256 sha256Hash = SHA256.Create();
                        var hashPass = BusLogLayer.GetHash(sha256Hash, userDto.UserPassword);
                        var guid = Guid.NewGuid().ToString();

                        // new user object to be stored in database
                        User user = new User();
                        user.Guid = guid;
                        user.Email = userDto.Email;
                        user.Password = hashPass;
                        user.FirstName = userDto.FirstName;
                        user.LastName = userDto.LastName;

                        // determines user.IsActive value from status
                        if (status == true)
                        {
                            user.IsActive = true;
                        }
                        else
                        {
                            user.IsActive = false;
                        }
                        user.LevelID = 4;

                        var results = dl.PostANewUserAsync(user);
                        // if post is successful and user is not a duplicate
                        if (results.Result > 0)
                        {
                            return Ok($"user is {user.IsActive}");
                        }
                        // user is a duplicate
                        else
                        {
                            return BadRequest("Not successful updating user to the database");
                        }
                    }
                    // JSON was incorrect
                    else
                    {
                        return BadRequest("not valid user info to post");
                    }
                }
                // inactive admin key
                else
                {
                    return BadRequest("admin key not active");
                }
            }
            // no JSON
            else
            {
                return BadRequest("Admin key is not valid");
            }
        }


        // ex. PATCH api/v1/user/f2161cae-5fe4-49d6-b61a-73203d94bdf7/894cde31-5592-49dc-8a13-803e0007f935?status=false -- WORKING
        /// <summary>
        /// a PATCH request that updates the status of a user after validation checks are passed
        /// for the admin key and the user key
        /// </summary>
        /// <param name="adminKey">admin GUID</param>
        /// <param name="userKey">user GUID</param>
        /// <param name="status">user status</param>
        /// <returns></returns>
        [HttpPatch("{adminKey}/{userKey}")]
        public ActionResult<string> PatchUserStatus(string adminKey, string userKey, [FromQuery] bool status)
        {
            // is adminkey valid (1 or 2?) and active
            // validate status returns true or false
            // return correct responses
            DataLayer dl = new DataLayer();
            Task<User>? adminResults = dl.GetAUserByGUIDAsync(adminKey);
            // checks that admin key is in system and the appropriate level
            if (adminResults.Result != null && (adminResults.Result.LevelID == 1 || adminResults.Result.LevelID == 2))
            {
                // checks admin key is active
                if (adminResults.Result.IsActive == true)
                {
                    var userResults = dl.GetAUserByGUIDAsync(userKey);
                    // if user is valid
                    if (userResults.Result != null && (userResults.Result.LevelID != 1 || userResults.Result.LevelID != 2))
                    {
                        var userUpdate = dl.PutAUsersStateAsync(userKey, status);
                        // if user key updates
                        if (userUpdate.Result > 0)
                        {
                            // if admin changed user status to active
                            if (status == true)
                            {
                                return Ok($"{userKey} is set to active");
                            }
                            // if admin changed user status to inactive
                            else
                            {
                                return Ok($"{userKey} is set to inactive");
                            }
                        }
                        // PutAUsersState failed with status not being a bool
                        else
                        {
                            return BadRequest("user not updated");
                        }

                    }
                    // user GUID not in database
                    else
                    {
                        return BadRequest("invalid user key.");
                    }
                }
                // admin is inactive
                else
                {
                    return BadRequest("admin status is not active to execute user update.");
                }
            }
            // admin key is incorrect
            else
            {
                return BadRequest("invalid admin key.");
            }
        }
        // ex. DELETE api/v1/user/f2161cae-5fe4-49d6-b61a-73203d94bdf7/2a610bbf-97c8-47c9-b024-244f5c6dfee3 -- working
        /// <summary>
        /// DELETE request where an admin can delete a user that isn't an admin. both the admin key
        /// and the user key have to pass validation checks in order to work.
        /// </summary>
        /// <param name="adminKey">admin GUID</param>
        /// <param name="userKey">user GUID</param>
        /// <returns></returns>
        [HttpDelete("{adminKey}/{userKey}")]
        public ActionResult<string> DeleteUser(string adminKey, string userKey)
        {
            DataLayer dl = new DataLayer();
            var adminResults = dl.GetAUserByGUIDAsync(adminKey);

            // checks that admin key is in system and the appropriate level
            if (adminResults.Result != null && (adminResults.Result.LevelID == 1 || adminResults.Result.LevelID == 2))
            {
                // checks if admin is active
                if (adminResults.Result.IsActive == true)
                {
                    var userResults = dl.GetAUserByGUIDAsync(userKey);
                    // checks if user is valid
                    if (userResults.Result != null)
                    {
                        // checks if user is an admin
                        if ((userResults.Result.LevelID != 1) || (userResults.Result.LevelID != 2))
                        {
                            var userDelete = dl.DeleteAUserAsync(userKey);
                            // checks if user is deleted
                            if (userDelete.Result != null)
                            {
                                return Ok("user deleted");
                            }
                            // user not deleted. would need to check logger as to why
                            else
                            {
                                return BadRequest("user not deleted");
                            }
                        }
                        // admin cannot delete user because of level
                        else
                        {
                            return BadRequest($"{adminKey} can NOT delete {userKey}");
                        }
                    }
                    // invalid user key
                    else
                    {
                        return BadRequest("That user does not exist");
                    }
                }
                // admin is not active
                else
                {
                    return BadRequest("Admin is not active");
                }
            }
            // not an admin
            else
            {
                return BadRequest("400 status -- not an admin");
            }
        }
    }
}
