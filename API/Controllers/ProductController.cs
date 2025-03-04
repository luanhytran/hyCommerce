using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers;

public class ProductController : BaseController
{
    public ProductController() {}

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}