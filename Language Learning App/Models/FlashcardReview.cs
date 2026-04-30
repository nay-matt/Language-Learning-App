using System.ComponentModel.DataAnnotations;

namespace Language_Learning_App.Models
{
    public class FlashcardReview
    {
        [Key]
        public int FlashcardReviewID { get; set; }

        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        public int FlashcardID { get; set; }
        public Flashcard Flashcard { get; set; }

        public int Interval { get; set; } = 0;
        public double EaseFactor { get; set; } = 2.5;
        public int TimesReviewed { get; set; } = 0;

        [DataType(DataType.Date)]
        public DateTime NextReviewDate { get; set; } = DateTime.Now;

        public DateTime LastReviewed { get; set; } = DateTime.Now;
    }
}