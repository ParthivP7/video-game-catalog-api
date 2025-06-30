using Application.VideoGames.Commands;
using Domain.VideoGames;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Xunit;

namespace Tests.Commands;

public class DeleteVideoGameCommandTests
{
    private async Task<ApplicationDbContext> CreateDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        context.VideoGames.Add(new VideoGame
        {
            VideoGameId = 1,
            Title = "Game To Delete",
            Genre = "Arcade",
            ReleaseDate = new DateTime(2015, 5, 5)
        });

        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task Should_Mark_VideoGame_As_Deleted()
    {
        // Arrange
        var context = await CreateDbContextAsync();
        var handler = new DeleteVideoGameCommand.Handler(context);

        var command = new DeleteVideoGameCommand
        {
            VideoGameId = 1
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var deleted = await context.VideoGames.FindAsync(result.VideoGameId);
        Assert.NotNull(deleted);
        Assert.NotNull(deleted.DeletedAt);
    }
}