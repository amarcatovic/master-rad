using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using MongoDB.Driver;
using StackExchange.Redis;
using System;
using System.Text.Json;

namespace Application.Posts.Mongo.Commands
{
    public class CreateTopFavouritePostsOnMongoCommand : IRequest<bool>
    {
        public List<Post> Posts { get; set; }
    }

    public class CreateTopFavouritePostsCommandHandler : IRequestHandler<CreateTopFavouritePostsOnMongoCommand, bool>
    {
        private readonly IMongoDBClient _mongoDbClient;

        public CreateTopFavouritePostsCommandHandler(IMongoDBClient mongoDbClient)
        {
            _mongoDbClient = mongoDbClient;
        }

        public async Task<bool> Handle(CreateTopFavouritePostsOnMongoCommand request, CancellationToken cancellationToken)
        {
            if (request.Posts == null)
            {
                throw new ArgumentNullException(nameof(request.Posts));
            }

            var collection = _mongoDbClient.GetCollection<Post>("TOP_POSTS");

            try
            {
                var filter = Builders<Post>.Filter.Empty;
                await collection.DeleteManyAsync(filter, cancellationToken);
                await collection.InsertManyAsync(request.Posts, cancellationToken: cancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
