using Microsoft.SqlServer.Server;
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
    public partial class PersonelStats : Form
    {
        static string connectionString = @"Data Source=DESKTOP-5FTUGTC\SQLEXPRESS;Initial Catalog=PersonelVeriTabani;User ID=sa;Password=1";
        SqlConnection connection = new(connectionString); 
        public PersonelStats()
        {
            InitializeComponent();
        }
        public void GetPerStats(string query, Label label)
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    label.Text = query.Contains("SUM") || query.Contains("AVG") ? $"{reader[0].ToString()} ₺" : reader[0].ToString();

            }
            connection.Close();
        }
        private void PersonelStats_Load(object sender, EventArgs e)
        {
            string queryTotalPer = "Select Count(*) From Tbl_Personel";
            GetPerStats(queryTotalPer, totalPer);

            string queryEvliPer = "Select Count(*) From Tbl_Personel where PerDurum = 1";
            GetPerStats(queryEvliPer, evliPer);

            string queryBekarPer = "Select Count(*) From Tbl_Personel where PerDurum = 0";
            GetPerStats(queryBekarPer, bekarPer);

            string querySehir = "Select count(distinct(PerSehir)) from Tbl_Personel";
            GetPerStats(querySehir, sehirSayi);

            string queryOrtMaas = "Select AVG(PerMaas) from Tbl_Personel";
            GetPerStats(queryOrtMaas, ortMaas);


            string queryTotalMaas = "Select SUM(PerMaas) from Tbl_Personel";
            GetPerStats(queryTotalMaas, totalMaas);

        }
    }
}
