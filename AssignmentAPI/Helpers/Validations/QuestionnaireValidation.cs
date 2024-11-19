using AssignmentAPI.Models;

namespace AssignmentAPI.Helpers
{
    public static class QuestionnaireValidation
    {
        private static readonly string[] ValidDepartments = { "marketing", "sales", "development", "reception" };

        public static (bool IsValid, string ErrorMessage) ValidatePostAnswersRequest(AnswerRequestModel request, List<QuestionResponseModel> questions)
        {
            if (request == null)
            {
                return (false, "Request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.Department))
            {
                return (false, "Department is required.");
            }

            if (!ValidDepartments.Contains(request.Department.ToLower()))
            {
                return (false, "Invalid department. Valid options are: marketing, sales, development, reception.");
            }

            if (request.UserId <= 0)
            {
                return (false, "Invalid UserId. It must be a positive number.");
            }

            if (request.Answers == null || !request.Answers.Any())
            {
                return (false, "Answers list cannot be null or empty.");
            }

            foreach (var answer in request.Answers)
            {
                if (!questions.Select(x => x.QuestionId).Contains(answer.QuestionId))
                {
                    return (false, $"Question couldn't found.");
                }

                var validAnswerIds = questions
                   .Where(q => q.QuestionId == answer.QuestionId)
                   .SelectMany(q => q.AnswerResponse.Select(a => a.AnswerId))
                   .ToList();
                if (!string.IsNullOrEmpty(answer.FreeTextAnswer) && answer.AnswerId == 0)
                {
                    continue;
                }

                if (!validAnswerIds.Contains(answer.AnswerId))
                {
                    return (false, $"Answer should be selected below options.");
                }
            }

            var duplicateQuestions = request.Answers
               .GroupBy(a => a.QuestionId)
               .Where(g => g.Count() > 1)
               .Select(g => g.Key)
               .ToList();

            if (duplicateQuestions.Any())
            {
                return (false, $"Duplicate answers found for QuestionId(s): {string.Join(", ", duplicateQuestions)}.");
            }

            return (true, string.Empty);
        }
    }

}

