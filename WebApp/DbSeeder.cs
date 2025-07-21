using Domain.VideoGames;
using Persistence;

namespace WebApp;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        try
        {
            // Ensure the database is created
            context.Database.EnsureCreated();

            // Skip seeding if already populated
            if (context.VideoGames.Any())
                return;
            
            List<VideoGame> games = new()
            {
                new VideoGame
                {
                    Title = "The Legend of Zelda: Breath of the Wild", Genre = "Adventure",
                    ReleaseDate = new DateTime(2017, 3, 3)
                },
                new VideoGame { Title = "Halo Infinite", Genre = "Shooter", ReleaseDate = new DateTime(2021, 12, 8) },
                new VideoGame { Title = "God of War", Genre = "Action", ReleaseDate = new DateTime(2018, 4, 20) },
                new VideoGame { Title = "Elden Ring", Genre = "RPG", ReleaseDate = new DateTime(2022, 2, 25) },
                new VideoGame { Title = "Minecraft", Genre = "Sandbox", ReleaseDate = new DateTime(2011, 11, 18) },
                new VideoGame
                    { Title = "Stardew Valley", Genre = "Simulation", ReleaseDate = new DateTime(2016, 2, 26) },
                new VideoGame { Title = "Cyberpunk 2077", Genre = "RPG", ReleaseDate = new DateTime(2020, 12, 10) },
                new VideoGame { Title = "The Witcher 3", Genre = "RPG", ReleaseDate = new DateTime(2015, 5, 19) },
                new VideoGame
                    { Title = "Grand Theft Auto V", Genre = "Action", ReleaseDate = new DateTime(2013, 9, 17) },
                new VideoGame
                    { Title = "Super Mario Odyssey", Genre = "Platformer", ReleaseDate = new DateTime(2017, 10, 27) },
                new VideoGame
                    { Title = "Red Dead Redemption 2", Genre = "Action", ReleaseDate = new DateTime(2018, 10, 26) },
                new VideoGame { Title = "Hades", Genre = "Roguelike", ReleaseDate = new DateTime(2020, 9, 17) },
                new VideoGame { Title = "Celeste", Genre = "Platformer", ReleaseDate = new DateTime(2018, 1, 25) },
                new VideoGame { Title = "Tunic", Genre = "Adventure", ReleaseDate = new DateTime(2022, 3, 16) },
                new VideoGame
                    { Title = "Slay the Spire", Genre = "Card Game", ReleaseDate = new DateTime(2019, 1, 23) },
                new VideoGame { Title = "Fortnite", Genre = "Battle Royale", ReleaseDate = new DateTime(2017, 7, 21) },
                new VideoGame { Title = "Valorant", Genre = "Shooter", ReleaseDate = new DateTime(2020, 6, 2) },
                new VideoGame { Title = "Overwatch", Genre = "Shooter", ReleaseDate = new DateTime(2016, 5, 24) },
                new VideoGame
                    { Title = "Apex Legends", Genre = "Battle Royale", ReleaseDate = new DateTime(2019, 2, 4) },
                new VideoGame { Title = "Terraria", Genre = "Sandbox", ReleaseDate = new DateTime(2011, 5, 16) }
            };

            context.VideoGames.AddRange(games);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
