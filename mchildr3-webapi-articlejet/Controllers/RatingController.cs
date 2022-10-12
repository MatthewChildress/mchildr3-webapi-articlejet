﻿using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace mchildr3_webapi_articlejet.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        // ex. GET api/v1/rating/11?key=1112
        // valid user can get the average rating for article ?type=avg
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">article id</param>
        /// <param name="key">user id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> RatingGet(int id, int key)
        {
            if (key != 1111)
                return Unauthorized("Status 401 -- Not Authorized");
            else
                return Ok($"Status 200 -- Here's all the ratings for Article {id}");
        }

        // ex. POST api/v1/rating/11?key=1111 -- working
        // valid user can rate an article
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">article id</param>
        /// <param name="key">user id</param>
        /// <param name="rating">rating from form value</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public ActionResult<string> RatingsPost(int id, int key, [FromForm] string rating)
        {
            if (key != 1111)
                return Unauthorized("Status 401 -- Not Authorized");
            else
            {
                //if body is an empty string returns this error. if there is NOTHING in body. will be 415 error
                if (string.IsNullOrEmpty(rating))
                    return BadRequest("Status 400 -- Invalid Request");
                else
                    return Ok($"Status 201 -- New Rating of {rating} posted to Article with an ID of {id}! ");
            }
        }

        // ex. PATCH api/v1/rating/2?key=1111 -- working
        // valid user can update a rating of an article
        /// <summary>
        /// This method updates a rating if a valid user id is applied
        /// </summary>
        /// <param name="id">rating id</param>
        /// <param name="key">user id</param>
        /// <param name="rating">rating from form value</param>
        /// <returns>returns not authorized http response if not valid user, else updates rating.</returns>
        [HttpPatch("{id}")]
        public ActionResult<string> RatingsPatch(int id, int key, [FromForm] string rating)
        {
            if (key != 1111)
                return Unauthorized("Status 401 -- Not Authorized");
            else
                return Ok($"Status 201 -- Status Updated! Rating '{id}' is now '{rating}'.");
        }

        // ex. DELETE api/v1/rating/5/?key=1111 -- working
        // admin can delete a rating
        /// <summary>
        /// This method removes a rating via DELETE request if a valid admin key is applied
        /// </summary>
        /// <param name="id">rating id</param>
        /// <param name="key">admin key</param>
        /// <returns>returns not authoriazed http response if not admin, else successful deletion.</returns>
        [HttpDelete("{id}")]
        public ActionResult<string> RatingsDelete(int id, int key)
        {
            if (key != 1111)
                return Unauthorized("Status 401 -- Not Authorized");
            else
                return Ok($"Status 200 -- Successfully Deleted! Rating of '{id}' has been removed.");
        }
    }
}
