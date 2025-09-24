using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace practice.Pages
{
    public partial class EmpManage : Page
    {
        private List<Employee> _allEmployees;

        public EmpManage()
        {
            InitializeComponent();
            _allEmployees = new List<Employee>(); 
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            _allEmployees.Clear();

            string connectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT e.employeeID,
                           e.firstName,
                           e.lastName,
                           e.middleName,
                           p.position      
                    FROM Employee e
                    INNER JOIN EmployeePosition p ON e.positionID = p.positionID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string fullName = $"{reader["lastName"]}, {reader["firstName"]} {reader["middleName"]}";

                        _allEmployees.Add(new Employee
                        {
                            EmployeeNo = reader["employeeID"].ToString(),
                            Name = fullName,
                            Position = reader["position"].ToString()  
                        });
                    }
                }
            }

            employeeDataGrid.ItemsSource = null;
            employeeDataGrid.ItemsSource = _allEmployees;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }

    public class Employee
    {
        public string EmployeeNo { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
    }
}
