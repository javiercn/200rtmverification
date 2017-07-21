using EntityFrameworkVerificationApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EntityFrameworkVerificationApp.Tests
{
    public class BasicTests
    {
        [Fact]
        public async Task CanCreateBillingContextAsync()
        {
            var context = CreateDbContext<BillingContext>();
            await context.Database.EnsureCreatedAsync();
        }

        [Fact]
        public async Task CanCreateShowsContextAsync()
        {
            var context = CreateDbContext<ShowsContext>();
            await context.Database.EnsureCreatedAsync();
        }

        [Fact]
        public async Task CanCreateCatalogContextAsync()
        {
            var context = CreateDbContext<CatalogContext>();
            await context.Database.EnsureCreatedAsync();
        }

        [Fact]
        public async Task CanInsertCatalogSeedDataAsync()
        {
            var context = CreateDbContext<CatalogContext>();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            var movies = CreateMovies(10).ToArray();
            var artists = CreateArtists(10).ToArray();
            context.AddRange(movies);
            context.AddRange(artists);
            LinkCast();

            await context.SaveChangesAsync();

            IEnumerable<Movie> CreateMovies(int seed)
            {
                var movieList = new List<Movie>();
                for (int i = 1; i <= seed; i++)
                {
                    var movie = new Movie
                    {
                        Title = $"Movie {i}",
                        Description = $"Movie description {i}",
                        Details = new MovieDetails
                        {
                            Country = $"Country {i}",
                            Language = $"Language {i}",
                            PosterUrl = $"/Poster/{i}",
                            ReleaseDate = DateTimeOffset.UtcNow.AddDays(-i * 15)
                        },
                    };
                    movieList.Add(movie);
                }

                return movieList;
            }

            IEnumerable<Artist> CreateArtists(int seed)
            {
                for (int i = 1; i <= seed; i++)
                {
                    yield return new Artist
                    {
                        ImageUrl = $"/Profile/{i}",
                        PersonalInformation = new PersonalInformation
                        {
                            GivenName = $"Given name {i}",
                            LastName = $"Last name {i}",
                            BirthDate = DateTimeOffset.UtcNow.AddYears(-20 - seed),
                            BirthPlace = new BirthPlace
                            {
                                City = $"City {i}",
                                Country = $"Country {i}"
                            }
                        }
                    };
                }
            }

            void LinkCast()
            {
                for (int i = 1; i <= movies.Length; i++)
                {
                    var cast = new List<MovieArtist>();
                    for (int j = 1; j <= Math.Min(i, artists.Length); j++)
                    {
                        var link = new MovieArtist
                        {
                            ArtistId = artists[j - 1].Id,
                            MovieId = movies[i - 1].Id
                        };

                        cast.Add(link);
                    }

                    movies[i - 1].Cast = cast;
                }
            }
        }

        [Fact]
        public async Task CanInsertShowsSeedDataAsync()
        {
            var context = CreateDbContext<ShowsContext>();
            await context.Database.EnsureCreatedAsync();

            context.Shows.AddRange(CreateShows(10));

            IEnumerable<Show> CreateShows(int seed)
            {
                for (int i = 0; i < seed; i++)
                {
                    var show = new Show
                    {
                        Id = i,
                        MovieId = i / 2,
                        Theater = new Theater
                        {
                            Id = i % 2,
                            Location = new Address
                            {
                                Street = $"Street {i % 2}",
                                City = $"City {i % 2}",
                                Province = $"State {i % 2}",
                                Country = $"Country {i % 2}",
                                ZipCode = new string((i % 2).ToString()[0], 5),
                            },
                            Zones = ((i % 2) + 1) * 4,
                            RowsPerZone = ((i % 2) + 1) * 10,
                            SeatsPerRow = ((i % 2) + 1) * 10
                        },
                    };

                    show.Sessions = CreateSessions(seed, show).ToList();
                    yield return show;
                }

                yield break;
            }

            IEnumerable<Session> CreateSessions(int number, Show show)
            {
                for (int i = 1; i <= 2; i++)
                {
                    yield return new Session
                    {
                        Id = show.Id,
                        Price = 10 + i,
                        Start = DateTimeOffset.UtcNow.AddDays(-number),
                        Seats = CreateSeats(show).ToList()
                    };

                }
            }

            IEnumerable<Seat> CreateSeats(Show show)
            {
                for (int i = 0; i < show.Theater.Zones; i++)
                {
                    for (int j = 0; j < show.Theater.RowsPerZone * show.Theater.SeatsPerRow; j++)
                    {
                        yield return new Seat
                        {
                            Id = i * show.Theater.Zones + j,
                            Number = j,
                            Zone = i,
                            Status = 0
                        };
                    }
                }

                yield break;
            }
        }

        public TContext CreateDbContext<TContext>(string dataBaseName = null)
            where TContext : DbContext
        {
            var services = new ServiceCollection();
            services.AddDbContext<TContext>(options =>
                options.UseSqlServer(GenerateConnectionString(dataBaseName)));
            var provider = services.BuildServiceProvider();
            return provider.GetRequiredService<TContext>();
        }

        private string GenerateConnectionString(string databaseName) => $"Server=(localdb)\\mssqllocaldb;Database={databaseName ?? "EntityFrameworkVerificationApp-" + Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=true";
    }
}
