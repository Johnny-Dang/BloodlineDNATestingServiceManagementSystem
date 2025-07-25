using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TestResultRepository
    {
        private readonly DnatestingServiceContext _db;

        public TestResultRepository()
        {
            _db = new DnatestingServiceContext();
        }
        public List<TestResult> GetAll() => _db.TestResults.Include(t => t.Booking).ToList();

        public List<TestResult> Search(string searchText)
        {
            return _db.TestResults
                .Where(x => x.ResultConclution.Contains(searchText.ToLower()) ||
                            x.ResultFile.Contains(searchText.ToLower()))
                .ToList();
        }
        public void Add(TestResult result)
        {
            _db.TestResults.Add(result);
            _db.SaveChanges();
        }
        public void Update(TestResult result)
        {
            var formDB = _db.TestResults.Find(result.TestResultId);
            if (formDB != null)
            {
                formDB.BookingId = result.BookingId;
                formDB.ResultDate = result.ResultDate;
                formDB.CreatedBy = result.CreatedBy;
                formDB.CreatedDate = result.CreatedDate;
                formDB.ResultConclution = result.ResultConclution;
                formDB.ResultFile = result.ResultFile;
                formDB.UpdatedBy = result.UpdatedBy;
                formDB.UpdatedDate = result.UpdatedDate;

                _db.TestResults.Update(formDB);
                _db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var result = _db.TestResults.Find(id);
            if (result != null)
            {
                _db.TestResults.Remove(result);
                _db.SaveChanges();
            }
        }
    }
}
