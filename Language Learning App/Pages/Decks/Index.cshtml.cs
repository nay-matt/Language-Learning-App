using System.Security.Claims;
using Language_Learning_App.Data;
using Language_Learning_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Language_Learning_App.Pages.Decks
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

        public List<Deck> Decks { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Decks = await _context.Decks
                .Include(d => d.Language)
                .Where(d => d.UserID == userId)
                .ToListAsync();
        }
        public async Task<IActionResult> OnPostResetDeckAsync(int deckId)
        {
            var userId = _userManager.GetUserId(User);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC dbo.usp_ResetDeckProgress @DeckID = {0}, @UserID = {1}",
                deckId, userId);

            return RedirectToPage();
        }
    }
}