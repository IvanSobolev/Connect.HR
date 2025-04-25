using MatchMicroservice.Manager.Interfaces;
using MatchMicroservice.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MatchMicroservice.Controllers;

[ApiController]
[Route("swipe/")]
public class SwipeController (ISwipeManager swipeManager) : ControllerBase
{
    private readonly ISwipeManager _swipeManager = swipeManager;
    
    [HttpPost]
    [Route("RegisterDecision")]
    public async Task<IActionResult> RegisterDecision(DecisionControllerDto decision)
    {
        DecisionDto decisionDto =
            new DecisionDto(Guid.Parse(decision.IdA), Guid.Parse(decision.IdB), decision.Decision);
        return Ok(await _swipeManager.RegisterDecision(decisionDto));
    }

    [HttpGet]
    [Route("GetMatch/{id}/{page}/{pagesize}")]
    public async Task<IActionResult> GetMatchAsync(string id, int page, int pageSize)
    {
        return Ok(await _swipeManager.GetMatchAsync(Guid.Parse(id), page, pageSize));
    }
    
    [HttpGet]
    [Route("Get/{id}/{page}/{pagesize}")]
    public async Task<IActionResult> GetByUserIdAsync(string id, int page, int pageSize)
    {
        return Ok(await _swipeManager.GetByUserIdAsync(Guid.Parse(id), page, pageSize));
    }
}