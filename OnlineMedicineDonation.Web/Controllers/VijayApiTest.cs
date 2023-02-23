using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineMedicineDonation.Data;
using OnlineMedicineDonation.Filter;
using OnlineMedicineDonation.Model.Models;
using static OnlineMedicineDonation.Data.DataBaseContext;
using Microsoft.EntityFrameworkCore;


namespace OnlineMedicineDonation.Web.Controllers
{

    [ExceptionLog]
    [Route("api/[controller]")]
    [ApiController]
    public class VijayApiTest : ControllerBase
    {
        private readonly DataBaseContext DataContext;

        public VijayApiTest(DataBaseContext dBContext)
        {
            DataContext = dBContext;
        }
        //Employee261222 employee261222s= new Employee261222();
        [HttpGet]
        public IActionResult GetUserDetais(int id )
        {
            try
            {
                if ( id >0)
                {
                    var Data = DataContext.Employee.Find(id);


                    return Ok(Data);
                }
                else
                {
                    //var Data=(from data in DataContext.Employee select data).ToList();

                   var Data = DataContext.Employee.ToList();
                    return Ok(Data);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        [HttpPost]
        public async Task<ActionResult<DbEmployee>> PostUserDetails(DbEmployee employee)
        {
            try
            {
                if(employee==null)
                {
                    return BadRequest();
                }
                else
                {
                 if(employee.Id>0)
                    {
                        DataContext.Entry(employee).State = EntityState.Modified;
                        await DataContext.SaveChangesAsync();

                        return Ok(new { status = true, });
                    }
                    else
                    {

                    }
                        DataContext.Employee.Add(employee);
                        await DataContext.SaveChangesAsync();

                        return Ok(new { status = true, });
                    
                   
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUserDetails(int id)
        {
            try
            {
                if (id >0)
                {
                    var Employee = DataContext.Employee.Find(id);
                    DataContext.Employee.Remove(Employee);
                    return Ok(new { status = true, });
                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch(Exception ex)
            {
              throw ex;
            }
            
        }
       

    }

}
