using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mchildr3_webapi_articlejet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        // GET: api/v1/ratings/1/1111 -- working
        // valid user can get all ratings for an article
        [HttpGet("{id}/{key}")]
        public ActionResult<string> Get(int id, int key)
        {
            if (key != 1111)
                return NotFound("Status 404 -- not an article");
            else
                return Ok($"Status 200 -- Hey! Good Key! Here's Article {id}");
        }
    }
}
