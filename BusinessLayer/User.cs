using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer
{
    public class User : IdentityUser
    {
        public List<Listing> Listings { get; set; }

        public List<Bid> Bids { get; set; }

        public User() 
        {
            Listings = new List<Listing>();
            Bids = new List<Bid>();
        }

        public User(string username_, string email_)
        {
            UserName = username_;
            Email = email_;

            Listings = new List<Listing>();
            Bids = new List<Bid>();
        }
        
    }
}
