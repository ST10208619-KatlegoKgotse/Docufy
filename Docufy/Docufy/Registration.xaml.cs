using Microsoft.Data.SqlClient;
using System;
using System.Windows;

namespace Docufy
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\lab_services_student\\Documents\\Docufy.mdf;Integrated Security=True;Connect Timeout=30";

        public Registration()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                Wpf.Ui.Appearance.Watcher.Watch(this,
                    Wpf.Ui.Appearance.BackgroundType.Mica,
                    true);
            };
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Hide();
        }
        public bool SearchExistingUser(string conn, string email, string password)
        {
            string query = "SELECT COUNT(*) FROM [USER] WHERE EMAIL_ADDRESS = @Email";
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                try
                {

                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.Parameters.AddWithValue("@Email", email);
                    int result = (int)command.ExecuteScalar();
                    if (result == 1)
                    {
                        MessageBox.Show($"This user exists");
                        return (true);
                    }
                    else
                    {
                        MessageBox.Show($"Please Login");
                        return (false);
                    }
                }
                catch (SqlException s)
                {
                    MessageBox.Show($"{s}");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Message: {e}");
                }
                return (false);
            }

        }
    }
}
