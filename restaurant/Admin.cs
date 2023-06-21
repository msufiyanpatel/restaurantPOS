using Guna.UI2.WinForms;
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
    public partial class Admin : Form
    {
        private DatabaseConnection dataConnection;
        public Admin()
        {
            InitializeComponent();
            dataConnection = new DatabaseConnection();
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void user_TextChanged(object sender, EventArgs e)
        {

        }

        private void pass_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            string username = user.Text;
            string password = pass.Text;

            if (ValidateLogin(username, password))
            {
               

                
                Dashboard form2 = new Dashboard();
                form2.Show();
                this.Hide();
            }
            else
            {
                
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            string query = $"SELECT COUNT(*) FROM admin WHERE Username = '{username}' AND Password = '{password}'";
            dataConnection.OpenConnection();
            int count = (int)dataConnection.ExecuteQuery(query).Rows[0][0];
            dataConnection.CloseConnection();

            return count > 0;
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }
    }
}
