using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MiidWeb.Controllers
{
    public class ServiceController : ApiController
    {
        // GET: api/Service
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Service/5
        public string Get(int id)
        {
            switch (id)
            {
                case 1: TicketRepository.ClearExpiredTicketSeats(); break;
                default: break;
            }
            return id.ToString(); ;
        }

        // POST: api/Service
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Service/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Service/5
        public void Delete(int id)
        {
        }
    }
}
