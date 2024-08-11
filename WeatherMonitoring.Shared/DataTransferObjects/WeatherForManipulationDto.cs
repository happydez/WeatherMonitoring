using System.ComponentModel.DataAnnotations;

namespace WeatherMonitoring.Shared.DataTransferObjects
{
    public abstract record WeatherForManipulationDto
    {
        [Required(ErrorMessage = "Weather Condition Code is a required field.")]
        [Range(1000, int.MaxValue, ErrorMessage = "ConditionCode is required and it can't be lower than 1000")]
        public int ConditionCode { get; init; }

        [Required(ErrorMessage = "Weather Condition Text is a required field.")]
        [MaxLength(64, ErrorMessage = "Maximum length for the Condition Text is 64 characters.")]
        public string? ConditionText { get; init; }

        [Required(ErrorMessage = "Temperature Celsius is a required field.")]
        public double TemperatureCelsius { get; init; }

        [Required(ErrorMessage = "Humidity is a required field.")]
        public int Humidity { get; init; }

        [Required(ErrorMessage = "PressureIn is a required field.")]
        public double PressureIn { get; init; }

        [Required(ErrorMessage = "WindSpeedKph is a required field.")]
        public double WindSpeedKph { get; init; }

        [Required(ErrorMessage = "LastUpdatedEpoch is a required field.")]
        public long LastUpdatedEpoch { get; init; }

        [Required(ErrorMessage = "LastUpdated is a required field.")]
        public DateTime LastUpdated { get; init; }
    }
}
