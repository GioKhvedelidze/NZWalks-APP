using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.Dto;

public class UpdateRegionRequestDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 character")]
    [MaxLength(3, ErrorMessage = "Code has to be a minimum of 3 character")]
    public string Code { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage = "Code has to be a minimum of 100 character")]
    public string Name { get; set; }
    public string? RegionImageUrl { get; set; }
}