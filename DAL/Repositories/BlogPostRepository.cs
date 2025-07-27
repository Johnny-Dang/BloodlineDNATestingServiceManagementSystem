using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BlogPostRepository
    {
        private readonly DnatestingServiceContext _context;

        public BlogPostRepository()
        {
            _context = new DnatestingServiceContext();
        }

        public List<BlogPost> GetAllPosts() =>
            _context.BlogPosts.Include(p => p.User).ToList();

        public BlogPost? GetPostById(int id) =>
            _context.BlogPosts.FirstOrDefault(p => p.PostId == id);

        public void CreatePost(BlogPost post)
        {
            _context.BlogPosts.Add(post);
            _context.SaveChanges();
        }

        public void UpdatePost(BlogPost post)
        {
            _context.BlogPosts.Update(post);
            _context.SaveChanges();
        }

        public void DeletePost(int id)
        {
            var post = _context.BlogPosts.Find(id);
            if (post != null)
            {
                _context.BlogPosts.Remove(post);
                _context.SaveChanges();
            }
        }
    }

}
