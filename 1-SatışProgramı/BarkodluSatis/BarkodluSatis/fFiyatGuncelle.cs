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
    public partial class fFiyatGuncelle : Form
    {
        public fFiyatGuncelle()
        {
            InitializeComponent();
        }

        private void tBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) 
            {
                using(var db = new BarkodDBEntities()) 
                {
                    if(db.Urun.Any(x=>x.Barkod==tBarkod.Text)) 
                    { 
                        var getir = db.Urun.Where(x=>x.Barkod==tBarkod.Text).SingleOrDefault();
                        lBarkod.Text = getir.Barkod;
                        lUrunAd.Text = getir.UrunAd;
                        double mevcutFiyat = Convert.ToDouble(getir.SatisFiyat);
                        lMevcutFiyat.Text = mevcutFiyat.ToString("C2");
                        tYeniFiyat.Focus();
                    }else
                    {
                        MessageBox.Show("Ürün Kayıtlı Değil");
                    }
                }
            }
        }

        private void bEkle_Click(object sender, EventArgs e)
        {
            if(tYeniFiyat.Text!=""&&lBarkod.Text!="")
            {
                using(var db = new BarkodDBEntities())
                {
                    var guncelle =db.Urun.Where(x=>x.Barkod==lBarkod.Text).SingleOrDefault();
                    guncelle.SatisFiyat = Islemler.DoubleYap(tYeniFiyat.Text);
                    int kdvorani = Convert.ToInt16(guncelle.KDVOrani);
                    Math.Round(Islemler.DoubleYap(tYeniFiyat.Text) * Convert.ToInt32(kdvorani) / 100, 2);
                    db.SaveChanges();
                    MessageBox.Show("Ürün Fiyatı Güncellenmiştir");
                    lBarkod.Text = "...";
                    lUrunAd.Text = "...";
                    lMevcutFiyat.Text = "...";
                    tBarkod.Clear();
                    tYeniFiyat.Clear();
                    tBarkod.Focus();
                }
            }
            else
            {
                MessageBox.Show("Ürün Okutunuz.");
            }
        }
    }
}
