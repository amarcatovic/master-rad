using Domain.Entities;
using MediatR;
using Infrastructure.Persistence;

namespace Application.Posts.InMemory.Commands
{
    public class CreateTopFavouritePostsOnInMemoryDbCommand : IRequest<bool>
    {
        public List<Post> Posts { get; set; }
    }

    public class CreateTopFavouritePostsCommandHandler : IRequestHandler<CreateTopFavouritePostsOnInMemoryDbCommand, bool>
    {
        private readonly InMemoryPostCacheDbContext _context;

        public CreateTopFavouritePostsCommandHandler(InMemoryPostCacheDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateTopFavouritePostsOnInMemoryDbCommand request, CancellationToken cancellationToken)
        {
            if (request.Posts == null)
            {
                throw new ArgumentNullException(nameof(request.Posts));
            }

            _context.RemoveRange(_context.Posts.ToList());

            await _context.Posts.AddRangeAsync(request.Posts, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
