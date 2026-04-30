namespace Language_Learning_App.Models
{
    public class Tag
    {
        public int TagID { get; set; }
        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
    }
}