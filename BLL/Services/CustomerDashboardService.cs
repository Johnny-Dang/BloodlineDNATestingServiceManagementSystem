using DAL.Entities;
using DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services;

public class CustomerDashboardService
{
    private readonly BookingRepository _bookingRepo;
    private readonly BlogPostRepository _blogPostRepo;

    public CustomerDashboardService(BookingRepository bookingRepo, BlogPostRepository blogPostRepo)
    {
        _bookingRepo = bookingRepo;
        _blogPostRepo = blogPostRepo;
    }

    public IEnumerable<Booking> GetBookingsByCustomerId(int customerId)
    {
        return _bookingRepo.GetAll().Where(b => b.UserId == customerId);
    }

    public IEnumerable<BlogPost> GetActiveBlogPosts()
    {
        return _blogPostRepo.GetAll().Where(p => p.Status == "active");
    }
} 