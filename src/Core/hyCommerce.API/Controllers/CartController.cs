using hyCommerce.Application.Services;
using hyCommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace hyCommerce.API.Controllers;

public class CartController(ICartService cartService) : BaseApiController
{
    [HttpGet()]
    public async Task<ActionResult<Cart>> GetCart()
    {
        return Ok();
    }
}