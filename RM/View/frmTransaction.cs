using MySql.Data.MySqlClient;
using RM.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.View
{
    public partial class frmTransaction : Form
    {
        private int id;

        public frmTransaction()
        {
            InitializeComponent();
        }

        public int MainID = 0;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTransaction_Load(object sender, EventArgs e)
        {
            dataGridView2.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            ProductPanel.Controls.Clear();
            LoadProducts();
        }

        private void AddCategory()
        {
            string query = "SELECT * FROM category";
            MySqlCommand cmd = new MySqlCommand(query, MainClass.connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            CategoryPanel.Controls.Clear();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(50, 55, 89);
                    b.Size = new Size(189, 50);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row["catName"].ToString();

                    //ketika untuk klik
                    b.Click += new EventHandler(b_Click);

                    CategoryPanel.Controls.Add(b);

                }
            }
        }

        private void b_Click(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;

            List<ucProduct> productsToShow = new List<ucProduct>();

            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;

                if (pro.PCategory.ToLower() == b.Text.Trim().ToLower())
                {
                    productsToShow.Add(pro);
                }
            }

            // Set visibility based on whether the product is in the filtered list
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = productsToShow.Contains(pro);
            }
        }




        private void AddItems(string id, string name, string cat, string price, Image pimage)
        {
            var w = new ucProduct()
            {
                PName = name,  // Set PName to the actual product name from the database
                PPrice = price,
                PCategory = cat,
                PImage = pimage,
                id = Convert.ToInt32(id)
            };

            ProductPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;

                foreach (DataGridViewRow item in dataGridView2.Rows)
                {
                    if (Convert.ToInt32(item.Cells["dgvid"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                                                        double.Parse(item.Cells["dgvPrice"].Value.ToString());

                        return;
                    }
                }
                //TAMBAH PRODUK BARU
                dataGridView2.Rows.Add(new object[] { 0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                GetTotal();
            };
        }

        //MENDAPATKAN PRODUK DARI DATABASE

        private void LoadProducts()
        {
            string query = "Select * from products inner join category on catID = CategoryID";
            MySqlCommand cmd = new MySqlCommand(query, MainClass.connection);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (byte[])item["pImage"];
                byte[] immagebytearray = imagearray;

                AddItems(item["pID"].ToString(), item["pName"].ToString(), item["catName"].ToString(),
                    item["pPrice"].ToString(), Image.FromStream(new MemoryStream(imagearray)));



            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        private void dataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Untuk no serial
            int count = 0;


            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                count++;
                row.Cells[0].Value = count;

            }
        }

        private void GetTotal()
        {
            double tot = 0;
            lblTotal.Text = "";
            foreach (DataGridViewRow item in dataGridView2.Rows)
            {
                tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());
            }

            lblTotal.Text = tot.ToString("N2");
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            // Create an instance of the frmBillList form
            frmBillList frm = new frmBillList();

            // Show the frmBillList form as a modal dialog
            frm.ShowDialog();

            if (frm.MainID > 0)
            {
                LoadEntries();
            }
        }

        private void LoadEntries()
        {
            try
            {
                string query = @"SELECT p.*, c.CategoryName, u.UserName 
                         FROM products p
                         INNER JOIN category c ON p.catID = c.catID
                         INNER JOIN users u ON p.UserID = u.UserID";

                // Use a DataTable to store the results
                DataTable dt = new DataTable();

                using (SqlConnection connection = new SqlConnection("rm"))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        // Use SqlDataAdapter to fill the DataTable
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                // Check if there are rows in the DataTable
                if (dt.Rows.Count > 0)
                {
                    // Assuming you have a DataGridView named 'dataGridView1'
                    dataGridView2.Rows.Clear();

                    foreach (DataRow row in dt.Rows)
                    {
                        // Access the columns by name
                        string productID = row["pID"].ToString();
                        string productName = row["PName"].ToString();
                        string category = row["catName"].ToString();
                        string user = row["userID"].ToString();

                        // Add data to the DataGridView
                        dataGridView2.Rows.Add(productID, productName, category, user);
                    }
                }
                else
                {
                    // Handle the case when there are no entries
                    MessageBox.Show("No entries found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            

            // Assuming you have the lblTotal value available
             string totalAmount = lblTotal.Text;

            // Create an instance of the frmCheckout form
            frmCheckout checkoutForm = new frmCheckout();

            // Set the BillAmount property before showing the form
            checkoutForm.BillAmount = totalAmount;

            // Show the frmCheckout form as a modal dialog
            checkoutForm.ShowDialog();

            // You can perform additional actions after the checkout form is closed, if needed
            // For example, clear the items or update the UI
            dataGridView2.Rows.Clear();
            lblTotal.Text = "00";

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

       
    }

}
