using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace HastaneKayit
{
    public partial class randevuAra : Form
    {
        string tcNo = "";
        string Parola = "";
        public randevuAra()
        {
            InitializeComponent();
        }
      
        public randevuAra(string tc,string parola)
        {
            InitializeComponent();
            tcNo = tc;
            Parola = parola;
        }

        string klinikid = "";
        Form1 frm1 = new Form1();
        DateTime datee = DateTime.Now;
        
        private void randevuAra_Load(object sender, EventArgs e)
        {
            this.Location = new Point(0, 0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            uyeBilgileriniGoster();
            klinileriGoster();

            foreach (Control asd in panel1.Controls)
            {
                if (asd.Text != "Randevu Saatini Seçin")
                {
                    if (asd.BackColor != Color.Red)
                    {
                        asd.Click += new EventHandler(asd_Click);
                    }
                }
            }
        }

        void uyeBilgileriniGoster()
        {
            frm1.uyelerBaglantisi.Open();
            OleDbCommand cmd = new OleDbCommand("Select * From Uyeler where TC='" + tcNo + "' ", frm1.uyelerBaglantisi);
            cmd.ExecuteNonQuery();
            OleDbDataReader rd = cmd.ExecuteReader();//www.gorselprogramlama.com
            while (rd.Read())
            {
                lblTC.Text = rd["TC"].ToString();
                lblAdSoyad.Text = rd["Adı"].ToString() + " " + rd["Soyadı"].ToString();
                lblDogumYeriTarihi.Text = rd["DogumYeri"].ToString() + " / " + rd["DogumTarihi"].ToString();
               
                lblTel.Text = rd["Ceptel"].ToString();

            }
            frm1.uyelerBaglantisi.Close();
        }

        void tarihDuzenleme()
        {
            int i = 0;
            while (cmBoxRandevTarihi.Items.Count != 15)
            {
                if (datee.AddDays(i).DayOfWeek.ToString() != "Saturday" && datee.AddDays(i).DayOfWeek.ToString() != "Sunday")
                {
                    cmBoxRandevTarihi.Items.Add(datee.AddDays(i).ToShortDateString());
                }
                i++;
            }
        }

        void klinileriGoster()
        {
            frm1.uyelerBaglantisi.Open();
            try
            {
                cmBoxKlinik.Items.Clear();
                OleDbCommand cmd = new OleDbCommand("Select * From Klinik", frm1.uyelerBaglantisi);
                cmd.ExecuteNonQuery();
                OleDbDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    cmBoxKlinik.Items.Add(rd["KlinikAdi"].ToString());
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
            frm1.uyelerBaglantisi.Close();
        }

        void KlinikidsiniBul()
        {
            try
            {
                frm1.uyelerBaglantisi.Open();
                OleDbCommand cmd = new OleDbCommand("Select * From Klinik where KlinikAdi='" + cmBoxKlinik.Text + "'", frm1.uyelerBaglantisi);
                cmd.ExecuteNonQuery();
                OleDbDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    klinikid = dr["Klinikid"].ToString();
                }
                frm1.uyelerBaglantisi.Close();
                doktorlariGoster();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
                frm1.uyelerBaglantisi.Close();
            }
        }

        void doktorlariGoster()
        {
            frm1.uyelerBaglantisi.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand("Select * From Doktorlar where Klinikid=@id", frm1.uyelerBaglantisi);
                cmd.Parameters.AddWithValue("@id", klinikid);

                cmd.ExecuteNonQuery();
                OleDbDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    cmBoxDoktor.Items.Add(rd["DoktorAdiSoyadi"].ToString());
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
            frm1.uyelerBaglantisi.Close();
        }

        void secilenTarihBugunmu()
        {

            foreach (Control  saatlerYesil in panel1.Controls)
            {
                if (saatlerYesil.Text!="Randevu Saatini Seçin")
                {
                    saatlerYesil.BackColor = Color.LawnGreen;
                }
            }

            DateTime date = DateTime.Now;

            if (date.ToShortDateString() == cmBoxRandevTarihi.Text)
            {
               
                int dakika = date.Minute;
                int saat = date.Hour;
                foreach (Control asd in panel1.Controls)
                {
                    if (asd.Text != "Randevu Saatini Seçin")
                    {
                        if (int.Parse(asd.Text.Substring(0, 2)) < saat)
                        {
                            asd.BackColor = Color.Red;
                            asd.Cursor = Cursors.Default;
                        }
                        if (int.Parse(asd.Text.Substring(0, 2)) == saat)
                        {
                            if (int.Parse(asd.Text.Substring(3)) <= dakika)
                            {
                                asd.BackColor = Color.Red;
                                asd.Cursor = Cursors.Default;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Control asd in panel1.Controls)
                {
                    if (asd.Text != "Randevu Saatini Seçin")
                    {
                        asd.BackColor = Color.LawnGreen;
                        asd.Cursor = Cursors.Hand;
                    }
                }
            }
            


           

            try
            {
                frm1.uyelerBaglantisi.Open();

                OleDbCommand cmd = new OleDbCommand("Select * From Randevular where DoktorAdi='" + cmBoxDoktor.Text + "' and Tarih='" + cmBoxRandevTarihi.Text + "'", frm1.uyelerBaglantisi);
                cmd.ExecuteNonQuery();
                OleDbDataReader rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    foreach (Control asd in panel1.Controls)
                    {
                        if (rd["Saat"].ToString() == asd.Text)
                        {
                            asd.BackColor = Color.Red;
                            asd.Cursor = Cursors.Default;
                        }
                    }
                }
               
                frm1.uyelerBaglantisi.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }







        }
        void enbastanForm()
        {
            cmBoxKlinik.Items.Clear();
            cmBoxDoktor.Items.Clear();
            cmBoxRandevTarihi.Items.Clear();
            panel1.Visible = false;
        }
        bool ayniSaatVarmi = false;
        void asd_Click(object sender, EventArgs e)
        {
            secilenTarihBugunmu();
            ayniSaatVarmi = false;
            foreach (Control saatler in panel1.Controls)
            {
                if (saatler.Text == sender.ToString().Substring(34))
                {
                    if (saatler.BackColor != Color.Red)
                    {
                        frm1.uyelerBaglantisi.Open();
                        OleDbCommand cmdAyniSaatteVarmi = new OleDbCommand("Select * From Randevular where Tc='" + lblTC.Text + "' and Tarih='" + cmBoxRandevTarihi.Text + "'", frm1.uyelerBaglantisi);
                        cmdAyniSaatteVarmi.ExecuteNonQuery();
                        OleDbDataReader saatKontrol = cmdAyniSaatteVarmi.ExecuteReader();
                        while (saatKontrol.Read())
                        {
                            if (saatler.Text == saatKontrol["Saat"].ToString())
                            {
                                ayniSaatVarmi = true;
                                MessageBox.Show("Aynı Gün ve Saatte Yalnızca Bir Adet Randevu Alabilirsiniz");
                                frm1.uyelerBaglantisi.Close();
                                break;
                            }

                        }
                        frm1.uyelerBaglantisi.Close();
                    }
                    if (saatler.BackColor != Color.Red)
                    {
                        if (ayniSaatVarmi == false)
                        {
                            frm1.uyelerBaglantisi.Close();
                            DialogResult evetmi;
                            evetmi = MessageBox.Show(cmBoxRandevTarihi.Text + " Tarihinde " + sender.ToString().Substring(34) + " Saatinde " + cmBoxKlinik.Text + " Kliniğinde \n" + cmBoxDoktor.Text + " Tarafından Muayene İçin Randevu Alınıyor ", "GUR HOSPITAL", MessageBoxButtons.YesNo);
                            if (evetmi.ToString() == "Yes")
                            {
                                try
                                {
                                    frm1.uyelerBaglantisi.Open();

                                    OleDbCommand cmd = new OleDbCommand("insert into Randevular (Tc,KlinikAdi,DoktorAdi,Tarih,Saat) values ('" + lblTC.Text + "','" + cmBoxKlinik.Text + "','" + cmBoxDoktor.Text + "','" + cmBoxRandevTarihi.Text + "','" + saatler.Text + "')", frm1.uyelerBaglantisi);
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Randevu İşleminiz Başarıyla Gerçekleşmiştir");
                                    frm1.uyelerBaglantisi.Close();
                                    enbastanForm();
                                    cmBoxKlinik.Select();
                                    klinileriGoster();
                                }
                                catch (Exception hata)
                                {
                                    MessageBox.Show(hata.Message);
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }

        private void cmBoxKlinik_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmBoxDoktor.Select();
            cmBoxDoktor.Items.Clear();
            cmBoxDoktor.Text = "";
            panel1.Visible = false;
            cmBoxRandevTarihi.Items.Clear();
            cmBoxRandevTarihi.Text = "";
            KlinikidsiniBul();    
        }

        private void cmBoxDoktor_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmBoxRandevTarihi.Select();
            cmBoxRandevTarihi.Items.Clear();
            cmBoxRandevTarihi.Text = "";
            panel1.Visible = false;
            tarihDuzenleme();
        }

        private void cmBoxRandevTarihi_SelectedIndexChanged(object sender, EventArgs e)
        {
            asd_Click(sender, e);
           
            panel1.Visible = true;
            secilenTarihBugunmu();
           

        }

        private void randevuAra_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm1.uyelerBaglantisi.Close();
            Application.Exit();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
           
        }

        private void btnHesap_Click(object sender, EventArgs e)
        {
           
        }



             


        int renkA = 0;
        int renkB = 0;
        int renkC = 0;
        bool duzmu = true;
        private void timer2_Tick(object sender, EventArgs e)
        {

            if (duzmu)
            {
                if (renkA < 255)
                {
                    renkA++;
                }
                else if (renkB < 255)
                {
                    renkB++;
                }
                else if (renkC < 255)
                {
                    renkC++;
                }
                else
                {
                    duzmu = false;
                }
            }
            else
            {
                if (renkA > 0)
                {
                    renkA--;
                }
                else if (renkB > 0)
                {
                    renkB--;
                }
                else if (renkC > 0)
                {
                    renkC--;
                }
                else
                {
                    duzmu = true;
                }
            }
        }

       

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            uyeBilgileriniGoster();
        }

        private void btnRandevu_Click(object sender, EventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            enbastanForm();
            klinileriGoster();
            AlinanRandevular aRandevu = new AlinanRandevular(lblTC.Text);
            aRandevu.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            uyeGuncelle guncelle = new uyeGuncelle(lblTC.Text, Parola);
            guncelle.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            frm1.Show();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            asd_Click(sender, e);
            panel1.Visible = true;
            secilenTarihBugunmu();
        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
