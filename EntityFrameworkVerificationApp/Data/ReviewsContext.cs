using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkVerificationApp.Data
{
    public class ReviewsContext : DbContext
    {
        public ReviewsContext(DbContextOptions<ReviewsContext> context) : base(context)
        {
        }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Review>(review =>
            {
                review
                    .HasMany(r => r.Replies)
                    .WithOne(r => r.ReplyTo)
                    .IsRequired(false)
                    .HasPrincipalKey(r => r.Id)
                    .HasForeignKey(r => r.ReplyToId);
                review.OwnsOne(r => r.Details);
            });
        }
    }

    public class Review
    {
        public int Id { get; set; }
        public int? ReplyToId { get; set; }
        public int TopicId { get; set; }
        public Review ReplyTo { get; set; }
        public ReviewDetails Details { get; set; }
        public ICollection<Review> Replies { get; set; }
    }

    public class ReviewDetails
    {
        public string AuthorId { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public DateTimeOffset PublishDate { get; set; }
    }
}
