using SoftwareTestingProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareTestingProject.DataAccessLayer
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
    }
}
