using SoftwareTestingProject.DataAccessLayer;
using SoftwareTestingProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTestingProject.BusinessLogicLayer
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository repository;

        public PersonService(IPersonRepository repository)
        {
            this.repository = repository;
        }

        public string Delete(Person entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            if (entity.PersonId == null)
                throw new Exception("PersonId cannot be null!");
            
            var person = SelectById(entity.PersonId);
            
            if (person == null)
                throw new Exception("Person doesn't exist!");
            
            try
            {
                var message = repository.Delete(person);
                return message;
            }
            catch (Exception)
            {
                throw new Exception("Error deleting a person!");
            }
        }

        public string Insert(Person entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            if (!Validate(entity, out string validateMessage))
                throw new Exception("Invalid parameter: " + validateMessage);
            try
            {
                var message = repository.Insert(entity);
                return message;
            }
            catch (Exception)
            {
                throw new Exception("Error inserting a person!");
            }
        }

        public IEnumerable<Person> SelectAll()
        {
            try
            {
                return repository.SelectAll();
            }
            catch (Exception)
            {
                throw new Exception("Error selectin people!");
            }
        }

        public Person SelectById(int? id)
        {
            if (id == null)
                throw new Exception("PersonId cannot be null!");
            var person = repository.SelectById(id);
            if (person == null)
                throw new Exception("Person doesn't exist!");
            return person;
        }

        public string Update(Person entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            if (entity.PersonId == null)
                throw new Exception("PersonId cannot be null!");

            if (!Validate(entity, out string validateMessage))
                throw new Exception("Invalid parameter: " + validateMessage);

            try
            {
                var person = SelectById(entity.PersonId);
            }
            catch (Exception)
            {
                throw new Exception("Person doesn't exist!");
            }

            try
            {
                var message = repository.Update(entity);
                return message;
            }
            catch (Exception)
            {
                throw new Exception("Error updating person!");
            }
        }

        public bool Validate(Person entity, out string message)
        {
            message = string.Empty;

            if (string.IsNullOrEmpty(entity.FirstName))
            {
                message = "Enter first name!";
                return false;
            }
            else
            {
                if (!char.IsUpper(entity.FirstName.ElementAt(0)))
                {
                    message = "First name must start with upper case!";
                    return false;
                }
            }

            if (string.IsNullOrEmpty(entity.LastName))
            {
                message = "Enter last name!";
                return false;
            }
            else
            {
                if (!char.IsUpper(entity.LastName.ElementAt(0)))
                {
                    message = "Last name must start with upper case!";
                    return false;
                }
            }

            if (string.IsNullOrEmpty(entity.RegistrationNumber))
            {
                message = "Enter registration number!";
                return false;
            }
            else
            {
                if (entity.RegistrationNumber.Length != 13)
                {
                    message = "Registration number must have 13 characters!";
                    return false;
                }
            }

            if (string.IsNullOrEmpty(entity.Email))
            {
                message = "Enter email!";
                return false;
            }
            else
            {
                if (!entity.Email.EndsWith(".rs") || !entity.Email.Contains("@"))
                {
                    message = "Email must contain @ with .rs domain!";
                    return false;
                }
            }

            if (entity.Height != null && entity.Height < 35)
            {
                message = "Minimum height is 35";
                return false;
            }

            if (entity.Weight != null && (entity.Weight > 250 || entity.Weight < 10))
            {
                message = "Weight must be in a range of [10,250]";
                return false;
            }

            if (string.IsNullOrEmpty(entity.DateOfBirth.ToString()))
            {
                message = "Enter date of birth!";
                return false;
            }

            if (entity.PlaceId == null)
            {
                message = "Enter a place!";
                return false;
            }

            if (!string.IsNullOrEmpty(entity.PhoneNumber) && !entity.PhoneNumber.StartsWith("+381") && (entity.PhoneNumber.Length > 13 || entity.PhoneNumber.Length < 12))
            {
                message = "Enter a phone number in a format: +381********!";
                return false;
            }

            return true;
        }
    }
}
