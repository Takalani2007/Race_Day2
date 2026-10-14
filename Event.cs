using System.ComponentModel.DataAnnotations;

namespace Race_Events2.Model
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        [MaxLength(100)]
        public string Location { get; set; } = string.Empty;

        // 🔗 Navigation properties
        public ICollection<Weather> Weathers { get; set; } = new List<Weather>();
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
    }
}