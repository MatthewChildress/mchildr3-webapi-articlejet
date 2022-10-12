using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mchildr3_webapi_articlejet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet("/status/{key}")]
        public ActionResult<string> Get(string key)
        {
            if (key != "1111")
                return NotFound("Status 404 -- not an article");
            else
                return Ok("Status 200 -- Hey! Good Key!");
        }

        // POST api/<UserController>
        // user requests to be added, returns new key
        [HttpPost]
        public ActionResult<string> Post([FromBody] string value)
        {
            return Ok("Status 201 -- Post Success");
        }

        // POST api/<UserController>
        // admin adds a new user as active or inactive, returns key
        [HttpPost]
        public ActionResult<string> Post(int key, [FromBody] string value)
        {
            // id should be UserID
            if (key != 1111)
                return BadRequest("Status 401 -- Not Authorized");
            else
                return Ok($"Status 201 -- Post Success!");
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public ActionResult<string> Put(int id, [FromBody] string value)
        {
            return Ok($"Status 201 -- Update Success! {id} is {value}");
        }
        // 
        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
