using BusinessLayer;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MVCApplication.Models
{
    public class ListingViewModel
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

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

        [BindProperty]
        public BufferedFileUploadDb FileUpload { get; set; }

        public List<Image> Images { get; set; }

        public ListingViewModel()
        {
            Images = new List<Image>();
        }

        public ListingViewModel(Listing listing, Car car)
        {
            Images = car.Images;

            this.Name = listing.Name;
            this.Description = listing.Description; 
            this.Price = listing.Price;

            this.Model = car.Model;
            this.Make = car.Make;
            this.HorsePower = car.HorsePower;
            this.Color = car.Color;
            this.Transmittion = car.Transmittion;
            this.Mileage = car.Mileage;
            this.FuelType = car.FuelType;
        }
    }
}
