using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NinjaNye.SearchExtensions;
using NinjaNye.SearchExtensions.Models;

namespace POC01.Model
{
    public interface IPatientRepository
    {
        IQueryable<IRanked<Patient>> SearchRanked(string tenantId, string query);
        IQueryable<Patient> Search(string tenantId, string query);
        Patient GetSingle(string tenantId, int id);
        IQueryable<Patient> GetAll(string tenantId);
    }
}