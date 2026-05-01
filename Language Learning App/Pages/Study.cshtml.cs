using Language_Learning_App.Data;
using Language_Learning_App.Models;
using Language_Learning_App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Language_Learning_App.Pages
{
    public class StudyModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly SRSService _srsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudyModel(ApplicationDbContext context, SRSService srsService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _srsService = srsService;
            _userManager = userManager;
        }

        public FlashcardReview CurrentReview { get; set; }
        public int RemainingCount { get; set; }
        public List<Deck> AvailableDecks { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? DeckId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? deckId)
        {
            if (deckId.HasValue) DeckId = deckId;
            var userId = _userManager.GetUserId(User);

            AvailableDecks = await _context.Decks
                .Where(d => d.UserID == userId)
                .ToListAsync() ?? new List<Deck>();

            var query = _context.FlashcardReviews
                .Include(r => r.Flashcard)
                .Where(r => r.UserID == userId && r.NextReviewDate <= DateTime.Now);

            if (DeckId.HasValue && DeckId.Value > 0)
            {
                query = query.Where(r => r.Flashcard.DeckID == DeckId.Value);
            }

            RemainingCount = await query.CountAsync();

            CurrentReview = await query.OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostRateAsync(int reviewId, int quality)
        {
            var review = await _context.FlashcardReviews
                .Include(r => r.Flashcard)
                .FirstOrDefaultAsync(r => r.FlashcardReviewID == reviewId);

            if (review != null)
            {
                _srsService.CalculateNextReview(review, quality);
                await _context.SaveChangesAsync();

                DeckId = review.Flashcard.DeckID;
            }

            return RedirectToPage(new { deckId = DeckId });
        }
    }
}