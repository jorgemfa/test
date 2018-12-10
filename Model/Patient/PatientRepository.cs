using System.Linq;
using System.Collections.Generic;
using POC01.Model.DataContext;
using NinjaNye.SearchExtensions;
using NinjaNye.SearchExtensions.Models;
using NinjaNye.SearchExtensions.Levenshtein;

namespace POC01.Model
{    
    public class PatientRepository : IPatientRepository
    {
        private readonly AzureDataContext _context;

        public PatientRepository(AzureDataContext context) => _context = context;

        public IQueryable<Patient> GetAll(string tenantId) => _context.Patients.Where(x=>x.TenantId.Equals(tenantId)).OrderBy(x => x.Name);

        public Patient GetSingle(string tenantId, int id) => _context.Patients.Where(x=>x.TenantId.Equals(tenantId)).FirstOrDefault(x => x.Id.Equals(id));

        public IQueryable<Patient> Search(string tenantId, string query) => _context.Patients.Search(x => x.Name).Containing(query);

        public IQueryable<IRanked<Patient>> SearchRanked(string tenantId, string query)
        {
            var results = _context.Patients
                                  .Search(x => x.Name, x => x.Address1, x=>x.City)
                                  .Containing(query).ToRanked();
            return results.OrderByDescending(x=>x.Hits);
        }

        //    .Where(x => x.TenantId.Equals(tenantId))
        //    .Where(x => x.Name.Contains(query) || x.Address1.Contains(query));
    }
}