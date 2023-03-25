using Domain.Entities;
using MediatR;
using StackExchange.Redis;
using System.Text.Json;

namespace Application.Posts.Redis.Commands
{
    public class CreateTopFavouritePostsOnRedisCommand : IRequest<bool>
    {
        public List<Post> Posts { get; set; }
    }

    public class CreateTopFavouritePostsCommandHandler : IRequestHandler<CreateTopFavouritePostsOnRedisCommand, bool>
    {
        private readonly IConnectionMultiplexer _redis;

        public CreateTopFavouritePostsCommandHandler(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<bool> Handle(CreateTopFavouritePostsOnRedisCommand request, CancellationToken cancellationToken)
        {
            if (request.Posts == null)
            {
                throw new ArgumentNullException(nameof(request.Posts));
            }

            var db = _redis.GetDatabase();

            var serialItem = JsonSerializer.Serialize(request.Posts);
            return await db.StringSetAsync("TOP_POSTS", serialItem);
        }
    }
}
