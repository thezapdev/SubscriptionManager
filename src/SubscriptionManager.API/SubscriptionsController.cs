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
        var result = await _subscriptionService.GetAllSubscriptionsAsync();
        
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

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> CancelSubscription(Guid id)
    {
        await _subscriptionService.CancelSubscriptionAsync(id);
        return Ok(); 
    }

    [HttpPost("{id:guid}/renew")]
    public async Task<IActionResult> RenewSubscription(Guid id)
    {
        await _subscriptionService.RenewSubscriptionAsync(id);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSubscription(Guid id)
    {
        await _subscriptionService.DeleteSubscriptionAsync(id);
        return NoContent(); 
    }
    [HttpPost("{id:guid}/resume")]
    public async Task<IActionResult> ResumeSubscriptionAsync(Guid id,[FromBody] DateOnly request)
    {
        await _subscriptionService.ResumeSubscriptionAsync(id,request);
        return Ok();
    }
   

}