using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WeatherMonitoring.Shared.DataTransferObjects
{
    public record LocationForCreationDto : LocationForManipulationDto
    {
        [DefaultValue("Etc/UTC")]
        public string? TzId { get; set; }

        [DefaultValue(true)]
        public bool Active { get; init; }

        [DefaultValue(true)]
        public bool Included { get; init; }

        public IEnumerable<WeatherForCreationDto>? Weathers { get; init; }
    }
}
