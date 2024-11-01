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
    public partial class frmBillList : SampleAdd
    {
        public frmBillList()
        {
            InitializeComponent();
        }

        public int MainID = 0;

        private void LoadData()
        {
            string query = @"select pID, uName,pName, total from
                        where status <> 'pending' ";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvProduct);
            lb.Items.Add(dgvTotal);


            MainClass.LoadData(query, dataGridView2, lb);
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

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                MainID = Convert.ToInt32(dataGridView2.CurrentRow.Cells["dgvid"].Value);
                this.Close();

            }
        }
    }

    }