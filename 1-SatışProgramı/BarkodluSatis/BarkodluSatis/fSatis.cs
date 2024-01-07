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
    public partial class fSatis : Form
    {
        public fSatis()
        {
            InitializeComponent();
        }
        BarkodDBEntities db = new BarkodDBEntities();
        private void tBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) 
            {
                string barkod = tBarkod.Text.Trim();
                if(barkod.Length <=2 )//barkodun uzunluğu 2 ve küçükse miktar kısmına aktar
                {
                    tMiktar.Text = barkod;
                    tBarkod.Clear();
                    tBarkod.Focus();
                }
                else
                {
                    //--Normal Barkod Etiketli Ürün Başlangıç--
                    if (db.Urun.Any(x => x.Barkod == barkod))//any komutu true-false döner-----  barkoddan okunan değer true ise
                    {
                        var urun =db.Urun.Where(x=>x.Barkod==barkod).FirstOrDefault();//ürün bulunur
                        UrunGetirListeye(urun, barkod, Convert.ToDouble(tMiktar.Text));

                    }
                    //--Normal Barkod Etiketli Ürün Bitiş--

                    //barkodlu ürün yoksa teraziyi kontrol ediyor.

                    //27-00001-1250-09 barkodlu terazi kodu 27,28,29 olur bu teraziden gelen ürün anlamına gelir. 00001-ürün kodudur. 1250--satılan ürün gramı, 09-firma kodu
                    
                    //--teraziden gelen ürün başlangıcı//
                    else//teraziden gelen ürün için
                    {
                        int onek =Convert.ToInt16( barkod.Substring(0, 2));//okunan barkodun ilk 2 sayısı alınır.
                        if(db.Terazi.Any(a=>a.TeraziOnEk==onek))//terazi tablomuzda onek 27,28,29 mu diye kontrol eder. tablomuzda sadece bu 3 değer vardır.
                            //true ise gir değilse girme - any true false döndürür
                        {
                            string teraziurunno = barkod.Substring(2, 5);//burası ürün kodunu alır. bu ürün kodu urunler tablosundaki barkod numarasıdır.ör:00001-Domates
                            if(db.Urun.Any(a=>a.Barkod==teraziurunno))//urun tanlosunda barkod var mı kontrol edilir.
                            {
                                var urunterazi = db.Urun.Where(a => a.Barkod == teraziurunno).FirstOrDefault();//varsa ürünü bul getir.
                                double miktarkg=Convert.ToDouble(barkod.Substring(7, 5))/1000;//barkoddan okunan değeri aldık ve gramı kg çevirdik
                                UrunGetirListeye(urunterazi, teraziurunno, miktarkg);//eklemek için metoda gönderdik
                            }
                            else //----eğer gelen ürün teraziden de değilse ürün ekleme sayfasına yönlendirilir.
                            { 
                                Console.Beep(900, 1000);//çıkan ses ve uzunluğu
                                MessageBox.Show("KG ürün ekleme Sayfası"); 
                            }
                            

                        }
                        else//barkoddan gelen değer 27,28,29 değilse bu ürün yoktur ve ürün ekleme satfasına gider.
                        {
                          
                            fUrunGiris f = new fUrunGiris();
                            f.tBarkod.Text = barkod;
                            f.ShowDialog();
                            
                        }
                    }
                    //--teraziden gelen ürün bitişi//
                }
                gridSatisListesi.ClearSelection();
                GenelToplam();
               
            }
           
        }
        private void UrunGetirListeye(Urun urun,string barkod,double miktar)
        {
            int satirsayisi = gridSatisListesi.Rows.Count;//satırları say
            bool eklemismi = false;
            if (satirsayisi > 0) //datagridde ürün varsa
            {
                for (int i = 0; i < satirsayisi; i++)//datagrid satırsayısı kadat dön
                {
                    //girilen barkod daha önce eklenmiş mi kontrol edilir.Girilmişse
                    if (gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString() == barkod)//true-false
                    {
                        //gridin içindeki miktar ile yeni eklenen miltar toplanır.
                        gridSatisListesi.Rows[i].Cells["Miktar"].Value = miktar + Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Miktar"].Value);
                        //Girilen miktar ile tablodaki fiyat çarpılıp toplam bulunur. Math.round ile yuvarlama işlemi yapılır.
                        gridSatisListesi.Rows[i].Cells["Toplam"].Value = Math.Round(Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Miktar"].Value) * Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Fiyat"].Value), 2);
                        //işlem tamamsa true olur.Böylece bir sonraki döngüye girmemesini sağlanır.
                        eklemismi = true;

                        //if ifadesi true olduğu zaman çalışır.
                    }
                }

            }
            //eğer ürün daha önce eklenmiş ise yukarıdan true gelir ve ! (ünlem) ifadesi ile eklemismi false yapılarak aşağıdaki if ifadesine girmemesi sağlanır.
            //eğer ürün daha önce eklenmemişse burada true olur ve if ifadesi içine girerek yeni satır oluşturur.

            if (!eklemismi) //true -- Daha önceden eklenmemişse gir yeni satır oluştur ve verileri çek.
                            //***!(ÜNLEM)---->mantıksal DEĞİL" operatörüdür. ! operatörü, bir boolean ifadenin değerini tersine çevirir.

            {
                gridSatisListesi.Rows.Add();
                gridSatisListesi.Rows[satirsayisi].Cells["Barkod"].Value = barkod;
                gridSatisListesi.Rows[satirsayisi].Cells["UrunAdi"].Value = urun.UrunAd;
                gridSatisListesi.Rows[satirsayisi].Cells["UrunGrup"].Value = urun.UrunGrup;
                gridSatisListesi.Rows[satirsayisi].Cells["Birim"].Value = urun.Birim;
                gridSatisListesi.Rows[satirsayisi].Cells["Fiyat"].Value = urun.SatisFiyat;
                gridSatisListesi.Rows[satirsayisi].Cells["Miktar"].Value = miktar;
                gridSatisListesi.Rows[satirsayisi].Cells["Toplam"].Value = Math.Round(miktar * (double)urun.SatisFiyat, 2);
                gridSatisListesi.Rows[satirsayisi].Cells["AlisFiyat"].Value = urun.AlisFiyat;
                gridSatisListesi.Rows[satirsayisi].Cells["KdvTutari"].Value = urun.KDVTutari;
            }
        }

        private void GenelToplam()
        {
            
                double toplam = 0;
                for (int i = 0; i < gridSatisListesi.Rows.Count; i++)
                {
                    toplam += Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Toplam"].Value);
                }
                tGenelToplam.Text=toplam.ToString("C2");//c2 tl yazısını çıkarmak için
                 tMiktar.Text = "1";
                tBarkod.Clear();
                tBarkod.Focus();
                
            
        }


        //gridde çarpıya basınca silme işlemi yapıyor.
        private void gridSatisListesi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==9)//gizlediğimiz satırlarda sayılarak kaçıncı hücreye basacağımızı söylüyoruz.
            {
                gridSatisListesi.Rows.Remove(gridSatisListesi.CurrentRow);//seçilen satırı sil
                gridSatisListesi.ClearSelection();//seçimi temizliyoruz
                GenelToplam();
                tBarkod.Focus();
            }
        }

        private void fSatis_Load(object sender, EventArgs e)
        {
            b5.Text=5.ToString("C2");
            b10.Text = 10.ToString("C2");
            b20.Text = 20.ToString("C2");
            b50.Text = 50.ToString("C2");
            b100.Text = 100.ToString("C2");
            b200.Text = 200.ToString("C2");
            HizliButonDoldur();
            using(var db= new BarkodDBEntities())
            {
                var sabit = db.Sabit.FirstOrDefault();
                chYazdirmaDurumu.Checked=Convert.ToBoolean(sabit.Yazici);
            }
        }

        private void HizliButonDoldur()
        {
            var hizliUrun =db.HizliUrun.ToList();
            foreach (var item in hizliUrun)
            {
                Button bt =this.Controls.Find("bH"+item.Id,true).FirstOrDefault() as Button;
                if (bt != null) 
                {
                    double fiyat=Islemler.DoubleYap(item.Fiyat.ToString());
                   bt.Text=item.UrunAd+"\n"+fiyat.ToString("C2");
                }
            }
        }


        //form üzerindeki bütün butonlar seçilir ve click olayından hizlibutonclick seçilir.
        //hızlıbutona tıkladığımız zaman bilgileri object olarak buraya geliyor
        
        private void HizliButonClick(object sender, EventArgs e)
        {

            //gelen sender değerinin buton olduğunu belirtiyoruz
            Button b = (Button)sender;

            //isimden gelen name özelliğininin bh'si silinir ve kalan değer id olarak alınır
            int butonid = Convert.ToInt16(b.Name.ToString().Substring(2, b.Name.Length - 2));

           

            if (b.Text.ToString().StartsWith("-"))
            {
               fHizliButonUrunEkle f= new fHizliButonUrunEkle();
                f.lButonID.Text=butonid.ToString();
                f.ShowDialog();
            }
            else
            {
                
                var urunbarkod = db.HizliUrun.Where(x => x.Id == butonid).Select(x => x.Barkod).FirstOrDefault();
                var urun = db.Urun.Where(x => x.Barkod == urunbarkod).FirstOrDefault();
                UrunGetirListeye(urun, urunbarkod, Convert.ToDouble(tMiktar.Text));
                GenelToplam();
            }
           

        }
        private void bh_MouseDown(object sender, MouseEventArgs e)//mouse sağ tıklanma özelliğinde açılacak
        {
            if(e.Button == MouseButtons.Right) 
            {
                Button b = (Button )sender;
                if(!b.Text.StartsWith("-")) //boş değilse
                {
                    int butonid = Convert.ToInt16(b.Name.ToString().Substring(2, b.Name.Length - 2));
                    ContextMenuStrip s = new ContextMenuStrip();
                    ToolStripMenuItem sil = new ToolStripMenuItem();    
                    sil.Text ="Temizle - Buton No:"+butonid.ToString();
                    sil.Click += sil_click;    
                    s.Items.Add( sil );
                    this.ContextMenuStrip = s;
                }
            }
        }

        private void sil_click(object sender, EventArgs e)//sağ tıklandığında açılan pencereye tıklandığında oluşacak olay
        {
            int butonid = Convert.ToInt16(sender.ToString().Substring(19, sender.ToString().Length - 19));
            var guncelle = db.HizliUrun.Find(butonid);
            guncelle.Barkod = "-";
            guncelle.UrunAd = "-";
            guncelle.Fiyat = 0;
            db.SaveChanges();
            double fiyat = 0;
            Button b = this.Controls.Find("bH" + butonid, true).FirstOrDefault() as Button;
            b.Text = "-" + "\n" +fiyat.ToString("C2");
        }

        private void bNx_Click(object sender, EventArgs e)//numeric tıklandığı zaman
        {
            Button b=(Button)sender;
            if(b.Text==",")
            {
                int virgül = tNumarator.Text.Count(x => x == ',');
                if(virgül<1) 
                {
                    tNumarator.Text += b.Text;

                }
            }
            else if(b.Text=="<")
            {
                if(tNumarator.Text.Length>0)
                {
                    tNumarator.Text=tNumarator.Text.Substring(0,tNumarator.Text.Length-1);

                }
            }
            else
            {
                tNumarator.Text += b.Text;

            }
        }

        private void bAdet_Click(object sender, EventArgs e)
        {
            if(tNumarator.Text!="")
            {
                tMiktar.Text = tNumarator.Text;
                tNumarator.Clear();
                tBarkod.Clear();
                tBarkod.Focus();
            }
        }

        private void bOdenen_Click(object sender, EventArgs e)
        {
            if(tNumarator.Text!="")
            {
                double sonuc = Islemler.DoubleYap(tNumarator.Text) - Islemler.DoubleYap(tGenelToplam.Text);
                tParaUstu.Text = sonuc.ToString("C2");
                tOdenen.Text = Islemler.DoubleYap(tNumarator.Text).ToString("C2");
                tNumarator.Clear();
                tBarkod.Focus();

            }
        }

        private void bBarkod_Click(object sender, EventArgs e)
        {
            if(tNumarator.Text!="")
            {
                if(db.Urun.Any(x=>x.Barkod==tNumarator.Text))
                {
                    var urun=db.Urun.Where(x=>x.Barkod==tNumarator.Text).FirstOrDefault();  
                    UrunGetirListeye(urun, tNumarator.Text,Convert.ToDouble(tMiktar.Text));
                    tNumarator.Clear();
                    GenelToplam();
                    tBarkod.Focus();
                }
                else
                {
                    MessageBox.Show("Ürün Ekleme Sayfasını Aç");
                }
            }
        }

        private void ParaUstuHesapla_Click(object sender, EventArgs e) 
        {
            Button b= (Button)sender;

            double sonuc=Islemler.DoubleYap(b.Text)-Islemler.DoubleYap(tGenelToplam.Text);
            tOdenen.Text=Islemler.DoubleYap(b.Text).ToString("C2");
            tParaUstu.Text = sonuc.ToString("C2");
        }

        private void bDigerUrun_Click(object sender, EventArgs e)
        {
            if(tNumarator.Text!="")
            {
                int satirsayisi =gridSatisListesi.Rows.Count;
                gridSatisListesi.Rows.Add();
                gridSatisListesi.Rows[satirsayisi].Cells["Barkod"].Value = "1111111111116";
                gridSatisListesi.Rows[satirsayisi].Cells["UrunAdi"].Value = "Barkodsuz Ürün";
                gridSatisListesi.Rows[satirsayisi].Cells["UrunGrup"].Value = "Barkodsuz Ürün";
                gridSatisListesi.Rows[satirsayisi].Cells["Birim"].Value = "Adet";
                gridSatisListesi.Rows[satirsayisi].Cells["Miktar"].Value = 1;
                gridSatisListesi.Rows[satirsayisi].Cells["AlisFiyat"].Value = 0;
                gridSatisListesi.Rows[satirsayisi].Cells["Fiyat"].Value = Convert.ToDouble(tNumarator.Text);
                gridSatisListesi.Rows[satirsayisi].Cells["KdvTutari"].Value = 0;
                gridSatisListesi.Rows[satirsayisi].Cells["Toplam"].Value = Convert.ToDouble(tNumarator.Text);
                tNumarator.Text = "";
                GenelToplam();
                tBarkod.Focus();

            }
        }

        private void bIade_Click(object sender, EventArgs e)
        {
            if(chSatisIadeIslemi.Checked)
            {
                chSatisIadeIslemi.Checked = false;
                chSatisIadeIslemi.Text = "Satış Yapılıyor.";
            }
            else
            {
                chSatisIadeIslemi.Checked = true;
                chSatisIadeIslemi.Text = "İade İşlemi";
            }
        }

        private void bTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void Temizle()
        {
            tMiktar.Text = "1";
            tBarkod.Clear();
            tOdenen.Clear();
            tParaUstu.Clear();
            tGenelToplam.Text = 0.ToString("C2");
            chSatisIadeIslemi.Checked = false;
            tNumarator.Clear();
            gridSatisListesi.Rows.Clear();
            tBarkod.Clear();
            tBarkod.Focus();
        }

        public void SatisYap(String OdemeSekli)
        {
            int satirsayisi = gridSatisListesi.Rows.Count;
            bool satisiade = chSatisIadeIslemi.Checked;
            double alisfiyatToplam = 0;

            if(satirsayisi>0)
            {
                int? islemno = db.Islem.First().IslemNo;//int? int türünün null değerini de alabilen bir versiyonudur. 
                Satis satis = new Satis();
                for (int i = 0; i < satirsayisi; i++)
                {
                    satis.IslemNo = islemno;
                    satis.UrunAd = gridSatisListesi.Rows[i].Cells["UrunAdi"].Value.ToString();
                    satis.UrunGrup= gridSatisListesi.Rows[i].Cells["UrunGrup"].Value.ToString();
                    satis.Barkod= gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString();
                    satis.Birim= gridSatisListesi.Rows[i].Cells["Birim"].Value.ToString();
                    satis.AlisFiyat = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["AlisFiyat"].Value.ToString());
                    satis.SatisFiyat = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Fiyat"].Value.ToString());
                    satis.Miktar = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString());
                    satis.Toplam = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Toplam"].Value.ToString());
                    satis.KdvTutari = Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["KdvTutari"].Value.ToString())* Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString());
                    satis.OdemeSekli = OdemeSekli;
                    satis.Iade = satisiade;
                    satis.Tarih=DateTime.Now;
                    satis.Kullanici = lKullanici.Text;
                    db.Satis.Add(satis);
                    db.SaveChanges();
                   
                   
                    if(!satisiade)//satış iade durumuna göre stok düşürüp artırma
                    {
                        Islemler.StokAzalt(gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString(), Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString()));
                    }
                    else
                    {
                        Islemler.StokArtir(gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString(), Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString()));
                    }
                    alisfiyatToplam += Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["AlisFiyat"].Value.ToString())* Islemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString());

                }

                IslemOzet io= new IslemOzet();
                io.IslemNo = islemno;
                io.Iade = satisiade;
                io.AlisFiyatToplam = alisfiyatToplam;
                io.Gelir = false;
                io.Gider = false;
                if(!satisiade)//Satış-iade durumuna göre açıklama doldurulacak 
                {
                    io.Aciklama = OdemeSekli + " Satış";
                }
                else
                {
                    io.Aciklama = "iade işlemi (" + OdemeSekli + ")";
                }
                io.OdemeSekli = OdemeSekli;
                io.Kullanici = lKullanici.Text;
                io.Tarih=DateTime.Now;
                switch(OdemeSekli)
                {
                    case "Nakit": io.Nakit = Islemler.DoubleYap(tGenelToplam.Text);io.Kart = 0;break;
                    case "Kart": io.Nakit = 0; io.Kart = Islemler.DoubleYap(tGenelToplam.Text);break;
                    case "Kart-Nakit": io.Nakit = Islemler.DoubleYap(lNakit.Text);io.Kart=Islemler.DoubleYap(lKart.Text);break;
                }

                db.IslemOzet.Add(io);
                db.SaveChanges();


                var islemnoartir = db.Islem.First();
                islemnoartir.IslemNo += 1;
                db.SaveChanges();
                if(chYazdirmaDurumu.Checked) 
                {
                    Yazdir yazdir = new Yazdir(islemno);
                    yazdir.YazdirmayaBasla();
                }
                
                Temizle();

                
            }

        }

        private void bNakit_Click(object sender, EventArgs e)
        {
            SatisYap("Nakit");
        }

        private void bKart_Click(object sender, EventArgs e)
        {
            SatisYap("Kart");
        }

        private void nNakitKart_Click(object sender, EventArgs e)
        {
            fNakitKart f = new fNakitKart();
            f.ShowDialog();
        }

        private void tBarkod_KeyPress(object sender, KeyPressEventArgs e)//sadece sayı ve backspace çalışır.
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08)
            {
                e.Handled = true;
            }
        }

        private void tMiktar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08)
            {
                e.Handled = true;
            }
        }

        private void fSatis_KeyDown(object sender, KeyEventArgs e)//kısayol tuşları f11,F12
        {
            //öncelikle formun keypreview false olarak ayarladık

            if(e.KeyCode==Keys.F1)
            {
                SatisYap("Nakit");
            }
            if(e.KeyCode==Keys.F2)
            {
                SatisYap("Kart");
            }
            if (e.KeyCode==Keys.F3)
            {
                fNakitKart f = new fNakitKart();
                f.ShowDialog();
            }
          
        }
        
        private void Bekle()
        {
            int satir =gridSatisListesi.Rows.Count;
            int sutun = gridSatisListesi.Columns.Count;
            if(satir > 0) 
            {
                for (int i = 0; i < satir; i++)
                {
                    gridBekle.Rows.Add();
                    for (int j = 0; j < sutun-1; j++)
                    {
                        gridBekle.Rows[i].Cells[j].Value = gridSatisListesi.Rows[i].Cells[j].Value;
                    }
                }
            }

        }

        private void BeklemedenCik()
        {
            int satir = gridBekle.Rows.Count;
            int sutun = gridBekle.Columns.Count;
            if (satir > 0)
            {
                for (int i = 0; i < satir; i++)
                {
                    gridSatisListesi.Rows.Add();
                    for (int j = 0; j < sutun - 1; j++)
                    {
                        gridSatisListesi.Rows[i].Cells[j].Value = gridBekle.Rows[i].Cells[j].Value;
                    }
                }
            }

        }

        private void tIslemBeklet_Click(object sender, EventArgs e)
        {
            if(tIslemBeklet.Text=="İşlem Beklet")
            {
                Bekle();
                tIslemBeklet.BackColor = System.Drawing.Color.OrangeRed;
                tIslemBeklet.Text = "İşlem Bekliyor";
                gridSatisListesi.Rows.Clear();
            }
            else
            {
                BeklemedenCik();
                tIslemBeklet.BackColor = System.Drawing.Color.Linen;
                tIslemBeklet.Text = "İşlem Beklet";
                gridBekle.Rows.Clear();
            }
            
        }

        private void chSatisIadeIslemi_CheckedChanged(object sender, EventArgs e)
        {
            if (chSatisIadeIslemi.Checked) chSatisIadeIslemi.Text = "İade Yapılıyor";
            else chSatisIadeIslemi.Text = "Satış Yapılıyor";
        }
    }
}
