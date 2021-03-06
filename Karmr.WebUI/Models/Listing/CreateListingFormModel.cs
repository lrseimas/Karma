﻿using System.ComponentModel.DataAnnotations;

namespace Karmr.WebUI.Models.Listing
{
    public sealed class CreateListingFormModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; }

        [Display(Name = "Location")]
        [MaxLength(1000, ErrorMessage = "Location cannot exceed 1000 characters")]
        public string LocationName { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}