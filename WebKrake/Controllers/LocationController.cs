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
    public class LocationController : ApiController
    {
        private Connection dbConn;
        public LocationController()
        {
            dbConn = new Connection();
        }

        // GET: api/Event
        [Route("api/Location/{value}")]
        public String Get(string value)
        {
            return JsonConvert.SerializeObject(dbConn.GetLocations(value));
        }

        [Route("api/Location/All/Citys")]
        public String GetAllCitys()
        {
            return JsonConvert.SerializeObject(dbConn.GetAllCitys());
        }
    }
}