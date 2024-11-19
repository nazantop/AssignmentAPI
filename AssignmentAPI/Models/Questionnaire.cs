using System.Text.Json.Serialization;

namespace AssignmentAPI.Models
{
	public class Questionnaire
	{
        [JsonPropertyName("questionnaireId")]
        public int? QuestionnaireId { get; set; }

        [JsonPropertyName("questionnaireItems")]
        public List<QuestionnaireItem>? QuestionnaireItems { get; set; }
    }
}