using BusinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace MVCApplication.Models
{
    public class ListingAuctionDetailsViewModel
    {
        public AuctionListing Listing { get; set; }

        public int HighestBid { get; set; }

        [BindProperty]
        public ContactForm ContactForm { get; set; }

        public List<AuctionListingWithVBids<AuctionListing>> allListings { get; set; }

    }
}
