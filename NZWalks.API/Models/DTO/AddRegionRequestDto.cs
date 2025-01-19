﻿using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDto
    {
        [Required]
        [MinLength(3,ErrorMessage ="Code has to be a minimum of 3 character")]
        [MaxLength(3, ErrorMessage = "Code should not exceed 3 character")]
        public string Code { get; set; }

        [Required]
        [MaxLength(3, ErrorMessage = "Name has to be a maximum of 100 character")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
