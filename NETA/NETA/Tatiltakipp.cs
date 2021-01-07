using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace NETA
{
    public partial class Tatiltakipp : Form
    {
        public Tatiltakipp()
        {
            InitializeComponent();
            VerileriGoster("SELECT * FROM Tatiller");
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");

        public void VerileriGoster(string veriler)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler, baglantı);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

        }

        private void Tatiltakipp_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            VerileriGoster("SELECT * FROM Tatiller");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Tatiller(TatilAdi,BaşlangıçTarihi,BitişTarihi) VALUES(@tatiladi,@baslangictarihi,@bitistarihi)", baglantı);
            cmd.Parameters.AddWithValue("@tatiladi", textBox1.Text);
            cmd.Parameters.AddWithValue("@baslangictarihi", textBox2.Text);
            cmd.Parameters.AddWithValue("@bitistarihi", textBox3.Text);
            cmd.ExecuteNonQuery();
            VerileriGoster("SELECT * FROM Tatiller");
            baglantı.Close();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Satırı Silmek İstediğinize Emin misiniz? ", "Satır Silindi.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                baglantı.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Tatiller  WHERE  TatilAdi= '" + textBox1.Text+ "'", baglantı);
                command.ExecuteNonQuery();
                baglantı.Close();
                VerileriGoster("SELECT * FROM Tatiller");
            }
            else
            {
                MessageBox.Show("Satır Silinmedi.", "Satırı sil.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Tatiller WHERE TatilAdi like '%" + textBox5.Text + "%'", baglantı);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglantı.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string TatilAdi = dataGridView1.Rows[secilialan].Cells[0].Value.ToString();
            string BaşlangıçTarihi = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
            string BitişTarihi = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();

            textBox1.Text = TatilAdi;
            textBox6.Text = TatilAdi;
            textBox2.Text = BaşlangıçTarihi;
            textBox3.Text = BitişTarihi;
        }

       private void button5_Click(object sender, EventArgs e)
        {
           baglantı.Open();
            string x = textBox2.Text;
            DateTime y = Convert.ToDateTime(textBox2.Text);
            x=y.ToString("yyyy-MM-dd HH:mm:ss");
         
            DateTime y2 = Convert.ToDateTime(textBox3.Text);
            string x2= y2.ToString("yyyy-MM-dd HH:mm:ss");
            SqlCommand cmd = new SqlCommand("Update Tatiller SET Tatiladi='"+textBox6.Text+"', BaşlangıçTarihi='" + x + "',BitişTarihi='" +x2 + "' WHERE TatilAdi= '" + textBox1.Text + "' ", baglantı);
             
            cmd.ExecuteNonQuery();
           VerileriGoster("SELECT * FROM Tatiller");
            baglantı.Close();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Menüler mn = new Menüler();
            mn.Show();
            this.Hide();
        }
    }
}
