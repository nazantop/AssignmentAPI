using System.Text.Json.Serialization;

namespace AssignmentAPI.Models
{
	public class QuestionnaireItem
	{
            [JsonPropertyName("subjectId")]
            public int? SubjectId { get; set; }

            [JsonPropertyName("orderNumber")]
            public int? OrderNumber { get; set; }

            [JsonPropertyName("texts")]
            public Dictionary<string, string>? Texts { get; set; }

            [JsonPropertyName("itemType")]
            public int? ItemType { get; set; }

            [JsonPropertyName("questionId")]
            public int? QuestionId { get; set; }

            [JsonPropertyName("answerCategoryType")]
            public int? AnswerCategoryType { get; set; }

            [JsonPropertyName("answerId")]
            public int? AnswerId { get; set; }

            [JsonPropertyName("answerType")]
            public int? AnswerType { get; set; }

            [JsonPropertyName("answerOrderNumber")]
            public int? AnswerOrderNumber { get; set; }

            [JsonPropertyName("answerItemType")]
            public int? AnswerItemType { get; set; }

        [JsonPropertyName("questionnaireItems")]
        public List<QuestionnaireItem>? QuestionnaireItems { get; set; }

    }
	
}