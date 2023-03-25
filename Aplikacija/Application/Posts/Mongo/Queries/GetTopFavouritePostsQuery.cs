using MediatR;
using Domain.Entities;
using Infrastructure.Persistence;
using MongoDB.Driver;

namespace Application.Posts.Mongo.Queries
{
    public class GetTopFavouritePostsFromMongoCacheQuery : IRequest<List<Post>>
    {
    }

    public class GetTopFavouritePostsQueryHandler : IRequestHandler<GetTopFavouritePostsFromMongoCacheQuery, List<Post>>
    {
        private readonly IMongoDBClient _mongoDbClient;

        public GetTopFavouritePostsQueryHandler(IMongoDBClient mongoDbClient)
        {
            _mongoDbClient = mongoDbClient;
        }

        public async Task<List<Post>> Handle(GetTopFavouritePostsFromMongoCacheQuery request, CancellationToken cancellationToken)
        {
            var collection = _mongoDbClient.GetCollection<Post>("TOP_POSTS");
            var posts = await collection.Find(Builders<Post>.Filter.Empty).ToListAsync(cancellationToken: cancellationToken);

            if (!posts.Any())
            {
                return null;
            }

            return posts;
        }
    }
}
