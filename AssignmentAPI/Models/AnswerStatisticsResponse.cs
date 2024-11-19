namespace AssignmentAPI.Models
{
    public class AnswerStatisticsResponse
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public double Average { get; set; }
    }
}