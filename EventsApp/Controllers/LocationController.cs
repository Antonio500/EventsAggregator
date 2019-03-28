using EventsApp.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EventsApp.DataBase;

namespace EventsApp.Controllers
{
    [RoutePrefix("api/locations")]
    public class LocationController : ApiController
    {
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllLocations()
        {
            var context = new SqlConnectionLocations();
            List<Location> locations = context.GetAllLocationsFromDB();
            return Ok(locations);
        }


        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetLocation(int id)
        {
            Location location = GetLocationFromDB(id);
            return Ok(location);
        }


        [Route("{latitude}/{longitude}")]
        [HttpGet]
        public IHttpActionResult GetLocation(float latitude, float longitude)
        {
            Location location = GetLocationFromDB(latitude, longitude);
            return Ok(location);
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult PostLocation(AddLocation location)
        {
            PostLocationToDB(location);
            return Ok(location);
        }

        [Route("{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteLocation(int id)
        {
            DeleteLocationInDB(id);
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

    }
}
