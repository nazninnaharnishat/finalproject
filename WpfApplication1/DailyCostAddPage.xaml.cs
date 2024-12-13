using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace HostelManagment
{
    public partial class DailyCostAddPage : Window
    {
        public DailyCostAddPage()
        {
            InitializeComponent();
        }

        // Handle Add New Cost Button Click
        private void AddCostButton_Click(object sender, RoutedEventArgs e)
        {
            // Get data from the form
            int month = int.Parse(((ComboBoxItem)MonthComboBox.SelectedItem).Tag.ToString());
            DateTime date = DatePicker.SelectedDate.Value;
            string cause = CauseTextBox.Text;
            decimal amount = decimal.Parse(AmountTextBox.Text);

            // Insert into database
            InsertDailyCost(month, date, cause, amount);
        }

        private void InsertDailyCost(int month, DateTime date, string cause, decimal amount)
        {
            string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";
            string query = "INSERT INTO daily_costs (month, date, couse, amount) VALUES (@month, @date, @cause, @amount)";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@month", month);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@cause", cause);
                    command.Parameters.AddWithValue("@amount", amount);

                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("New cost added successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }
}
