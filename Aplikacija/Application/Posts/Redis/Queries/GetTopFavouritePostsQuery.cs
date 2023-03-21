using MediatR;
using Domain.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace Application.Posts.Redis.Queries
{
    public class GetTopFavouritePostsFromRedisCacheQuery : IRequest<List<Post>>
    {
    }

    public class GetTopFavouritePostsQueryHandler : IRequestHandler<GetTopFavouritePostsFromRedisCacheQuery, List<Post>>
    {
        private readonly IConnectionMultiplexer _redis;

        public GetTopFavouritePostsQueryHandler(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<List<Post>> Handle(GetTopFavouritePostsFromRedisCacheQuery request, CancellationToken cancellationToken)
        {
            var db = _redis.GetDatabase();
            var cachedItem = await db.StringGetAsync("TOP_POSTS");

            if (string.IsNullOrEmpty(cachedItem))
            {
                return null;
            }

            return JsonSerializer.Deserialize<List<Post>>(cachedItem);
        }
    }
}
