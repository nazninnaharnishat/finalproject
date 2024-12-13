using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace HostelManagment
{
  
    public partial class SummeryAdd : Window
    {
        string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";

        public SummeryAdd()
        {
            InitializeComponent();
            LoadMembers();
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

                    var members = new List<Member>();
                    while (reader.Read())
                    {
                        members.Add(new Member
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }

                    MemberComboBox.ItemsSource = members;
                    MemberComboBox.DisplayMemberPath = "Name";
                    MemberComboBox.SelectedValuePath = "Id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading members: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MemberComboBox.SelectedValue == null || MonthComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a member and a month.");
                    return;
                }

                decimal totalCost = decimal.Parse(TotalCostTextBox.Text);
                decimal fixtCost = decimal.Parse(FixtCostTextBox.Text);
                decimal paidCost = decimal.Parse(PaidCostTextBox.Text);
                decimal dueCost = decimal.Parse(DueCostTextBox.Text);
                decimal totalMil = decimal.Parse(TotalMillTextBox.Text);

                int month = Convert.ToInt32(((ComboBoxItem)MonthComboBox.SelectedItem).Tag);
                int memberId = (int)MemberComboBox.SelectedValue;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO monthly_summery (total_cost, fixed_cost, pay, total_mill, due, month, created_at, member_id) " +
                                   "VALUES (@totalCost, @fixtCost, @paidCost, @total_mill, @dueCost, @month, @createdAt, @memberId)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@totalCost", totalCost);
                    command.Parameters.AddWithValue("@paidCost", paidCost);
                    command.Parameters.AddWithValue("@total_mill", totalMil);
                    command.Parameters.AddWithValue("@fixtCost", fixtCost);
                    command.Parameters.AddWithValue("@dueCost", dueCost);
                    command.Parameters.AddWithValue("@month", month);
                    command.Parameters.AddWithValue("@createdAt", DateTime.Now);
                    command.Parameters.AddWithValue("@memberId", memberId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Summary added successfully!");
                        this.Close();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add summary.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding summary: " + ex.Message);
            }
        }


        private void ClearForm()
        {
            TotalCostTextBox.Clear();
            PaidCostTextBox.Clear();
            DueCostTextBox.Clear();
            MemberComboBox.SelectedIndex = -1;
            MonthComboBox.SelectedIndex = -1;
        }
    }

    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
