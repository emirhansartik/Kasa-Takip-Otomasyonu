using System.Data.SqlClient;

namespace Kasa_Takip_Otomasyonu
{
    public class Baglanti
    {
        private readonly string connectionString = @"Server=EMIRHAN\SQLEXPRESS;Database=KasaTakipDb;Integrated Security=True;";

        public SqlConnection Baglan()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}