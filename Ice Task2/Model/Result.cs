using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ice_Task2.Model
{
    public class Result
    {
        [Key]
        public int ResultId  { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int ParticipantId  { get; set; }

        [MaxLength(100)]
        public string Outcome { get; set; } = string.Empty;

        public int Score { get; set; }

        [ForeignKey(nameof(EventId))]
        public Event? Event { get; set; }
    }
}
