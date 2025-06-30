using Application.VideoGames.Queries;
using Domain.VideoGames;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Xunit;

namespace Tests.Queries;

public class GetVideoGameDetailByIdQueryTests
{
    private async Task<ApplicationDbContext> SeedDbAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        context.VideoGames.Add(new VideoGame
        {
            VideoGameId = 1,
            Title = "Test Game",
            Genre = "Adventure",
            ReleaseDate = new DateTime(2022, 8, 8)
        });

        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task Should_Return_VideoGame_By_Id()
    {
        // Arrange
        var context = await SeedDbAsync();
        var handler = new GetVideoGameDetailByIdQuery.Handler(context);
        var query = new GetVideoGameDetailByIdQuery { VideoGameId = 1 };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Game", result.Title);
        Assert.Equal("Adventure", result.Genre);
    }

    [Fact]
    public async Task Should_Return_Null_For_Invalid_Id()
    {
        // Arrange
        var context = await SeedDbAsync();
        var handler = new GetVideoGameDetailByIdQuery.Handler(context);
        var query = new GetVideoGameDetailByIdQuery { VideoGameId = 99 };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}