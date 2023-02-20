using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMedicineDonation.Filter;
using OnlineMedicineDonation.Model.APIModel;
using OnlineMedicineDonation.Services;

namespace OnlineMedicineDonation.Web.Controllers
{
    [Authorize]
    [ExceptionLog]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { status = true, response = "hello" });
        }
       

    }
}
