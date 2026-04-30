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
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Flashcard Flashcard { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var flashcard = await _context.Flashcards
                .Include(f => f.Deck)
                .FirstOrDefaultAsync(m => m.FlashcardID == id);

            if (flashcard == null) return NotFound();

            Flashcard = flashcard;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            var flashcard = await _context.Flashcards.FindAsync(id);

            if (flashcard != null)
            {
                int deckId = flashcard.DeckID;
                Flashcard = flashcard;
                _context.Flashcards.Remove(Flashcard);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index", new { deckId = deckId });
            }

            return RedirectToPage("./Index");
        }
    }
}