using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DetailResultRepositorys
    {
        private readonly DnatestingServiceContext _db;

        public DetailResultRepositorys()
        {
            _db = new DnatestingServiceContext();
        }

        public List<DetailResult> GetAll()
        {
            return _db.DetailResults.Include(dr => dr.TestResult).ToList();
        }

        public List<DetailResult> GetByTestResultId(int testResultId)
        {
            return _db.DetailResults.Where(dr => dr.TestResultId == testResultId).ToList();
        }

        public void Add(DetailResult detailResult)
        {
            _db.DetailResults.Add(detailResult);
            _db.SaveChanges();
        }

        public void Update(DetailResult detailResult)
        {
            var fromDb = _db.DetailResults.Find(detailResult.DetailResultId);
            if (fromDb != null)
            {
                fromDb.LocusName = detailResult.LocusName;
                fromDb.P1Allele1 = detailResult.P1Allele1;
                fromDb.P1Allele2 = detailResult.P1Allele2;
                fromDb.P2Allele1 = detailResult.P2Allele1;
                fromDb.P2Allele2 = detailResult.P2Allele2;
                fromDb.PaternityIndex = detailResult.PaternityIndex;
                fromDb.TestResultId = detailResult.TestResultId;

                _db.DetailResults.Update(fromDb);
                _db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var detail = _db.DetailResults.Find(id);
            if (detail != null)
            {
                _db.DetailResults.Remove(detail);
                _db.SaveChanges();
            }
        }
    }
}
