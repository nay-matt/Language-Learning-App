using Language_Learning_App.Data;
using Language_Learning_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Language_Learning_App.Pages.Flashcards
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Flashcard Flashcard { get; set; } = default!;

        [BindProperty]
        public string TagInput { get; set; } = string.Empty;



        public IActionResult OnGet(int deckId)
        {
            Flashcard = new Flashcard { DeckID = deckId };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Flashcard.Tags");
            ModelState.Remove("Flashcard.Deck");

            if (!ModelState.IsValid) return Page();

            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            if (!string.IsNullOrWhiteSpace(TagInput))
            {
                var tagNames = TagInput.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var name in tagNames)
                {
                    var trimmedName = name.Trim();
                    var existingTag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == trimmedName);
                    Flashcard.Tags.Add(existingTag ?? new Tag { Name = trimmedName });
                }
            }

            var initialReview = new FlashcardReview
            {
                UserID = userId,
                NextReviewDate = DateTime.Now,
                LastReviewed = DateTime.Now,
                Interval = 0,
                EaseFactor = 2.5,
                TimesReviewed = 0
            };

            Flashcard.FlashcardReviews.Add(initialReview);

            _context.Flashcards.Add(Flashcard);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { deckId = Flashcard.DeckID });
        }
    }
}