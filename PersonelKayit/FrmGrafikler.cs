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
using System.Windows.Forms.DataVisualization.Charting;
namespace PersonelKayit
{
    public partial class FrmGrafikler : Form
    {
        public FrmGrafikler()
        {
            InitializeComponent();
        }
        static string connectionString = @"Data Source=DESKTOP-5FTUGTC\SQLEXPRESS;Initial Catalog=PersonelVeriTabani;User ID=sa;Password=1";
        SqlConnection connection = new(connectionString);
        private void FrmGrafikler_Load(object sender, EventArgs e)
        {
            // Grafik 1
            connection.Open();
            string query = "Select PerSehir, Count(*) From Tbl_Personel Group by PerSehir";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataReader dr1 = cmd.ExecuteReader();
            while (dr1.Read())
            {
                chart1.Series["Sehirler"].Points.AddXY(dr1[0], dr1[1]);

            }
            connection.Close();

            // Grafik 2
            connection.Open();
            string query2 = "Select PerMeslek, Avg(PerMaas) From Tbl_Personel Group by PerMeslek";
            SqlCommand cmd2 = new SqlCommand(query2, connection);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                chart2.Series["Meslek-Maas"].Points.AddXY(dr2[0], dr2[1]);

            }
            connection.Close();

        }
    }
}
