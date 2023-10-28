using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Completions;

namespace webapi.Controllers;

[ApiController]
[Route("v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class LlmController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly OpenAIAPI _openAi;

    public LlmController(IConfiguration configuration)
    {
        _configuration = configuration;
        _openAi = new OpenAIAPI(_configuration["OpenAI:ClientSecret"]);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] string prompt)
    {
        var completionRequest = new CompletionRequest
        {
            Prompt = prompt,
            MaxTokens = 512,
            Temperature = 0.8,
            Model = OpenAI_API.Models.Model.ChatGPTTurbo,
        };

        var completions = await _openAi.Completions.CreateCompletionAsync(completionRequest);

        if (completions == null)
        {
            return BadRequest("No se pudo completar la solicitud");
        }

        var finalText = "";
        completions.Completions.Aggregate(finalText, (current, completion) => current + completion.Text);
        
        return Ok(finalText);
    }
}