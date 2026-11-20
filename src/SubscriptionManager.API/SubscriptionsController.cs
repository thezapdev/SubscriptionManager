using Microsoft.AspNetCore.Mvc;
using SubscriptionManager.Application.DTOs;
using SubscriptionManager.Application.Interfaces;

namespace SubscriptionManager.API;
[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    [HttpGet("{id:guid}", Name = "GetSubscriptionById") ]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _subscriptionService.GetSubscriptionByIdAsync(id);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = _subscriptionService.GetAllSubscriptionsAsync();
        
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionDto dto)
    {
        var result = await _subscriptionService.AddSubscriptionAsync(dto);
        return Ok(result);
    }
}