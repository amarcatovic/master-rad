using Application.Posts.NormalApproach.Queries;
using Application.Posts.Redis.Commands;
using Application.Posts.Redis.Queries;
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
            var result = await Mediator.Send(new GetTopFavouritePostsQuery());

            await Mediator.Send(new CreateTopFavouritePostsOnRedisCommand { Posts = result });

            return Ok(result);
        }

        [HttpGet("top-favourite-redis")]
        public async Task<IActionResult> GetTopFavouritePostsFromRedisAsync()
        {
            var cachedPosts = await Mediator.Send(new GetTopFavouritePostsFromRedisCacheQuery());
            if (cachedPosts == null)
            {
                return await GetTopFavouritePostsAsync();
            }

            return Ok(cachedPosts);
        }
    }
}
