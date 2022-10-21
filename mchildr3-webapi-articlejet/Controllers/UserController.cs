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
        // ex. GET: api/v1/user/status/1111 -- working
        /// <summary>
        /// This method sends a GET request to retrieve status of a specific user
        /// </summary>
        /// <param name="key">user id</param>
        /// <returns>if user id is valid, returns user status</returns>
        [HttpGet("status/{key}")]
        public ActionResult<string> GetUser(int key)
        {
            if (key != 1111)
                return NotFound("Status 404 -- not a valid user");
            else
                return Ok("Status 200 -- You might be active or inactive! We have to implement that feature still!");
        }

        // ex. POST api/v1/user/ -- working
        // user requests to be added, returns new key
        /// <summary>
        /// This method submits user request and returns a key
        /// </summary>
        /// <param name="value">From Body inputs</param>
        /// <returns>valid key if From Body is not null</returns>
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
                    user.LevelID = 1;

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


        // ex. POST api/v1/user/1111/?active=false -- working
        // admin adds a new user as active or inactive, returns key
        /// <summary>
        /// This method adds a new user with either an active or inactive status if a valid admin key is applied
        /// </summary>
        /// <param name="id">admin key</param>
        /// <param name="value">From Body info</param>
        /// <param name="active">active status</param>
        /// <returns>returns not authorized http response if not admin, adds user and sets active status</returns>
        [HttpPost("{id}")]
        public ActionResult<string> PostNewUser(int id, bool active, [FromBody] UserDto data)
        {
            // key demo
            Random random = new Random();
            if (id != 1111)
                return Unauthorized("Status 401 -- Not Authorized");
            //if body is an empty string returns this error. if there is NOTHING in body. will be 415 error
            else if (data == null)
                return BadRequest("Status 400 -- Invalid Request");
            else
                {
                    if (active == true)
                        return Ok($"Status 201 -- Active User Added. Key is {random.Next(100000)}.");
                    else if (active == false)
                        return Ok($"Status 201 -- Inactive User Added. Key is {random.Next(100000)}.");
                    else
                        return BadRequest("Status 406 -- Not Acceptable. User must be active or inactive");
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
        public ActionResult<string> PatchUser(int id, bool active)
        {
            if (id != 1111)
                return Unauthorized("Status 401 -- Not Authorized");
            else
            {
                if (active == true)
                    return Ok($"Status 201 -- Status Updated! User is active.");
                else
                    return Ok($"Status 201 -- Status Updated! User is inactive.");
            }
        }

        // 
        // ex. DELETE api/v1/user/5?key=1111 -- working
        /// <summary>
        /// This method removes a user via DELETE request if a valid admin key is applied
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="key">Admin Key</param>
        /// <returns>returns not authoriazed http response if not admin, else successful deletion.</returns>
        [HttpDelete("{id}")]
        public ActionResult<string> DeleteUser(int id, int key)
        {
            if (key != 1111)
                return Unauthorized("Status 401 -- Not Authorized");
            else
                return Ok($"Status 200 -- Successfully Deleted! User {id} has been removed.");
        }
    }
}
