﻿using Application.Posts.InMemory.Commands;
using Application.Posts.InMemory.Queries;
using Application.Posts.Mongo.Commands;
using Application.Posts.Mongo.Queries;
using Application.Posts.NormalApproach.Commands;
using Application.Posts.NormalApproach.Queries;
using Application.Posts.Redis.Commands;
using Application.Posts.Redis.Queries;
using Application.Posts.SqlCache.Commands;
using Application.Posts.SqlCache.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ApiControllerBase
    {
        public PostsController()
        {
                
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPostByIdAsync(int id)
        {
            var post = await Mediator.Send(new GetPostByIdQuery {Id = id});

            if (post is null)
            {
                return NotFound("This post does not exist!");
            }

            await Mediator.Send(new UpdatePostViewCountCommand {Id = id});

            return Ok(post);
        }

        [HttpGet("top-favourite")]
        public async Task<IActionResult> GetTopFavouritePostsAsync()
        {
            var result = await Mediator.Send(new GetTopFavouritePostsQuery());

            // Update all cache
            await Mediator.Send(new CreateTopFavouritePostsOnRedisCommand { Posts = result });
            await Mediator.Send(new CreateTopFavouritePostsOnMongoCommand { Posts = result });
            await Mediator.Send(new CreateTopFavouritePostsOnSqlServerCommand { Posts = result });
            await Mediator.Send(new CreateTopFavouritePostsOnInMemoryDbCommand { Posts = result });

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

            return Ok(cachedPosts.OrderByDescending(p => p.FavoriteCount));
        }

        [HttpGet("top-favourite-in-memory")]
        public async Task<IActionResult> GetTopFavouritePostsFromInMemoryCacheAsync()
        {
            var cachedPosts = await Mediator.Send(new GetTopFavouritePostsFromInMemoryCacheQuery());
            if (cachedPosts == null)
            {
                return await GetTopFavouritePostsAsync();
            }

            return Ok(cachedPosts.OrderByDescending(p => p.FavoriteCount));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePostAsync(CreateNewPostCommand command)
        {
            await Mediator.Send(command);
            return StatusCode(201);
        }
    }
}
