using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarkodluSatis
{
    public partial class fRapor : Form
    {
        public fRapor()
        {
            InitializeComponent();
        }

        public void bGoster_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;//butona basıldığında fare imlecini değiştirir. işlem bitince eski haline gelir
            DateTime baslangic = DateTime.Parse(dateBaslangic.Value.ToShortDateString());
            DateTime bitis = DateTime.Parse(dateBitis.Value.ToShortDateString());
            bitis = bitis.AddDays(1);
            using (var db = new BarkodDBEntities())
            {
                if (listFiltrelemeTuru.SelectedIndex == 0)
                {
                    db.IslemOzet.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis).OrderByDescending(x => x.Tarih).Load();//load ile headerlara basınca sıralama yapar
                    var islemozet = db.IslemOzet.Local.ToBindingList();
                    gridUrunler.DataSource = islemozet;

                    tSatisNakit.Text = Convert.ToDouble(islemozet.Where(x => x.Iade == false && x.Gelir == false && x.Gider == false).Sum(x => x.Nakit)).ToString("C2");
                    tSatisKart.Text = Convert.ToDouble(islemozet.Where(x => x.Iade == false && x.Gelir == false && x.Gider == false).Sum(x => x.Kart)).ToString("C2");

                    tIadeNakit.Text = Convert.ToDouble(islemozet.Where(x => x.Iade == true).Sum(x => x.Nakit)).ToString("C2");
                    tIadeKart.Text = Convert.ToDouble(islemozet.Where(x => x.Iade == true).Sum(x => x.Kart)).ToString("C2");

                    tGelirNakit.Text = Convert.ToDouble(islemozet.Where(x => x.Gelir == true).Sum(x => x.Nakit)).ToString("C2");
                    tGelirKart.Text = Convert.ToDouble(islemozet.Where(x => x.Gelir == true).Sum(x => x.Kart)).ToString("C2");

                    tGiderNakit.Text = Convert.ToDouble(islemozet.Where(x => x.Gider == true).Sum(x => x.Nakit)).ToString("C2");
                    tGiderKart.Text = Convert.ToDouble(islemozet.Where(x => x.Gider == true).Sum(x => x.Kart)).ToString("C2");

                    db.Satis.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis).Load();
                    var satistablosu = db.Satis.Local.ToBindingList();
                    double kdvtutariSatis = Islemler.DoubleYap(satistablosu.Where(x => x.Iade == false).Sum(x => x.KdvTutari).ToString());
                    double kdvtutariIade = Islemler.DoubleYap(satistablosu.Where(x => x.Iade == true).Sum(x => x.KdvTutari).ToString());
                    tKdvToplam.Text = (kdvtutariSatis - kdvtutariIade).ToString("C2");
                }
                else if(listFiltrelemeTuru.SelectedIndex==1)
                {
                    db.IslemOzet.Where(x=>x.Tarih>=baslangic&& x.Tarih<=bitis && x.Iade==false&&x.Gelir==false&&x.Gider==false).Load();
                    var islemozet =db.IslemOzet.Local.ToBindingList();  
                    gridUrunler.DataSource = islemozet;
                }
                else if (listFiltrelemeTuru.SelectedIndex == 2)
                {
                    db.IslemOzet.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis && x.Iade == true).Load();
                    var islemozet = db.IslemOzet.Local.ToBindingList();
                    gridUrunler.DataSource = islemozet;
                }
                else if (listFiltrelemeTuru.SelectedIndex == 3)
                {
                    db.IslemOzet.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis && x.Gelir == true).Load();
                    var islemozet = db.IslemOzet.Local.ToBindingList();
                    gridUrunler.DataSource = islemozet;
                }
                else if (listFiltrelemeTuru.SelectedIndex == 4)
                {
                    db.IslemOzet.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis && x.Gider == true).Load();
                    var islemozet = db.IslemOzet.Local.ToBindingList();
                    gridUrunler.DataSource = islemozet;
                }
            }

            Islemler.GridDuzenle(gridUrunler);

            Cursor.Current = Cursors.Default; //butona basıldığında fare imlecini değiştirir. işlem bitince eski haline gelir  
        }

        private void fRapor_Load(object sender, EventArgs e)
        {
            bGoster_Click(null, null);
            listFiltrelemeTuru.SelectedIndex = 0;

            tKartKomisyon.Text=Islemler.KartKomisyon().ToString();
        }

        private void gridUrunler_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2|| e.ColumnIndex == 6|| e.ColumnIndex == 7) 
            {
                if(e.Value is bool)
                {
                    bool value = (bool)e.Value;
                    e.Value = (value) ? "Evet" : "Hayır";
                    e.FormattingApplied = true;
                }
            }
        }

        private void bGelirEkle_Click(object sender, EventArgs e)
        {
            fGelirGider f= new  fGelirGider();
            f.GelirGider = "Gelir";
            f.Kullanici = lKullanici.Text;
            f.ShowDialog();
        }

        private void bGiderEkle_Click(object sender, EventArgs e)
        {
            fGelirGider f = new fGelirGider();
            f.GelirGider = "Gider";
            f.Kullanici = lKullanici.Text;
            f.ShowDialog();
        }

        private void detayGösterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(gridUrunler.Rows.Count > 0) 
            {
                int islemno =Convert.ToInt32( gridUrunler.CurrentRow.Cells["IslemNo"].Value.ToString());
                if(islemno != 0)
                {
                    fDetayGoster f= new fDetayGoster();
                    f.islemnumarası=islemno;
                    f.Show();
                }
            }
        }

        private void bRaporAl_Click(object sender, EventArgs e)
        {
            Raporlar.Baslik = "GENEL RAPORLAR";
            Raporlar.SatisKart=tSatisKart.Text;
            Raporlar.SatisNakit=tSatisNakit.Text;
            Raporlar.IadeKart=tIadeKart.Text;
            Raporlar.IadeNakit=tIadeNakit.Text;
            Raporlar.GelirKart=tGelirKart.Text;
            Raporlar.GelirNakit = tGelirNakit.Text;
            Raporlar.GiderKart=tGiderKart.Text;
            Raporlar.GiderNakit = tGiderNakit.Text;
            Raporlar.TarihBaslangic=dateBaslangic.Value.ToShortDateString();
            Raporlar.TarihBitis=dateBitis.Value.ToShortDateString();
            Raporlar.KdvToplam = tKdvToplam.Text;
            Raporlar.KartKomisyon=tKartKomisyon.Text;
            Raporlar.RaporSayfasiRaporu(gridUrunler);
        }
    }
}
