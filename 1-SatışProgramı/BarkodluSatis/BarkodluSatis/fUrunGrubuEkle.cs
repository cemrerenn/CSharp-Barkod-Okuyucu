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
    public partial class fUrunGrubuEkle : Form
    {
        public fUrunGrubuEkle()
        {
            InitializeComponent();
        }
        BarkodDBEntities db = new BarkodDBEntities();
        private void fUrunGrubuEkle_Load(object sender, EventArgs e)
        {
            GrupDoldur();
        }

        private void GrupDoldur()
        {
            ListUrunGrup.DisplayMember = "UrunGrupAd";
            ListUrunGrup.ValueMember = "Id";
            ListUrunGrup.DataSource = db.UrunGrup.OrderBy(a => a.UrunGrupAd).ToList();
        }

        private void bEkle_Click(object sender, EventArgs e)
        {
            if(tUrunGrupAd.Text!="")
            {
                UrunGrup ug = new UrunGrup();   
                ug.UrunGrupAd = tUrunGrupAd.Text;
                db.UrunGrup.Add(ug);
                db.SaveChanges();
                GrupDoldur();
                tUrunGrupAd.Clear();
                MessageBox.Show("Ürün Grubu Eklenmiştir.");
                fUrunGiris f = (fUrunGiris)Application.OpenForms["fUrunGiris"];//mevcutta açık bir form varsa bu şekilde çağrılır.
                if(f != null)
                {
                    f.GrupDoldur();
                }

            }
            else
            {
                MessageBox.Show("Grup Bilgisi Giriniz.");
            }
        }

        private void bSil_Click(object sender, EventArgs e)
        {
            int grupid = Convert.ToInt32(ListUrunGrup.SelectedValue.ToString());
            string grupad = ListUrunGrup.Text;
            DialogResult onay = MessageBox.Show(grupad+" grubunu silmek istiyor musunuz ","Silme İşlemi",MessageBoxButtons.YesNo,MessageBoxIcon.Question);

            if(onay == DialogResult.Yes) 
            {
                var grup= db.UrunGrup.FirstOrDefault(x=>x.Id==grupid);
                db.UrunGrup.Remove(grup);
                db.SaveChanges();
                GrupDoldur();
                tUrunGrupAd.Focus();
                MessageBox.Show(grupad + " Ürün grubu Silindi.");
                fUrunGiris f = (fUrunGiris)Application.OpenForms["fUrunGiris"];
                f.GrupDoldur();
               
            }
        }
    }
}
