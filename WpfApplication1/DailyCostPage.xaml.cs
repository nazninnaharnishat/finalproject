using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MySql.Data.MySqlClient;

namespace HostelManagment
{
    public partial class DailyCostPage : Window
    {
        public List<DailyCost> Costs { get; set; }

        public DailyCostPage()
        {
            InitializeComponent();
            LoadDailyCosts();
        }

        private void LoadDailyCosts()
        {
            Costs = FetchDailyCostsFromDatabase();
            CostDataGrid.ItemsSource = Costs;
        }

        private List<DailyCost> FetchDailyCostsFromDatabase()
        {
            var costs = new List<DailyCost>();
            string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";
            string query = "SELECT id, month, date, couse, amount FROM daily_costs";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            costs.Add(new DailyCost
                            {
                                Id = reader.GetInt32("id"),
                                Month = reader.GetInt32("month"),
                                MonthName = GetMonthName(reader.GetInt32("month")),
                                Date = reader.GetDateTime("date"),
                                Cause = reader.GetString("couse"),
                                Amount = reader.GetDecimal("amount")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data: {ex.Message}");
            }

            return costs;
        }

        private string GetMonthName(int month)
        {
            return new DateTime(1, month, 1).ToString("MMMM");
        }

        private void AddNewCost_Click(object sender, RoutedEventArgs e)
        {
            var addPage = new DailyCostAddPage();
            addPage.ShowDialog();
            LoadDailyCosts(); // Refresh the list after adding
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as FrameworkElement).Tag);
            var editPage = new DailyCostEditPage(id);
            editPage.ShowDialog();
            LoadDailyCosts(); // Refresh the list after editing
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((sender as FrameworkElement).Tag);

            if (MessageBox.Show("Are you sure you want to delete this entry?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DeleteDailyCostFromDatabase(id);
                LoadDailyCosts(); // Refresh the list after deletion
            }
        }

        private void DeleteDailyCostFromDatabase(int id)
        {
            string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";
            string query = "DELETE FROM daily_costs WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Cost deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting data: {ex.Message}");
            }
        }
    }

    public class DailyCost
    {
        public int Id { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public DateTime Date { get; set; }
        public string Cause { get; set; }
        public decimal Amount { get; set; }
    }
}
