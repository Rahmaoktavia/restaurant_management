using Guna.UI2.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM.Model
{
    public partial class frmCheckout : SampleAdd
    {
        public frmCheckout()
        {
            InitializeComponent();
        }

        public double amt;
        public int MainID = 0;

        // Add a property to set txtBillAmount value
        public string BillAmount
        {
            set { txtBillAmount.Text = value; }
        }

        private void txtReceived_TextChanged(object sender, EventArgs e)
        {
            double amt = 0;
            double receipt = 0;
            double change = 0;

            double.TryParse(txtBillAmount.Text, out amt);
            double.TryParse(txtReceived.Text, out receipt);

            change = receipt - amt;

            txtChange.Text = change.ToString();
        }

        /*public override void btnSave_Click(object sender, EventArgs e)
        {
            string query = @"Update tblMain set total = @total, received @rec, change = @change 
                                where MainID = @id";

            Hashtable ht = new Hashtable();
            ht.Add("@total", txtBillAmount.Text);
            ht.Add("@rec", txtReceived.Text);
            ht.Add("@change", txtChange.Text);

            if (MainClass.SQL(query, ht) > 0)
            {
                MessageBox.Show("Berhasil Tersimpan");
                this.Close();
            }
        }*/

        private void txtBillAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtChange_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Pembayaran berhasil");
        }
    }
}
