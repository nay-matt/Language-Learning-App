namespace Language_Learning_App.Models
{
    public class DeckStudyStatus
    {
        public string Language { get; set; } = string.Empty;
        public string DeckName { get; set; } = string.Empty;
        public int DeckID { get; set; }
        public int TotalCards { get; set; }
        public int CardsDueToday { get; set; }
        public string UserID { get; set; } = string.Empty;
    }
}