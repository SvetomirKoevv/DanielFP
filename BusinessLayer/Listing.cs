using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Listing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Car Car { get; set; }

        [Required] 
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        public List<User> Users { get; set; }

        public Listing()
        {
            Users = new List<User>();
        }

        public Listing(string name_, string description_, int price_) 
        {
            Name = name_;
            Description = description_;
            Price = price_;

            Users = new List<User>();
        }
    }
}
