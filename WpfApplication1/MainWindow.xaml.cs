using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace HostelManagment
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void submit_login(object sender, RoutedEventArgs e)
        {
            string email = text_mail.Text; 
            string password = tex_password.Password; 

            if (AuthenticateAdmin(email, password))
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

        private bool AuthenticateAdmin(string email, string password)
        {
            bool isAuthenticated = false;
            string connectionString = "server=localhost;user id=root;database=hostelmanagement;SslMode=None";
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                    SELECT COUNT(*) 
                    FROM admin m
                    WHERE m.email = @Email
                    AND m.password = @password
                    AND m.is_active = 1";

                    var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        isAuthenticated = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

            return isAuthenticated;
        }
    }
}
