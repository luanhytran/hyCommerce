using Microsoft.AspNetCore.Mvc;

namespace eCommerceAPI.API.Controllers;

public class ProductController : BaseController
{
    public ProductController() {}

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}