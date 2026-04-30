using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Language_Learning_App.Models;
using Language_Learning_App.Data;
using Microsoft.AspNetCore.Identity;

namespace Language_Learning_App.Pages.Flashcards
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Flashcard Flashcard { get; set; } = default!;
        public FlashcardReview? CurrentReview { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            // Load the card and include its reviews
            Flashcard = await _context.Flashcards
                .Include(f => f.Deck)
                .Include(f => f.FlashcardReviews)
                .FirstOrDefaultAsync(m => m.FlashcardID == id);

            if (Flashcard == null) return NotFound();

            // Find the specific review record for the logged-in user
            CurrentReview = Flashcard.FlashcardReviews
                .FirstOrDefault(r => r.UserID == userId);

            return Page();
        }
    }
}