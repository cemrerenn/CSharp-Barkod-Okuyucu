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
    public partial class fDetayGoster : Form
    {
        public fDetayGoster()
        {
            InitializeComponent();
        }

        public int islemnumarası { get; set; }
        private void fDetayGoster_Load(object sender, EventArgs e)
        {
            lIslemNo.Text = "İşlem No : "+islemnumarası ;
            using (var db = new BarkodDBEntities())
            {
                gridUrunler.DataSource = db.Satis.Select(x=> new {x.IslemNo,x.UrunAd,x.UrunGrup,x.Miktar,x.Toplam}).Where(x => x.IslemNo == islemnumarası).ToList();
                Islemler.GridDuzenle(gridUrunler);
            }
        }
    }
}
