using System.Security.Claims;
using MatchMicroservice.Manager.Interfaces;
using MatchMicroservice.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchMicroservice.Controllers;

[ApiController]
[Route("swipe/")]
public class SwipeController (ISwipeManager swipeManager) : ControllerBase
{
    private readonly ISwipeManager _swipeManager = swipeManager;
    
    [HttpPost]
    [Authorize]
    [Route("RegisterDecision")]
    public async Task<IActionResult> RegisterDecision(DecisionControllerDto decision)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
        {
            return Unauthorized("Not Valid token");
        }
        DecisionDto decisionDto =
            new DecisionDto(Guid.Parse(id), Guid.Parse(decision.UserId), decision.Decision);
        return Ok(await _swipeManager.RegisterDecision(decisionDto));
    }

    [HttpGet]
    [Authorize]
    [Route("GetMatch/{page}/{pagesize}")]
    public async Task<IActionResult> GetMatchAsync(int page, int pageSize)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
        {
            return Unauthorized("Not Valid token");
        }
        return Ok(await _swipeManager.GetMatchAsync(Guid.Parse(id), page, pageSize));
    }
    
    [HttpGet]
    [Authorize]
    [Route("Get/{id}/{page}/{pagesize}")]
    public async Task<IActionResult> GetByUserIdAsync(int page, int pageSize)
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (id == null)
        {
            return Unauthorized("Not Valid token");
        }
        return Ok(await _swipeManager.GetByUserIdAsync(Guid.Parse(id), page, pageSize));
    }
}