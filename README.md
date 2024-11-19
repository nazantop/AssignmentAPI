# AssignmentAPI
Good to know before starting:
  I had to use 7.0 instead of 8.0 because of I'm a Mac user, 8.0 is not supported in Mac.
  If there is a question with no optional answers, user can enter free text for given question with answerId 0.
  You can test via Swagger.
##API Endpoints##

1. Get All Questions by Language
URL: /questionnaire/GetAllQuestionsByLanguage
Method: GET
Description: Retrieves all questions based on the provided language. Supports pagination.

Query Parameters:
  page (integer, optional): The page number for pagination (default is 1).
  pageSize (integer, optional): The number of questions per page (default is 10).
  language (string, optional): The language code for questions (default is "en-US").
Response:
  CurrentPage (integer): The current page of results.
  PageSize (integer): The number of results per page.
  TotalItems (integer): The total number of questions.
  TotalPages (integer): The total number of pages based on the pageSize.
  Items (array): List of questions, each containing:
  QuestionId (integer): The question's ID.
  Text (string): The question text.
  AnswerResponse (array): The list of possible answers.
  
2. Get Question by Language
  URL: /questionnaire/GetQuestionByLanguage
  Method: GET
  Description: Retrieves a single question based on the provided questionId and language.

Query Parameters:
  questionId (integer): The ID of the question to retrieve.
  language (string, optional): The language code for the question (default is "en-US").
  Response:
  QuestionId (integer): The question's ID.
  Text (string): The question text.
  AnswerResponse (array): List of possible answers.

3. Submit Answers
  URL: /questionnaire/SubmitAnswers
  Method: POST
  Description: Allows a user to submit answers for a given department and questionnaire.
  
Request Body:
  UserId (integer): The unique ID of the user submitting answers.
  Department (string): The department of the user (e.g., "Development", "Marketing", "Reception", "Sales").
  Answers (array): A list of answers, each containing:
    QuestionId (integer): The ID of the question being answered.
    AnswerId (integer): The ID of the answer (if applicable).
    FreeTextAnswer (string): A free-text answer if the user provides one.
    
4. Get Answer Statistics
  URL: /questionnaire/GetAnswerStatistics
  Method: GET
  Description: Retrieves answer statistics, such as minimum, maximum, and average answers submitted per question.
  
Response:
  AnswerId (integer): The ID of the answer (if applicable).
  QuestionId (integer): The ID of the question for given answer.
  Min (integer): The minimum number of responses for a given answer.
  Max (integer): The maximum number of responses for a given answer.
  Average (double): The average number of responses for a given answer.
