using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EntityFrameworkVerificationApp.Data
{
    public class ShowsContext : DbContext
    {
        public ShowsContext(DbContextOptions<ShowsContext> options) : base(options)
        {
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Theater> Theaters { get; set; }
    }

    public class Show
    {
        public int Id { get; set; }
        public int ShowId { get; set; }
        public Theater Theater { get; set; }
        public ICollection<Session> Sessions { get; set; }
    }

    public class Theater
    {
        public Address LocationAddress { get; set; }
        public int Zones { get; set; }
        public int SeatsPerZone { get; set; }
        public int SeatsPerRow { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class Session
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset Start { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }

    public class Seat
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Zone { get; set; }
        public int Status { get; set; }
    }
}
