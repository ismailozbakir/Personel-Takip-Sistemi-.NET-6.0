using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelKayit
{
    internal class PersonelTabloDAO
    {
        string connectionString = @"Data Source=DESKTOP-5FTUGTC\SQLEXPRESS;Initial Catalog=PersonelVeriTabani;User ID=sa;Password=1";
        public List<PersonelTablo> Listele()
        {
            string query = "SELECT * FROM Tbl_Personel";

            List<PersonelTablo> returnedList = new List<PersonelTablo>();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand(query, connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string durum = reader.GetValue(5).ToString();
                    PersonelTablo personelTablo = new PersonelTablo()
                    {
                        Id = reader.GetInt16(0),
                        Ad = reader.GetValue(1), // reader[1], olarak da yazılabilirdi.
                        Soyad = reader.GetValue(2),
                        Sehir = reader.GetValue(3),
                        Maas = reader.GetInt16(4),
                        Durum = durum == "1" || durum == "Evli" ? "Evli" : durum == "0" || durum == "Bekar" ? "Bekar" : "Belirtilmedi",
                        Meslek = reader.GetValue(6),
                    };
                    returnedList.Add(personelTablo);
                }
            }
            connection.Close();
            return returnedList;
        }

    }
}
