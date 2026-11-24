using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Kasa_Takip_Otomasyonu
{
    public partial class FrmCategories : Form
    {
        private readonly Baglanti bgl = new Baglanti();

        public FrmCategories()
        {
            InitializeComponent();
        }

        private void FrmCategories_Load(object sender, EventArgs e)
        {
            KategoriListele();
        }

        private void KategoriListele()
        {
            try
            {
                using (SqlConnection conn = bgl.Baglan())
                {
                    string query = "SELECT KategoriId AS [ID], KategoriAd AS [Kategori Adı] FROM Kategoriler ORDER BY KategoriId ASC";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listeleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Temizle()
        {
            txtCategoriesId.Clear();
            txtCategoriesName.Clear();
            txtCategoriesName.Focus();
        }

        private void btnCategoryList_Click(object sender, EventArgs e)
        {
            KategoriListele();
        }

        private void btnCreateCategory_Click(object sender, EventArgs e)
        {
            string kategoriAd = txtCategoriesName.Text.Trim();
            if (string.IsNullOrEmpty(kategoriAd))
            {
                MessageBox.Show("Lütfen kategori adı girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = bgl.Baglan())
                {
                    string query = "INSERT INTO Kategoriler (KategoriAd) VALUES (@ad)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", kategoriAd);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Kategori başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Temizle();
                KategoriListele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ekleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRemoveCategory_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCategoriesId.Text.Trim(), out int id))
            {
                MessageBox.Show("Lütfen tablodan silinecek bir kategori seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = bgl.Baglan())
                {
                    string query = "DELETE FROM Kategoriler WHERE KategoriId = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Kategori başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Temizle();
                KategoriListele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme hatası (bu kategoriye bağlı işlem olabilir): " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCategoriesId.Text.Trim(), out int id))
            {
                MessageBox.Show("Lütfen tablodan güncellenecek bir kategori seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string kategoriAd = txtCategoriesName.Text.Trim();
            if (string.IsNullOrEmpty(kategoriAd))
            {
                MessageBox.Show("Kategori adı boş bırakılamaz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = bgl.Baglan())
                {
                    string query = "UPDATE Kategoriler SET KategoriAd = @ad WHERE KategoriId = @id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", kategoriAd);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Kategori başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Temizle();
                KategoriListele();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

   
        private void frmDashboardForm_Click_1(object sender, EventArgs e)
        {
            FrmDashboard frm = new FrmDashboard();
            frm.FormClosed += (s, args) => Application.Exit();
            frm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e) { }
        private void btnBanksForm_Click_1(object sender, EventArgs e) { }
        private void button4_Click(object sender, EventArgs e) { }
        private void btnBankProcessesForm_Click(object sender, EventArgs e) { }
        private void btnSettings_Click(object sender, EventArgs e) { }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}