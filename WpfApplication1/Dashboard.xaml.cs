using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;

namespace HostelManagment
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public ObservableCollection<Summery> Summaries { get; set; }

        public Dashboard()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Summaries = GetSummeryData();
            DemoDataGrid.ItemsSource = Summaries;
        }
        public ObservableCollection<Summery> GetSummeryData()
        {
            var summaries = new ObservableCollection<Summery>();
            string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    monthly_summery.id, 
                    members.name AS member_name, 
                    monthly_summery.total_cost, 
                    monthly_summery.pay, 
                    monthly_summery.due, 
                    monthly_summery.total_mill, 
                    monthly_summery.fixed_cost, 
                    monthly_summery.created_at
                FROM 
                    monthly_summery
                INNER JOIN 
                    members 
                ON 
                    monthly_summery.member_id = members.id";

                    using (var command = new MySqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            summaries.Add(new Summery
                            {
                                Id = reader.GetInt32("id"),
                                MemberName = reader.GetString("member_name"),
                                TotalCost = reader.GetDecimal("total_cost"),
                                PaidCost = reader.GetDecimal("pay"),
                                DueCost = reader.GetDecimal("due"),
                                TotalMill = reader.GetInt32("total_mill"),
                                FixedCost = reader.GetDecimal("fixed_cost"),
                                CreatedAt = reader.GetDateTime("created_at"),
                                FormattedDate = reader.GetDateTime("created_at").ToString("yyyy-MM-dd | HH:mm")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            return summaries;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int id;
            if (button != null && int.TryParse(button.Tag.ToString(), out id))
            {
                var editWindow = new EditSummery(id);
                editWindow.ShowDialog();
                LoadData();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int id;
            if (button != null && int.TryParse(button.Tag.ToString(), out id))
            {
                var result = MessageBox.Show("Are you sure you want to delete this record?", "Delete Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";
                    using (var connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM monthly_summery WHERE id = @id";
                        var command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                        LoadData();
                    }
                }
            }
        }

        private void DemoDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DemoDataGrid.SelectedItem != null)
            {
                MessageBox.Show("Selection Changed: " + DemoDataGrid.SelectedItem.ToString());
            }
        }
        private void NavigateDashboard(object sender, RoutedEventArgs e)
        {
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
            SummeryAddWindow.ShowDialog();
            LoadData();
        }
    }
    public class Summery
    {
        public int Id { get; set; }
        public string MemberName { get; set; }
        public decimal TotalCost { get; set; }
        public decimal PaidCost { get; set; }
        public decimal DueCost { get; set; }
        public int TotalMill { get; set; }
        public decimal FixedCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FormattedDate { get; set; } 
    }
}
