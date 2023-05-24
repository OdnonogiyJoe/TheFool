using ModelsApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TheFool.ViewModels;

namespace TheFool.Views
{
    /// <summary>
    /// Логика взаимодействия для EditProfilePageWindow.xaml
    /// </summary>
    public partial class EditProfilePageWindow : Window
    {
        public EditProfilePageWindow()
        {
            InitializeComponent();
            DataContext = new EditProfilePageWindow(null);
        }
        public EditProfilePageWindow(AnnouncementApi computer)
        {
            InitializeComponent();
            DataContext = new EditProfilePageWindowViewModel(computer);
        }
    }
}
