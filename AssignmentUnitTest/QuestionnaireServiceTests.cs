using AssignmentAPI.Services;
using AssignmentAPI.Models;

namespace AssignmentUnitTest
{
    [TestFixture]
    public class QuestionnaireServiceTests
    {
        private QuestionnaireService _questionnaireService;

        [SetUp]
        public void Setup()
        {
            _questionnaireService = new QuestionnaireService();
        }

        [Test]
        public async Task GetAnswersStatistics_ShouldReturnCorrectStatisticsWithNewDepartments()
        {
            // Arrange
            _questionnaireService.StoreAnswers(new AnswerRequestModel
            {
                UserId = 1,
                Department = "Development",
                Answers = new List<AnswerSubmission>
                {
                    new AnswerSubmission { QuestionId = 1, AnswerId = 1, FreeTextAnswer = "" },
                    new AnswerSubmission { QuestionId = 2, AnswerId = 2, FreeTextAnswer = "" }
                }
            });

            _questionnaireService.StoreAnswers(new AnswerRequestModel
            {
                UserId = 2,
                Department = "Marketing",
                Answers = new List<AnswerSubmission>
                {
                    new AnswerSubmission { QuestionId = 1, AnswerId = 1, FreeTextAnswer = "" },
                    new AnswerSubmission { QuestionId = 2, AnswerId = 2, FreeTextAnswer = "" }
                }
            });

            _questionnaireService.StoreAnswers(new AnswerRequestModel
            {
                UserId = 3,
                Department = "Reception",
                Answers = new List<AnswerSubmission>
                {
                    new AnswerSubmission { QuestionId = 1, AnswerId = 1, FreeTextAnswer = "" }
                }
            });

            var result = await _questionnaireService.GetAnswersStatistics();

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count); 

            var statsQuestion1 = result.FirstOrDefault(s => s.QuestionId == 1);
            Assert.NotNull(statsQuestion1);
            Assert.AreEqual(1, statsQuestion1.Min);
            Assert.AreEqual(1, statsQuestion1.Max);
            Assert.AreEqual(1.0, statsQuestion1.Average);

            var statsQuestion2 = result.FirstOrDefault(s => s.QuestionId == 2);
            Assert.NotNull(statsQuestion2);
            Assert.AreEqual(1, statsQuestion2.Min);
            Assert.AreEqual(1, statsQuestion2.Max);
            Assert.AreEqual(1.0, statsQuestion2.Average);
        }

        [Test]
        public void StoreAnswers_ShouldAddAnswerToStoreWithMultipleDepartments()
        {
            // Arrange
            var answerRequest = new AnswerRequestModel
            {
                UserId = 1,
                Department = "Development",
                Answers = new List<AnswerSubmission>
                {
                    new AnswerSubmission { QuestionId = 1, AnswerId = 1, FreeTextAnswer = "" }
                }
            };

            var answerRequest2 = new AnswerRequestModel
            {
                UserId = 2,
                Department = "Reception",
                Answers = new List<AnswerSubmission>
                {
                    new AnswerSubmission { QuestionId = 1, AnswerId = 1, FreeTextAnswer = "" }
                }
            };

            var answerRequest3 = new AnswerRequestModel
            {
                UserId = 3,
                Department = "Marketing",
                Answers = new List<AnswerSubmission>
                {
                    new AnswerSubmission { QuestionId = 1, AnswerId = 1, FreeTextAnswer = "" }
                }
            };

            // Act
            _questionnaireService.StoreAnswers(answerRequest);
            _questionnaireService.StoreAnswers(answerRequest2);
            _questionnaireService.StoreAnswers(answerRequest3);

            // Assert
            Assert.AreEqual(3, _questionnaireService.AnswerSubmissions.Count);
            Assert.AreEqual(1, _questionnaireService.AnswerSubmissions.Count(a => a.Department == "Development"));
            Assert.AreEqual(1, _questionnaireService.AnswerSubmissions.Count(a => a.Department == "Reception"));
            Assert.AreEqual(1, _questionnaireService.AnswerSubmissions.Count(a => a.Department == "Marketing"));
        }

        [Test]
        public void GetQuestions_ShouldReturnAllStoredQuestionsIncludingDepartments()
        {
            // Arrange
            var questionResponse = new List<QuestionResponseModel>
            {
                new QuestionResponseModel { QuestionId = 1, Text = "Test Question 1" },
                new QuestionResponseModel { QuestionId = 2, Text = "Test Question 2" }
            };
            _questionnaireService.Questions = questionResponse;

            // Act
            var result = _questionnaireService.GetQuestions();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Test Question 1", result.First().Text);
            Assert.AreEqual("Test Question 2", result.Last().Text);
        }
    }
}
