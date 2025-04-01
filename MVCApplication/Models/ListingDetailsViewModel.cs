using BusinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace MVCApplication.Models
{
    public class ListingDetailsViewModel
    {
        public Listing Listing { get; set; }

        [BindProperty]
        public ContactForm ContactForm { get; set; }

        public List<Listing> allListings { get; set; }

        public ListingDetailsViewModel()
        {
            ContactForm = new ContactForm();
        }
    }
}
