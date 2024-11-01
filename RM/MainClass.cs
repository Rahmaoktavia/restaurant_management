using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections;

namespace RM
{
    internal class MainClass
    {
        //private static MySqlConnection connection = new MySqlConnection("Server=localhost;Database=rm;Uid=root;Pwd=;");

        public static MySqlConnection connection = new MySqlConnection("Server=localhost; Database=rm;Uid=root;Pwd=;");

        public static bool IsValidUser(string username, string password)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                string query = "SELECT * FROM Users WHERE username = @username AND uPass = @password";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }         

        }

        public static string user;

        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }

        //Method for crud operation

        public static int SQL(string query, Hashtable ht)
        {
            int res = 0;

            try
            {
                //SqlCommand cmd = new SqlCommand(qry, connection);
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.CommandType = CommandType.Text;

                foreach (DictionaryEntry item  in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                res = cmd.ExecuteNonQuery();
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
                connection.Close();
            }

            return res;
        }

        //untuk loading data dari database

        public static void LoadData(string query, DataGridView gv, ListBox lb)
        {

            try
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < lb.Items.Count; i++)
                {
                    string colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colNam1].DataPropertyName = dt.Columns[i].ToString();
                }

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                connection.Close();
            }
        }


        //Untuk cb Fill

        public static void CBFill(string query, ComboBox cb)
        {

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cb.DisplayMember = "name";
            cb.ValueMember = "id";
            cb.DataSource = dt;
            cb.SelectedIndex = -1;
        }

    }
}
