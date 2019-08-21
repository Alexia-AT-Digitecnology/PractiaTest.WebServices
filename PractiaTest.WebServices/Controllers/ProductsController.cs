using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PractiaTest.Models.Entities;
using PractiaTest.Models.Responses;
using PractiaTest.WebServices.Services.Interfaces;

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
        private readonly IDatabaseService _databaseService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseService">The database service injected</param>
        /// <param name="logger">The logger injected</param>
        /// <param name="configuration">The configuration injected</param>
        public ProductsController(IDatabaseService databaseService, ILogger<ProductsController> logger, 
            IConfiguration configuration)
        {
            _databaseService = databaseService;
            _logger = logger;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Gets a product by id
        /// </summary>
        /// <param name="productId">The id for request</param>
        /// <returns>A product</returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<Result<Product>>> GetById(int productId)
        {
            Result<Product> result = new Result<Product>();

            result.Data = null;

            try
            {
                result.Data = await (from product in this._databaseService.GetNewContextInstance().Product
                    where (product.ProductId == productId)
                    select new Product()
                    {
                        Id = product.ProductId,
                        Name = product.Name,
                        UnitPrice = product.UnitPrice
                    }).FirstOrDefaultAsync();

                result.ResultCode = ResultCode.Ok;
            }
            catch (Exception e)
            {
                result.Data = null;
                result.ResultCode = ResultCode.InternalError;
                result.ErrorMessage = e.Message;
            }

            return result;
        }
    }
}