using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PractiaTest.Models.Entities;
using PractiaTest.Models.Requests.Invoice;

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
        /// <summary>
        /// Gets all by client id
        /// </summary>
        /// <param name="request">The client id for request</param>
        /// <returns> A list of invoices</returns>
        [HttpGet]
        [Route("GetAllByClientId")]
        [Consumes("application/json")]
        public ActionResult<List<Invoice>> GetAllByClientId([FromBody] GetAllByClientId request)
        {
            return null;
        }

        /// <summary>
        /// Get an invoice by id
        /// </summary>
        /// <param name="request">The id for request</param>
        /// <returns>An invoice</returns>
        [HttpGet]
        [Route("GetById")]
        [Consumes("application/json")]
        public ActionResult<Invoice> GetById([FromBody] GetById request)
        {
            return null;
        }
    }
}