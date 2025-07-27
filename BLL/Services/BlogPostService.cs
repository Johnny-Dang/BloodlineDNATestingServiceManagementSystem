using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services
{
    public class BlogPostService
    {
        private readonly BlogPostRepository _repository;

        public BlogPostService()
        {
            _repository = new BlogPostRepository();
        }

        public List<BlogPost> GetAllPosts() => _repository.GetAllPosts();

        public BlogPost? GetPostById(int id) => _repository.GetPostById(id);

        public void CreatePost(BlogPost post) => _repository.CreatePost(post);

        public void UpdatePost(BlogPost post) => _repository.UpdatePost(post);

        public void DeletePost(int id) => _repository.DeletePost(id);
    }

}
