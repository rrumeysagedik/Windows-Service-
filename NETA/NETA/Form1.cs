using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace NETA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglantı  = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                baglantı.Open();
                string sql = "SELECT * FROM Giris WHERE KullaniciAd=@kullaniciadi AND Sifre=@sifresi";
                SqlParameter prm1 = new SqlParameter("kullaniciadi", textBox1.Text);
                SqlParameter prm2 = new SqlParameter("sifresi", textBox2.Text);
                SqlCommand cmd = new SqlCommand(sql, baglantı);
                cmd.Parameters.Add(prm1);
                cmd.Parameters.Add(prm2);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if(dt.Rows.Count > 0)
                {
                    Form menüler = new Menüler();
                    menüler.Show();
                    this.Hide();
                }
            }
            catch (Exception exception)
            {

                MessageBox.Show("Hatalı Kullanıcı Adı veya Girişi " + exception.Message);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SifreGüncellenecek sfr = new SifreGüncellenecek();
            sfr.Show();
        }
    }
}
