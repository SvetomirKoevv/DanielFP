using BusinessLayer;

namespace MVCApplication.Models
{
    public class ListingDetailsViewModel
    {
        public Listing Listing { get; set; }

        public List<Listing> allListings { get; set; }
    }
}
