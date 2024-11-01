using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.Model
{
    public partial class frmCategoryAdd : SampleAdd
    {
        public frmCategoryAdd()
        {
            InitializeComponent();
        }

        public int id = 0;
        public override void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "";

                if (id == 0) // insert
                {
                    query = "INSERT INTO category (catName) Values(@Name)";
                }
                else //update
                {
                    query = "UPDATE category SET catName = @Name where catID = @id";
                }

                Hashtable ht = new Hashtable();
                ht.Add("@id", id);
                ht.Add("@Name", txtName.Text);

                if (MainClass.SQL(query, ht) > 0)
                {
                    MessageBox.Show("Data Berhasil disimpan");
                    id = 0;
                    txtName.Text = "";
                    txtName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void frmCategoryAdd_Load(object sender, EventArgs e)
        {

        }

        public event EventHandler DataChanged;
        private void CloseForm()
        {
            // ... (kode lainnya)
            DataChanged?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {

        }
    }
}
