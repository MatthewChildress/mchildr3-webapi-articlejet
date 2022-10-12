using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mchildr3_webapi_articlejet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        // GET: api/v1/articles/1111 -- working
        /// <summary>
        /// This method returns all articles is key shows valid user
        /// </summary>
        /// <param name="key">user id</param>
        /// <returns>returns all articles if user is valid</returns>
        [HttpGet("{key}")]
        public ActionResult<string> Get(string key)
        {
            if (key != "1111")
                return NotFound("Status 404 -- not an article");
            else
                return Ok("Status 200 -- Hey! Good Key!");
        }
    }
}
