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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void guna2GradientCircleButton1_Click(object sender, EventArgs e)
        {
            UserManagement um = new UserManagement();
            um.Show();
            this.Hide();
        }

        private void guna2GradientCircleButton3_Click(object sender, EventArgs e)
        {
            SalesForm sf = new SalesForm();
            sf.Show();
            this.Hide();
        }

        private void guna2GradientCircleButton2_Click(object sender, EventArgs e)
        {
            AddMenu am = new AddMenu();
            am.Show();
            this.Hide();
        }

        private void guna2GradientCircleButton4_Click(object sender, EventArgs e)
        {
            Order od = new Order();
            od.Show();
            this.Hide();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }
    }
}
