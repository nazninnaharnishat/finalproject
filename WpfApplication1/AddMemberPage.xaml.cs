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
using MySql.Data.MySqlClient; 

namespace HostelManagment
{
    /// <summary>
    /// Interaction logic for AddMemberPage.xaml
    /// </summary>
    public partial class AddMemberPage : Window
    {
        public AddMemberPage()
        {
            InitializeComponent();
        }
        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            string name = text_name.Text;
            string email = text_email.Text;
            string phone = text_phone.Text;
            string address = text_address.Text;

            // Correct MySQL connection string
            string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";

            // Query to insert a member
            string query = "INSERT INTO members (name, email, phone, address) VALUES (@name, @Email, @phone, @address)";

            try
            {
                using (var connection = new MySqlConnection(connectionString)) // Use MySqlConnection, not SqlConnection
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@address", address);

                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Member added successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }
}
