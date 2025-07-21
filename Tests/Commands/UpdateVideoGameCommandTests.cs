using Application.VideoGames.Commands;
using Domain.VideoGames;
using Microsoft.EntityFrameworkCore;
using Persistence;
using MediatR;

namespace Tests.Commands;

public class UpdateVideoGameCommandTests
{
    private async Task<ApplicationDbContext> CreateDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        // Seed existing game
        context.VideoGames.Add(new VideoGame
        {
            VideoGameId = 1,
            Title = "Old Title",
            Genre = "Old Genre",
            ReleaseDate = new DateTime(2020, 1, 1)
        });

        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task Should_Update_VideoGame_Details()
    {
        // Arrange
        var context = await CreateDbContextAsync();
        var handler = new UpdateVideoGameCommand.Handler(context);

        var command = new UpdateVideoGameCommand
        {
            VideoGameId = 1,
            Title = "Updated Title",
            Genre = "Adventure",
            ReleaseDate = new DateTime(2023, 5, 1)
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var updated = await context.VideoGames.FindAsync(result.VideoGameId);
        Assert.NotNull(updated);
        Assert.Equal("Updated Title", updated.Title);
        Assert.Equal("Adventure", updated.Genre);
    }

    [Fact]
    public async Task SanitizationBehavior_ShouldRemoveScriptTags_AndPreserveText()
    {
        // Arrange
        var command = new UpdateVideoGameCommand
        {
            VideoGameId = 1,
            Title = "<script>alert('x')</script>Updated",
            Genre = "Adventure<script>alert('hack')</script>",
            ReleaseDate = DateTime.Today
        };

        var behavior = new SanitizationBehavior<UpdateVideoGameCommand, Unit>();
        Task<Unit> NextHandler(CancellationToken cancellationToken) => Task.FromResult(Unit.Value);

        // Act
        var result = await behavior.Handle(command, NextHandler, CancellationToken.None);

        // Assert
        Assert.DoesNotContain("<script>", command.Title);
        Assert.DoesNotContain("<script>", command.Genre);
        Assert.Equal("Updated", command.Title);
        Assert.Equal("Adventure", command.Genre);
    }
}
