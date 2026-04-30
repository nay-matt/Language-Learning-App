using Language_Learning_App.Data;
using Language_Learning_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Language_Learning_App.Pages.Decks
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Deck Deck { get; set; }

        public List<SelectListItem> LanguageList { get; set; }

        public async Task OnGetAsync()
        {
            await LoadLanguages();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Deck.UserID = userId;

            ModelState.Remove("Deck.UserID");
            if (!ModelState.IsValid)
            {
                await LoadLanguages();
                return Page();
            }
            Deck.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Decks.Add(Deck);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

        private async Task LoadLanguages()
        {
            LanguageList = await _context.Languages
                .Select(l => new SelectListItem
                {
                    Value = l.LanguageID.ToString(),
                    Text = l.Name
                })
                .ToListAsync();
        }
    }
}