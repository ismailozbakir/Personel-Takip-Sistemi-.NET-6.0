using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace PersonelKayit
{
    public partial class FrmAnaForm : Form
    {
        BindingSource bindingSource = new();
        static string connectionString = @"Data Source=DESKTOP-5FTUGTC\SQLEXPRESS;Initial Catalog=PersonelVeriTabani;User ID=sa;Password=1";
        SqlConnection connection = new(connectionString);

        public FrmAnaForm()
        {
            InitializeComponent();
        }
        private void Temizle()
        {
            radioBekar.Checked = false;
            radioEvli.Checked = false;
            txbAd.Clear();
            txbSoyad.Clear();
            txbMeslek.Clear();
            maskedMaas.Clear();
            comboSehir.Text = "";
            txbAd.Focus();
        }
        private void btnListele_Click(object sender, EventArgs e)
        {
            PersonelTabloDAO personelTabloDAO = new PersonelTabloDAO();
            bindingSource.DataSource = personelTabloDAO.Listele();
            dataGridView1.DataSource = bindingSource;
            txbAd.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            connection.Open();
            bool maasDurum = int.TryParse(maskedMaas.Text, out int maas);
            string query = $"INSERT INTO Tbl_Personel (PerAd, PerSoyad, PerSehir, PerMaas, PerDurum, PerMeslek) VALUES ('{txbAd.Text}', '{txbSoyad.Text}', '{comboSehir.Text}', '{maskedMaas.Text}', '{(radioEvli.Checked ? "1" : radioBekar.Checked ? "0" : null)}', '{txbMeslek.Text}')";
            SqlCommand cmd = new SqlCommand(query, connection);
            
            if (txbAd.Text == "" || txbSoyad.Text == "")
                MessageBox.Show("Ad ve Soyad girilmesi zorunludur!");
            
            else if (int.TryParse(txbAd.Text, out int _) || int.TryParse(txbSoyad.Text, out int _))
                MessageBox.Show("Ad ve/veya Soyad sayýsal deðer olamaz!");

            else
            {
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Veriler Kaydedildi");

            }
            Temizle();


            //// Murat Yücedað Udemy dersinde aþaðýdaki gibi yapmýþ
            
            //connection.Open();
            //string query = "INSERT INTO Tbl_Personel (PerAd, PerSoyad, PerSehir, PerMaas, PerDurum, PerMeslek) VALUES (@p1, @p2, @p3, @p4, @p5, @p6)";
            //SqlCommand cmd = new SqlCommand(query, connection);
            //cmd.Parameters.AddWithValue("@p1", txbAd.Text);
            //cmd.Parameters.AddWithValue("@p2", txbSoyad.Text);
            //cmd.Parameters.AddWithValue("@p3", comboSehir.Text);
            //cmd.Parameters.AddWithValue("@p4", maskedMaas.Text);
            //cmd.Parameters.AddWithValue("@p5", radioEvli.Checked ? "1" : radioBekar.Checked ? "0" : "Belirtilmedi");
            //cmd.Parameters.AddWithValue("@p6", txbMeslek.Text);
            //cmd.ExecuteNonQuery();
            //connection.Close();
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            txbAd.Text = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();
            txbSoyad.Text = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();
            comboSehir.Text = dataGridView1.Rows[selectedRowIndex].Cells[3].Value.ToString();
            maskedMaas.Text = dataGridView1.Rows[selectedRowIndex].Cells[4].Value.ToString();
            radioBekar.Checked = dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString() == "Bekar" ? true : false;
            radioEvli.Checked = dataGridView1.Rows[selectedRowIndex].Cells[5].Value.ToString() == "Evli" ? true : false;
            txbMeslek.Text = dataGridView1.Rows[selectedRowIndex].Cells[6].Value.ToString();

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Silmek için lütfen onaylayýn.", "Silme Onayý", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result.Equals(DialogResult.OK))
            {
                connection.Open();
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                short perId = (short)dataGridView1.Rows[selectedRowIndex].Cells[0].Value;
                string query = $"Delete From Tbl_Personel where PerId = {perId}";
                string query2 = @"declare @max int select @max=max(PerId) from Tbl_Personel if @max IS NULL SET @max = 0 DBCC CHECKIDENT ('Tbl_Personel', RESEED, @max)";
                string beforeDeleteId = perId.ToString();
                string beforeDeleteAd = txbAd.Text;
                string beforeDeleteSoyad = txbSoyad.Text;
                SqlCommand cmdDelete = new(query, connection);
                SqlCommand cmdReset = new(query2, connection);
                cmdDelete.ExecuteNonQuery();
                cmdReset.ExecuteNonQuery();
                connection.Close();
                Temizle();
                MessageBox.Show($"{beforeDeleteId} nolu '{beforeDeleteAd} {beforeDeleteSoyad}' isimli kayýt silindi.");
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            connection.Open();
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            short perId = (short)dataGridView1.Rows[selectedRowIndex].Cells[0].Value;
            string query = $"Update Tbl_Personel Set PerAd = '{txbAd.Text}', PerSoyad = '{txbSoyad.Text}', PerSehir = '{comboSehir.Text}', PerMaas = {maskedMaas.Text}, PerDurum = '{(radioEvli.Checked ? "1" : radioBekar.Checked ? "0" : null)}', PerMeslek = '{txbMeslek.Text}' where PerId = {perId}";
            SqlCommand cmdGuncelle = new(query, connection);
            cmdGuncelle.ExecuteNonQuery();
            connection.Close();
            Temizle();
            MessageBox.Show("Güncellendi");

        }

        private void btnStatistic_Click(object sender, EventArgs e)
        {
            PersonelStats personelStats = new PersonelStats();
            personelStats.Show();
        }

        private void btnGrafik_Click(object sender, EventArgs e)
        {
            FrmGrafikler frmGrafikler = new();
            frmGrafikler.Show();
        }

        private void FrmAnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}