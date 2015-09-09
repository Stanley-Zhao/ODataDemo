using Demo.DataSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;

namespace Demo.Controllers
{
    [EnableQuery]
    public class TripsController : ODataController
    {
        public IHttpActionResult Get()
        {
            return Ok(DemoDataSources.Instance.Trips.AsQueryable());
        }
    }
}