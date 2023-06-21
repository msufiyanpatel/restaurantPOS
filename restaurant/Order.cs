using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace restaurant
{
    public partial class Order : Form
    {
        private DatabaseConnection dbConnection = new DatabaseConnection();
        private DataTable menuDataTable;
        private DataTable editedItemsDataTable;
        public static int TotalPrice= 0;
        public static int TotalOrderCount = 1;

        public Order()
        {
            InitializeComponent();
            LoadOrderDetailsFromXml();
        }

        private void Order_Load(object sender, EventArgs e)
        {
            LoadMenuItems();

            
            InitializeEditedItemsDataTable();
        }

        private void LoadMenuItems()
        {
            dbConnection.OpenConnection();

            string query = "SELECT ID, item, Price FROM Menu";
            menuDataTable = dbConnection.ExecuteQuery(query);

            dbConnection.CloseConnection();

            dataGridViewMenu.DataSource = menuDataTable;
        }

        private void InitializeEditedItemsDataTable()
        {
            editedItemsDataTable = new DataTable();
            editedItemsDataTable.Columns.Add("ID", typeof(int));
            editedItemsDataTable.Columns.Add("item", typeof(string));
            editedItemsDataTable.Columns.Add("Price", typeof(int));

            dataGridViewEditedItems.DataSource = editedItemsDataTable;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridViewMenu.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridViewMenu.Rows[selectedRowIndex];

            string itemId = selectedRow.Cells["ID"].Value.ToString();
            string itemName = selectedRow.Cells["item"].Value.ToString();
            string itemPrice = selectedRow.Cells["Price"].Value.ToString();

            if (dataGridViewOrder.Columns.Count == 0)
            {
                dataGridViewOrder.Columns.Add("ID", "ID");
                dataGridViewOrder.Columns.Add("Name", "Name");
                dataGridViewOrder.Columns.Add("Price", "Price");
            }

            dataGridViewOrder.Rows.Add(itemId, itemName, itemPrice);

            txtEditedItemId.Text = "";
            txtEditedItemName.Text = "";
            txtEditedItemPrice.Text = "";
            dataGridViewMenu.ClearSelection();

            
        }

        private void btnEditItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrder.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewOrder.SelectedRows[0];

                // Get the selected item's details
                string itemId = selectedRow.Cells["ID"].Value.ToString();
                string itemName = selectedRow.Cells["Name"].Value.ToString();
                string itemPrice = selectedRow.Cells["Price"].Value.ToString();
                TotalPrice = TotalPrice + Convert.ToInt32(itemPrice);
                // Show the item details in the edit textboxes
                txtEditedItemId.Text = itemId;
                txtEditedItemName.Text = itemName;
                txtEditedItemPrice.Text = itemPrice;
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEditedItemId.Text) && !string.IsNullOrEmpty(txtEditedItemName.Text) && !string.IsNullOrEmpty(txtEditedItemPrice.Text))
            {
                int editedItemId = int.Parse(txtEditedItemId.Text);
                string editedItemName = txtEditedItemName.Text;
                int editedItemPrice = int.Parse(txtEditedItemPrice.Text);

                // Update the item in the order DataGridView
                foreach (DataGridViewRow row in dataGridViewOrder.Rows)
                {
                    int itemId = int.Parse(row.Cells["ID"].Value.ToString());

                    if (itemId == editedItemId)
                    {
                        row.Cells["Name"].Value = editedItemName;
                        row.Cells["Price"].Value = editedItemPrice;
                        break;
                    }
                }

                // Update the item in the edited items DataGridView
                DataRow editedItemRow = editedItemsDataTable.Select($"ID = {editedItemId}").FirstOrDefault();

                if (editedItemRow != null)
                {
                    editedItemRow["Name"] = editedItemName;
                    editedItemRow["Price"] = editedItemPrice;
                }
                else
                {
                    editedItemsDataTable.Rows.Add(editedItemId, editedItemName, editedItemPrice);
                }

                // Clear the edit textboxes
                txtEditedItemId.Text = string.Empty;
                txtEditedItemName.Text = string.Empty;
                txtEditedItemPrice.Text = string.Empty;

                // Recalculate the total price
                //CalculateTotalPrice();
            }

        }
        public class OrderDetails
        {
            public int OrderNumber { get; set; }
            public string TotalAmount { get; set; }
            public DateTime OrderDateTime { get; set; }
        }
        private static List<OrderDetails> orderDetailsList = new List<OrderDetails>();
        string XMLfileName = "OrderDetails.xml";
        private void btnDownload_Click(object sender, EventArgs e)
        {
            


            if (dataGridViewOrder.Rows.Count > 0)
            {
                
                Document document = new Document();
                string fileName = "Order" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));

                document.Open();

                
                PdfPTable table = new PdfPTable(3);

                
                table.AddCell("ID");
                table.AddCell("Item");
                table.AddCell("Price");

                
               

                foreach (DataGridViewRow row in dataGridViewOrder.Rows)
                {
                    if (row.Cells["ID"].Value != null && row.Cells["Name"].Value != null && row.Cells["Price"].Value != null)
                    {
                        string itemId = row.Cells["ID"].Value.ToString();
                        string itemName = row.Cells["Name"].Value.ToString();
                        string itemPrice = row.Cells["Price"].Value.ToString();
                        TotalPrice = TotalPrice + Convert.ToInt32(itemPrice);
                        table.AddCell(itemId);
                        table.AddCell(itemName);
                        table.AddCell(itemPrice);
                    }
                    

                }
                lblTotalPrice.Text = TotalPrice.ToString();
                OrderDetails orderDetails = new OrderDetails
                {
                    OrderNumber = TotalOrderCount,
                    TotalAmount = lblTotalPrice.Text,
                    OrderDateTime = DateTime.Now
                };
                orderDetailsList.Add(orderDetails);


                SaveOrderDetailsToXml();


                TotalOrderCount++;
                document.Add(new Paragraph($"Bahria Restaurant"));
                document.Add(new Paragraph($"Order Online: 032420397555"));
                document.Add(new Paragraph($"Hussainabad FB AREA Karachi"));
                document.Add(table);

           
                document.Add(new Paragraph($"Total Price: {lblTotalPrice.Text}"));

                
                document.Close();

                MessageBox.Show("Order downloaded as PDF");

                
                dataGridViewOrder.Rows.Clear();
                lblTotalPrice.Text = "";
                TotalPrice= 0;
            }
            else
            {
                MessageBox.Show("No items in the order to download.");
            }
        }
        private void SaveOrderDetailsToXml()
        {
            string XMLfileName = "OrderDetails.xml";

            XDocument xmlDoc;
            if (File.Exists(XMLfileName))
            {
                xmlDoc = XDocument.Load(XMLfileName);
            }
            else
            {
                xmlDoc = new XDocument(new XElement("OrderDetailsList"));
            }

            foreach (OrderDetails orderDetails in orderDetailsList)
            {
                XElement orderElement = new XElement("Order",
                    new XAttribute("OrderNumber", orderDetails.OrderNumber),
                    new XAttribute("TotalAmount", orderDetails.TotalAmount),
                    new XAttribute("OrderDateTime", orderDetails.OrderDateTime));

                xmlDoc.Element("OrderDetailsList").Add(orderElement);
            }

            // Save the changes
            xmlDoc.Save(XMLfileName);
        }
        private void LoadOrderDetailsFromXml()
        {
            string XMLfileName = "OrderDetails.xml";

            if (File.Exists(XMLfileName))
            {
                XDocument xmlDoc = XDocument.Load(XMLfileName);

                orderDetailsList.Clear();

                foreach (XElement orderElement in xmlDoc.Element("OrderDetailsList").Elements("Order"))
                {
                    int orderNumber = int.Parse(orderElement.Attribute("OrderNumber").Value);
                    string totalAmount = orderElement.Attribute("TotalAmount").Value;
                    DateTime orderDateTime = DateTime.Parse(orderElement.Attribute("OrderDateTime").Value);

                    OrderDetails orderDetails = new OrderDetails
                    {
                        OrderNumber = orderNumber,
                        TotalAmount = totalAmount,
                        OrderDateTime = orderDateTime
                    };

                    orderDetailsList.Add(orderDetails);
                }
            }
        }

        private void dataGridViewOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewMenu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)  
            {
                DataGridViewRow selectedRow = dataGridViewMenu.Rows[e.RowIndex];

                if (selectedRow.Cells.Count > 0)
                {
                    DataGridViewCell idCell = selectedRow.Cells["ID"];
                    DataGridViewCell nameCell = selectedRow.Cells["item"];
                    DataGridViewCell priceCell = selectedRow.Cells["Price"];

                    if (idCell.Value != null && nameCell.Value != null && priceCell.Value != null)
                    {
                        string itemId = idCell.Value.ToString();
                        string itemName = nameCell.Value.ToString();
                        string itemPrice = priceCell.Value.ToString();

                        if (dataGridViewOrder.Columns.Count == 0)
                        {
                            dataGridViewOrder.Columns.Add("ID", "ID");
                            dataGridViewOrder.Columns.Add("Name", "Name");
                            dataGridViewOrder.Columns.Add("Price", "Price");
                        }

                        
                        dataGridViewOrder.Rows.Add(itemId, itemName, itemPrice);

                    }
                }
            }
        }

        private void dataGridViewEditedItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)  
            {
                DataGridViewRow selectedRow = dataGridViewMenu.Rows[e.RowIndex];

         
                if (selectedRow.Cells.Count > 0)
                {
                    DataGridViewCell idCell = selectedRow.Cells["ID"];
                    DataGridViewCell nameCell = selectedRow.Cells["item"];
                    DataGridViewCell priceCell = selectedRow.Cells["Price"];

                    if (idCell.Value != null && nameCell.Value != null && priceCell.Value != null)
                    {
                        string itemId = idCell.Value.ToString();
                        string itemName = nameCell.Value.ToString();
                        string itemPrice = priceCell.Value.ToString();

                        if (dataGridViewOrder.Columns.Count == 0)
                        {
                            dataGridViewOrder.Columns.Add("ID", "ID");
                            dataGridViewOrder.Columns.Add("Name", "Name");
                            dataGridViewOrder.Columns.Add("Price", "Price");
                        }

                        
                        dataGridViewOrder.Rows.Add(itemId, itemName, itemPrice);

                        
                    }
                }
            }

        }
    }
}

