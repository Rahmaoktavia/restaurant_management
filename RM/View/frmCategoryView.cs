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
    public partial class frmCategoryView : SampleView
    {
        
        public frmCategoryView()
        {
            InitializeComponent();
            // Mendaftarkan penangan event
            frmCategoryAdd frm = new frmCategoryAdd();
            frm.DataChanged += Frm_DataChanged;
        }
        private void Frm_DataChanged(object sender, EventArgs e)
        {
            GetData(); // Menyegarkan data setelah form tambahan ditutup
        }

        public void GetData()
        {
            string query = "SELECT * FROM category where catName like '%"+ txtSearch.Text +"%' ";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvName);

            MainClass.LoadData(query, dataGridView2, lb);
        }

        private void frmCategoryView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            frmCategoryAdd frm = new frmCategoryAdd();
            frm.ShowDialog();
            GetData(); // Tambahkan baris ini untuk menyegarkan data setelah menutup form tambahan
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView2.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                frmCategoryAdd frm = new frmCategoryAdd();
                frm.id = Convert.ToInt32(dataGridView2.CurrentRow.Cells["dgvid"].Value);
                frm.txtName.Text = Convert.ToString(dataGridView2.CurrentRow.Cells["dgvName"].Value);
                frm.ShowDialog();
                GetData() ;
            }

            if (dataGridView2.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                // butuh konfirmasi dulu sebelum hapus
                DialogResult result = MessageBox.Show("Apakah kamu yakin ingin menghapusnya?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dataGridView2.CurrentRow.Cells["dgvid"].Value);
                    string query = "DELETE from category where catID= " + id + "";
                    Hashtable ht = new Hashtable();
                    MainClass.SQL(query, ht);

                    MessageBox.Show("Data berhasil dihapus");
                    GetData();
                }
            }

        }


        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            GetData();
        }
    }
}
