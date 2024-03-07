using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebKrake.Models;
using WebKrake.Network;

namespace WebKrake.Controllers
{
    public class BarController : ApiController
    {
        private Connection dbConn;
        public BarController()
        {
            dbConn = new Connection();
        }

        [Route("api/Bar/{value}")]
        public List<Event> Get(string value)
        {
            return dbConn.GetEvents(value, "bar");
        }
    }
}