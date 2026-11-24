using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Kasa_Takip_Otomasyonu
{
    public partial class FrmDashboard : Form
    {
        private readonly Baglanti bgl = new Baglanti();

        public FrmDashboard()
        {
            InitializeComponent();
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {
            KartVerileriniGetir();
            GrafikleriDoldur();
        }

        private void KartVerileriniGetir()
        {
            try
            {
                using (SqlConnection conn = bgl.Baglan())
                {
                    // 1. Mavi Kart: Toplam Kasa Bakiyesi
                    SqlCommand cmdBakiye = new SqlCommand("SELECT ISNULL(SUM(Bakiye), 0) FROM Kasalar", conn);
                    decimal toplamBakiye = Convert.ToDecimal(cmdBakiye.ExecuteScalar());
                    lblToplamBakiye.Text = toplamBakiye.ToString("N2") + " ₺";

                    // 2. Yeşil Kart: Aylık Toplam Gelir (İçinde bulunulan ay)
                    string queryGelir = @"SELECT ISNULL(SUM(Tutar), 0) FROM GelirGiderler 
                                          WHERE IslemTuru = N'Gelir' 
                                          AND MONTH(Tarih) = MONTH(GETDATE()) 
                                          AND YEAR(Tarih) = YEAR(GETDATE())";
                    SqlCommand cmdGelir = new SqlCommand(queryGelir, conn);
                    decimal aylikGelir = Convert.ToDecimal(cmdGelir.ExecuteScalar());
                    lblAylikGelir.Text = aylikGelir.ToString("N2") + " ₺";

                    // 3. Kırmızı Kart: Aylık Toplam Gider (İçinde bulunulan ay)
                    string queryGider = @"SELECT ISNULL(SUM(Tutar), 0) FROM GelirGiderler 
                                          WHERE IslemTuru = N'Gider' 
                                          AND MONTH(Tarih) = MONTH(GETDATE()) 
                                          AND YEAR(Tarih) = YEAR(GETDATE())";
                    SqlCommand cmdGider = new SqlCommand(queryGider, conn);
                    decimal aylikGider = Convert.ToDecimal(cmdGider.ExecuteScalar());
                    lblAylikGider.Text = aylikGider.ToString("N2") + " ₺";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kart verileri yüklenemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GrafikleriDoldur()
        {
            try
            {
                using (SqlConnection conn = bgl.Baglan())
                {
                    // Sol Grafik (Sütun): Kasalar ve Bakiyeleri
                    chart1.Series.Clear();
                    var seriKasa = chart1.Series.Add("Kasa Bakiyesi");
                    seriKasa.ChartType = SeriesChartType.Column;

                    SqlCommand cmdKasa = new SqlCommand("SELECT KasaAd, Bakiye FROM Kasalar", conn);
                    using (SqlDataReader drKasa = cmdKasa.ExecuteReader())
                    {
                        while (drKasa.Read())
                        {
                            seriKasa.Points.AddXY(drKasa["KasaAd"].ToString(), drKasa["Bakiye"]);
                        }
                    }

                    // Sağ Grafik (Pasta): Kategorilere Göre Gider Dağılımı
                    chart2.Series.Clear();
                    var seriKategori = chart2.Series.Add("Gider Dağılımı");
                    seriKategori.ChartType = SeriesChartType.Pie;

                    string queryKategori = @"SELECT K.KategoriAd, ISNULL(SUM(G.Tutar), 0) AS Toplam 
                                             FROM GelirGiderler G 
                                             INNER JOIN Kategoriler K ON G.KategoriId = K.KategoriId 
                                             WHERE G.IslemTuru = N'Gider' 
                                             GROUP BY K.KategoriAd";

                    SqlCommand cmdKat = new SqlCommand(queryKategori, conn);
                    using (SqlDataReader drKat = cmdKat.ExecuteReader())
                    {
                        while (drKat.Read())
                        {
                            seriKategori.Points.AddXY(drKat["KategoriAd"].ToString(), drKat["Toplam"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Grafik verileri yüklenemedi: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCategoriesForm_Click(object sender, EventArgs e)
        {
            FrmCategories frm = new FrmCategories();
            frm.FormClosed += (s, args) => Application.Exit();
            frm.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
    }
}