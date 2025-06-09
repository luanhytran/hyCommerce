using hyCommerce.Application.Services;
using hyCommerce.Domain.Entities.Order;
using Microsoft.AspNetCore.Mvc;

namespace hyCommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await orderService.GetOrderById(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<List<Order>>> GetOrders(int userId)
    {
        // TODO: change userId to string later
        var orders = await orderService.GetOrderById(userId);
        return Ok(orders);
    }

    [HttpPost("{userId:int}")]
    public async Task<ActionResult<Order>> CreateOrder(int userId)
    {
        // TODO: change userId to string later
        var order = await orderService.CreateOrderFromCart(userId);
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }
}