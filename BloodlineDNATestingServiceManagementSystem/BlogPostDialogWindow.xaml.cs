using System.Windows;
using System.Windows.Controls;
using DAL.Entities;

namespace BloodlineDNATestingServiceManagementSystem.View
{
    public partial class BlogPostDialogWindow : Window
    {
        public BlogPost BlogPost { get; private set; }

        public BlogPostDialogWindow(BlogPost? post = null)
        {
            InitializeComponent();

            if (post != null)
            {
                BlogPost = post;
                txtTitle.Text = post.Title;
                txtContent.Text = post.Content;
                cbStatus.SelectedItem = cbStatus.Items.Cast<ComboBoxItem>()
                    .FirstOrDefault(i => i.Content.ToString() == post.Status);
            }
            else
            {
                BlogPost = new BlogPost();
                cbStatus.SelectedIndex = 0;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            BlogPost.Title = txtTitle.Text;
            BlogPost.Content = txtContent.Text;
            BlogPost.Status = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
