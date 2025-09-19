using practice.Pages;
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

namespace practice.Landing_Page
{
    /// <summary>
    /// Interaction logic for Mainpage.xaml
    /// </summary>
    public partial class Mainpage : Window
    {
        public Mainpage()
        {
            InitializeComponent();
        }

        private void DashboardBtn_Click(object sender, RoutedEventArgs e)
        {

            Navigate_Panel.Navigate(new Dashboard());
        }

        private void EmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (employeePanel.Visibility == Visibility.Collapsed)
                employeePanel.Visibility = Visibility.Visible;

            else
                employeePanel.Visibility = Visibility.Collapsed;
        }

        private void ReserveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (reservePanel.Visibility == Visibility.Collapsed)
                reservePanel.Visibility = Visibility.Visible;
            else
                reservePanel.Visibility = Visibility.Collapsed;
        }

        private void empaddBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate_Panel.Navigate(new EmpAdd());
        }

        private void empmanagedBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate_Panel.Navigate(new EmpManage());
        }

        private void emppayrollBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate_Panel.Navigate(new EmpPayroll());
        }

        private void emprecordBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate_Panel.Navigate(new EmpRecord());
        }

        private void reserveaddBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate_Panel.Navigate(new ReserveAdd());
        }

        private void reservemanageBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigate_Panel.Navigate(new ReserveManage());
        }
    }
}
