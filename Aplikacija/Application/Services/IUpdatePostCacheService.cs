using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using StackExchange.Redis;
using System.Text.Json;

namespace Application.Services
{
    public interface IUpdatePostCacheService
    {
        Task UpdateNewPostCacheAsync(Post post);
        Task UpdatePostViewCountInCacheAsync(Post post);
    }

    public class UpdatePostCacheService : IUpdatePostCacheService
    {
        private readonly InMemoryPostCacheDbContext _inMemoryPostCacheDbContext;
        private readonly PostCacheDbContext _postCacheDbContext;
        private readonly IMongoDBClient _mongoDbClient;
        private readonly IConnectionMultiplexer _redis;

        public UpdatePostCacheService(InMemoryPostCacheDbContext inMemoryPostCacheDbContext,
            PostCacheDbContext postCacheDbContext,
            IMongoDBClient mongoDbClient,
            IConnectionMultiplexer redis)
        {
            _inMemoryPostCacheDbContext = inMemoryPostCacheDbContext;
            _postCacheDbContext = postCacheDbContext;
            _mongoDbClient = mongoDbClient;
            _redis = redis;
        }

        public async Task UpdateNewPostCacheAsync(Post post)
        {
            await UpdateRedisCache(post);
            await UpdateMongoCache(post);
            await UpdateSqlServerCache(post);
            await UpdateInMemoryCache(post);
        }

        public async Task UpdatePostViewCountInCacheAsync(Post post)
        {
            await UpdateViewCountInRedisAsync(post.Id, post.ViewCount);
            await UpdateViewCountInMongoAsync(post.Id, post.ViewCount);
            await UpdateViewCountInSqlServerAsync(post.Id, post.ViewCount);
            await UpdateViewCountInMemoryAsync(post.Id, post.ViewCount);
        }

        #region UpdateNewPostCacheAsync privates

        private async Task UpdateRedisCache(Post post)
        {
            var db = _redis.GetDatabase();
            var cachedItem = await db.StringGetAsync("TOP_POSTS");

            if (string.IsNullOrEmpty(cachedItem))
            {
                return;
            }

            var posts = JsonSerializer.Deserialize<List<Post>>(cachedItem);
            posts = HandleAddingNewPost(posts, post);

            var serialItem = JsonSerializer.Serialize(posts);
            await db.StringSetAsync("TOP_POSTS", serialItem);
        }

        private async Task UpdateMongoCache(Post post)
        {
            var collection = _mongoDbClient.GetCollection<Post>("TOP_POSTS");
            var posts = await collection.Find(Builders<Post>.Filter.Empty).ToListAsync();

            if (posts.Any())
            {
                posts = HandleAddingNewPost(posts, post);

                var filter = Builders<Post>.Filter.Empty;
                await collection.DeleteManyAsync(filter);
                await collection.InsertManyAsync(posts);
            }
        }

        private async Task UpdateSqlServerCache(Post post)
        {
            var posts = await _postCacheDbContext.Posts.ToListAsync();

            if (posts.Any())
            {
                posts = HandleAddingNewPost(posts, post);

                await _postCacheDbContext.Database.ExecuteSqlRawAsync("DELETE FROM Posts");
                await _postCacheDbContext.Posts.AddRangeAsync(posts);
                await _postCacheDbContext.SaveChangesAsync();
            }
        }

        private async Task UpdateInMemoryCache(Post post)
        {
            var posts = await _inMemoryPostCacheDbContext.Posts.ToListAsync();

            if (posts.Any())
            {
                posts = HandleAddingNewPost(posts, post);

                _inMemoryPostCacheDbContext.RemoveRange(_inMemoryPostCacheDbContext.Posts.ToList());
                await _inMemoryPostCacheDbContext.SaveChangesAsync();
                await _inMemoryPostCacheDbContext.Posts.AddRangeAsync(posts);
                await _inMemoryPostCacheDbContext.SaveChangesAsync();
            }
        }

        private static List<Post> HandleAddingNewPost(List<Post> postCache, Post post)
        {
            if (postCache.Last().FavoriteCount < post.FavoriteCount)
            {
                postCache[^1] = post;
            }

            return postCache
                .OrderByDescending(p => p.FavoriteCount)
                .ToList();
        }

        #endregion

        #region UpdatePostViewCountInCacheAsync privates

        private async Task UpdateViewCountInRedisAsync(int postId, int viewCount)
        {
            var db = _redis.GetDatabase();
            var cachedItem = await db.StringGetAsync("TOP_POSTS");

            if (string.IsNullOrEmpty(cachedItem))
            {
                return;
            }

            var posts = JsonSerializer.Deserialize<List<Post>>(cachedItem);
            var post = posts.SingleOrDefault(p => p.Id == postId);

            if (post is not null)
            {
                post.ViewCount = viewCount;
                var serialItem = JsonSerializer.Serialize(posts);
                await db.StringSetAsync("TOP_POSTS", serialItem);
            }
        }

        private async Task UpdateViewCountInMongoAsync(int postId, int viewCount)
        {
            var collection = _mongoDbClient.GetCollection<Post>("TOP_POSTS");
            var posts = await collection.Find(Builders<Post>.Filter.Empty).ToListAsync();

            if (posts.Any())
            {
                var post = posts.SingleOrDefault(p => p.Id == postId);

                if (post is not null)
                {
                    post.ViewCount = viewCount;
                    var filter = Builders<Post>.Filter.Empty;
                    await collection.DeleteManyAsync(filter);
                    await collection.InsertManyAsync(posts);
                }
            }
        }

        private async Task UpdateViewCountInSqlServerAsync(int postId, int viewCount)
        {
            var posts = await _postCacheDbContext.Posts.ToListAsync();

            if (posts.Any())
            {
                var post = posts.SingleOrDefault(p => p.Id == postId);
                if (post is not null)
                { 
                    post.ViewCount = viewCount;
                    await _postCacheDbContext.Database.ExecuteSqlRawAsync("DELETE FROM Posts");
                    await _postCacheDbContext.Posts.AddRangeAsync(posts);
                    await _postCacheDbContext.SaveChangesAsync();
                }
            }
        }

        private async Task UpdateViewCountInMemoryAsync(int postId, int viewCount)
        {
            var posts = await _inMemoryPostCacheDbContext.Posts.ToListAsync();

            if (posts.Any())
            {
                var post = posts.SingleOrDefault(p => p.Id == postId);
                if (post is not null)
                {
                    post.ViewCount = viewCount;
                    _inMemoryPostCacheDbContext.RemoveRange(_inMemoryPostCacheDbContext.Posts.ToList());
                    await _inMemoryPostCacheDbContext.SaveChangesAsync();
                    await _inMemoryPostCacheDbContext.Posts.AddRangeAsync(posts);
                    await _inMemoryPostCacheDbContext.SaveChangesAsync();
                }
            }
        }

        #endregion
    }
}
