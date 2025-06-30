using Domain.VideoGames;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.VideoGames.Commands;

public class AddVideoGameCommand : IRequest<AddVideoGameCommand.Result>
{
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public DateTime ReleaseDate { get; set; }

    public class Validator : AbstractValidator<AddVideoGameCommand>
    {
        public Validator()
        {
            RuleFor(p => p.Title).NotEmpty();
            RuleFor(p => p.Genre).NotEmpty();
            RuleFor(p => p.ReleaseDate).GreaterThan(DateTime.MinValue);
        }
    }

    public class Handler : IRequestHandler<AddVideoGameCommand, Result>
    {
        private readonly ApplicationDbContext _context;

        public Handler(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(AddVideoGameCommand command, CancellationToken cancellationToken)
        {
            VideoGame newGame = new()
            {
                Title = command.Title,
                Genre = command.Genre,
                ReleaseDate = command.ReleaseDate
            };

            _context.VideoGames.Add(newGame);
            await _context.SaveChangesAsync(cancellationToken);

            return new Result
            {
                VideoGameId = newGame.VideoGameId
            };
        }
    }

    public sealed class Result
    {
        public int VideoGameId { get; set; }
    }
}