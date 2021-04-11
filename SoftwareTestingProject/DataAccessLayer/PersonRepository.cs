using SoftwareTestingProject.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTestingProject.DataAccessLayer
{
    public class PersonRepository : IPersonRepository
    {
        public string Delete(Person entity)
        {
            throw new NotImplementedException();
        }

        public string Insert(Person entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> SelectAll()
        {
            throw new NotImplementedException();
        }

        public Person SelectById(int? id)
        {
            throw new NotImplementedException();
        }

        public string Update(Person entity)
        {
            throw new NotImplementedException();
        }
    }
}
