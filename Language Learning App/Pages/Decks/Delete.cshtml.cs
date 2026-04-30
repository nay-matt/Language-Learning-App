using Language_Learning_App.Data;
using Language_Learning_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Language_Learning_App.Pages.Decks
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public DeleteModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public Deck Deck { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Deck = await _context.Decks
                .Include(d => d.Language)
                .FirstOrDefaultAsync(m => m.DeckID == id);

            if (Deck == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var deckToDelete = await _context.Decks.FindAsync(id);

            if (deckToDelete != null)
            {
                _context.Decks.Remove(deckToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
