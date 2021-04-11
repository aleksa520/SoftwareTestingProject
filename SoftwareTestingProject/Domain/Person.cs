using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTestingProject.Domain
{
    public class Person
    {
        public int? PersonId { get; set; }
        public string RegistrationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public EyeCollor? EyeCollor{ get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int? PlaceId { get; set; }
        public Place Place { get; set; }
    }

    public enum EyeCollor
    {
        Other, 
        Black,
        Brown,
        Green,
        Blue,
        Gray
    }
}
