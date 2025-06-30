using Domain.VideoGames;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.VideoGames.Commands;

public class DeleteVideoGameCommand : IRequest<DeleteVideoGameCommand.Result>
{
    public int VideoGameId { get; set; }

    public class Validator : AbstractValidator<DeleteVideoGameCommand>
    {
        public Validator()
        {
            RuleFor(p => p.VideoGameId).GreaterThan(0);
        }
    }

    public class Handler : IRequestHandler<DeleteVideoGameCommand, Result>
    {
        private readonly ApplicationDbContext _context;

        public Handler(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteVideoGameCommand command, CancellationToken cancellationToken)
        {
            VideoGame? existingGame = await _context.VideoGames.FindAsync(command.VideoGameId, cancellationToken);
            if (existingGame == null)
                return new Result { };

            existingGame.DeletedAt = DateTime.UtcNow;
            
            await _context.SaveChangesAsync(cancellationToken);

            return new Result
            {
                VideoGameId = existingGame.VideoGameId
            };
        }
    }

    public sealed class Result
    {
        public int VideoGameId { get; set; }
    }
}