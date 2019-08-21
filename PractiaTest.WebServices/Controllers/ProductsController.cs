using Microsoft.AspNetCore.Mvc;
using PractiaTest.Models.Entities;
using PractiaTest.Models.Requests.Product;

namespace PractiaTest.WebServices.Controllers
{
    /// <summary>
    /// Products controller class
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Gets a product by id
        /// </summary>
        /// <param name="request">The id for request</param>
        /// <returns>A product</returns>
        [HttpGet]
        [Route("GetById")]
        [Consumes("application/json")]
        public ActionResult<Product> GetById([FromBody] GetById request)
        {
            return null;
        }
    }
}