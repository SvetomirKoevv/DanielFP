using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MVCApplication.Models
{
    public class BidViewModel
    {
        [Required]
        public int ListingId { get; set; }

        [Required]
        [Range(500, 5000, ErrorMessage = "Bid should be between 500 and 5000 лв.")]
        public int BidPrice { get; set; }

    }
}
