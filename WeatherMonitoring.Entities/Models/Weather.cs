using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WeatherMonitoring.Entities.Models
{
    public class Weather
    {
        [Column("WeatherId")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("ccode")]
        [Required(ErrorMessage = "Weather Condition Code is a required field.")]
        public int ConditionCode { get; set; }

        [Column("ctext")]
        [Required(ErrorMessage = "Weather Condition Text is a required field.")]
        [MaxLength(64, ErrorMessage = "Maximum length for the Condition Text is 64 characters.")]
        public string? ConditionText { get; set; }

        [Column("temp_c")]
        [Required(ErrorMessage = "Temperature Celsius is a required field.")]
        public double TemperatureCelsius { get; set; }

        [Column("humidity")]
        [Required(ErrorMessage = "Humidity is a required field.")]
        public int Humidity { get; set; }

        [Column("pressure_in")]
        [Required(ErrorMessage = "PressureIn is a required field.")]
        public double PressureIn { get; set; }

        [Column("wind_kph")]
        [Required(ErrorMessage = "WindSpeedKph is a required field.")]
        public double WindSpeedKph { get; set; }

        [Column("lastup_epoch")]
        [Required(ErrorMessage = "LastUpdatedEpoch is a required field.")]
        public long LastUpdatedEpoch { get; set; }

        [Column("lastup")]
        [Required(ErrorMessage = "LastUpdated is a required field.")]
        public DateTime LastUpdated { get; set; }


        [ForeignKey(nameof(Location))]
        public Guid LocationId { get; set; }

        public Location? Location { get; set; }
    }
}
