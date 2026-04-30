namespace Language_Learning_App.Models
{
    public class Deck
    {
        public int DeckID { get; set; }
        public string UserID { get; set; }
        public int LanguageID { get; set; }
        public string Name { get; set; } = string.Empty;
        public Language? Language { get; set; }
        public ICollection<Flashcard>? Flashcards { get; set; }

    }
}
