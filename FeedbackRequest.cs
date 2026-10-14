namespace Race_Events2.Model
{
    public class FeedbackRequest
    {
        public int UserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
    }
}
