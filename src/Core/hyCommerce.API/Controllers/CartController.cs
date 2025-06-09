using hyCommerce.Application.Services;
using hyCommerce.Domain.Entities.Cart;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<ActionResult<Cart>> GetCart(string userId)
    {
        //var cart = await cartService.GetCart(userId);
        //if (cart == null) return NotFound();
        //return Ok(cart);
        return Ok();
    }

    [HttpPost("{userId}")]
    public async Task<ActionResult<Cart>> AddOrUpdateCart(string userId, [FromBody] List<CartItem> items)
    {
        //var cart = await cartService.AddOrUpdateCart(userId, items);
        //return Ok(cart);
        return Ok();
    }

    [HttpDelete("{cartId}")]
    public async Task<IActionResult> RemoveCart(int cartId)
    {
        //var result = await cartService.RemoveCart(cartId);
        //if (!result) return NotFound();
        return NoContent();
    }
}
