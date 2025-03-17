using BusinessLayer;

namespace MVCApplication.Models
{
    public class ListingAuctionDetailsViewModel
    {
        public AuctionListing Listing { get; set; }

        public int HighestBid { get; set; } 

        public List<AuctionListingWithVBids<AuctionListing>> allListings { get; set; }

    }
}
