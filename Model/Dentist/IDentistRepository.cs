using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POC01.Model
{
    public interface IDentistRepository
    {
        IQueryable<Dentist> Search(string tenantId, string query);
        Dentist GetSingle(string tenantId, int id);
        IQueryable<Dentist> GetAll(string tenantId);
        Task<IEnumerable<Dentist>> GetPatientsAsync(string tenantId, int id);
    }
}