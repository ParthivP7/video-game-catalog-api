using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.VideoGames.Queries;

public class GetVideoGameDetailByIdQuery : IRequest<GetVideoGameDetailByIdQuery.Result?>
{
    public int VideoGameId { get; set; }

    public class Handler : IRequestHandler<GetVideoGameDetailByIdQuery, Result?>
    {
        private readonly ApplicationDbContext _context;

        public Handler(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result?> Handle(GetVideoGameDetailByIdQuery query, CancellationToken cancellationToken)
        {
            Result? videoGame = await _context.VideoGames
                .AsNoTracking()
                .Where(p => p.VideoGameId == query.VideoGameId && p.DeletedAt == null)
                .Select(v => new Result
                {
                    VideoGameId = v.VideoGameId,
                    Title = v.Title,
                    Genre = v.Genre,
                    ReleaseDate = v.ReleaseDate,
                    CreatedAt = v.CreatedAt,
                    UpdatedAt = v.UpdatedAt
                })
                .SingleOrDefaultAsync(cancellationToken);

            return videoGame;
        }
    }


    public record Result
    {
        public int VideoGameId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}