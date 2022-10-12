using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mchildr3_webapi_articlejet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        // ex. GET: api/v1/article/1/1111 -- working
        // valid user gets an article
        /// <summary>
        /// This method sends a GET request to retrieve information from a specific article id
        /// </summary>
        /// <param name="id">article ID</param>
        /// <param name="key">valid user key</param>
        /// <returns>returns http status that article query is not found, else successful status.</returns>
        [HttpGet("{id}/{key}")]
        public ActionResult<string> GetArticles(int id, int key)
        {
            // key denotes valid user. id is article.id
            if (key != 1111)
                return NotFound("Status 404 -- not an article");
            else
                return Ok($"Status 200 -- Hey! Good Key! Here is article {id}");
        }

        // ex. POST api/v1/article/?key=1111 -- working
        // valid user can add a new article
        /// <summary>
        /// This method makes a POST request adding a new article if UserID is valid
        /// </summary>
        /// <param name="key">User ID</param>
        /// <param name="value">Article Content</param>
        /// <returns>returns a not authorized http response if bad. else, http status code for successful creation </returns>
        [HttpPost]
        public ActionResult<string> PostNewArticle(int key, [FromBody] string value)
        {
            // id should be UserID
            if (key != 1111)
                return Unauthorized("Status 401 -- Not Authorized");
            else
                return Ok($"Status 201 -- Post Success!");
        }

        // ex. DELETE api/v1/article/5?key=1111 -- working
        // admin can delete an article
        /// <summary>
        /// This method deletes an article via DELETE request for an article if a valid admin key is applied
        /// </summary>
        /// <param name="id">article id</param>
        /// <param name="key">admin key</param>
        /// <returns>returns not authoriazed http response if not admin, else successful deletion.</returns>
        [HttpDelete("{id}")]
        public ActionResult<string> DeleteArticle(int id, int key)
        {
            if (key != 1111)
                return Unauthorized("Status 401 -- Not Authorized");
            else
                return Ok($"Status 200 -- Successfully Deleted! {id} has been removed.");
        }
    }
}
