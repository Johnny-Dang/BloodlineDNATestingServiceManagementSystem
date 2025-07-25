using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class FeedBackRepository
    {
       private readonly DnatestingServiceContext _db;

        public FeedBackRepository()
        {
            _db = new DnatestingServiceContext();
        }
        public List<Feedback> GetAll()
        {
            return _db.Feedbacks.Include(f => f.Booking).ToList();
        }

        public Feedback? GetById(int id)
        {
            return _db.Feedbacks.Include(f => f.Booking).FirstOrDefault(f => f.FeedbackId == id);
        }

        public void Add(Feedback feedback)
        {
            _db.Feedbacks.Add(feedback);
            _db.SaveChanges();
        }
        public void Update(Feedback feedback)
        {
            var fromDb = _db.Feedbacks.Find(feedback.FeedbackId);
            if (fromDb != null)
            {
                fromDb.BookingId = feedback.BookingId;
                fromDb.Comments = feedback.Comments;
                fromDb.Answers = feedback.Answers;
                fromDb.Rating = feedback.Rating;
                fromDb.CreateDate = feedback.CreateDate;
                fromDb.ReturnDate = feedback.ReturnDate;
                fromDb.Status = feedback.Status;

                _db.Feedbacks.Update(fromDb);
                _db.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var feedback = _db.Feedbacks.Find(id);
            if (feedback != null)
            {
                _db.Feedbacks.Remove(feedback);
                _db.SaveChanges();
            }
        }
    }
}
