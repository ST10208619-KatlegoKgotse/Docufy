using Nest;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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

namespace Docufy
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();

        }




        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (dgvDocuments.SelectedItems.Count > 0)
            {
                int selectedId = ((Document)dgvDocuments.SelectedItem).Id;
            }
            else
            {
                MessageBox.Show("Please select a document to open.");
            }
        }

        private void SaveFile(string filepath)
        {
            byte[] buffer = File.ReadAllBytes(filepath);
            FileInfo fileInfo = new FileInfo(filepath);
            string extn = fileInfo.Extension;
            string name = fileInfo.Name;

            string query = "INSERT INTO Documents (Id, FileName, Data, Extension) VALUES (@id, @name, @data, @extn)";

            using (SqlConnection cn = GetConnection())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = GetNextId();
                    cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                    cmd.Parameters.Add("@data", SqlDbType.VarBinary).Value = buffer;
                    cmd.Parameters.Add("@extn", SqlDbType.VarChar).Value = extn;
                    cmd.ExecuteNonQuery();
                }

                cn.Close();
            }
        }

        private void OpenFile(int id)
        {
            using (SqlConnection cn = GetConnection())
            {
                string query = "SELECT ID, FileName, Data, Extension FROM Documents WHERE ID = @id";
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = reader["FileName"].ToString();
                            int ID = Convert.ToInt32(reader["ID"].ToString());
                            byte[] data = (byte[])reader["Data"];
                            string extn = reader["Extension"].ToString();
                            string newFileName = $"{name}_{DateTime.Now.ToString("ddMMyyyyhhmmss")}{extn}";
                            File.WriteAllBytes(newFileName, data);
                            System.Diagnostics.Process.Start(newFileName);
                        }
                    }
                }
                cn.Close();
            }
        }

        private SqlConnection GetConnection()
        {
            //CONNECTION STRING 
            return new SqlConnection("YourConnectionStringHere");
        }

        private void LoadData()
        {
            using (SqlConnection cn = GetConnection())
            {
                string query = "SELECT ID, FileName, Data, Extension FROM Documents";
                SqlDataAdapter adp = new SqlDataAdapter(query, cn);
                DataTable dt = new DataTable();
                adp.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dgvDocuments.ItemsSource = dt.DefaultView;
                }
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void dgvDocuments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }