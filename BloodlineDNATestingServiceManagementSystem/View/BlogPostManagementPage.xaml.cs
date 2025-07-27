using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BLL.Services;
using DAL.Entities;

namespace BloodlineDNATestingServiceManagementSystem.View
{
    public partial class BlogPostManagementPage : UserControl
    {
        private readonly BlogPostService _service;
        private readonly string _currentRole;
        private List<BlogPost> _allPosts;

        public BlogPostManagementPage()
        {
            InitializeComponent();
            _service = new BlogPostService();

            var user = SessionManager.CurrentUser;
            _currentRole = user?.Role?.RoleName?.ToUpper() ?? "GUEST";

            SetPermissionUI();
            LoadData();
        }

        private void SetPermissionUI()
        {
            if (_currentRole != "ADMIN" && _currentRole != "MANAGER")
            {
                BtnAdd.Visibility = Visibility.Collapsed;
                BtnEdit.Visibility = Visibility.Collapsed;
                BtnDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadData()
        {
            _allPosts = _service.GetAllPosts();

            if (_currentRole != "ADMIN" && _currentRole != "MANAGER")
            {
                _allPosts = _allPosts.Where(p => p.Status?.ToLower() == "active").ToList();
            }

            PostsPanel.ItemsSource = null;
            PostsPanel.ItemsSource = _allPosts;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new BlogPostDialogWindow();
            if (dialog.ShowDialog() == true)
            {
                var newPost = dialog.BlogPost;
                var user = SessionManager.CurrentUser;
                newPost.UserId = user.UserId;
                newPost.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

                _service.CreatePost(newPost);
                LoadData();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedPost = PostsPanel.SelectedItem as BlogPost;
            if (selectedPost == null)
            {
                MessageBox.Show("Vui lòng chọn bài viết để sửa.");
                return;
            }

            var dialog = new BlogPostDialogWindow(new BlogPost
            {
                PostId = selectedPost.PostId,
                Title = selectedPost.Title,
                Content = selectedPost.Content,
                Status = selectedPost.Status
            });

            if (dialog.ShowDialog() == true)
            {
                var updated = dialog.BlogPost;
                selectedPost.Title = updated.Title;
                selectedPost.Content = updated.Content;
                selectedPost.Status = updated.Status;
                selectedPost.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
                selectedPost.UpdateBy = SessionManager.CurrentUser.UserId;

                _service.UpdatePost(selectedPost);
                LoadData();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedPost = PostsPanel.SelectedItem as BlogPost;
            if (selectedPost == null)
            {
                MessageBox.Show("Vui lòng chọn bài viết để xóa.");
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa bài viết: {selectedPost.Title}?", "Xác nhận", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                _service.DeletePost(selectedPost.PostId);
                LoadData();
            }
        }
    }
}
