using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarkodluSatis
{
    public partial class fBaslangic : Form
    {
        public fBaslangic()
        {
            InitializeComponent();
        }

        private void bSatisIslemi_Click(object sender, EventArgs e)
        {
            fSatis f = new fSatis();
            f.lKullanici.Text = lKullanici.Text;
            f.ShowDialog();
        }

        private void bGenelRapor_Click(object sender, EventArgs e)
        {
            fRapor f = new fRapor();    
            f.lKullanici.Text= lKullanici.Text;
            f.ShowDialog();
        }

        private void bStokTakibi_Click(object sender, EventArgs e)
        {
            fStok f = new fStok();
            f.lKullanici.Text=lKullanici.Text;
            f.ShowDialog();
        }

        private void bUrunGiris_Click(object sender, EventArgs e)
        {
            fUrunGiris f = new fUrunGiris();
            f.lKullanici.Text = lKullanici.Text;
            f.ShowDialog();
        }

        private void bCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bFiyatGuncelle_Click(object sender, EventArgs e)
        {
            fFiyatGuncelle f = new fFiyatGuncelle();
            f.ShowDialog();
        }

        private void bAyarlar_Click(object sender, EventArgs e)
        {
            fAyarlar fAyarlar = new fAyarlar();
            fAyarlar.ShowDialog();
        }

        private void bYedekleme_Click(object sender, EventArgs e)
        {
            Islemler.Backup();
        }

        private void bKullaniciDegistir_Click(object sender, EventArgs e)
        {
            fLogin login = new fLogin();
            login.Show();
            this.Hide();
        }
    }
}
