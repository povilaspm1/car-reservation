using System.Text.Json.Serialization;

namespace CarReservation.Models
{
    public class Reservation
    {
        [JsonIgnore]
        public string? Id { get; set; }
        public string? CarId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
