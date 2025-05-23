﻿using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MVCApplication.Models
{
    public class AuctionListingCreateModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int StartingPrice { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int HorsePower { get; set; }

        [Required]
        public int Mileage { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public FuelType FuelType { get; set; }

        [Required]
        public TransmissionType Transmittion { get; set; }

        [Required]
        public CarColor Color { get; set; }

        [BindProperty]
        public BufferedFileUploadDb FileUpload { get; set; }

        public List<Image> Images { get; set; }

        public AuctionListingCreateModel()
        {
            Images = new List<Image>();
        }

        public AuctionListingCreateModel(AuctionListing listing, Car car)
        {
            Images = car.Images;

            this.Name = listing.Name;
            this.Description = listing.Description;
            this.StartingPrice = listing.StartingPrice;
            this.StartDate = listing.StartDateTime;
            this.Duration = listing.DurationInHours;
         
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

