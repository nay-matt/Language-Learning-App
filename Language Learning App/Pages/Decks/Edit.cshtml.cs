using System.Security.Claims;
using Language_Learning_App.Data;
using Language_Learning_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Language_Learning_App.Pages.Decks
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public EditModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public Deck Deck { get; set; }
        public List<SelectListItem> LanguageList { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Deck = await _context.Decks.FindAsync(id);
            if (Deck == null) return NotFound();

            await LoadLanguages();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Deck.UserID = userId;

            ModelState.Remove("Deck.Language");
            ModelState.Remove("Deck.UserID");

            if (!ModelState.IsValid)
            {
                await LoadLanguages();
                return Page();
            }

            _context.Attach(Deck).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task LoadLanguages()
        {
            LanguageList = await _context.Languages
                .Select(l => new SelectListItem { Value = l.LanguageID.ToString(), Text = l.Name })
                .ToListAsync();
        }
    }
}
