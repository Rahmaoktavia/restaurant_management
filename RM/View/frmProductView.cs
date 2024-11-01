using RM.Model;
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

namespace RM.View
{
    public partial class frmProductView : SampleView
    {
        public frmProductView()
        {
            InitializeComponent();
        }

        private void frmProductView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        public void GetData()
        {
            string query = "SELECT pID, pName, pPrice, CategoryID, c.catName FROM products p inner join category c on c.catID = p.CategoryID where pName like '%" + txtSearch.Text + "%' ";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvPrice);
            lb.Items.Add(dgvcatID);
            lb.Items.Add(dgvcat);
            

            MainClass.LoadData(query, dataGridView2, lb);
        }


        public override void btnAdd_Click(object sender, EventArgs e)
        {
            frmProductAdd frm = new frmProductAdd();
            frm.ShowDialog();
            GetData(); // Tambahkan baris ini untuk menyegarkan data setelah menutup form tambahan
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }


        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                frmProductAdd frm = new frmProductAdd();
                frm.id = Convert.ToInt32(dataGridView2.CurrentRow.Cells["dgvid"].Value);
                frm.cID = Convert.ToInt32(dataGridView2.CurrentRow.Cells["dgvcatID"].Value);
                frm.ShowDialog();
                GetData();
            }

            if (dataGridView2.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                // butuh konfirmasi dulu sebelum hapus
                DialogResult result = MessageBox.Show("Apakah kamu yakin ingin menghapusnya?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView2.CurrentRow.Cells["dgvid"].Value);
                    string query = "DELETE from products where pID= " + id + "";
                    Hashtable ht = new Hashtable();
                    MainClass.SQL(query, ht);

                    MessageBox.Show("Data berhasil dihapus");
                    GetData();
                }
            }

        }

    }
}
