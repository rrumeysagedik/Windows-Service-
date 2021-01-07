using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Timers;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace RCServ
{
    partial class RCService : ServiceBase
    {
        public RCService()
        {
            InitializeComponent();
        }

        //[DllImport("plcommpro.dll",EntryPoint ="Connect")]
        //public static extern IntPtr Connect(string genelBaglanti);

        //[DllImport("plcomms.dll")]
        //public static extern void plcomms();

        //[DllImport("plrscagent.dll")]
        //public static extern void plrscagent();

        //[DllImport("plrscomm.dll")]
        //public static extern void plrscomm();

        //[DllImport("pltcpcomm.dll")]
        //public static extern void pltcpcomm();

        //private IntPtr Connect()
        //{
        //    string con = "protocol=TCP/IP,ipaddress = ippppp, port = porrrrrtt, timeout=sürreee, passwd=passs";

        //    IntPtr sonuc = Connect(con);            
        //    return sonuc;
        //}

        //private void VeriCek(IntPtr sonuc)
        //{
        //    sonuc = Connect();
        //    if (sonuc != IntPtr.Zero)
        //    {
        //        //veriyi çek....
        //    }
        //    else
        //    {
        //        DosyayaYaz("Bağlantı olmadığı için veri çekilemedi!!!");
        //    }
        //}

        Timer tmr = new Timer();
        protected override void OnStart(string[] args)
        {

            tmr.Interval = 5000;
            tmr.Elapsed += new ElapsedEventHandler(tmr_Elapsed);
            tmr.Start();
            // TODO: Add code here to start your service.
            DosyayaYaz("BASLADI");
        }
        
        void tmr_Elapsed(object sender, ElapsedEventArgs e)
        {
          //  if (DateTime.Now.Hour < 18 && DateTime.Now.Hour >7)
            {
                if ((int)DateTime.Now.Date.DayOfWeek < 6)
                {
                    try
                    {
                        foreach (Personel p in Personel.PersonelGetir())
                        {
                            if (Personel.MailKontrol(p.PersonelKod))
                            {
                                if (!PersonelBigi.GecisKontrol(p.PersonelKod))
                                {
                                    if (!Tatiller.TatilKontrol())
                                    {
                                        MailGonder(p.Email, "Uyarı mesajıdır");
                                        Personel.MailBilgi(p.PersonelKod);
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex) { DosyayaYaz("hata : " + ex.Message.ToString()); }
                }
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }


        public void DosyayaYaz(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        


        public static bool MailGonder(string _mail,string _mesaj)
        {
            bool dondur = false;
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("yaztest0101@gmail.com");
                mail.To.Add(_mail);
                mail.Subject = "Mail Uyarı";
                mail.Body = _mesaj;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("yaztest0101@gmail.com", "19441944");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                dondur = true;
            }
            catch (Exception ex)
            {

                dondur = false;
            }

            return dondur;
        }
    }

    public class Personel
    {
        public int pID { get; set; }
        public string PersonelAdSoyad { get; set; }
        public string PersonelKod { get; set; }
        public string Email { get; set; }



        public static List<Personel> PersonelGetir()
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
            SqlCommand cmd = new SqlCommand("SELECT * FROM Personel", con);

            List<Personel> Liste = new List<Personel>();
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Liste.Add(new Personel { PersonelAdSoyad = dr[1].ToString(), PersonelKod = dr[2].ToString(), pID = Convert.ToInt32(dr[0]),Email = dr["Email"].ToString() });
            }
            con.Close();
            return Liste;
        }

        public static void MailBilgi(string _kulKod)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
            SqlCommand cmd = new SqlCommand("INSERT INTO MailBilgi (Gonderildi,PersonelKod,Tarih) VALUES (@p1,@p2,@p3)", con);
            cmd.Parameters.AddWithValue("@p1", 1);
            cmd.Parameters.AddWithValue("@p2", _kulKod);
            cmd.Parameters.AddWithValue("@p3", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }
        public static bool MailKontrol(string _kulKod)
        {
            bool Gonderildi = false;
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM MailBilgi WHERE PersonelKod = @p1 AND MONTH(Tarih) =@p2 and DAY(Tarih)=@p3 and YEAR(Tarih) = @p4", con);
            cmd.Parameters.AddWithValue("@p1", _kulKod);
            cmd.Parameters.AddWithValue("@p2", DateTime.Now.Month);
            cmd.Parameters.AddWithValue("@p3", DateTime.Now.Day);
            cmd.Parameters.AddWithValue("@p4", DateTime.Now.Year);
            con.Open();
            if (int.Parse(cmd.ExecuteScalar().ToString())== 0)
                Gonderildi = true;
            con.Close();
            return Gonderildi;
        }
    }

    public class Izin
    {
        public int kID { get; set; }
        public string PersonelKod { get; set; }
        public DateTime BasTar { get; set; }
        public DateTime BitTar { get; set; }

        public static List<Izin> IzinGetir()
        {
            SqlConnection con;
            SqlCommand cmd;
            con = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
            string sql = "SELECT * FROM PersonelIzın";
            List<Izin> Liste = new List<Izin>();
            con.Open();
            cmd = new SqlCommand(sql, con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                int b;
                string c;
                DateTime e, f;
                b = Convert.ToInt32(dr[0]);
                c = Convert.ToString(dr[1]);
                e = Convert.ToDateTime(dr[2]);
                f = Convert.ToDateTime(dr[3]);
                Liste.Add(new Izin { kID = b, PersonelKod = c, BasTar = e, BitTar = f });
                //System.Windows.Forms.MessageBox.Show("Test");
            }



          
            return Liste;
        }
    }

    public class PersonelBigi
    {
        public int bID { get; set; }
        public DateTime GecisTarih { get; set; }
        public string PersonelKod { get; set; }

        public static List<PersonelBigi> PersonelBigiGetir()
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PersonelBilgi", con);

            List<PersonelBigi> Liste = new List<PersonelBigi>();
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Liste.Add(new PersonelBigi { GecisTarih = DateTime.Parse(dr["GecisTarih"].ToString()), PersonelKod = dr["PersonelKod"].ToString(), bID = Convert.ToInt32(dr["bId"]) });
            }
            con.Close();
            return Liste;
        }


        public static bool GecisKontrol(string _kulKod)
        {
            bool Gonderildi = false;
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PersonelBilgi WHERE PersonelKod = @p1 AND MONTH(GecisTarih) =@p2 and DAY(GecisTarih)=@p3 and YEAR(GecisTarih) = @p4", con);
            cmd.Parameters.AddWithValue("@p1", _kulKod);
            cmd.Parameters.AddWithValue("@p2", DateTime.Now.Month);
            cmd.Parameters.AddWithValue("@p3", DateTime.Now.Day);
            cmd.Parameters.AddWithValue("@p4", DateTime.Now.Year);
            con.Open();
            if (int.Parse(cmd.ExecuteScalar().ToString()) > 0)
                Gonderildi = true;
            con.Close();
            return Gonderildi;
        }
    }

    public class Tatiller
    {
        public DateTime BaşlangıçTarihi { get; set; }
        public DateTime BitişTarihi { get; set; }
        public string TatilAdi { get; set; }

        public static List<Tatiller> TatilBigiGetir()
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
            SqlCommand cmd = new SqlCommand("SELECT * FROM Tatiller", con);

            List<Tatiller> Liste = new List<Tatiller>();
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Liste.Add(new Tatiller { BaşlangıçTarihi = DateTime.Parse(dr["BaşlangıçTarihi"].ToString()), TatilAdi = dr["TatilAdı"].ToString(), BitişTarihi = DateTime.Parse(dr["BaşlangıçTarihi"].ToString()), });
            }
            con.Close();
            return Liste;
        }


        public static bool TatilKontrol()
        {
            bool Gonderildi = false;
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Tatiller WHERE @p1 >= BaşlangıçTarihi AND @p1 < BitişTarihi", con);
            
            cmd.Parameters.AddWithValue("@p1", DateTime.Now.Date);
            con.Open();
            if (int.Parse(cmd.ExecuteScalar().ToString()) > 0)
                Gonderildi = true;
            con.Close();
            return Gonderildi;
        }
    }

}
