using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ice_Task2.Model
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int UserId { get; set; }

        [MaxLength(30)]
        public string Status { get; set; } = "Confirmed";

        [ForeignKey(nameof(EventId))]
        public Event? Event { get; set; }
    }
}