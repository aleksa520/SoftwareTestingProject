using Moq;
using NUnit.Framework;
using SoftwareTestingProject.BusinessLogicLayer;
using SoftwareTestingProject.DataAccessLayer;
using SoftwareTestingProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUnitTestProject
{
    public class PersonTests
    {
        private Mock<IPersonRepository> mockPersonRepository = NUnitTestProject.Mock.MockPersonRepository();
        private PersonService service;

        public PersonTests()
        {
            service = new PersonService(mockPersonRepository.Object);
        }

        [Test]
        public void TestSelectAll()
        {
            var result = service.SelectAll();

            Assert.IsNotNull(result);
            Assert.IsAssignableFrom<List<Person>>(result);
        }

        [TestCase(null)]
        [TestCase(-1)]
        [TestCase(1)]
        [TestCase(100)]
        public void TestSelectById(int? id)
        {

            switch (id)
            {
                case 1:
                    var person = service.SelectById(id);
                    Assert.That(person.PersonId == 1);
                    Assert.IsNotNull(person);
                    break;

                case -1:
                    Assert.Throws<Exception>(() =>
                                  service.SelectById(id), "PersonId can't be under 0!");
                    break;

                case null:
                    Assert.Throws<Exception>(() =>
                                  service.SelectById(id), "PersonId can't be null!");
                    break;

                default:
                    Assert.Throws<Exception>(() =>
                                 service.SelectById(id), "Person doesn't exist!");
                    break;
            }
        }

        [TestCase(1)]
        [TestCase(null)]
        public void TestDeletePerson(int? id)
        {
            switch (id)
            {
                case null:
                    Assert.Throws<ArgumentNullException>(() =>
                               service.Delete(null));
                    break;
                case 1:
                    var person = service.SelectById(1);
                    var result = service.Delete(person);

                    Assert.That(result.Equals("Successful!"));
                    Assert.Throws<Exception>(() =>
                                service.SelectById(1), "PersonId can't be null!");
                    break;
                default: break;
            }
        }

        [Test]
        public void TestInsertValidPerson()
        {
            var person = new Person
            {
                FirstName = "Marko",
                LastName = "Markovic",
                RegistrationNumber = "1234567891234",
                Height = 200,
                Weight = 100,
                Address = "Radomira Putnika 2",
                Email = "marko@gmail.rs",
                PhoneNumber = "+381656086392",
                EyeCollor = EyeCollor.Blue,
                DateOfBirth = new DateTime(2001, 09, 01),
                PlaceId = 1,
                Note = "Grill maestro"
            };

            var result = service.Insert(person);

            Assert.NotNull(result);
            Assert.That(result.Equals("Successful!"));

            var addedPerson = service.SelectAll().Last();

            Assert.AreEqual(person.RegistrationNumber, addedPerson.RegistrationNumber);
        }

        [TestCase("FirstName", "joe")]
        [TestCase("LastName", "")]
        [TestCase("RegistrationNumber", "-999")]
        [TestCase("Height", 34)]
        [TestCase("Weight", 251)]
        [TestCase("Email", "joe.com")]
        [TestCase("DateOfBirth", null)]
        [TestCase("PlaceId", null)]
        public void TestInsertInvalidPersonShouldThrowException(string field, object value)
        {
            Person person = new Person
            {
                PersonId = 1,
                FirstName = "Jon",
                LastName = "Doe",
                RegistrationNumber = "1234567891234",
                Height = 200,
                Weight = 100,
                Email = "jon@gmail.rs",
                PhoneNumber = "+381656086392",
                EyeCollor = EyeCollor.Blue,
                Address = "8. Marta br. 5",
                DateOfBirth = new DateTime(2000, 07, 25),
                PlaceId = 1,
                Note = "Test slucaj"
            };

            switch (field)
            {
                case "FirstName":
                    person.FirstName = value.ToString();
                    Assert.Throws<Exception>(() => service.Insert(person), "First name must start with upper case!");
                    break;
                case "LastName":
                    person.LastName = value.ToString();
                    Assert.Throws<Exception>(() => service.Insert(person), "Last name cannot be empty!");
                    break;
                case "RegistrationNumber":
                    person.RegistrationNumber = value.ToString();
                    Assert.Throws<Exception>(() => service.Insert(person), "Invalid regustration number!");
                    break;
                case "Height":
                    person.Height = Convert.ToInt32(value);
                    Assert.Throws<Exception>(() => service.Insert(person), "Minimum height is 35");
                    break;
                case "Weight":
                    person.Weight = Convert.ToInt32(value);
                    Assert.Throws<Exception>(() => service.Insert(person), "Range for weight is [10,250]");
                    break;
                case "Email":
                    person.Email = value.ToString();
                    Assert.Throws<Exception>(() => service.Insert(person), "Email must contain @ wuth .rs domain!");
                    break;
                case "DateOfBirth":
                    person.DateOfBirth = null;
                    Assert.Throws<Exception>(() => service.Insert(person), "Invalid Date of birth!");
                    break;
                case "PlaceId":
                    person.PlaceId = null;
                    Assert.Throws<Exception>(() => service.Insert(person), "Invalid PlaceId!");
                    break;
                default: break;
            }
        }

        [Test]
        public void TestUpdateValidPerson()
        {
            var person = service.SelectById(1);
            person.LastName = "Jovi";

            var result = service.Update(person);

            Assert.That(result.Equals("Successful!"));

            var updatedPerson = service.SelectById(1);

            Assert.AreEqual(person.LastName, updatedPerson.LastName);

        }

        [TestCase("PersonId", null)]
        [TestCase("FirstName", "joe")]
        [TestCase("LastName", "")]
        [TestCase("RegistrationNumber", "-999")]
        [TestCase("Height", 34)]
        [TestCase("Weight", 251)]
        [TestCase("Email", "jon.com")]
        [TestCase("DateOfBirth", null)]
        [TestCase("PlaceId", null)]
        public void TestUpdateInvalidPersonShouldThrowException(string field, object value)
        {
            switch (field)
            {
                case "PersonId":
                    var person = service.SelectById(1);
                    person.PersonId = null;
                    Assert.Throws<Exception>(() => service.Update(person), "Invalid PersonId!");
                    break;
                case "FirstName":
                    person = service.SelectById(1);
                    person.FirstName = value.ToString();
                    Assert.Throws<Exception>(() => service.Update(person), "First name must start wth upper case!");
                    break;
                case "LastName":
                    person = service.SelectById(1);
                    person.LastName = value.ToString();
                    Assert.Throws<Exception>(() => service.Update(person), "Enter Last name!");
                    break;
                case "RegistrationNumber":
                    person = service.SelectById(1);
                    person.RegistrationNumber = value.ToString();
                    Assert.Throws<Exception>(() => service.Update(person), "Invalid Registration number!");
                    break;
                case "Height":
                    person = service.SelectById(1);
                    person.Height = Convert.ToInt32(value);
                    Assert.Throws<Exception>(() => service.Update(person), "Minimum height is 35!");
                    break;
                case "Weight":
                    person = service.SelectById(1);
                    person.Weight = Convert.ToInt32(value);
                    Assert.Throws<Exception>(() => service.Update(person), "Range for weight is [10,250]");
                    break;
                case "Email":
                    person = service.SelectById(1);
                    person.Email = value.ToString();
                    Assert.Throws<Exception>(() => service.Update(person), "Email must contain @ wuth .rs domain!");
                    break;
                case "DateOfBirth":
                    person = service.SelectById(1);
                    person.DateOfBirth = null;
                    Assert.Throws<Exception>(() => service.Update(person), "Invalid Date of birth!");
                    break;
                case "PlaceId":
                    person = service.SelectById(1);
                    person.PlaceId = null;
                    Assert.Throws<Exception>(() => service.Update(person), "Invalid PlaceId!");
                    break;
                default: break;
            }
        }
    }
}
