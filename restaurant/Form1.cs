using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna;


namespace restaurant
{
    public partial class Form1 : Form
    {
        private DatabaseConnection dataConnection;
        public Form1()
        {
            InitializeComponent();
            dataConnection = new DatabaseConnection();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            string username = user.Text;
            string password = pass.Text;

            if (ValidateLogin(username, password))
            {
                guna2HtmlLabel1.Text = "Login Successful...";

                
                Order od = new Order();
                od.Show();
                this.Hide();
            }
            else
            {
                guna2HtmlLabel1.Text = "Invalid username or password";
            }
        }

        private bool ValidateLogin(string username, string password)
        {
            string query = $"SELECT COUNT(*) FROM [user] WHERE Username = '{username}' AND Password = '{password}'";
            dataConnection.OpenConnection();
            int count = (int)dataConnection.ExecuteQuery(query).Rows[0][0];
            dataConnection.CloseConnection();

            return count > 0;
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            Admin ad = new Admin();
            ad.Show();
            this.Hide();
        }
    }
}
