using Application.VideoGames.Queries;
using Domain.VideoGames;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Xunit;

namespace Tests.Queries;

public class GetAllVideoGamesQueryTests
{
    private async Task<ApplicationDbContext> SeedDbAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        context.VideoGames.AddRange(
            new VideoGame { Title = "Game 1", Genre = "RPG", ReleaseDate = new DateTime(2020, 1, 1) },
            new VideoGame { Title = "Game 2", Genre = "Shooter", ReleaseDate = new DateTime(2021, 5, 10) }
        );

        await context.SaveChangesAsync();
        return context;
    }

    [Fact]
    public async Task Should_Return_All_VideoGames()
    {
        // Arrange
        var context = await SeedDbAsync();
        var handler = new GetAllVideoGamesQuery.Handler(context);
        var query = new GetAllVideoGamesQuery();

        // Act
        IEnumerable<GetAllVideoGamesQuery.Result> result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        // Assert.Equal(2, result.Count);
        Assert.True(result.Any(vg => vg.Title == "Game 1"), "Expected to find Game 1");
        Assert.True(result.Any(vg => vg.Title == "Game 2"), "Expected to find Game 2");
    }

}