using Microsoft.Data.SqlClient;
using practice.Landing_Page;
using System;
using System.Configuration; 
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace practice
{
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();

        }

        // Hashed Password
        private byte[] HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private string Login(string email, string password)
        {
            string role = null;

            string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT role FROM Employee WHERE email = @Email AND passwordHash = @PasswordHash";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;
                    cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, 32).Value = HashPassword(password);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        role = result.ToString();
                    }
                }
            }
            return role;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            string role = Login(email, password); 

            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Invalid email or password.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (role.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Welcome Admin!");

                Mainpage main = new Mainpage();
                main.Show();
                this.Close();
            }
            else if (role.Equals("user", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Welcome User!");

                Mainpage main = new Mainpage();
                main.Show();
                this.Close();
            }      
        }

    }
}
