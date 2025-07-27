using DAL.Entities;
using DAL.Repositories;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ParticipantService
    {
        private readonly ParticipantRepository _repo;

        public ParticipantService()
        {
            _repo = new ParticipantRepository();
        }

        public List<Participant> GetAll()
        {
            return _repo.GetAll();
        }

        public List<Participant> GetByBookingId(int bookingId)
        {
            return _repo.GetByBookingId(bookingId);
        }

        public List<Participant> GetByUserId(int userId)
        {
            return _repo.GetByUserId(userId);
        }

        public List<Participant> Search(string searchText)
        {
            return _repo.Search(searchText);
        }

        public void Add(Participant participant)
        {
            _repo.Add(participant);
        }

        public void Update(Participant participant)
        {
            _repo.Update(participant);
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        public Participant GetById(int id)
        {
            return _repo.GetById(id);
        }
    }
} 