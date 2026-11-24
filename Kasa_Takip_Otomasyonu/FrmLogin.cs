using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Kasa_Takip_Otomasyonu
{
    public partial class FrmLogin : Form
    {
        private readonly Baglanti bgl = new Baglanti();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Kullanıcı adı ve şifre boş bırakılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = bgl.Baglan())
                {
                    string query = "SELECT COUNT(*) FROM Kullanicilar WHERE KullaniciAdi = @user AND Sifre = @pass";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@pass", password);

                        int sonuc = (int)cmd.ExecuteScalar();

                        if (sonuc > 0)
                        {
                            FrmDashboard frm = new FrmDashboard();
                            frm.FormClosed += (s, args) => Application.Exit();
                            frm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı adı veya şifre hatalı!", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e) { }
        private void FrmLogin_Load_1(object sender, EventArgs e) { }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}