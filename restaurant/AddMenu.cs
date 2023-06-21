using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace restaurant
{
    public partial class AddMenu : Form
    {
        DatabaseConnection dbConnection = new DatabaseConnection();
        public AddMenu()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            string items = item.Text;
            string prices = price.Text;
            string quantitys = quantity.Text;


            dbConnection.OpenConnection();
            string query = $"INSERT INTO menu (item, price, quantity) VALUES ('{items}', '{prices}', '{quantitys}')";
            int rowsAffected = dbConnection.ExecuteNonQuery(query);
            dbConnection.CloseConnection();

            if (rowsAffected > 0)
            {
                MessageBox.Show("New Item Added");
                guna2DataGridView1.Refresh();
                item.Text = "";
                price.Text = "";
                price.Text = "";
            }
            else
            {
                MessageBox.Show("Failed To Add Item");
            }
        }

        private void AddMenu_Load(object sender, EventArgs e)
        {
            dbConnection.OpenConnection();
            string query = "SELECT * FROM menu";
            DataTable dataTable = dbConnection.ExecuteQuery(query);
            dbConnection.CloseConnection();

            guna2DataGridView1.DataSource = dataTable;
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            string items = item.Text;
            dbConnection.OpenConnection();

            string query = "DELETE FROM menu WHERE id = @ItemId";
            SqlCommand command = new SqlCommand(query, dbConnection.connection);
            command.Parameters.AddWithValue("@ItemId", items);

            int rowsAffected = command.ExecuteNonQuery();
            dbConnection.CloseConnection();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Item deleted successfully");
                guna2DataGridView1.Refresh();
                item.Text = "";
                price.Text = "";
                quantity.Text = "";
            }
            else
            {
                MessageBox.Show("Failed to delete item");
            }
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {
            Dashboard f1 = new Dashboard();
            f1.Show();
            this.Hide();
        }
    }
}
