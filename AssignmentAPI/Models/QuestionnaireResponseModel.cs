namespace AssignmentAPI.Models
{
	public class QuestionnaireResponseModel
	{
        public int? SubjectId { get; set; }
        public string? Text { get; set; }
        public List<QuestionResponseModel>? QuestionResponse { get; set; } = new();
    }

    public class QuestionResponseModel
    {
        public int? SubjectId { get; set; }
        public int? QuestionId { get; set; }
        public string? Text { get; set; }
        public List<AnswerResponseModel>? AnswerResponse { get; set; } = new();
    }

    public class AnswerResponseModel
    {
        public int? QuestionId { get; set; }
        public int? AnswerId { get; set; }
        public string? Text { get; set; }
        public int? OrderNumber { get; set; }
    }
}