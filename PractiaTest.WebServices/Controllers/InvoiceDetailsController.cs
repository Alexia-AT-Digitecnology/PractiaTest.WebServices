using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PractiaTest.Models.Entities;
using PractiaTest.Models.Requests.InvoiceDetail;

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
        /// <summary>
        /// Gets all by invoice id
        /// </summary>
        /// <param name="request">The invoice id for request</param>
        /// <returns>A list of invoice details</returns>
        [HttpGet]
        [Route("GetAllByInvoiceId")]
        [Consumes("application/json")]
        public ActionResult<List<InvoiceDetail>> GetAllByInvoiceId([FromBody] GetAllByInvoiceId request)
        {
            return null;
        }

        /// <summary>
        /// Gets an incoive detail by id
        /// </summary>
        /// <param name="request">The id for request</param>
        /// <returns>An invoice detail</returns>
        [HttpGet]
        [Route("GetById")]
        [Consumes("application/json")]
        public ActionResult<InvoiceDetail> GetById([FromBody] GetById request)
        {
            return null;
        }
    }
}