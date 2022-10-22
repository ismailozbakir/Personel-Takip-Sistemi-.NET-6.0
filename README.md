## Personel Kayit Sistemi - Windows Forms .NET 6.0
> Bu çalışmada personel kayıt sistemi (kaydetme, güncelleme, silme, istatistik, grafiksel gösterim vb. işlemler) ile ilgili windows form uygulaması geliştirdim. Bu uygulamayı, Murat Yücedağ'ın .NET framework 4.x ile anlattığı [UDEMY](https://www.udemy.com/course/sifirdan-ileri-seviye-csharp-programlama/) kursundaki SQL bölümünde yer verilen uygulamadan esinlenerek .NET 6.0 çatısında yazdım.

> Özellikle .NET 6.0 çatısı altında bu uygulamayı geliştirmek istedim. Çünkü bilindiği gibi .NET 5.0 ile Microsoft önemli bir manevra yaparak, bu çatıyı açık kaynak kodlu hale getirmiş ve çapraz platformlar için yazılımcıların kullanımına sunmuştur. 

<p align="center">
  <img width="700" height="200" src="https://user-images.githubusercontent.com/31667471/197296098-bdb1e5fc-3aa3-4a37-a303-adb308a47ec0.png">
</p>

---
> Önceki .NET framework 4.x olarak bilinen windows form yapısındaki birçok araç (tool), .NET 6.0'a taşınmış olsa da özellikle SQL veri tabanı ile ilgili işlemlerde veri tabanına bağlantı oluşturma sihirbazı ve bazı grafik araçlarından yoksundur. Bu eksiklikleri bir takım ek kodlama çalışmaları ile aşarak kursta anlatıldığı gibi çalışan bir uygulama yazabildim. 

<p align="center">
  <img width="750" height="400" src="https://user-images.githubusercontent.com/31667471/197298795-c4bd2974-d298-4c1f-ade5-6b65aec9fa0c.PNG">
</p>

---
Ek olarak :
* [x] admin panelinde textboxlara veri girişi yapıldıktan sonra "enter" tuşunun işlevsel hale getirilmesi ve "Ding" sesinin kesilmesi,
<p align="center">
  <img width="250" height="150" src="https://user-images.githubusercontent.com/31667471/197298511-71164a00-14ea-4d6f-b3e3-ef648826d53b.PNG">
</p>

---
* [x] programın kapatılması için penceredeki X butonuna tıklandığında geçiş yapılan bütün formların kapatılması için "event" kodlarının girilmesi,
* [x] silinen verilerin ardından Personel ID numaralarının sonraki eklenecek veri için otomatik güncellenmesi (aşağıda örnek olarak SQL kodu verilmiştir),

```sql
DBCC CHECKIDENT ('Tbl_Personel', RESEED, @max)
```
* [x] verilerin silinmesi için onay alınması ve silinen kaydın ekranda bilgi olarak aktarılması,
<p align="center">
  <img width="1100" height="400" src="https://user-images.githubusercontent.com/31667471/197301465-162e5602-239c-486b-a76a-2542dc509fc7.PNG">
</p>

---

> Uygulamanın kodlarını yazarken özellikle kod semantiğine dikkat etmeye çalıştım. Birbirlerini tekrarlayan kodlar için çeşitli metodlar yazdım. Özellikle istatistik elde etmek için kullanılan kodlar bölümünde,
* [x] **GetPerStats()** metodunu yazarak kod tekrarından kurtuldum.

```c#
static string connectionString = @"Data Source=DESKTOP-5FTKYC\SQLEXPRESS;Initial Catalog=PersonelVeriTabani;User ID=sa;Password=1";
SqlConnection connection = new(connectionString); 
        
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
```
---
> Ayrıca textbox gibi araçlardan veri alınarak, SQL veri tabanından sorgular yapacak **C#** kodlarının yazımında *string interpolasyon* formatını kullandım. Bu şekilde kodların okunabilirliği artmıştır. 
```c#
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
```
---
> Özetle bu çalışma ile windows form uygulamalarının SQL ile ilişkili durumlarda, yeni .NET 6.0 çatısı altında da yazılabildiğini göstermiş oldum.
