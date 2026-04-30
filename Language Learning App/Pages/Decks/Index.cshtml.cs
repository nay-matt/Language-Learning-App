using Language_Learning_App.Data;
using Language_Learning_App.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Language_Learning_App.Pages.Decks
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
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
    }
}