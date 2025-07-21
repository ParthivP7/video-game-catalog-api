using Application.VideoGames.Commands;
using Microsoft.EntityFrameworkCore;
using Persistence;
using MediatR;

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
            ReleaseDate = new DateTime(2024, 5, 10)
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var saved = await context.VideoGames.FindAsync(result.VideoGameId);
        Assert.NotNull(saved);
        Assert.Equal("New Game", saved.Title);
    }

    [Fact]
    public async Task SanitizationBehavior_ShouldRemoveScriptTags_AndPreserveText()
    {
        // Arrange
        var command = new AddVideoGameCommand
        {
            Title = "<script>alert('XSS')</script>Game",
            Genre = "Action<script>alert(1)</script>",
            ReleaseDate = DateTime.Today
        };

        var behavior = new SanitizationBehavior<AddVideoGameCommand, Unit>();
        

        Task<Unit> NextHandler(CancellationToken cancellationToken) => Task.FromResult(Unit.Value);

        // Act
        var result = await behavior.Handle(command, NextHandler, CancellationToken.None);

        // Assert
        Assert.DoesNotContain("<script>", command.Title);
        Assert.DoesNotContain("<script>", command.Genre);
        Assert.Equal("Game", command.Title);
        Assert.Equal("Action", command.Genre);
    }
}
