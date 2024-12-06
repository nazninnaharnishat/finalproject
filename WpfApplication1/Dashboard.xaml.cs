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

namespace HostelManagment
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
        }
        private void DemoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle the selection change here.
            if (DemoDataGrid.SelectedItem != null)
            {
                MessageBox.Show("Selection Changed: " + DemoDataGrid.SelectedItem.ToString());
            }
        }
        private void NavigateDashboard(object sender, RoutedEventArgs e)
        {
            // Navigate to another instance of Dashboard (or refresh)
            var dashboard = new Dashboard();
            dashboard.Show();
            this.Close();
        }
        private void NavigateMembers(object sender, RoutedEventArgs e)
        {
            var membersPage = new HostelManagement.MemberPage();
            membersPage.Show();
        }
        private void NavigateAddMember(object sender, RoutedEventArgs e)
        {
            var addMemberPage = new AddMemberPage();
            addMemberPage.Show();
        }
        private void NavigateDailyCost(object sender, RoutedEventArgs e)
        {
            var dailyCostPage = new DailyCostPage();
            dailyCostPage.Show();
        }

        private void add_summery_btn(object sender, RoutedEventArgs e)
        {
            SummeryAdd SummeryAddWindow = new SummeryAdd();
            SummeryAddWindow.Show();
        }
    }
}
