using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.Model
{
    public partial class frmProductAdd : SampleAdd
    {
        public frmProductAdd()
        {
            InitializeComponent();
        }

        public int id = 0;
        public int cID = 0;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmProductAdd_Load(object sender, EventArgs e)
        {
            // Untuk cb fill
            string query = "SELECT catID 'id' , catName 'name' FROM category";

            MainClass.CBFill(query, cbCat);

            if (cID >0) // untuk update
            {
                cbCat.SelectedValue = cID;
            }

            if (id > 0)
            {
                ForUpdateLoadData();
            }
        }

        string filePath;
        Byte[] imageByteArray;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg, .png)|* .png; *.jpg";
            if (ofd.ShowDialog()== DialogResult.OK )
            {
                filePath = ofd.FileName;
                txtImage.Image = new Bitmap(filePath);
            }
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "";

                if (id == 0) // insert
                {
                    query = "INSERT INTO products (pName, pPrice, CategoryID, pImage) Values(@Name, @price, @cat, @img)";
                }
                else //update
                {
                    query = "UPDATE products SET pName = @Name, pPrice = @price, CategoryID = @cat, pImage = @img where pID = @id";
                }


                //Untuk image
                Image temp = new Bitmap(txtImage.Image);
                MemoryStream ms = new MemoryStream();
                temp.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
                imageByteArray = ms.ToArray();

                Hashtable ht = new Hashtable();
                ht.Add("@id", id);
                ht.Add("@Name", txtName.Text);
                ht.Add("@price", txtPrice.Text);
                ht.Add("@cat", Convert.ToInt32(cbCat.SelectedValue));
                ht.Add("@img", imageByteArray);

                if (MainClass.SQL(query, ht) > 0)
                {
                    MessageBox.Show("Data Berhasil disimpan");
                    id = 0;
                    cID = 0;
                    txtName.Text = "";
                    txtPrice.Text = "";
                    cbCat.SelectedIndex = 0;
                    cbCat.SelectedIndex = -1;
                    txtImage.Image = RM.Properties.Resources.productPic;
                    txtName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ForUpdateLoadData()
        {
            string query = "SELECT * FROM products where pid = "+id +"";
            MySqlCommand cmd = new MySqlCommand(query, MainClass.connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                txtName.Text = dt.Rows[0]["pName"].ToString();
                txtPrice.Text = dt.Rows[0]["pPrice"].ToString();

                Byte[] imageArray = (byte[])(dt.Rows[0]["pImage"]);
                byte[] imageByte = imageArray;
                txtImage.Image = Image.FromStream(new MemoryStream(imageArray));
            }
        }
    }
}
