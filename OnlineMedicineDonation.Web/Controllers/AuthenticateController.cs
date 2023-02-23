//using OnlineMedicineDonation.Model.APIModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMedicineDonation.Filter;
using OnlineMedicineDonation.Model.Models;
using OnlineMedicineDonation.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineMedicineDonation.Web.Controllers
{
    [ExceptionLog]
    [Route("API/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ICustomAuthenticationManager customAuthenticationManager;
        Log log = Log.Getinstance;
        public AuthenticateController(ICustomAuthenticationManager customAuthenticationManager)
        {
            this.customAuthenticationManager = customAuthenticationManager;
        }

        [HttpPost]
        public IActionResult Post([FromBody] EmployeeModel ObjEmployee)
        {
            try
            {
                var token = customAuthenticationManager.Authenticate(ObjEmployee.EmpName, ObjEmployee.EmpContact);
                if (token == null)
                    return Unauthorized();
                return Ok(new { status = true, response = token });
            }
            catch (Exception ex)
            {
                log.WriteLog("AuthenticatePost" + ex.Message);
                return NotFound(new { status = false, response = ex.Message });
            }
        }
    }
}
