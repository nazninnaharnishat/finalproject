using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;

namespace HostelManagement
{
    public partial class MemberPage : Window
    {
        // Connection string

        public MemberPage()
        {
            InitializeComponent();
            LoadMembers();
        }
        private string connectionString = "Server=localhost;Database=hostelmanagement;User ID=root;Password=;SslMode=None;";

        private void LoadMembers()
        {
            // Clear any existing data
            membersDataGrid.ItemsSource = null;

            List<Member> members = new List<Member>();
            string query = "SELECT id, name, email, phone, address FROM members";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Read data from the database
                    while (reader.Read())
                    {
                        members.Add(new Member
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Email = reader.GetString("email"),
                            Phone = reader.GetString("phone"),
                            Address = reader.GetString("address")
                        });
                    }
                }

                // Bind data to the DataGrid
                membersDataGrid.ItemsSource = members;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading members: {ex.Message}");
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item directly from the DataGrid
            var selectedMember = membersDataGrid.SelectedItem as Member;

            if (selectedMember != null)
            {
                EditMemberWindow editWindow = new EditMemberWindow(selectedMember);
                editWindow.ShowDialog(); // Use ShowDialog to open it as a modal window
                LoadMembers();  // Reload the members to reflect any changes
            }
            else
            {
                MessageBox.Show("Please select a member to edit.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item directly from the DataGrid
            var selectedMember = membersDataGrid.SelectedItem as Member;

            if (selectedMember != null)
            {
                var confirm = MessageBox.Show($"Are you sure you want to delete {selectedMember.Name}?", "Confirm Delete", MessageBoxButton.YesNo);

                if (confirm == MessageBoxResult.Yes)
                {
                    DeleteMember(selectedMember);
                    LoadMembers(); // Refresh data after deletion
                }
            }
            else
            {
                MessageBox.Show("Please select a member to delete.");
            }
        }


       

        private void DeleteMember(Member member)
        {
            string query = "DELETE FROM members WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", member.Id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Member deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while deleting member: {ex.Message}");
            }
        }
    }

    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
