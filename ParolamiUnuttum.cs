using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Net.Mail;
using System.Net;

namespace HastaneKayit
{
    public partial class ParolamiUnuttum : Form
    {               

        public ParolamiUnuttum()
        {
            InitializeComponent();
        }

     
        Form1 frm1 = new Form1();

        private void btnGonder_Click(object sender, EventArgs e)
        {

            try
            {
                frm1.uyelerBaglantisi.Open();
                OleDbCommand cmd = new OleDbCommand("Select * From Uyeler where TC='" + maskedtxtTC.Text + "'", frm1.uyelerBaglantisi);
                OleDbDataReader dr = cmd.ExecuteReader();
                bool parola_Verildimi = false;
                while (dr.Read())
                {
                    try
                    {
                        SmtpClient smtpserver = new SmtpClient();
                        MailMessage mail = new MailMessage();
                        String mailadresin = ("hospitalkar@gmail.com");
                        String mailsifren = ("adanali011"); // sifre
                        String smptsrvr = "smtp.gmail.com"; // dokunma
                        String kime = (dr["Eposta"].ToString());

                        String konu = ("Parola Hatırlatma"); //konu

                        String yazi = ("Yasal Uyari Bu mesaj ve ekleri, mesajda gönderildiği belirtilen kişi / kişilere özeldir ve gizlidir.Bu mesajın size yanlışlıkla ulaştığını düşünüyorsanız lütfen gönderen kişiyi bilgilendiriniz, mesajın hiçbir ekini açmayınız ve mesajı sisteminizden siliniz.KAR Sağlık Hizmetleri A.Ş bu mesajın ve eklerinin içeriği ile ilgili olarak herhangi bir hukuki sorumluluğu bulunmamaktadır. Gönderen kişi/ kişiler hata veya unutmalardan sorumluluk kabul etmez. Mesaj içeriğinin gizliliği ve bu gizlilik yükümlülüğüne uyulması zorunluluğu tarafınız için de söz konusudur. İnternet iletişiminde zamanında, güvenli, hatasız ya da virüssüz gönderim garanti edilemez.Şirketimiz mesajın ve bilgilerinin size değişikliğe uğrayarak veya geç ulaşmasından, bütünlüğünün ve gizliliğinin korunamamasından, virüs içermesinden ve bilgisayar sisteminize verebileceği herhangi bir zarardan sorumlu tutulamaz. " + "\nSayın " + dr["Adı"].ToString() + " " + dr["Soyadı"].ToString() + "\nParolanız = " + dr["Parola"].ToString());  // metin


                        smtpserver.Credentials = new NetworkCredential(mailadresin, mailsifren);
                        smtpserver.Port = 587;
                        smtpserver.Host = smptsrvr;
                        smtpserver.EnableSsl = true;
                        mail.From = new MailAddress(mailadresin);
                        mail.To.Add(kime);
                        mail.Subject = konu;
                        mail.Body = yazi;
                        smtpserver.Send(mail);
                        MessageBox.Show("Mailinize gelen Şifreyle Hesabınıza giriş yapınız.\nGüvenliğiniz İçin Ardından Lütfen şifrenizi değiştiriniz.");
                    }
                    catch (Exception hata)
                    {
                        MessageBox.Show(hata.Message);
                    }
                    parola_Verildimi = true;

                    if (parola_Verildimi)
                    {
                        this.Close();
                    }
                }
                if (parola_Verildimi == false)
                {
                    MessageBox.Show("Hatalı Bilgiler");
                }
                frm1.uyelerBaglantisi.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
                this.Close();
            }
        }

        private void maskedtxtTC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnGonder_Click(sender, e);
            }
        }

        private void ParolamiUnuttum_Load(object sender, EventArgs e)
        {
            maskedtxtTC.Text = "1";
            maskedtxtTC.Text = "";
            maskedtxtTC.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }   

        private void ParolamiUnuttum_FormClosed(object sender, FormClosedEventArgs e)
        {
            frm1.uyelerBaglantisi.Close();
        }

        private void maskedtxtTC_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
