using Application.Posts.NormalApproach.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ApiControllerBase
    {
        public PostsController()
        {
                
        }

        [HttpGet("top-favourite")]
        public async Task<IActionResult> GetTopFavouritePostsAsync()
        {
            return Ok(await Mediator.Send(new GetTopFavouritePostsQuery()));
        }
    }
}
