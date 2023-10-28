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
        public bool SearchExistingUser(string conn, string email)
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
                        return (true);
                    }
                    else
                    { 
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

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string firstName = edtName.Text;
            string lastName = edtLastName.Text;
            string emailAdd = edtEmailAddress.Text;
            string passwordAdd = edtPassword.Text;
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\lab_services_student\\Documents\\Docufy.mdf;Integrated Security=True;Connect Timeout=30";
            bool userExist = SearchExistingUser(connectionString, emailAdd);
            if (!userExist)
            {
                CreateUser(connectionString, firstName, lastName, emailAdd, passwordAdd);
                MainWindow mw = new MainWindow();
                mw.Show();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void CreateUser(string conn, string firstName, string lastName, string emailAdd, string passwordAdd)
        {
            int id = 1;
            string query = "INSERT INTO [USER] VALUES (@Id, @FN, @LN, @EA, @PASS)";
            using (SqlConnection connection = new SqlConnection(conn))
            {

                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@FN", firstName);
                command.Parameters.AddWithValue("@LN", lastName);
                command.Parameters.AddWithValue("@EA", emailAdd);
                command.Parameters.AddWithValue("@PASS", passwordAdd);
                command.ExecuteNonQuery();

            }
        }
    }
}
