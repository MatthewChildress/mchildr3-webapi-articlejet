using mchildr3_webapi_articlejet.BusinessLayer;
using mchildr3_webapi_articlejet.Data;
using mchildr3_webapi_articlejet.DataTransfer;
using mchildr3_webapi_articlejet.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mchildr3_webapi_articlejet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // ex.GET: api/v1/user/status/GUID -- working
        [HttpGet("status/{key}")]
        public ActionResult<string> GetAUserStatus(string key)
        {
            // user checks their own status, returns status
            // check for valid key (guid)
            // return string status true/false with 200 status
            DataLayer dl = new DataLayer();
            var userResults = dl.GetAUserByGUIDAsync(key);
            if (userResults.Result != null)
            {
                if (userResults.Result.IsActive == false)
                {
                    return Ok("User is active");
                }
                else
                {
                    return Ok("User in inactive");
                }
            }
            else
            {
                return BadRequest("bad user key");
            }
        }

        // ex. POST api/v1/user/ -- working
        /// <summary>
        /// Validates user fields except password to create new user.
        /// </summary>
        /// <param name="userDto">JSON body information</param>
        /// <returns>if valid, returns 200 status. Otherwise, returns 400 error status</returns>
        [HttpPost]
        public ActionResult<string> PostUserRequest([FromBody] UserDto userDto)
        {
            DataLayer dl = new DataLayer();
            if (userDto != null)
            {
                if (BusLogLayer.ValidateUserDto(userDto).ValidUserDto)
                {
                    SHA256 sha256Hash = SHA256.Create();
                    var hashPass = BusLogLayer.GetHash(sha256Hash, userDto.UserPassword);
                    var guid = Guid.NewGuid().ToString();
                    User user = new User();
                    user.Guid = guid;
                    user.Email = userDto.Email;
                    user.Password = hashPass;
                    user.FirstName = userDto.FirstName;
                    user.LastName = userDto.LastName;
                    user.IsActive = false;
                    user.LevelID = 4;

                    var results = dl.PostANewUserAsync(user);
                    if (results.Result > 0)
                    {
                        return Ok(userDto);
                    }
                    else
                    {
                        return BadRequest("Not successful adding user to the database.");
                    }
                }
                else
                {
                    return BadRequest(userDto);
                }
            }
            else
            {
                return BadRequest("no data");
            }
        }


        // ex. POST api/v1/user/1111/admingGUID -- need help
        // admin adds a new user as active or inactive, returns key
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">admin key</param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("{key}")]
        public ActionResult<string> PostNewUserActiveStatus(string key, [FromBody] UserDto userDto)
        {
            // admin key is 1 or 2
            DataLayer dl = new DataLayer();
            var keyResults = dl.GetAUserByGUIDAsync(key);

            if (keyResults.Result != null)
            {
                if (BusLogLayer.ValidateUserDto(userDto).ValidUserDto)
                {
                    SHA256 sha256Hash = SHA256.Create();
                    var hashPass = BusLogLayer.GetHash(sha256Hash, userDto.UserPassword);
                    var guid = Guid.NewGuid().ToString();
                    User user = new User();
                    user.Guid = guid;
                    user.IsActive = false;
                    user.LevelID = 4;

                    var results = dl.PostANewUserAsync(user);
                    if (results.Result > 0)
                    {
                        return Ok(user.IsActive);
                    }
                    else
                    {
                        return BadRequest("Not successful updating user to the database");
                    }
                }
                else
                {
                    return BadRequest(userDto);
                }
            }
            else
            {
                return BadRequest("Admin key is not valid");
            }
        }
        // ex. PATCH api/v1/user/1111?active=false -- working
        /// <summary>
        /// This method updates a users active status if a valid admin key is applied
        /// </summary>
        /// <param name="id">admin key</param>
        /// <param name="active">active status</param>
        /// <returns>returns not authorized http response if not admin, else updates user active status.</returns>
        [HttpPatch("{id}")]
        public ActionResult<string> PatchUserStatus(string adminKey, [FromQuery] bool status)
        {
            // is adminkey valid (1 or 2?) and active
            // validate status returns true or false
            // return correct responses
            DataLayer dl = new DataLayer();
            Task<User>? userResults = dl.GetAUserByGUIDAsync(adminKey);
            if (userResults.Result != null && (userResults.Result.LevelID == 1 || userResults.Result.LevelID == 2))
            {
                if (status == true)
                {
                    return Ok("Status is active");
                }
                else
                {
                    return BadRequest("Status is inactive");
                }
            }
            else
            {
                return BadRequest("");
            }
        }
        // ex. DELETE api/v1/user/adminKey/userKey -- working
        // admin can delete a user
        [HttpDelete("{adminKey}/{userKey}")]
        public ActionResult<string> DeleteUser(string adminKey, string userKey)
        {
            // admin key is LevelID 1
            DataLayer dl = new DataLayer();
            var adminResults = dl.GetAUserByGUIDAsync(adminKey);    

            if (adminResults.Result != null & adminResults.Result.LevelID == 1)
            {
                var userResults = dl.GetAUserByGUIDAsync(userKey);
                if (userResults.Result != null && userResults.Result.LevelID != 1)
                {
                    return Ok($"{adminKey} and {userKey} exists to be deleted");
                }
                else
                {
                    return BadRequest("not a valid user key");
                }    
            }
            else
            {
                return BadRequest("400 status -- not an admin");
            }
        }
    }
}
