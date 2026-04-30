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
        public DbSet<DeckStudyStatus> DeckStudyStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Always call the base method first for Identity to work
            base.OnModelCreating(builder);

            // 1. Configure the Many-to-Many relationship (Flashcards <-> Tags)
            builder.Entity<Flashcard>()
                .HasMany(f => f.Tags)
                .WithMany(t => t.Flashcards)
                .UsingEntity(j => j.ToTable("FlashcardTags"));

            // 2. Configure the Database View (DeckStudyStatus)
            builder.Entity<DeckStudyStatus>(eb =>
            {
                eb.HasNoKey();
                eb.ToView("view_DeckStudyStatus");
            });
        }
    }
}