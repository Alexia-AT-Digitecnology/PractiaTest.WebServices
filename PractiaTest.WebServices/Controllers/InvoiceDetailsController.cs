using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    /// Invoice Details controller class
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class InvoiceDetailsController : ControllerBase
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
        public InvoiceDetailsController(IDatabaseService databaseService, ILogger<InvoiceDetailsController> logger, 
            IConfiguration configuration)
        {
            _databaseService = databaseService;
            _logger = logger;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Gets all by invoice id
        /// </summary>
        /// <param name="invoiceId">The invoice id for request</param>
        /// <returns>A list of invoice details</returns>
        [HttpGet]
        [Route("GetAllByInvoiceId")]
        public async Task<ActionResult<Result<List<InvoiceDetail>>>> GetAllByInvoiceId(int invoiceId)
        {
            Result<List<InvoiceDetail>> result = new Result<List<InvoiceDetail>>();
            
            result.Data = new List<InvoiceDetail>();

            try
            {
                result.Data = await (from invoiceDetail in this._databaseService.GetNewContextInstance().InvoiceDetail
                    where (invoiceDetail.InvoiceId == invoiceId)
                    select new InvoiceDetail()
                    {
                        Amount = invoiceDetail.Amount,
                        InvoiceId = invoiceDetail.InvoiceId,
                        ProductId = invoiceDetail.ProductId,
                        ProductName = invoiceDetail.Product.Name,
                        SellPrice = invoiceDetail.SellPrice,
                        Total = invoiceDetail.Total
                    }).ToListAsync();

                result.ResultCode = ResultCode.Ok;
            }
            catch (Exception e)
            {
                result.Data = new List<InvoiceDetail>();
                result.ResultCode = ResultCode.InternalError;
                result.ErrorMessage = e.Message;
            }

            return result;
        }
    }
}