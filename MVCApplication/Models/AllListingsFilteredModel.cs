using BusinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace MVCApplication.Models
{
    public class AllListingsFilteredModel
    {
        public List<Listing> Listings { get; set; }

        [BindProperty]
        public FilterModel FilterModel { get; set; }

        public AllListingsFilteredModel(List<Listing> listings, FilterModel filter)
        {
            this.Listings = listings;
            this.FilterModel = filter;
        }
    }

    public class AllAuctionListingsModel
    {
        public List<AuctionListing> AuctionListings { get; set; }

        [BindProperty]
        public FilterModel FilterModel { get; set; }

        public AllAuctionListingsModel(List<AuctionListing> listings, FilterModel filter)
        {
            this.AuctionListings = listings;
            this.FilterModel = filter;
        }
    }
}
