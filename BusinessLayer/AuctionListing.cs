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
        public int StartingPrice { get; set; }

        [Required]
        public string Name { get; set; }    

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; } 

        [Required]
        public int DurationInHours { get; set; }    

        public List<Bid> Bids { get; set; }

        public AuctionListing()
        {
            Bids = new List<Bid>();
        }

        public AuctionListing
            (Car car_, 
            int startingPrice_, 
            string name_, 
            string description_, 
            DateTime startDateTime_, 
            int durationInHours_)
        {
            Car = car_;
            StartingPrice = startingPrice_;
            Name = name_;
            Description = description_;
            StartDateTime = startDateTime_;
            DurationInHours = durationInHours_;

            Bids = new List<Bid>();
        }
    }
}
