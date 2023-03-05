using MediatR;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.NormalApproach.Queries
{
    public class GetTopFavouritePostsQuery : IRequest<List<Post>>
    {
    }

    public class GetTopFavouritePostsQueryHandler : IRequestHandler<GetTopFavouritePostsQuery, List<Post>>
    {
        private readonly StackOverflow2010Context _context;

        public GetTopFavouritePostsQueryHandler(StackOverflow2010Context context)
        {
            _context = context;
        }

        public async Task<List<Post>> Handle(GetTopFavouritePostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _context
                .Posts
                .Where(p => p.Title != null)
                .OrderByDescending(p => p.FavoriteCount)
                .Take(10)
                .ToListAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return posts;
        }
    }
}
