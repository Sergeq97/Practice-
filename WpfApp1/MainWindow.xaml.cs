using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.View;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PFrame.Content = new AttainmentView();
        }

        private void PFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void typeP_Click(object sender, RoutedEventArgs e)
        {
            PFrame.Content = new TypePage();
        }

        private void studentsP_Click(object sender, RoutedEventArgs e)
        {
            PFrame.Content = new StudentsView();
        }

        private void achievements_Click(object sender, RoutedEventArgs e)
        {
            PFrame.Content = new AttainmentView();
        }

        private void group_Click(object sender, RoutedEventArgs e)
        {
            PFrame.Content = new View.СlassificationP();
        }
    }
}
