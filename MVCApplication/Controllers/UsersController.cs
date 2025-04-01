using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace MVCApplication.Controllers
{
    public class UsersController : Controller
    {
        private readonly RevHausDbContext _context;

        public UsersController(RevHausDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        { 

            return View(_context.Users);
        }
    }
}
