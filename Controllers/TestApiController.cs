using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MiidWeb.Controllers
{
    public class TestApiController : ApiController
    {
        // GET: api/TestApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TestApi/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TestApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TestApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TestApi/5
        public void Delete(int id)
        {
        }
    }
}
