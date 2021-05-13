using Moq;
using SoftwareTestingProject.DataAccessLayer;
using SoftwareTestingProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUnitTestProject
{
    public class Mock
    {
        public static Mock<IPlaceRepository> MockPlaceRepository()
        {

            var places = new List<Place>()
            {
                new Place
                {
                    PlaceId = 1,
                    Name = "Kragujevac",
                    Zipcode = 34000,
                    Population = 147000
                },
                new Place
                {
                    PlaceId = 2,
                    Name = "Beograd",
                    Zipcode = 11000,
                    Population = 1560000,     
                }
            };

            var mockRepository = new Mock<IPlaceRepository>();

            mockRepository.Setup(repo => repo.SelectAll()).Returns(places);

            mockRepository.Setup(repo => repo.SelectById(It.IsAny<int>())).Returns((int i) => places.SingleOrDefault(x => x.PlaceId == i));
           
            mockRepository.Setup(i => i.Insert(It.IsAny<Place>())).Callback((Place place) =>
            {
                var id = places.Count() + 1;
                place.PlaceId = id;
                places.Add(place);

            }).Returns("Successful!");

            mockRepository.Setup(m => m.Update(It.IsAny<Place>())).Callback((Place target) =>
            {
                var original = places.FirstOrDefault(
                    q => q.PlaceId == target.PlaceId);

                if (original != null)
                {
                    original.Name = target.Name;
                    original.Population = target.Population;
                    original.Zipcode = target.Zipcode;
                }

            }).Returns("Successful!");

            mockRepository.Setup(m => m.Delete(It.IsAny<Place>())).Callback((Place i) =>
            {
                var original = places.FirstOrDefault(
                    q => q.PlaceId == i.PlaceId);

                if (original != null)
                {
                    places.Remove(original);
                }

            }).Returns("Successful!");

            return mockRepository;
        }

        public static Mock<IPersonRepository> MockPersonRepository()
        {
            var people = new List<Person>()
            {
                new Person
                {
                    PersonId = 1,
                    RegistrationNumber = "2706997720013",
                    FirstName = "Aleksa",
                    LastName = "Pavlovic",
                    DateOfBirth = new DateTime(1997, 06, 27),
                    Height = 193,
                    Weight = 98,
                    PhoneNumber = "+381656086398",
                    Email = "ap20160012@gmail.rs",
                    Address = "Brace Miladinov 4",
                    EyeCollor = EyeCollor.Brown,
                    Note = "Student master akademskih studija na FON-u",
                    PlaceId = 1,
                }
            };

            var mockRepository = new Mock<IPersonRepository>();

            mockRepository.Setup(repo => repo.SelectAll()).Returns(people);

            mockRepository.Setup(repo => repo.SelectById(It.IsAny<int>())).Returns((int i) => people.SingleOrDefault(x => x.PlaceId == i));
           
            mockRepository.Setup(i => i.Insert(It.IsAny<Person>())).Callback((Person person) =>
            {
                var id = people.Count() + 1;
                person.PersonId = id;
                people.Add(person);

            }).Returns("Successful!");

            mockRepository.Setup(m => m.Update(It.IsAny<Person>())).Callback((Person target) =>
            {
                var original = people.FirstOrDefault(
                    q => q.PersonId == target.PersonId);

                if (original != null)
                {
                    original.FirstName = target.FirstName;
                    original.LastName = target.LastName;
                    original.RegistrationNumber = target.RegistrationNumber;
                    original.DateOfBirth = target.DateOfBirth;
                    original.PhoneNumber = target.PhoneNumber;
                    original.Height = target.Height;
                    original.Weight = target.Weight;
                    original.EyeCollor = target.EyeCollor;
                    original.Address = target.Address;
                    original.Email = target.Email;
                    original.Note = target.Note;
                    original.PlaceId = target.PlaceId;
                }

            }).Returns("Successful!");

            mockRepository.Setup(m => m.Delete(It.IsAny<Person>())).Callback((Person i) =>
            {
                var original = people.FirstOrDefault(
                    q => q.PersonId == i.PersonId);

                if (original != null)
                {
                    people.Remove(original);
                }

            }).Returns("Successful!");

           return mockRepository;
        }
    }
}
