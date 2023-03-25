using MediatR;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.InMemory.Queries
{
    public class GetTopFavouritePostsFromInMemoryCacheQuery : IRequest<List<Post>>
    {
    }

    public class GetTopFavouritePostsQueryHandler : IRequestHandler<GetTopFavouritePostsFromInMemoryCacheQuery, List<Post>>
    {
        private readonly InMemoryPostCacheDbContext _context;

        public GetTopFavouritePostsQueryHandler(InMemoryPostCacheDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> Handle(GetTopFavouritePostsFromInMemoryCacheQuery request, CancellationToken cancellationToken)
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
