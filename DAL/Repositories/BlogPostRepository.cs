using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories;

public class BlogPostRepository
{
    private readonly DnatestingServiceContext _context;

    public BlogPostRepository(DnatestingServiceContext context)
    {
        _context = context;
    }

    public IEnumerable<BlogPost> GetAll()
    {
        return _context.BlogPosts.ToList();
    }

    public BlogPost? GetById(int id)
    {
        return _context.BlogPosts.FirstOrDefault(x => x.PostId == id);
    }

    public void Add(BlogPost post)
    {
        _context.BlogPosts.Add(post);
        _context.SaveChanges();
    }

    public void Update(BlogPost post)
    {
        _context.BlogPosts.Update(post);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _context.BlogPosts.FirstOrDefault(x => x.PostId == id);
        if (entity != null)
        {
            _context.BlogPosts.Remove(entity);
            _context.SaveChanges();
        }
    }
} 