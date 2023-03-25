using Domain.Entities;
using MediatR;
using StackExchange.Redis;
using System.Text.Json;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.SqlCache.Commands
{
    public class CreateTopFavouritePostsOnSqlServerCommand : IRequest<bool>
    {
        public List<Post> Posts { get; set; }
    }

    public class CreateTopFavouritePostsCommandHandler : IRequestHandler<CreateTopFavouritePostsOnSqlServerCommand, bool>
    {
        private readonly PostCacheDbContext _context;

        public CreateTopFavouritePostsCommandHandler(PostCacheDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateTopFavouritePostsOnSqlServerCommand request, CancellationToken cancellationToken)
        {
            if (request.Posts == null)
            {
                throw new ArgumentNullException(nameof(request.Posts));
            }

            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Posts", cancellationToken: cancellationToken);
            await _context.Posts.AddRangeAsync(request.Posts, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
