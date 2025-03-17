using BusinessLayer;

namespace MVCApplication.Models
{
    public class AuctionListingWithVBids<T>
    {
        public T Listing { get; set; }

        public List<Bid> Bids { get; set; }

        public int HighestBid { get; set; } 

        public AuctionListingWithVBids(T listing_, List<Bid> bids_, int highestBid_) 
        {
            Listing = listing_; 
            Bids = bids_;
            HighestBid = highestBid_;
        }

    }
}
