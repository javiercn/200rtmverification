using EntityFrameworkVerificationApp.Data;
using Microsoft.AspNetCore.Authentication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkVerificationApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Catalog")));

            services.AddDbContext<ReviewsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Reviews")));

            if (Environment.IsDevelopment())
            {
                services.AddTransient<IStartupFilter, DatabaseInitializer>();
            }
            services.AddAzureAdB2CAuthentication();

            services.AddMvc().AddRazorPagesOptions(rpo => rpo.Conventions.AuthorizeFolder("/").AllowAnonymousToFolder("/"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }

        public class DatabaseInitializer : IStartupFilter
        {
            public DatabaseInitializer(IServiceScopeFactory scopeFactory)
            {
                var movies = CreateMovies(30).ToArray();
                var artists = CreateArtists(30).ToArray();
                using (var scope = scopeFactory.CreateScope())
                {
                    var catalog = scope.ServiceProvider.GetRequiredService<CatalogContext>();
                    catalog.Database.EnsureDeleted();
                    catalog.Database.EnsureCreated();
                    catalog.Movies.AddRange(movies);
                    catalog.Artists.AddRange(artists);
                    LinkCast();
                    catalog.SaveChanges();

                    var reviews = scope.ServiceProvider.GetRequiredService<ReviewsContext>();
                    reviews.Database.EnsureDeleted();
                    reviews.Database.EnsureCreated();
                    reviews.Reviews.Add(new Review
                    {
                        TopicId = 12,
                        Details = new ReviewDetails
                        {
                            AuthorId = "44515703-d98e-4e44-8cc9-6d419c7fb954",
                            Author = "Javier",
                            Body = "Some outrageous comment",
                            PublishDate = DateTimeOffset.UtcNow.AddDays(-21)
                        },
                        Replies = new List<Review>
                        {
                            new Review
                            {
                                TopicId = 12,
                                Details = new ReviewDetails
                                {
                                    AuthorId = "44515703-d98e-4e44-8cc9-6d419c7fb954",
                                    Author = "Javier",
                                    Body = "Review reply 1",
                                    PublishDate = DateTimeOffset.UtcNow.AddDays(-20)
                                }
                            },
                            new Review
                            {
                                TopicId = 12,
                                Details = new ReviewDetails
                                {
                                    AuthorId = "44515703-d98e-4e44-8cc9-6d419c7fb954",
                                    Author = "Javier",
                                    Body = "Review reply 2",
                                    PublishDate = DateTimeOffset.UtcNow.AddDays(-20)
                                },
                                Replies = new List<Review>
                                {
                                    new Review
                                    {
                                        TopicId = 12,
                                        Details = new ReviewDetails
                                        {
                                            AuthorId = "44515703-d98e-4e44-8cc9-6d419c7fb954",
                                            Author = "Javier",
                                            Body = "Review reply to reply 2",
                                            PublishDate = DateTimeOffset.UtcNow.AddDays(-20)
                                        }
                                    }
                                }
                            }
                        }
                    });

                    reviews.SaveChanges();
                }

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
                                PosterUrl = $"/Movies/Poster/{i}",
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
                            ImageUrl = $"/Artists/Profile/Image/{i}",
                            PersonalInformation = new PersonalInformation
                            {
                                GivenName = $"Given name {i}",
                                LastName = $"Last name {i}",
                                BirthDate = DateTimeOffset.UtcNow.AddYears(-20 - seed),
                                Biography = string.Join(" ", Enumerable.Repeat($"Biography {i}", 10)),
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

            public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => next;
        }
    }
}