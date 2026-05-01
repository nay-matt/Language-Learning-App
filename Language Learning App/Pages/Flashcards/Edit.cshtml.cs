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

        public List<Tag> AllTags { get; set; }

        [BindProperty]
        public string TagString { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            Flashcard = await _context.Flashcards
                .Include(f => f.Tags)
                .FirstOrDefaultAsync(m => m.FlashcardID == id);

            if (Flashcard == null) return NotFound();

            TagString = string.Join(", ", Flashcard.Tags.Select(t => t.Name));

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cardToUpdate = await _context.Flashcards
                .Include(f => f.Tags)
                .FirstOrDefaultAsync(f => f.FlashcardID == Flashcard.FlashcardID);

            if (cardToUpdate == null) return NotFound();

            cardToUpdate.FrontText = Flashcard.FrontText;
            cardToUpdate.BackText = Flashcard.BackText;

            cardToUpdate.Tags.Clear();

            if (!string.IsNullOrWhiteSpace(TagString))
            {
                var tagNames = TagString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(t => t.Trim());

                foreach (var name in tagNames)
                {
                    var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == name)
                              ?? new Tag { Name = name };

                    cardToUpdate.Tags.Add(tag);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index", new { deckId = cardToUpdate.DeckID });
        }
    }
}
