using AssignmentAPI.Models;

namespace AssignmentAPI.IServices
{
	public interface IQuestionnaireService
	{
        public Task<List<QuestionResponseModel>> GetAllQuestionsByLanguageAsync(string language);

        public void StoreAnswers(AnswerRequestModel answerRequestModel);

        public List<QuestionResponseModel> GetQuestions();

        public Task<List<AnswerStatisticsResponse>> GetAnswersStatistics();
    }
}