using SoftwareTestingProject.DataAccessLayer;
using SoftwareTestingProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTestingProject.BusinessLogicLayer
{
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepository repository;

        public PlaceService(IPlaceRepository repository)
        {
            this.repository = repository;
        }

        public string Delete(Place entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            if (entity.PlaceId == null)
                throw new Exception("PlaceId cannot be null!");

            var place = SelectById(entity.PlaceId);

            if (place == null)
                throw new Exception("Place doesn't exist!");
            try
            {
                var message = repository.Delete(place);
                return message;
            }
            catch (Exception)
            {
                throw new Exception("Error deleatin place!");
            }
        }

        public string Insert(Place entity)
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
                throw new Exception("Error inserting place!");
            }
        }

        public IEnumerable<Place> SelectAll()
        {
            try
            {
                return repository.SelectAll();
            }
            catch (Exception)
            {
                throw new Exception("Error selecting places!");
            }
        }

        public Place SelectById(int? id)
        {
            if (id == null)
                throw new Exception("PlaceId cannot be null!");

            var place = repository.SelectById(id);

            if (place == null)
                throw new Exception("Place doesn't exist!");

            return place;
        }

        public string Update(Place entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            if (entity.PlaceId == null)
                throw new Exception("PlaceId cannot be null!");

            if (entity.PlaceId < 1)
                throw new Exception("PlaceId cannot be less than 1!");

            if (!Validate(entity, out string validateMessage))
                throw new Exception("Invalid parameter: " + validateMessage);

            try
            {
                var message = repository.Update(entity);
                return message;
            }
            catch (Exception)
            {
                throw new Exception("Error updating place!");
            }
        }

        public bool Validate(Place entity, out string message)
        {
            message = String.Empty;

            if (string.IsNullOrEmpty(entity.Name))
            {
                message = "Enter name!";
                return false;
            }
            else
            {
                if (!char.IsUpper(entity.Name.ElementAt(0)))
                {
                    message = "Name must start with upper case!";
                    return false;
                }
            }

            if (entity.Zipcode == null)
            {
                message = "Enter zipcode!";
                return false;
            }

            if (entity.Zipcode < 11000)
            {
                message = "Zipcode invalid!";
                return false;
            }

            if (entity.Population != null && entity.Population < 0)
            {
                message = "Population must be greater than 0";
                return false;
            }

            return true;
        }
    }
}
