using hyCommerce.Application.Services;
using hyCommerce.Domain.Entities.Order;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        //var order = await orderService.GetOrder(id);
        //if (order == null) return NotFound();
        //return Ok(order);
        return Ok();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Order>>> GetOrders(string userId)
    {
        //var orders = await orderService.GetOrders(userId);
        //return Ok(orders);
        return Ok();
    }

    [HttpPost("{userId}")]
    public async Task<ActionResult<Order>> CreateOrder(string userId)
    {
        //var order = await orderService.CreateOrderFromCart(userId);
        //return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        return Ok();
    }
}
