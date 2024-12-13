using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace HostelManagment
{
    public partial class EditSummery : Window
    {
        private readonly int _summeryId;
        private string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";

        public EditSummery(int summeryId)
        {
            InitializeComponent();
            _summeryId = summeryId;
            LoadMembers();
            LoadSummeryData();
        }

        private void LoadMembers()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, Name FROM Members";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    var members = new List<HostelMember>();

                    // Add the default "Select Member" option
                    members.Add(new HostelMember { MemberId = 0, MemberName = "Select Member" });

                    // Fetch members from the database
                    while (reader.Read())
                    {
                        members.Add(new HostelMember
                        {
                            MemberId = reader.GetInt32(0),  // MemberId from the database
                            MemberName = reader.GetString(1)  // Name from the database
                        });
                    }

                    // Bind the members list to the ComboBox
                    MemberComboBox.ItemsSource = members;
                    MemberComboBox.DisplayMemberPath = "MemberName";  // Show MemberName in the ComboBox
                    MemberComboBox.SelectedValuePath = "MemberId";  // Use MemberId as the value
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading members: " + ex.Message);
            }
        }

        private void LoadSummeryData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT total_cost, fixed_cost, pay, total_mill, due, month, member_id FROM monthly_summery WHERE id = @id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", _summeryId);

                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        TotalCostTextBox.Text = reader.GetDecimal("total_cost").ToString();
                        FixtCostTextBox.Text = reader.GetDecimal("fixed_cost").ToString();
                        PaidCostTextBox.Text = reader.GetDecimal("pay").ToString();
                        DueCostTextBox.Text = reader.GetDecimal("due").ToString();
                        TotalMillTextBox.Text = reader.GetDecimal("total_mill").ToString();
                        MonthComboBox.SelectedIndex = reader.GetInt32("month") - 1;

                        // Handle null or 0 selection for member_id
                        int? memberId = reader.IsDBNull(reader.GetOrdinal("member_id")) ? (int?)null : reader.GetInt32("member_id");
                        MemberComboBox.SelectedValue = memberId;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading summary data: " + ex.Message);
            }
        }


        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Parse input values
                decimal totalCost = decimal.Parse(TotalCostTextBox.Text);
                decimal fixtCost = decimal.Parse(FixtCostTextBox.Text);
                decimal paidCost = decimal.Parse(PaidCostTextBox.Text);
                decimal dueCost = decimal.Parse(DueCostTextBox.Text);
                decimal totalMil = decimal.Parse(TotalMillTextBox.Text);

                // Get selected month and memberId
                int month = Convert.ToInt32(((ComboBoxItem)MonthComboBox.SelectedItem).Tag);
                int memberId = (int)MemberComboBox.SelectedValue;  // Ensure memberId is a valid integer

                // Update query
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE monthly_summery SET total_cost=@totalCost, fixed_cost=@fixtCost, pay=@paidCost, total_mill=@totalMil, due=@dueCost, month=@month, created_at=@createdAt, member_id=@memberId WHERE id=@id";

                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Adding parameters
                    command.Parameters.AddWithValue("@totalCost", totalCost);
                    command.Parameters.AddWithValue("@fixtCost", fixtCost);
                    command.Parameters.AddWithValue("@paidCost", paidCost);
                    command.Parameters.AddWithValue("@totalMil", totalMil);
                    command.Parameters.AddWithValue("@dueCost", dueCost);
                    command.Parameters.AddWithValue("@month", month);
                    command.Parameters.AddWithValue("@createdAt", DateTime.Now);  // or use specific date if required
                    command.Parameters.AddWithValue("@memberId", memberId);
                    command.Parameters.AddWithValue("@id", _summeryId);  // _summeryId should be set properly (i.e., the record's ID)

                    // Execute query
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Summary updated successfully!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update summary.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating summary: " + ex.Message);
            }
        }


    }

    public class HostelMember
    {
        public int MemberId { get; set; }  // Changed 'Id' to 'MemberId' to avoid ambiguity
        public string MemberName { get; set; }  // Renamed 'Name' to 'MemberName' to avoid ambiguity
    }
}
