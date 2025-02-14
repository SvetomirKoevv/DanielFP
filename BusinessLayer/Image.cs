using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] Data { get; set; }    

        [ForeignKey("Car")]
        public int VehicleId { get; set; } 

        [Required]
        public Car Car { get; set; }

        public Image()
        {

        }

        public Image(byte[] data_, Car car_)
        {
            Data = data_;
            Car = car_;
        }
    }
}
