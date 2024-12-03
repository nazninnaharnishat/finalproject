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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;


namespace Final_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void submit_login(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string phone = phoneBox.Text;
            if (AuthenticateAdmin(email, phone))
            {

                MessageBox.Show("Login successful!");
                Dashboard dashboardWindow = new Dashboard();
                dashboardWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid credentials or expired login.");
            }

           
        }
        private bool AuthenticateAdmin(string email, string phone)
        {
            bool isAuthenticated = false;
            string connectionString = "Server=127.0.0.1;Port=3306;Database=hostelmanagement;Uid=root;Pwd=;";
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // MessageBox.Show("Database connection successful!"); // Debugging message

                    string query = @"
                SELECT COUNT(*) 
                FROM members m
                JOIN managerlogs ml ON m.id = ml.manager_id
                WHERE m.email = @Email
                AND m.phone = @Phone
                AND m.is_admin = 1
                AND ml.active_until > NOW()";

                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        isAuthenticated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}"); // Show the exception message
            }

            return isAuthenticated;
        }
    }
}
