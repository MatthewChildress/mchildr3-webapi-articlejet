using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mchildr3_webapi_articlejet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        // ex. GET: api/v1/ratings/1/1111 -- working
        // valid user can get all ratings for an article
        /// <summary>
        /// This method uses a GET http request to get all ratings for an article if user is valid
        /// </summary>
        /// <param name="id">article id</param>
        /// <param name="key">user id</param>
        /// <returns>returns all the ratings for a speicifc article if user id is valid</returns>
        [HttpGet("{id}/{key}")]
        public ActionResult<string> GetRatings(int id, int key)
        {
            if (key != 1111)
                return Unauthorized("Status 401 -- You're not authorized to do that");
            else
                return Ok($"Status 200 -- Here's all the ratings for Article {id}");
        }
    }
}
