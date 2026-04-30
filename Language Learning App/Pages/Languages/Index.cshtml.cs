using Language_Learning_App.Data;
using Language_Learning_App.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Language_Learning_App.Pages.Languages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Language> Languages { get; set; }

        public async Task OnGetAsync()
        {
            Languages = await _context.Languages.ToListAsync();
        }
    }
}