using Application.Posts.Mongo.Commands;
using Application.Posts.Mongo.Queries;
using Application.Posts.NormalApproach.Queries;
using Application.Posts.Redis.Commands;
using Application.Posts.Redis.Queries;
using Application.Posts.SqlCache.Commands;
using Application.Posts.SqlCache.Queries;
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

            // Update all cache
            await Mediator.Send(new CreateTopFavouritePostsOnRedisCommand { Posts = result });
            await Mediator.Send(new CreateTopFavouritePostsOnMongoCommand { Posts = result });
            await Mediator.Send(new CreateTopFavouritePostsOnSqlServerCommand { Posts = result });

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

        [HttpGet("top-favourite-mongo")]
        public async Task<IActionResult> GetTopFavouritePostsFromMongoAsync()
        {
            var cachedPosts = await Mediator.Send(new GetTopFavouritePostsFromMongoCacheQuery());
            if (cachedPosts == null)
            {
                return await GetTopFavouritePostsAsync();
            }

            return Ok(cachedPosts);
        }

        [HttpGet("top-favourite-sql-cache")]
        public async Task<IActionResult> GetTopFavouritePostsFromSqlCacheAsync()
        {
            var cachedPosts = await Mediator.Send(new GetTopFavouritePostsFromSqlCacheQuery());
            if (cachedPosts == null)
            {
                return await GetTopFavouritePostsAsync();
            }

            return Ok(cachedPosts);
        }
    }
}
