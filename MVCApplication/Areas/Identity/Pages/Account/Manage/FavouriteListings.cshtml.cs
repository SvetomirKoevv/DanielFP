using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVCApplication.Areas.Identity.Pages.Account.Manage
{
    public class FavouriteListingsModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RevHausDbContext _context;
        private readonly IdentityContext _identityContext;
        private readonly ListingContext _listingContext;

        public List<Listing> FavouriteListings;
        private User LoggedInUser;

        public FavouriteListingsModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RevHausDbContext context,
            IdentityContext identityContext,
            ListingContext listingContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            FavouriteListings = new List<Listing>();
            _identityContext = identityContext;
            _listingContext = listingContext;
        }

        [BindProperty]
        public int ItemId { get; set; }

        public IActionResult OnPostToggleFavorite([FromBody] FavoriteRequestModel model)
        {
            if (model == null || model.ItemId == 0)
            {
                return BadRequest(new { message = "Invalid request" });
            }

            // Perform logic here to toggle favorite status (e.g., update the database)

            return new JsonResult(new { success = true, message = "Favorite status updated!" });
        }

        private async Task LoadAsync(User user)
        {
            foreach (Listing listingId in LoggedInUser.Listings)
            {
                Listing listingFromDb = await _listingContext.ReadAsync(listingId.Id, true);
                FavouriteListings.Add(listingFromDb);
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            LoggedInUser = await _identityContext.ReadUserAsync(user.Id, true);

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }

    public class FavoriteRequestModel
    {
        public int ItemId { get; set; }
    }
}
