using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace HostelManagment
{
    public partial class DailyCostEditPage : Window
    {
        private int CostId;

        public DailyCostEditPage(int id)
        {
            InitializeComponent();
            CostId = id;
            LoadCostDetails();
        }

        private void LoadCostDetails()
        {
            string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";
            string query = "SELECT month, date, couse, amount FROM daily_costs WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", CostId);

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MonthComboBox.SelectedIndex = reader.GetInt32("month") - 1;
                            DatePicker.SelectedDate = reader.GetDateTime("date");
                            CauseTextBox.Text = reader.GetString("couse");
                            AmountTextBox.Text = reader.GetDecimal("amount").ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int month = int.Parse(((ComboBoxItem)MonthComboBox.SelectedItem).Tag.ToString());
            DateTime date = DatePicker.SelectedDate.Value;
            string couse = CauseTextBox.Text;
            decimal amount = decimal.Parse(AmountTextBox.Text);

            UpdateDailyCost(CostId, month, date, couse, amount);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateDailyCost(int id, int month, DateTime date, string couse, decimal amount)
        {
            string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";
            string query = "UPDATE daily_costs SET month = @month, date = @date, couse = @couse, amount = @amount WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@month", month);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@couse", couse);
                    command.Parameters.AddWithValue("@amount", amount);

                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Cost updated successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating data: {ex.Message}");
            }
        }
    }
}
