namespace Language_Learning_App.Models
{
    public class Flashcard
    {
        public int FlashcardID { get; set; }

        public int DeckID { get; set; }

        public string FrontText { get; set; } = string.Empty;
        public string BackText { get; set; } = string.Empty;

        public int Difficulty { get; set; } = 1;

        public Deck? Deck { get; set; }

        public virtual ICollection<FlashcardReview> FlashcardReviews { get; set; } = new List<FlashcardReview>();
        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}