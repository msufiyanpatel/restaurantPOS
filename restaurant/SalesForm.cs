using FastReport;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FastReport.DevComponents.DotNetBar.Controls;

namespace restaurant
{
    public partial class SalesForm : Form
    {
        private string XMLfileName = "OrderDetails.xml";

        public SalesForm()
        {
            InitializeComponent();
            
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(XMLfileName))
            {
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(XMLfileName);

                guna2.DataSource = dataSet.Tables[0];

            }
            else
            {
                MessageBox.Show("No XML data file found.");
            }
        }

        private void SalesForm_Load(object sender, EventArgs e)
        {

            LoadReport();
        }

        
        private void LoadReport()
        {
            
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {
            Dashboard f1 = new Dashboard();
            f1.Show();
            this.Hide();
        }
    }
    }

