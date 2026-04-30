using Language_Learning_App.Data;
using Language_Learning_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Language_Learning_App.Pages.Flashcards
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public EditModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public Flashcard Flashcard { get; set; }
        public string TagInput { get; set; } = string.Empty;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Flashcard = await _context.Flashcards
                .Include(f => f.Tags)
                .FirstOrDefaultAsync(m => m.FlashcardID == id);

            if (Flashcard == null) return NotFound();

            TagInput = string.Join(", ", Flashcard.Tags.Select(t => t.Name));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var cardToUpdate = await _context.Flashcards
                .Include(f => f.Tags)
                .FirstOrDefaultAsync(f => f.FlashcardID == id);

            if (cardToUpdate == null) return NotFound();

            cardToUpdate.FrontText = Flashcard.FrontText;
            cardToUpdate.BackText = Flashcard.BackText;

            cardToUpdate.Tags.Clear();

            if (!string.IsNullOrWhiteSpace(TagInput))
            {
                var tagNames = TagInput.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var name in tagNames)
                {
                    var trimmedName = name.Trim();
                    var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == trimmedName)
                              ?? new Tag { Name = trimmedName };

                    cardToUpdate.Tags.Add(tag);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index", new { deckId = cardToUpdate.DeckID });
        }
    }
}
