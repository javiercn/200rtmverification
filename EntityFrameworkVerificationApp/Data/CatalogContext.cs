using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EntityFrameworkVerificationApp.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<MovieArtist> MovieArtists { get; set; }
        public DbSet<MovieDetails> MovieDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>(movie =>
            {
                movie
                    .HasMany(m => m.Cast)
                    .WithOne(o => o.Movie)
                    .HasForeignKey(f => f.MovieId);
                movie.HasOne(m => m.Details);
            });

            modelBuilder.Entity<Artist>(artist =>
            {
                artist
                    .HasMany(a => a.Movies)
                    .WithOne(o => o.Artist)
                    .HasForeignKey(f => f.ArtistId);
                artist.OwnsOne(a => a.PersonalInformation, builder => builder.OwnsOne(pi => pi.BirthPlace));
            });

            modelBuilder.Entity<MovieArtist>(movieArtist =>
            {
                movieArtist.HasKey(ma => new { ma.MovieId, ma.ArtistId });
                movieArtist
                    .HasOne(a => a.Artist)
                    .WithMany(a => a.Movies)
                    .HasForeignKey(a => a.ArtistId);
                movieArtist
                    .HasOne(a => a.Movie)
                    .WithMany(m => m.Cast)
                    .HasForeignKey(m => m.MovieId);                    
            });
        }
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public MovieDetails Details { get; set; }
        public ICollection<MovieArtist> Cast { get; set; }
    }

    public class MovieDetails
    {
        public int Id { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string PosterUrl { get; set; }
    }

    public class MovieArtist
    {
        public int MovieId { get; set; }
        public int ArtistId { get; set; }
        public Movie Movie { get; set; }
        public Artist Artist { get; set; }
    }

    public class Artist
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public PersonalInformation PersonalInformation { get; set; }
        public ICollection<MovieArtist> Movies { get; set; }
    }

    public class PersonalInformation
    {
        public string GivenName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public BirthPlace BirthPlace { get; set; }
        public string Biography { get; set; }
    }

    public class BirthPlace
    {
        public string City { get; set; }
        public string Country { get; set; }
    }
}
