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
    public class PersonController : ControllerBase
    {
        private readonly IPersonService service;
        
        public PersonController(IPersonService service)
        {
            this.service = service;
        }

        // GET: api/<PersonController>
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            List<Person> people = new List<Person>();
            people.Add(new Person()
            {
                FirstName = "ALeksa"
            });
            return people;
            //return service.SelectAll();
        }

        // GET api/<PersonController>/5
        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return service.SelectById(id);
        }

        // POST api/<PersonController>
        [HttpPost]
        public void Post([FromBody] Person person)
        {
            service.Insert(person);
        }

        // PUT api/<PersonController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Person person)
        {
            person.PersonId = id;
            service.Update(person);
        }

        // DELETE api/<PersonController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Person person = service.SelectById(id);
            service.Delete(person);
        }
    }
}
