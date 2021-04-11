using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTestingProject.Domain
{
    public class Place
    {
        public int? PlaceId { get; set; }
        public string Name { get; set; }
        public int? Zipcode { get; set; }
        public int? Population { get; set; }
        public virtual IEnumerable<Person> People { get; set; }
    }
}
