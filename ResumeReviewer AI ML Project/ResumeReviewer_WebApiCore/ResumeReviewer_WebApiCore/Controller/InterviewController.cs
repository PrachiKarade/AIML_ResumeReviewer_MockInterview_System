using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.DTOs;
using ResumeReviewer_WebApiCore.Domain.Models;

namespace ResumeReviewer_WebApiCore.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewService _interviewService;

        public InterviewController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        // 🔹 Start Interview
        [HttpPost("start/{resumeId}")]
        public async Task<ActionResult<InterviewResponseModel>> StartInterview(int resumeId)
        {
            try
            {
                var response = await _interviewService.StartInterviewAsync(resumeId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("continue/{resumeId}")]
        public async Task<ActionResult<InterviewResponseModel>> ContinueInterview(int resumeId,[FromBody] InterviewAnswerRequestDto request)
        {
            try
            {
                var response = await _interviewService.ContinueInterviewAsync(resumeId, request.userAnswer);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

/*
 Controller
   ↓
MockInterviewService
   ↓
ChatService
   ↓
OpenAI / LLaMA
✔ Controllers don’t know OpenAI
✔ Services don’t know SQL
✔ Domain models are reusable
✔ Infrastructure handles integrations
 
 */
