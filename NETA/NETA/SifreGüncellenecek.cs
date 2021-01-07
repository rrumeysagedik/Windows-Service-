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
    public partial class SifreGüncellenecek : Form
    {
        public SifreGüncellenecek()
        {
            InitializeComponent();
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
        private void Veriler()
        {
            baglantı.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Giris ", baglantı);
            SqlDataReader oku = cmd.ExecuteReader();
            while (oku.Read())
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = oku["kID"].ToString();
                ekle.SubItems.Add(oku["KullaniciAd"].ToString());
                ekle.SubItems.Add(oku["Sifre"].ToString());
                listView1.Items.Add(ekle);
            }
            baglantı.Close();
        }
        int kID = 0;
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand cmd = new SqlCommand("UPDATE Giris SET KullaniciAd='" + textBox1.Text.ToString() + "', Sifre='" + textBox2.Text.ToString() + "' WHERE kID=" + kID + "", baglantı);
            cmd.ExecuteNonQuery();
            baglantı.Close();
            Veriler();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            kID = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[2].Text;
           
        }

        private void SifreGüncellenecek_Load(object sender, EventArgs e)
        {
            Veriler();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Menüler mn = new Menüler();
            mn.Show();
            this.Hide();
        }
    }
    
    
}
