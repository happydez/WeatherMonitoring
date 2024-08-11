using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WeatherMonitoring.Shared.DataTransferObjects
{
    public record LocationForManipulationDto
    {
        [Required(ErrorMessage = "Location name is a required field.")]
        [MaxLength(48, ErrorMessage = "Maximum length for the Name is 48 characters.")]
        public string? Name { get; init; }

        [MaxLength(48, ErrorMessage = "Maximum length for the Region is 48 characters.")]
        public string? Region { get; init; }

        [MaxLength(48, ErrorMessage = "Maximum length for the Country is 48 characters.")]
        public string? Country { get; init; }

        [Required(ErrorMessage = "Location Lat is a required field.")]
        public double Lat { get; init; }

        [Required(ErrorMessage = "Location Lon is a required field.")]
        public double Lon { get; init; }
    }
}
