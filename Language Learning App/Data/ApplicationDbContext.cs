using Microsoft.EntityFrameworkCore;
using Language_Learning_App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Language_Learning_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<FlashcardReview> FlashcardReviews { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Flashcard>()
                .HasMany(f => f.Tags)
                .WithMany(t => t.Flashcards)
                .UsingEntity(j => j.ToTable("FlashcardTags")); // This creates the hidden join table
        }
    }
}