using DAL.Entities;
using DAL.Repositories;
using System.Collections.Generic;

namespace BLL.Services
{
    public class SampleService
    {
        private readonly SampleRepository _repo;

        public SampleService()
        {
            _repo = new SampleRepository();
        }

        public List<Sample> GetAll()
        {
            return _repo.GetAll();
        }

        public List<Sample> GetByBookingId(int bookingId)
        {
            return _repo.GetByBookingId(bookingId);
        }

        public List<Sample> GetByParticipantId(int participantId)
        {
            return _repo.GetByParticipantId(participantId);
        }

        public List<Sample> GetByUserId(int userId)
        {
            return _repo.GetByUserId(userId);
        }

        public List<Sample> Search(string searchText)
        {
            return _repo.Search(searchText);
        }

        public void Add(Sample sample)
        {
            _repo.Add(sample);
        }

        public void Update(Sample sample)
        {
            _repo.Update(sample);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        public Sample GetById(int id)
        {
            return _repo.GetById(id);
        }
    }
} 