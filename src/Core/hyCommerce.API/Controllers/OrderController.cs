using hyCommerce.Application.DTOs;
using hyCommerce.Application.Services;
using hyCommerce.Extensions.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hyCommerce.API.Controllers;

[Authorize]
public class OrderController(IOrderService orderService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetOrders()
    {
        return await orderService.GetOrders(User.GetUsername());
    }
}