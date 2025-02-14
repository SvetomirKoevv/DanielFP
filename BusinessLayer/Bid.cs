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
        public decimal Money { get; set; }

        public Bid()
        {

        }

        public Bid (string userId, decimal money_)
        {
            UserId = userId;
            Money = money_;
        }

    }
}