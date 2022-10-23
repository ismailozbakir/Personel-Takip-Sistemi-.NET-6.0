using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonelKayit
{
    public partial class PersonelKayitLogin : Form
    {
        static string connectionString = @"Data Source=DESKTOP-5FTUGTC\SQLEXPRESS;Initial Catalog=PersonelVeriTabani;User ID=sa;Password=1";
        SqlConnection connection = new(connectionString);
        public PersonelKayitLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            connection.Open();
            //string query = $"Select * From Tbl_Personel_Login where UserName = '{txbUser.Text}' and Pass = '{txbPass.Text}'"; // Bu şekilde yapılırsa SQL Injection saldırısına açık olabilir.
            string query = "Select * From Tbl_Personel_Login where UserName = @user and Pass = @password";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@user", txbUser.Text);
            cmd.Parameters.AddWithValue("@password", txbPass.Text);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                FrmAnaForm form = new FrmAnaForm();
                form.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Giriş bilgileri hatalı");
            }
            connection.Close();
        }

        private void PersonelKayitLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void txbUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //btnLogin_Click(this, new EventArgs());
                btnLogin_Click(sender, e);
            }
        }

        private void txbPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                btnLogin_Click(sender, e);
      
            }
        }

        private void txbPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                e.Handled = true;
        }

        private void txbUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                e.Handled = true;
        }

        private void PersonelKayitLogin_Load(object sender, EventArgs e)
        {
            txbUser.Text = "admin";
        }
    }
}
