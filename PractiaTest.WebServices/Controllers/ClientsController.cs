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
using Invoice = PractiaTest.Database.Entities.Invoice;
using InvoiceDetail = PractiaTest.Database.Entities.InvoiceDetail;

namespace PractiaTest.WebServices.Controllers
{
    /// <summary>
    /// Clients controller class
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ClientsController : ControllerBase
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
        public ClientsController(IDatabaseService databaseService, ILogger<ClientsController> logger,
            IConfiguration configuration)
        {
            _databaseService = databaseService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Gets all clients
        /// </summary>
        /// <returns>A list of clients</returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<Result<List<Client>>>> GetAll()
        {
            Result<List<Client>> result = new Result<List<Client>>();

            result.Data = new List<Client>();

            try
            {
                result.Data = await (from client in this._databaseService.GetNewContextInstance().Client
                    select new Client()
                    {
                        Address = client.Address,
                        BornDate = client.BornDate,
                        Email = client.Email,
                        Has10PercentDiscount = HasClient10PercentDiscount(client.ClientId),
                        Id = client.ClientId,
                        Name = client.Name,
                        PhoneNumber = client.PhoneNumber,
                        Invoices = GetClientInvoicesIds(client.ClientId)
                    }).ToListAsync();

                result.ResultCode = ResultCode.Ok;
            }
            catch (Exception e)
            {
                result.Data = new List<Client>();
                result.ResultCode = ResultCode.InternalError;
                result.ErrorMessage = e.Message;
            }

            return result;
        }

        /// <summary>
        /// Gets a client by id
        /// </summary>
        /// <param name="clientId"> The id of a client for request</param>
        /// <returns>A client</returns>
        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<Result<Client>>> GetById(int clientId)
        {
            Result<Client> result = new Result<Client>();

            result.Data = null;

            try
            {
                result.Data = await (from client in this._databaseService.GetNewContextInstance().Client
                    where (client.ClientId == clientId)
                    select new Client()
                    {
                        Address = client.Address,
                        BornDate = client.BornDate,
                        Email = client.Email,
                        Has10PercentDiscount = HasClient10PercentDiscount(client.ClientId),
                        Id = client.ClientId,
                        Name = client.Name,
                        PhoneNumber = client.PhoneNumber,
                        Invoices = GetClientInvoicesIds(client.ClientId)
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

        /// <summary>
        /// Gets a complete client by id
        /// </summary>
        /// <param name="clientId"> The id of a client for request</param>
        /// <returns>A complete client</returns>
        [HttpGet]
        [Route("GetCompleteById")]
        public async Task<ActionResult<Result<CompleteClient>>> GetCompleteById(int clientId)
        {
            Result<CompleteClient> result = new Result<CompleteClient>();

            result.Data = null;

            try
            {
                result.Data = await (from client in this._databaseService.GetNewContextInstance().Client
                    where (client.ClientId == clientId)
                    select new CompleteClient()
                    {
                        Address = client.Address,
                        BornDate = client.BornDate,
                        Email = client.Email,
                        Has10PercentDiscount = HasClient10PercentDiscount(client.ClientId),
                        Id = client.ClientId,
                        Name = client.Name,
                        PhoneNumber = client.PhoneNumber,
                        Invoices = GetClientInvoices(client.ClientId)
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

        private List<Models.Entities.Invoice> GetClientInvoices(int clientId)
        {
            List<Models.Entities.Invoice> invoices =
                (from invoice in this._databaseService.GetNewContextInstance().Invoice
                    where (invoice.ClientId == clientId)
                    select new Models.Entities.Invoice()
                    {
                        ClientId = invoice.ClientId,
                        Id = invoice.InvoiceId,
                        IssueDate = invoice.IssueDate,
                        Paid = invoice.Paid,
                        Total = CalculateInvoiceTotal(invoice.InvoiceId)
                    }).ToList();

            return invoices;
        }

        private List<int> GetClientInvoicesIds(int clientId)
        {
            Database.Entities.Client client = this._databaseService.GetNewContextInstance().Client
                .FirstOrDefault(c => c.ClientId == clientId);
            List<int> invoices = new List<int>();

            foreach (Invoice invoice in client.Invoice)
            {
                invoices.Add(invoice.InvoiceId);
            }

            return invoices;
        }

        private decimal HasClient10PercentDiscount(int clientId)
        {
            List<int> invoiceIds = GetClientInvoicesIds(clientId);

            List<InvoiceDetail> invoiceDetails = this._databaseService.GetNewContextInstance().InvoiceDetail
                .Where(id => invoiceIds.Contains(id.InvoiceId)).ToList();

            decimal totalPrice = 0;

            foreach (InvoiceDetail invoiceDetail in invoiceDetails)
            {
                totalPrice += invoiceDetail.SellPrice - invoiceDetail.Product.UnitPrice;
            }

            if (totalPrice > 0)
            {
                totalPrice = 0;
            }
            else
            {
                totalPrice = (Math.Abs(totalPrice) * 10) / 100;
            }

            return totalPrice;
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