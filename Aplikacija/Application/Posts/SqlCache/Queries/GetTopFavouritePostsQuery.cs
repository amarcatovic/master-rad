using MediatR;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.SqlCache.Queries
{
    public class GetTopFavouritePostsFromSqlCacheQuery : IRequest<List<Post>>
    {
    }

    public class GetTopFavouritePostsQueryHandler : IRequestHandler<GetTopFavouritePostsFromSqlCacheQuery, List<Post>>
    {
        private readonly PostCacheDbContext _context;

        public GetTopFavouritePostsQueryHandler(PostCacheDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> Handle(GetTopFavouritePostsFromSqlCacheQuery request, CancellationToken cancellationToken)
        {
            var posts = await _context.Posts.ToListAsync(cancellationToken: cancellationToken);

            if (!posts.Any())
            {
                return null;
            }

            return posts;
        }
    }
}
