using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResumeReviewer_WebApiCore.Application.Interfaces;
using ResumeReviewer_WebApiCore.Domain.Models;
using ResumeReviewer_WebApiCore.Domain.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace ResumeReviewer_WebApiCore.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        public ResumeController(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        [HttpPost]
        [Route("uploadResume")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadResume([FromForm] ResumeUploadRequestDto request)
        {
            try
            { 
                if (request.File == null || request.File.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }
                int resumeId = await _resumeService.UploadResumeAsync(request.File);

                return Ok( new {ResumeId = resumeId , Message = "Resume uploaded and processed successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, $"Internal server error: {ex.Message} stackTrace : {ex.StackTrace} , innerMessage : {ex.InnerException}");
            }
        }

        [HttpGet("{resumeId}/analysis")]
        public async Task<ActionResult<ResumeAnalysisModel>> AnalyzeResume(int resumeId)
        {
            try
            {
                var analysis = await _resumeService.AnalyzeResumeAsync(resumeId);

                return Ok(analysis);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

