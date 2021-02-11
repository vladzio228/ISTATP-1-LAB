using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace GameLandWebApplication
{
    public partial class GameLandContext : DbContext
    {
        public GameLandContext()
        {
        }

        public GameLandContext(DbContextOptions<GameLandContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Developer> Developers { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GamesGenre> GamesGenres { get; set; }
        public virtual DbSet<GamesPlatform> GamesPlatforms { get; set; }
        public virtual DbSet<GamesSystemRequirement> GamesSystemRequirements { get; set; }
        public virtual DbSet<GamesUser> GamesUsers { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<SystemRequirement> SystemRequirements { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= WIN-GIPN47V20D7; Database=GameLand; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Developer>(entity =>
            {
                entity.Property(e => e.DeveloperId).HasColumnName("DeveloperID");

                entity.Property(e => e.DeveloperName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.DeveloperId).HasColumnName("DeveloperID");

                entity.Property(e => e.GameName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Photo).HasColumnType("image");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.HasOne(d => d.Developer)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.DeveloperId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Games_Developers");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Games_Publishers");
            });

            modelBuilder.Entity<GamesGenre>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.GenreId).HasColumnName("GenreID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamesGenres)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamesGenres_Games");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.GamesGenres)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamesGenres_Genres");
            });

            modelBuilder.Entity<GamesPlatform>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.PlatformId).HasColumnName("PlatformID");

                entity.Property(e => e.ReleaseDate).HasColumnType("date");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamesPlatforms)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamesPlatforms_Games");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.GamesPlatforms)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamesPlatforms_Platforms");
            });

            modelBuilder.Entity<GamesSystemRequirement>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cpu)
                    .HasMaxLength(50)
                    .HasColumnName("CPU");

                entity.Property(e => e.DirectX).HasMaxLength(50);

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.Os)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("OS");

                entity.Property(e => e.Ram)
                    .HasMaxLength(50)
                    .HasColumnName("RAM");

                entity.Property(e => e.SpaceOnStorage).HasMaxLength(50);

                entity.Property(e => e.SystemRequirementId).HasColumnName("SystemRequirementID");

                entity.Property(e => e.Videocard).HasMaxLength(50);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamesSystemRequirements)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamesSystemRequirements_Games");

                entity.HasOne(d => d.SystemRequirement)
                    .WithMany(p => p.GamesSystemRequirements)
                    .HasForeignKey(d => d.SystemRequirementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamesSystemRequirements_SystemRequirements");
            });

            modelBuilder.Entity<GamesUser>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GamesUsers)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamesUsers_Games");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GamesUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GamesUsers_Users");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.Property(e => e.GenreId).HasColumnName("GenreID");

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.Property(e => e.PlatformName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.PublisherName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SystemRequirement>(entity =>
            {
                entity.Property(e => e.SystemRequirementId).HasColumnName("SystemRequirementID");

                entity.Property(e => e.SystemRequirementName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NickName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NumberOfratedGames).HasColumnName("NumberOFRatedGames");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RegistrationDate).HasColumnType("date");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
