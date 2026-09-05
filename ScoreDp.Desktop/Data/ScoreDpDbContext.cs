using Microsoft.EntityFrameworkCore;
using ScoreDp.Desktop.Models;

namespace ScoreDp.Desktop.Data;

public class ScoreDpDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<GameVersion> Versions => Set<GameVersion>();
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<Score> Scores => Set<Score>();
    public DbSet<Option> Options => Set<Option>();

    public ScoreDpDbContext(DbContextOptions<ScoreDpDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.IidxId).HasColumnName("iidx_id");
            e.Property(x => x.DjName).HasColumnName("dj_name");
            e.Property(x => x.PasswordHash).HasColumnName("password_hash");
        });

        modelBuilder.Entity<GameVersion>(e =>
        {
            e.ToTable("versions");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Song>(e =>
        {
            e.ToTable("songs");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.ZasaId).HasColumnName("zasa_id");
            e.Property(x => x.Title).HasColumnName("title");
            e.Property(x => x.TitleNormalized).HasColumnName("title_normalized");
            e.Property(x => x.Level).HasColumnName("level");
            e.Property(x => x.Chart).HasColumnName("chart");
            e.Property(x => x.UnofficialLevel).HasColumnName("unofficial_level");
            e.Property(x => x.VersionId).HasColumnName("version_id");
            e.HasOne(x => x.Version).WithMany().HasForeignKey(x => x.VersionId);
        });

        modelBuilder.Entity<Score>(e =>
        {
            e.ToTable("scores");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.SongId).HasColumnName("song_id");
            e.Property(x => x.ClearType).HasColumnName("clear_type");
            e.Property(x => x.ScoreValue).HasColumnName("score");
            e.Property(x => x.DjLevel).HasColumnName("dj_level");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            e.HasOne(x => x.Song).WithMany().HasForeignKey(x => x.SongId);
        });

        modelBuilder.Entity<Option>(e =>
        {
            e.ToTable("options");
            e.HasKey(x => new { x.UserId, x.SongId });
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.SongId).HasColumnName("song_id");
            e.Property(x => x.Flip).HasColumnName("flip");
            e.Property(x => x.LeftArr).HasColumnName("left_arr");
            e.Property(x => x.RightArr).HasColumnName("right_arr");
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            e.HasOne(x => x.Song).WithMany().HasForeignKey(x => x.SongId);
        });
    }
}
