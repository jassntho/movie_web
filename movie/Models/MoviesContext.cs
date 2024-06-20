using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Movie_Web.Models;

public partial class MoviesContext : DbContext
{
    public MoviesContext()
    {
    }

    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Parameter> Parameters { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-FC9EO7E;Initial Catalog=MOVIES;User ID=hoangtung;Password=Atng1234567890;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("ACCOUNTS");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_ACCOUNTS_ROLES");
        });

        modelBuilder.Entity<Actor>(entity =>
        {
            entity.ToTable("ACTORS");

            entity.Property(e => e.ActorId).HasColumnName("ActorID");
            entity.Property(e => e.ActorAlias).HasMaxLength(50);
            entity.Property(e => e.ActorName).HasMaxLength(255);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("CATEGORIES");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("COUNTRIES");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.CountryAlias).HasMaxLength(50);
            entity.Property(e => e.CountryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("MOVIES");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Director).HasMaxLength(255);
            entity.Property(e => e.DirectorAlias).HasMaxLength(255);
            entity.Property(e => e.MovieName).HasMaxLength(255);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Country).WithMany(p => p.Movies)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK_MOVIES_COUNTRIES");

            entity.HasOne(d => d.Type).WithMany(p => p.Movies)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_MOVIES_TYPES");

            entity.HasMany(d => d.Actors).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MoviesActor",
                    r => r.HasOne<Actor>().WithMany()
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MOVIES_ACTORS_ACTORS"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MOVIES_ACTORS_MOVIES"),
                    j =>
                    {
                        j.HasKey("MovieId", "ActorId");
                        j.ToTable("MOVIES_ACTORS");
                        j.IndexerProperty<int>("MovieId").HasColumnName("MovieID");
                        j.IndexerProperty<int>("ActorId").HasColumnName("ActorID");
                    });

            entity.HasMany(d => d.Categories).WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MoviesCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MOVIES_CATEGORIES_CATEGORIES"),
                    l => l.HasOne<Movie>().WithMany()
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MOVIES_CATEGORIES_MOVIES"),
                    j =>
                    {
                        j.HasKey("MovieId", "CategoryId");
                        j.ToTable("MOVIES_CATEGORIES");
                        j.IndexerProperty<int>("MovieId").HasColumnName("MovieID");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("CategoryID");
                    });
        });

        modelBuilder.Entity<Parameter>(entity =>
        {
            entity.ToTable("PARAMETERS");

            entity.Property(e => e.ParameterId).HasColumnName("ParameterID");
            entity.Property(e => e.ParameterName).HasMaxLength(50);
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.HasKey(e => new { e.MovieId, e.AccountId });

            entity.ToTable("RATES");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Rate1).HasColumnName("Rate");

            entity.HasOne(d => d.Account).WithMany(p => p.Rates)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RATES_ACCOUNTS");

            entity.HasOne(d => d.Movie).WithMany(p => p.Rates)
                .HasForeignKey(d => d.MovieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RATES_MOVIES");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("ROLES");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(255);
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.ToTable("TYPES");

            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.TypeAlias)
                .HasMaxLength(50)
                .HasColumnName("TypeALias");
            entity.Property(e => e.TypeName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
