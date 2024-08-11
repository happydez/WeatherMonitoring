using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WeatherMonitoring.Entities.Models
{
    public class Location
    {
        [Column("LocationId")]
        public Guid Id { get; set; }

        [Column("name")]
        [Required(ErrorMessage = "Location name is a required field.")]
        [MaxLength(48, ErrorMessage = "Maximum length for the Name is 48 characters.")]
        public string? Name { get; set; }

        [Column("region")]
        [MaxLength(48, ErrorMessage = "Maximum length for the Region is 48 characters.")]
        public string? Region { get; set; }

        [Column("country")]
        [MaxLength(48, ErrorMessage = "Maximum length for the Country is 48 characters.")]
        public string? Country { get; set; }

        [Column("lat")]
        [Required(ErrorMessage = "Location Lat is a required field.")]
        public double Lat { get; set; }

        [Column("lon")]
        [Required(ErrorMessage = "Location Lon is a required field.")]
        public double Lon { get; set; }

        [Column("tzId")]
        [Required(ErrorMessage = "Location TzId is a required field.")]
        public string? TzId { get; set; }

        [Column("active")]
        [DefaultValue(true)]
        public bool Active { get; set; }

        [Column("included")]
        [DefaultValue(true)]
        public bool Included { get; set; }

        public ICollection<Weather>? Weathers { get; set; }
    }
}
