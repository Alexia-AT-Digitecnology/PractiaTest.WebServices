using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PractiaTest.Models.Entities;
using PractiaTest.Models.Requests.Client;

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
        
        /// <summary>
        /// Gets all clients
        /// </summary>
        /// <returns>A list of clients</returns>
        [HttpGet]
        [Route("GetAll")]
        [Consumes("application/json")]
        public ActionResult<List<Client>> GetAll()
        {
            return null;
        }

        /// <summary>
        /// Gets a client by id
        /// </summary>
        /// <param name="request"> The id of a client for request</param>
        /// <returns>A client</returns>
        [HttpGet]
        [Route("GetById")]
        [Consumes("application/json")]
        public ActionResult<Client> GetById([FromBody] GetById request)
        {
            return null;
        }
    }
}