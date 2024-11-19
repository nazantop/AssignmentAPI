namespace AssignmentAPI.Models
{
    public class AnswerRequestModel
    {
        public int UserId { get; set; }
        public string Department { get; set; }
        public List<AnswerSubmission> Answers { get; set; } = new();
    }

    public class AnswerSubmission
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string FreeTextAnswer { get; set; }
    }
}