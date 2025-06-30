using Application.VideoGames.Commands;
using Domain.VideoGames;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Xunit;

namespace Tests.Commands;

public class AddVideoGameCommandTests
{
    private async Task<ApplicationDbContext> CreateDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        return context;
    }

    [Fact]
    public async Task Should_Add_New_VideoGame()
    {
        // Arrange
        var context = await CreateDbContextAsync();
        var handler = new AddVideoGameCommand.Handler(context);

        var command = new AddVideoGameCommand
        {
            Title = "New Game",
            Genre = "Action",
            ReleaseDate = new DateTime(2022, 5, 10)
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var saved = await context.VideoGames.FindAsync(result.VideoGameId);
        Assert.NotNull(saved);
        Assert.Equal("New Game", saved.Title);
    }
}