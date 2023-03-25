using Application.Posts.NormalApproach.Queries;
using Application.Services;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.NormalApproach.Commands
{
    public class UpdatePostViewCountCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class UpdatePostViewCountCommandHandler : IRequestHandler<UpdatePostViewCountCommand, bool>
    {
        private readonly StackOverflow2010Context _context;
        private readonly IUpdatePostCacheService _updatePostCacheService;

        public UpdatePostViewCountCommandHandler(StackOverflow2010Context context,
            IUpdatePostCacheService updatePostCacheService)
        {
            _context = context;
            _updatePostCacheService = updatePostCacheService;
        }

        public async Task<bool> Handle(UpdatePostViewCountCommand request, CancellationToken cancellationToken)
        {
            var post = await _context
                .Posts
                .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

            if (post is null)
            {
                return false;
            }

            post.ViewCount += 1;

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            await _updatePostCacheService.UpdatePostViewCountInCacheAsync(post);

            return result;
        }
    }
}
