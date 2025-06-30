using Domain.VideoGames;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal class VideoGamesConfiguration :
    IEntityTypeConfiguration<VideoGame>
{
    public void Configure(EntityTypeBuilder<VideoGame> builder)
    {
        builder.ToTable("VideoGames");

        builder.HasKey(p => p.VideoGameId);
        builder.Property(p => p.VideoGameId)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Genre).HasMaxLength(50);
        builder.Property(p => p.Title).HasMaxLength(50);
    }
}