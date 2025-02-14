using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class AuctionListing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Car Car { get; set; }

        [Required]
        public decimal StartingPrice { get; set; }

        [Required]
        public string Name { get; set; }    

        [Required]
        public string Description { get; set; }

        public List<Bid> Bids { get; set; }

        public AuctionListing()
        {
            Bids = new List<Bid>();
        }

        public AuctionListing(Car car_, decimal startingPrice_, string name_, string description_)
        {
            Car = car_;
            StartingPrice = startingPrice_;
            Name = name_;
            Description = description_;
            
            Bids = new List<Bid>();
        }
    }
}
