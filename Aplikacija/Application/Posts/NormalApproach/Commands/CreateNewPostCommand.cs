using Application.Services;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;

namespace Application.Posts.NormalApproach.Commands
{
    public class CreateNewPostCommand : IRequest<bool>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int FavoriteCount { get; set; }
    }

    public class CreateNewPostCommandHandler : IRequestHandler<CreateNewPostCommand, bool>
    {
        private readonly StackOverflow2010Context _context;
        private readonly IUpdatePostCacheService _updatePostCacheService;

        public CreateNewPostCommandHandler(StackOverflow2010Context context,
            IUpdatePostCacheService updatePostCacheService)
        {
            _context = context;
            _updatePostCacheService = updatePostCacheService;
        }

        public async Task<bool> Handle(CreateNewPostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post
            {
                Title = request.Title,
                Body = request.Body,
                FavoriteCount = request.FavoriteCount,
                CreationDate = DateTime.Now,
                LastActivityDate = DateTime.Now
            };

            await _context.Posts.AddAsync(post, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            await _updatePostCacheService.UpdateNewPostCacheAsync(post);

            return result;
        }
    }
}
