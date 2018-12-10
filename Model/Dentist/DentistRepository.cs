using System.Linq;
using System.Collections.Generic;
using POC01.Model.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace POC01.Model
{
    public class DentistRepository : IDentistRepository
    {
        private readonly AzureDataContext _context;

        public DentistRepository(AzureDataContext context) => _context = context;

        public IQueryable<Dentist> GetAll(string tenantId) => _context.Dentists.Where(x => x.TenantId.Equals(tenantId)).OrderBy(x => x.Name);

        public async Task<IEnumerable<Dentist>> GetPatientsAsync(string tenantId, int id) => await _context.Dentists.Include(x => x.Patients).Where(x => x.Id.Equals(id)).Where(x => x.TenantId.Equals(tenantId)).ToListAsync();
        
        public Dentist GetSingle(string tenantId, int id) => _context.Dentists.Where(x => x.TenantId.Equals(tenantId)).FirstOrDefault(x => x.Id.Equals(id));

        public IQueryable<Dentist> Search(string tenantId, string query) => _context.Dentists.Where(x => x.TenantId.Equals(tenantId)).Where(x => x.Name.Contains(query));
    }
}