using AssignmentAPI.IServices;
using System.Text.Json;
using AssignmentAPI.Models;

namespace AssignmentAPI.Services
{
	public class QuestionnaireService : IQuestionnaireService
    {
        public List<AnswerRequestModel> AnswerSubmissions = new();

        public List<QuestionResponseModel> Questions = new();

        public async Task<List<QuestionResponseModel>> GetAllQuestionsByLanguageAsync(string language = "en-US")
		{
            var filePath = Path.Combine(AppContext.BaseDirectory, "Resources", "questionnaire.json");
            var jsonData = File.OpenRead(filePath);
            var data = await JsonSerializer.DeserializeAsync<Questionnaire>(jsonData);

            var response = data.QuestionnaireItems
              .Where(q => q.Texts != null && q.Texts.ContainsKey(language))
              .Select(q => new QuestionnaireResponseModel
              {
                  SubjectId = q.SubjectId,
                  Text = q.Texts.TryGetValue(language, out var text) ? text : string.Empty,
                  QuestionResponse = q.QuestionnaireItems?.Where(ao => ao.Texts != null && ao.Texts.ContainsKey(language))
                      .Select(ao => new QuestionResponseModel
                      {
                          QuestionId = ao.QuestionId,
                          SubjectId = ao.SubjectId,
                          Text = ao.Texts.TryGetValue(language, out var questionText) ? questionText : string.Empty,
                          AnswerResponse = ao.QuestionnaireItems?.Where(x => x.Texts != null && x.Texts.ContainsKey(language))
                              .Select(x => new AnswerResponseModel
                              {
                                  AnswerId = x.AnswerId,
                                  QuestionId = x.QuestionId,
                                  OrderNumber = x.OrderNumber,
                                  Text = x.Texts.TryGetValue(language, out var answerText) ? answerText : string.Empty,
                              }).ToList()
                      }).ToList()
              }).ToList();

            var totalQuestions = new List<QuestionResponseModel>();

            foreach (var questions in response.Select(x => x.QuestionResponse))
            {
                totalQuestions.AddRange(questions);
            }

            Questions = totalQuestions;

            return totalQuestions;
        }

        public async Task<List<AnswerStatisticsResponse>> GetAnswersStatistics()
        {
            var groupedAnswers = AnswerSubmissions
                .SelectMany(u => u.Answers)
                .GroupBy(a => new { a.QuestionId, a.AnswerId })
                .ToList();

            var result = groupedAnswers
                .Select(g =>
                {
                    var question = Questions.FirstOrDefault(q => q.QuestionId == g.Key.QuestionId);

                    if (question != null && !question.AnswerResponse.Any())
                    {
                        // For the questions for user opinions, free texts.
                        var freeTextAnswers = AnswerSubmissions
                            .Where(u => u.Answers.Any(a => a.QuestionId == g.Key.QuestionId && !string.IsNullOrEmpty(a.FreeTextAnswer)))
                            .GroupBy(u => u.Department)
                            .Select(departmentGroup => new
                            {
                                Department = departmentGroup.Key,
                                FreeTextCount = departmentGroup.Count()
                            })
                            .ToList();
                

                        return new AnswerStatisticsResponse
                        {
                            QuestionId = g.Key.QuestionId,
                            AnswerId = g.Key.AnswerId,
                            Min = freeTextAnswers.Min(d => d.FreeTextCount),
                            Max = freeTextAnswers.Max(d => d.FreeTextCount),
                            Average = freeTextAnswers.Average(d => d.FreeTextCount)
                        };
                    }

                    // For questions with predefined answers, calculate min, max, and average
                    var departmentAnswers = AnswerSubmissions
                        .Where(u => u.Answers.Any(a => a.QuestionId == g.Key.QuestionId && a.AnswerId == g.Key.AnswerId))
                        .GroupBy(u => u.Department)
                        .Select(departmentGroup => new
                        {
                            Department = departmentGroup.Key,
                            Count = departmentGroup.Count()
                        })
                        .ToList();

                    return new AnswerStatisticsResponse
                    {
                        QuestionId = g.Key.QuestionId,
                        AnswerId = g.Key.AnswerId,
                        Min = departmentAnswers.Min(d => d.Count),
                        Max = departmentAnswers.Max(d => d.Count),
                        Average = departmentAnswers.Average(d => d.Count)
                    };
                })
                .ToList();

            return result;
        }

        public void StoreAnswers(AnswerRequestModel answerRequestModel)
        {
            AnswerSubmissions.Add(answerRequestModel);
        }

        public List<QuestionResponseModel> GetQuestions()
        {
            if (Questions.Count() > 0)
            {
                return Questions;
            }
            else
            {
               var list = GetAllQuestionsByLanguageAsync();
            }

            return Questions;
            
        }
    }
}