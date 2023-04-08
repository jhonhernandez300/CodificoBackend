using System.ComponentModel.DataAnnotations;

namespace CodificoBackend.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string? MovieTitle { get; set; }
        public DateTime MovieYear { get; set; }
        public string? MovieGenre { get; set; }        
    }
}
