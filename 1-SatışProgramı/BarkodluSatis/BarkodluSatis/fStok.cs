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
    public partial class fStok : Form
    {
        public fStok()
        {
            InitializeComponent();
        }

        private void bAra_Click(object sender, EventArgs e)
        {
            gridListe.DataSource = null;
            using (var db = new BarkodDBEntities())
            {
                if(cmnIslemTuru.Text!="")
                {
                    string urungrubu = cmbUrunGrubu.Text;
                    if(cmnIslemTuru.SelectedIndex==0)
                    {
                        if(rdTumu.Checked) 
                        {
                            
                            db.Urun.OrderBy(x => x.Miktar).Load();
                            gridListe.DataSource= db.Urun.Local.ToBindingList();
                        }
                        else if(rdUrunGrubu.Checked) 
                        {
                            db.Urun.Where(x=>x.UrunGrup==urungrubu).OrderBy(x=>x.Miktar).Load();
                            gridListe.DataSource=db.Urun.Local.ToBindingList();
                        }
                        else
                        {
                            MessageBox.Show("Lütfen Ürün Grubunu Seçiniz.");
                        }

                    }
                    else if(cmnIslemTuru.SelectedIndex==1)
                    {
                        DateTime baslangic=DateTime.Parse(dateBaslangic.Value.ToShortDateString());
                        DateTime bitis = DateTime.Parse(dateBitis.Value.ToShortDateString());
                        bitis=bitis.AddDays(1);
                        if(rdTumu.Checked)
                        {
                            db.StokHareket.OrderByDescending(x => x.Tarih).Where(x => x.Tarih >= baslangic && x.Tarih <= bitis).Load();
                            gridListe.DataSource=db.StokHareket.Local.ToBindingList();
                        }
                        else if(rdUrunGrubu.Checked)
                        {
                            db.StokHareket.OrderByDescending(x => x.Tarih).Where(x => x.Tarih >= baslangic && x.Tarih <= bitis&&x.UrunGrup.Contains(urungrubu)).Load();
                            gridListe.DataSource = db.StokHareket.Local.ToBindingList();
                            
                        }
                        else
                        {
                            MessageBox.Show("Lütfen Filtreleme Türünü Seçiniz.");
                        }
                    }
                   
                }
                else
                {
                    MessageBox.Show("Lütfen İşlem Türünü Seçiniz.");
                }
            }
            Islemler.GridDuzenle(gridListe);
        }

        BarkodDBEntities dbx= new BarkodDBEntities();
        private void fStok_Load(object sender, EventArgs e)
        {
            cmbUrunGrubu.DisplayMember = "UrunGrupAd";
            cmbUrunGrubu.ValueMember = "Id";
            cmbUrunGrubu.DataSource = dbx.UrunGrup.ToList();
        }

        private void tUrunAra_TextChanged(object sender, EventArgs e)
        {
            if(tUrunAra.Text.Length > 0) 
            {
                string urunad =tUrunAra.Text;
                using (var db= new BarkodDBEntities()) 
                {
                    if(cmnIslemTuru.SelectedIndex == 0) 
                    {
                        db.Urun.Where(x=>x.UrunAd.Contains(urunad)).Load();
                        gridListe.DataSource=db.Urun.Local.ToBindingList();
                    }
                    else if (cmnIslemTuru.SelectedIndex==1)
                    {
                        db.StokHareket.Where(x => x.UrunAd.Contains(urunad)).Load();
                        gridListe.DataSource=db.StokHareket.Local.ToBindingList();
                    }
                }
            }
            Islemler.GridDuzenle(gridListe);
        }

        private void bRaporAl_Click(object sender, EventArgs e)
        {
            if (cmnIslemTuru.SelectedIndex==0)
            {
                Raporlar.Baslik = cmnIslemTuru.Text + " Raporu ";
                Raporlar.TarihBaslangic = dateBaslangic.Value.ToShortDateString();
                Raporlar.TarihBitis = dateBitis.Value.ToShortDateString();
                Raporlar.StokRaporu(gridListe);
            }
            if (cmnIslemTuru.SelectedIndex == 1)
            {
                Raporlar.Baslik = cmnIslemTuru.Text + " Raporu ";
                Raporlar.TarihBaslangic = dateBaslangic.Value.ToShortDateString();
                Raporlar.TarihBitis = dateBitis.Value.ToShortDateString();
                Raporlar.StokIzleme(gridListe);
            }
        }
    }
}
