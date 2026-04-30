using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Language_Learning_App.Models;
using Language_Learning_App.Data;
using Language_Learning_App.Services;
using Microsoft.AspNetCore.Identity;

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

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            // Get the count of all cards due today
            RemainingCount = await _context.FlashcardReviews
                .CountAsync(r => r.UserID == userId && r.NextReviewDate <= DateTime.Now);

            // Fetch the first card that is due
            CurrentReview = await _context.FlashcardReviews
                .Include(r => r.Flashcard)
                .Where(r => r.UserID == userId && r.NextReviewDate <= DateTime.Now)
                .FirstOrDefaultAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostRateAsync(int reviewId, int quality)
        {
            var review = await _context.FlashcardReviews.FindAsync(reviewId);
            if (review == null) return RedirectToPage();

            // Use your service to calculate the next date!
            _srsService.CalculateNextReview(review, quality);

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}