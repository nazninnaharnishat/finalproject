using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace HostelManagement
{
    public partial class EditMemberWindow : Window
    {
        private Member selectedMember;

        public EditMemberWindow(Member member)
        {
            InitializeComponent();
            selectedMember = member;

            // Populate the fields with selectedMember data
            NameTextBox.Text = selectedMember.Name;
            EmailTextBox.Text = selectedMember.Email;
            PhoneTextBox.Text = selectedMember.Phone;
            AddressTextBox.Text = selectedMember.Address;
        }

        // Ensure this event handler exists in the code-behind
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            // Update member object with the data entered in the textboxes
            selectedMember.Name = NameTextBox.Text;
            selectedMember.Email = EmailTextBox.Text;
            selectedMember.Phone = PhoneTextBox.Text;
            selectedMember.Address = AddressTextBox.Text;

            // Call method to update the member in the database
            UpdateMemberInDatabase(selectedMember);

            this.Close();  // Close the edit window after saving changes
        }

        private void UpdateMemberInDatabase(Member member)
        {
            string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";
            string query = "UPDATE members SET name = @name, email = @email, phone = @phone, address = @address WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", member.Name);
                    command.Parameters.AddWithValue("@email", member.Email);
                    command.Parameters.AddWithValue("@phone", member.Phone);
                    command.Parameters.AddWithValue("@address", member.Address);
                    command.Parameters.AddWithValue("@id", member.Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Member updated successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
