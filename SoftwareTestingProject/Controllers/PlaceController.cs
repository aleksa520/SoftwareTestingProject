using Microsoft.AspNetCore.Mvc;
using SoftwareTestingProject.BusinessLogicLayer;
using SoftwareTestingProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoftwareTestingProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {

        private readonly IPlaceService service;

        public PlaceController(IPlaceService service)
        {
            this.service = service;
        }

        // GET: api/<PlaceController>
        [HttpGet]
        public IEnumerable<Place> Get()
        {
            return service.SelectAll();
        }

        // GET api/<PlaceController>/5
        [HttpGet("{id}")]
        public Place Get(int id)
        {
            return service.SelectById(id);
        }

        // POST api/<PlaceController>
        [HttpPost]
        public void Post([FromBody] Place place)
        {
            service.Insert(place);
        }

        // PUT api/<PlaceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Place place)
        {
            place.PlaceId = id;
            service.Update(place);
        }

        // DELETE api/<PlaceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Place place = service.SelectById(id);
            service.Delete(place);
        }
    }
}
