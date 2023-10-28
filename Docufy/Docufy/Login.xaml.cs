using Microsoft.Data.SqlClient;
using System;
using System.Windows;

namespace Docufy
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\lab_services_student\\Documents\\Docufy.mdf;Integrated Security=True;Connect Timeout=30";
        public Login()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                Wpf.Ui.Appearance.Watcher.Watch(this,
                    Wpf.Ui.Appearance.BackgroundType.Mica,
                    true);
            };
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = edtEmailAddress.Text;
            string password = edtPassword.Text;
            bool auth = SearchExistingUser(connectionString, email, password);
            if (auth)
                MessageBox.Show("Successful");
            else
                MessageBox.Show("Please try again");
        }
        public bool SearchExistingUser(string conn, string email, string password)
        {
            string query = "SELECT COUNT(*) FROM [USER] WHERE EMAIL_ADDRESS = @Email AND PASSWORD = @Password";
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                try
                {

                    sqlConnection.Open();
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    int result = (int)command.ExecuteScalar();
                    if (result == 1)
                    {
                        MainWindow mw = new MainWindow();
                        mw.Show();
                        return (true);
                    }
                    else
                    {
                        MessageBox.Show($"Please retry the information");
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
