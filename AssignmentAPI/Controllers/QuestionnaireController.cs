using AssignmentAPI.Helpers;
using AssignmentAPI.IServices;
using AssignmentAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionnaireController : ControllerBase
{
    private readonly IQuestionnaireService _questionnaireService;

    public QuestionnaireController(IQuestionnaireService questionnaireService)
    {
        _questionnaireService = questionnaireService;
    }

    [HttpGet]
    [Route("GetAllQuestionsByLanguage")]
    public async Task<IActionResult> GetAllQuestionsByLanguage([FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string language = "en-US")
    {
        var totalQuestions = await _questionnaireService.GetAllQuestionsByLanguageAsync(language);

        var totalItems = totalQuestions.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return Ok(new PaginatedResponse<QuestionResponseModel>
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Items = totalQuestions
        });
    }

    [HttpGet]
    [Route("GetQuestionByLanguage")]
    public async Task<IActionResult> GetQuestionByLanguage([FromQuery] int questionId,
    [FromQuery] string language = "en-US")
    {
        var totalQuestions = await _questionnaireService.GetAllQuestionsByLanguageAsync(language);
        var question = totalQuestions.Where(x => x.QuestionId == questionId);

        if (question == null)
        {
            BadRequest("Question couldn't found.");
        }

        return Ok(question);
    }

    [HttpPost("SubmitAnswers")]
    public IActionResult SubmitAnswers([FromBody] AnswerRequestModel request)
    {
        var questions = _questionnaireService.GetQuestions();
        var validationResult = QuestionnaireValidation.ValidatePostAnswersRequest(request, questions);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ErrorMessage);
        }

        var result = new
        {
            UserId = request.UserId,
            Department = request.Department,
            SubmittedAnswers = request.Answers
        };

        _questionnaireService.StoreAnswers(request);

        return Ok(new { Message = "Answers submitted successfully.", Result = result });
    }

    [HttpGet]
    [Route("GetAnswerStatistics")]
    public async Task<IActionResult> GetAnswerStatistics()
    {
        var answerStatistics = await _questionnaireService.GetAnswersStatistics();

        return Ok(answerStatistics);
    }

}

