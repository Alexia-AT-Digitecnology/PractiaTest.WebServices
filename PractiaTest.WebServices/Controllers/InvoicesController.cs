using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PractiaTest.Models.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PractiaTest.Models.Responses;
using PractiaTest.WebServices.Services.Interfaces;
using InvoiceDetail = PractiaTest.Database.Entities.InvoiceDetail;

namespace PractiaTest.WebServices.Controllers
{
    /// <summary>
    /// Invoices controller class
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class InvoicesController: ControllerBase
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
        public InvoicesController(IDatabaseService databaseService, ILogger<InvoicesController> logger, 
            IConfiguration configuration)
        {
            _databaseService = databaseService;
            _logger = logger;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Gets all by client id
        /// </summary>
        /// <param name="clientId">The client id for request</param>
        /// <returns> A list of invoices</returns>
        [HttpGet]
        [Route("GetAllByClientId")]
        public async Task<ActionResult<Result<List<Invoice>>>> GetAllByClientId(int clientId)
        {
            Result<List<Invoice>> result = new Result<List<Invoice>>();
            
            result.Data = new List<Invoice>();

            try
            {
                Database.Entities.Client client = this._databaseService.GetNewContextInstance().Client
                    .FirstOrDefault(c => c.ClientId == clientId);

                result.Data = (from invoice in client.Invoice
                    select new Invoice()
                    {
                        ClientId = invoice.ClientId,
                        Id = invoice.InvoiceId,
                        IssueDate = invoice.IssueDate,
                        Paid = invoice.Paid,
                        Total = CalculateInvoiceTotal(invoice.InvoiceId)
                    }).ToList();

                result.ResultCode = ResultCode.Ok;
            }
            catch (Exception e)
            {
                result.Data = new List<Invoice>();
                result.ResultCode = ResultCode.InternalError;
                result.ErrorMessage = e.Message;
            }
            
            return result;
        }

        /// <summary>
        /// Get an invoice by id
        /// </summary>
        /// <param name="invoiceId">The id for request</param>
        /// <returns>An invoice</returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<Result<Invoice>>> GetById(int invoiceId)
        {
            Result<Invoice> result = new Result<Invoice>();
            
            result.Data = null;

            try
            {
                result.Data = (from invoice in this._databaseService.GetNewContextInstance().Invoice
                    where(invoice.InvoiceId == invoiceId)
                    select new Invoice()
                    {
                        ClientId = invoice.ClientId,
                        Id = invoice.InvoiceId,
                        IssueDate = invoice.IssueDate,
                        Paid = invoice.Paid,
                        Total = CalculateInvoiceTotal(invoice.InvoiceId)
                    }).FirstOrDefault();

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

        private decimal? CalculateInvoiceTotal(int invoiceId)
        {
            Database.Entities.Invoice invoice = this._databaseService.GetNewContextInstance().Invoice
                .FirstOrDefault(i => i.InvoiceId == invoiceId);

            decimal? total = 0;

            foreach (InvoiceDetail invoiceDetail in invoice.InvoiceDetail)
            {
                total += invoiceDetail.Total;
            }

            return total;
        }
    }
}