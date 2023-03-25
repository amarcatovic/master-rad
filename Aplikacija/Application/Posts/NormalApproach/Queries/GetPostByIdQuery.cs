using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Posts.NormalApproach.Queries
{
    public class GetPostByIdQuery : IRequest<Post>
    {
        public int Id { get; set; }
    }

    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Post>
    {
        private readonly StackOverflow2010Context _context;

        public GetPostByIdQueryHandler(StackOverflow2010Context context)
        {
            _context = context;
        }

        public async Task<Post> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _context
                    .Posts
                    .SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken: cancellationToken);

            return post;
        }
    }
}
