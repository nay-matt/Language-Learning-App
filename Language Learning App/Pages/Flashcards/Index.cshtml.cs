using Language_Learning_App.Data;
using Language_Learning_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Language_Learning_App.Pages.Flashcards
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
        public int DeckID { get; set; }

        public async Task OnGetAsync(int deckId)
        {
            DeckID = deckId;

            var userId = _userManager.GetUserId(User);

            Flashcards = await _context.Flashcards
                .Include(f => f.Tags)
                .Include(f => f.FlashcardReviews.Where(r => r.UserID == userId))
                .Where(f => f.DeckID == deckId)
                .ToListAsync();
        }
    }
}