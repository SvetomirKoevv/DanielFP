using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public List<Image> Images { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int HorsePower { get; set; }

        [Required]
        public int Mileage { get; set; }

        [Required]
        public FuelType FuelType { get; set; }

        [Required]
        public TransmissionType Transmittion { get; set; }

        [Required]
        public CarColor Color { get; set; }
        
        public Car()
        {
            Images = new List<Image>();
        }

        public Car(string make_, string model_, int horsePower, int mileage, FuelType fuelType, TransmissionType transmission_, CarColor color_)
        {
            Make = make_;
            Model = model_;
            HorsePower = horsePower;
            Mileage = mileage;
            FuelType = fuelType;
            Transmittion = transmission_;
            Color = color_;

            Images = new List<Image>();
        }
    }
}
