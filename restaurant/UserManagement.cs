using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace restaurant
{
    public partial class UserManagement : Form
    {
        DatabaseConnection dbConnection = new DatabaseConnection();
        public UserManagement()
        {
            InitializeComponent();
        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            
                dbConnection.OpenConnection();
                string query = "SELECT * FROM [user]";
                DataTable dataTable = dbConnection.ExecuteQuery(query);
                dbConnection.CloseConnection();

                guna2DataGridView1.DataSource = dataTable;
            
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            string username = user.Text;
            string password = pass.Text;

            
                dbConnection.OpenConnection();
                string query = $"INSERT INTO [user] (Username, Password) VALUES ('{username}', '{password}')";
                int rowsAffected = dbConnection.ExecuteNonQuery(query);
                dbConnection.CloseConnection();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("New User Added");
                    guna2DataGridView1.Refresh(); 
                    user.Text= "";
                    pass.Text = "";
                }
                else
                {
                    MessageBox.Show("Failed To Add User");
                }
            
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            guna2DataGridView1.Refresh();
            guna2DataGridView1.Update();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {
            Dashboard f1 = new Dashboard();
            f1.Show();
            this.Hide();
        }
    }
}
