using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeatherMonitoring.Shared.DataTransferObjects
{
    public record LocationForUpdateDto : LocationForManipulationDto
    {
        [Required(ErrorMessage = "Location TzId is a required field.")]
        public string? TzId { get; init; }

        [DefaultValue(true)]
        public bool Active { get; init; }

        [DefaultValue(true)]
        public bool Included { get; init; }
    }
}
