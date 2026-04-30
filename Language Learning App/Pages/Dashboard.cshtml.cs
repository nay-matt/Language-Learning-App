using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Language_Learning_App.Data;
using Language_Learning_App.Models;

namespace Language_Learning_App.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<DeckStudyStatus> DeckStatuses { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            DeckStatuses = await _context.DeckStudyStatuses
                .Where(s => s.UserID == userId)
                .OrderByDescending(s => s.CardsDueToday)
                .ToListAsync();

            return Page();
        }
    }
}