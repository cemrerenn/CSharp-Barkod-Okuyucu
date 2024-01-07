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
    public partial class fUrunGiris : Form
    {
        public fUrunGiris()
        {
            InitializeComponent();
        }

        BarkodDBEntities db = new BarkodDBEntities();
        private void tBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barkod = tBarkod.Text.Trim();
                if (db.Urun.Any(a => a.Barkod == barkod))
                {
                    var urun = db.Urun.Where(a => a.Barkod == barkod).SingleOrDefault();
                    tUrunAdi.Text = urun.UrunAd;
                    tAciklama.Text = urun.Aciklama;
                    cmbUrunGrup.Text = urun.UrunGrup;
                    tAlisFiyati.Text = urun.AlisFiyat.ToString();
                    tSatisFiyati.Text = urun.SatisFiyat.ToString();
                    tMiktar.Text = urun.Miktar.ToString();
                    tKdvOrani.Text = urun.KDVOrani.ToString();
                    if (urun.Birim == "KG")
                    {
                        chUrunTipi.Checked = true;
                    }
                    else
                    {
                        chUrunTipi.Checked = false;
                    }

                }
                else
                {
                    MessageBox.Show("Ürün Kayıtlı Değil,Ürünü kaydede Bilirsiniz.");
                }

            }
        }

        private void bKaydet_Click(object sender, EventArgs e)
        {
            if (tBarkod.Text != "" && cmbUrunGrup.Text != "" && tUrunAdi.Text != "" && tSatisFiyati.Text != "")
            {
                if (db.Urun.Any(a => a.Barkod == tBarkod.Text))
                {
                    var guncelle = db.Urun.Where(a => a.Barkod == tBarkod.Text).SingleOrDefault();
                    guncelle.Barkod = tBarkod.Text;
                    guncelle.UrunAd = tUrunAdi.Text;
                    guncelle.Aciklama = tAciklama.Text;
                    guncelle.UrunGrup = cmbUrunGrup.Text;
                    guncelle.AlisFiyat = Convert.ToDouble(tAlisFiyati.Text);
                    guncelle.SatisFiyat = Convert.ToDouble(tSatisFiyati.Text);
                    guncelle.KDVOrani = Convert.ToInt32(tKdvOrani.Text);
                    guncelle.KDVTutari = Math.Round(Islemler.DoubleYap(tSatisFiyati.Text) * Convert.ToInt32(tKdvOrani.Text) / 100, 2);
                    guncelle.Miktar += Convert.ToDouble(tMiktar.Text);
                    if (chUrunTipi.Checked)
                    {
                        guncelle.Birim = "KG";
                    }
                    else
                    {
                        guncelle.Birim = "Adet";
                    }

                    guncelle.Tarih = DateTime.Now;
                    guncelle.Kullanici = lKullanici.Text;
                    db.SaveChanges();
                    TEmizle();
                    gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(10).ToList();
                    Islemler.GridDuzenle(gridUrunler);
                }
                else
                {
                    Urun urun = new Urun()

                    {
                        Barkod = tBarkod.Text,
                        UrunAd = tUrunAdi.Text,
                        Aciklama = tAciklama.Text,
                        UrunGrup = cmbUrunGrup.Text,
                        AlisFiyat = Convert.ToDouble(tAlisFiyati.Text),
                        SatisFiyat = Convert.ToDouble(tSatisFiyati.Text),
                        KDVOrani = Convert.ToInt32(tKdvOrani.Text),
                        KDVTutari = Math.Round(Islemler.DoubleYap(tSatisFiyati.Text) * Convert.ToInt32(tKdvOrani.Text) / 100, 2),
                        Miktar = Convert.ToDouble(tMiktar.Text),
                        Tarih = DateTime.Now,
                        Kullanici = lKullanici.Text,

                    };
                    if (chUrunTipi.Checked)
                    {
                        urun.Birim = "KG";
                    }
                    else
                    {
                        urun.Birim = "Adet";
                    }
                    db.Urun.Add(urun);
                    db.SaveChanges();
                    if (tBarkod.Text.Length == 8)
                    {
                        var ozelbarkod = db.Barkod.First();
                        ozelbarkod.BarkodNo += 1;
                        db.SaveChanges();
                    }
                    Islemler.StokGiris(tBarkod.Text, tUrunAdi.Text, "Adet", Convert.ToDouble(tMiktar.Text), cmbUrunGrup.Text, lKullanici.Text);


                }
                gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList();
                TEmizle();

            }
            else
            {
                MessageBox.Show("Bilgileri Kontrol Ediniz.");
            }
        }

        private void TEmizle()
        {
            tBarkod.Clear();
            tUrunAdi.Clear();
            tAciklama.Clear();
            tAlisFiyati.Text = "0";
            tSatisFiyati.Text += "0";
            tMiktar.Text = "0";
            tKdvOrani.Text = "8";
            tBarkod.Focus();
            chUrunTipi.Checked = false;
        }

        private void tUrunAra_TextChanged(object sender, EventArgs e)
        {
            if (tUrunAra.Text.Length >= 1)
            {
                string urunad = tUrunAra.Text;
                gridUrunler.DataSource = db.Urun.Where(a => a.UrunAd.Contains(urunad)).ToList();
                Islemler.GridDuzenle(gridUrunler);
            }
        }

        private void bIptal_Click(object sender, EventArgs e)
        {
            TEmizle();
        }

        private void fUrunGiris_Load(object sender, EventArgs e)
        {
            tUrunSayisi.Text = db.Urun.Count().ToString();
            gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList();
            Islemler.GridDuzenle(gridUrunler);
            GrupDoldur();
        }

        private void bUrunGrubuEkle_Click(object sender, EventArgs e)
        {
            fUrunGrubuEkle f = new fUrunGrubuEkle();
            f.ShowDialog();
        }
        public void GrupDoldur()
        {
            cmbUrunGrup.DisplayMember = "UrunGrupAd";
            cmbUrunGrup.ValueMember = "Id";
            cmbUrunGrup.DataSource = db.UrunGrup.OrderBy(a => a.UrunGrupAd).ToList();
        }

        private void bBarkodOlustur_Click(object sender, EventArgs e)
        {
            var barkodno = db.Barkod.First();
            int karakter = barkodno.BarkodNo.ToString().Length;
            string sifirlar = string.Empty;
            for (int i = 0; i < 8 - karakter; i++)
            {
                sifirlar = sifirlar + "0";
            }
            string olusanBarkod = sifirlar + barkodno.BarkodNo.ToString();
            tBarkod.Text = olusanBarkod;
            tUrunAdi.Focus();
        }

        private void tSatisFiyati_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08 && e.KeyChar != (char)44 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridUrunler.Rows.Count > 0)
            {
                int urunid = Convert.ToInt32(gridUrunler.CurrentRow.Cells["UrunId"].Value.ToString());
                string UrunAd = gridUrunler.CurrentRow.Cells["UrunAd"].Value.ToString();
                string barkod = gridUrunler.CurrentRow.Cells["barkod"].Value.ToString();
                DialogResult onay = MessageBox.Show(UrunAd + " ürünü silmek istiyor musunuz ?", "Ürün Silme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (onay == DialogResult.Yes)
                {
                    var urun = db.Urun.Find(urunid);
                    db.Urun.Remove(urun);
                    db.SaveChanges();
                    var hizliurun = db.HizliUrun.Where(x => x.Barkod == barkod).FirstOrDefault();
                    if(hizliurun!=null)
                    {
                        hizliurun.Barkod = "-";
                        hizliurun.UrunAd = "-";
                        hizliurun.Fiyat = 0;
                    }
                    
                    db.SaveChanges();
                    MessageBox.Show("Ürün Silinmiştir.");
                    gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList();
                    Islemler.GridDuzenle(gridUrunler);
                    tBarkod.Focus();
                }
            }

        }

        private void chUrunTipi_CheckedChanged(object sender, EventArgs e)
        {
            if (chUrunTipi.Checked)
            {
                chUrunTipi.Text = "Gramajlı Ürün İşlemi";
                bBarkodOlustur.Enabled = false;
            }
            else
            {
                chUrunTipi.Text = "Barkodlu Ürün İşlemi";
                bBarkodOlustur.Enabled = true;
            }
        }

        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridUrunler.Rows.Count > 0)
            {
                tBarkod.Text = gridUrunler.CurrentRow.Cells["Barkod"].Value.ToString();
                tUrunAdi.Text = gridUrunler.CurrentRow.Cells["UrunAd"].Value.ToString();
                tAciklama.Text = gridUrunler.CurrentRow.Cells["Aciklama"].Value.ToString();
                cmbUrunGrup.Text = gridUrunler.CurrentRow.Cells["UrunGrup"].Value.ToString();
                tAlisFiyati.Text= gridUrunler.CurrentRow.Cells["AlisFiyat"].Value.ToString();
                tSatisFiyati.Text= gridUrunler.CurrentRow.Cells["SatisFiyat"].Value.ToString();
                tKdvOrani.Text= gridUrunler.CurrentRow.Cells["KDVOrani"].Value.ToString();
                tMiktar.Text= gridUrunler.CurrentRow.Cells["Miktar"].Value.ToString();
                string birim = gridUrunler.CurrentRow.Cells["Birim"].Value.ToString();

                if(birim=="KG")
                {
                    chUrunTipi.Checked = true;
                }
                else
                {
                    chUrunTipi.Checked = false;
                }

            }
        }
    }
}
