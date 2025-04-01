using BusinessLayer;
using DataLayer;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCApplication.Areas.Identity.Pages.Account.Manage
{
    public class AuctionsModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RevHausDbContext _context;
        private readonly IdentityContext _identityContext;
        private readonly AuctionListingContext _listingContext;

        private List<AuctionListing> PartListings;
        private List<AuctionListing> WonListings;
        public List<AuctionListing> DisplayListings;
        private User LoggedInUser;

        public AuctionsModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RevHausDbContext context,
            IdentityContext identityContext,
            AuctionListingContext listingContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            PartListings = new List<AuctionListing>();
            WonListings = new List<AuctionListing>();
            DisplayListings = new List<AuctionListing>();
            _identityContext = identityContext;
            _listingContext = listingContext;
        }

        public string PageType { get; set; }

        public async Task<IActionResult> OnGetAsync(string filter = "all")
        {
            User user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            LoggedInUser = await _identityContext.ReadUserAsync(user.Id, true);

            PartListings = user.Bids.Select(b => b.AuctionListing).ToList().Distinct().ToList();

            foreach (AuctionListing listing in PartListings)
            {
                if (listing.Winner != null)
                {
                    if (listing.Winner.Id == user.Id)
                    {
                        WonListings.Add(listing);
                    }
                }
            }

            PageType = filter;

            DisplayListings = filter == "won" ? WonListings : PartListings;

            return Page();
        }

    }
}
