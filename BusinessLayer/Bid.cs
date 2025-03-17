using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Bid
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public int Money { get; set; }

        [Required]
        public AuctionListing AuctionListing { get; set; }

        public Bid()
        {

        }

        public Bid (User user, AuctionListing listing,  int money_)
        {
            User = user;
            Money = money_;
            AuctionListing = listing;
        }

    }
}