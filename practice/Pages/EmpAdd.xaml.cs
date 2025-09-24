using System;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace practice.Pages
{
    public partial class EmpAdd : Page
    {
        private readonly string connectionString;

        public EmpAdd()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

            LoadPositions(); 
        }

        private byte[] HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }


        private void ClearForm()
        {
            firstnameField.Clear();
            middlenameField.Clear();
            lastnameField.Clear();
            contactField.Clear();
            cityprovinceField.Clear();
            brgyField.Clear();
            streetField.Clear();
            emailField.Clear();
            passwordField.Clear();

            statusComboBox.SelectedItem = null;
            sexComboBox.SelectedItem = null;
            positionComboBox.SelectedItem = null;
        }

        private void LoadPositions()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT positionID, position FROM EmployeePosition";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            positionComboBox.Items.Clear();
                            while (reader.Read())
                            {
                                ComboBoxItem item = new ComboBoxItem
                                {
                                    Content = reader["position"].ToString(),
                                    Tag = reader["positionID"] 
                                };
                                positionComboBox.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading positions: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addEmployeeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"INSERT INTO Employee 
                        (firstName, middleName, lastName, contact, status, sex, cityProvince, barangay, street, email, passwordHash, role, positionID)
                        VALUES (@FirstName, @MiddleName, @LastName, @ContactNo, @Status, @Sex, @CityProvince, @Barangay, @Street, @Email, @PasswordHash, @Role, @PositionID)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = firstnameField.Text.Trim();
                        cmd.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = middlenameField.Text.Trim();
                        cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = lastnameField.Text.Trim();
                        cmd.Parameters.Add("@ContactNo", SqlDbType.NVarChar).Value = contactField.Text.Trim();

                        var statusItem = statusComboBox.SelectedItem as ComboBoxItem;
                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = statusItem?.Content?.ToString() ?? "";

                        var sexItem = sexComboBox.SelectedItem as ComboBoxItem;
                        cmd.Parameters.Add("@Sex", SqlDbType.NVarChar).Value = sexItem?.Content?.ToString() ?? "";

                        cmd.Parameters.Add("@CityProvince", SqlDbType.NVarChar).Value = cityprovinceField.Text.Trim();
                        cmd.Parameters.Add("@Barangay", SqlDbType.NVarChar).Value = brgyField.Text.Trim();
                        cmd.Parameters.Add("@Street", SqlDbType.NVarChar).Value = streetField.Text.Trim();
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = emailField.Text.Trim();

                        // passwordHashing
                        byte[] passwordHash = HashPassword(passwordField.Password.Trim());
                        cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, 32).Value = passwordHash;

                        cmd.Parameters.Add("@Role", SqlDbType.NVarChar).Value = "user";

                        // Get selected PositionID
                        var positionItem = positionComboBox.SelectedItem as ComboBoxItem;
                        int positionID = positionItem != null ? Convert.ToInt32(positionItem.Tag) : 0;
                        cmd.Parameters.Add("@PositionID", SqlDbType.Int).Value = positionID;

                        int rows = cmd.ExecuteNonQuery();
                        this.ClearForm();

                        if (rows > 0)
                        {
                            MessageBox.Show("Employee added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to add employee.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while adding employee: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
