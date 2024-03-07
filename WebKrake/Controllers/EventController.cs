using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Web.Http;
using WebKrake.Models;
using WebKrake.Network;

namespace WebKrake.Controllers
{
    public class EventController : ApiController
    {
        private Connection dbConn;
        public EventController()
        {
            dbConn = new Connection();
        }

        // GET: api/Event
        [Route("api/Event/{value}")]
        public List<Event> Get(string value)
        {
            return dbConn.GetEvents(value, "party"); 
        }        

        [Route("api/Event/Location/{value}/{value2}")]
        public List<Event> GetEventsFromLocation(string value, string value2)
        {
            return dbConn.GetEvents(value2, null, value);
        }

        [Route("api/Event/test/connection")]
        public string GetConnectionTest()
        {
            return "yes";
        }

        // POST: api/Event
        //public void Post([FromBody]Event value)
        //{
        //    dbConn.AddEvent(value);
        //}
    }
}
