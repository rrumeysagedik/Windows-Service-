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
    public partial class Personelizin : Form
    {
        public Personelizin()
        {
            InitializeComponent();
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");

        public void izingoster(string izin)
        {
            SqlDataAdapter da = new SqlDataAdapter(izin , baglantı);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void Personelizin_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            izingoster("SELECT * FROM PersonelIzın");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            baglantı.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO PersonelIzın(PersonelKod,BasTar,BitTar) VALUES(@prsnlkod,@bastar,@bittar)", baglantı);
            cmd.Parameters.AddWithValue("@prsnlkod", textBox1.Text);
            cmd.Parameters.AddWithValue("@bastar", textBox2.Text);
            cmd.Parameters.AddWithValue("@bittar", textBox3.Text);
            cmd.ExecuteNonQuery();
            izingoster("SELECT * FROM PersonelIzın ");
            baglantı.Close();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Satırı Silmek İstediğinize Emin misiniz? ", "Satır Silindi.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try {
                    baglantı.Open();
                    SqlCommand command = new SqlCommand("DELETE FROM PersonelIzın  WHERE  PersonelKod= '" + textBox1.Text + "'", baglantı);
                    command.ExecuteNonQuery();
                    baglantı.Close();
                    izingoster("SELECT * FROM PersonelIzın");
                }
                catch (Exception exc) {
                    MessageBox.Show("Hata! .", exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Satır Silinmedi.", "Satırı sil.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string PersonelKod = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
            string BasTar = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();
            string BitTar = dataGridView1.Rows[secilialan].Cells[3].Value.ToString();

            textBox1.Text = PersonelKod;
            //textBox6.Text = TatilAdi;
            textBox2.Text = BasTar;
            textBox3.Text = BitTar;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            string t = textBox2.Text;
            DateTime d = Convert.ToDateTime(textBox2.Text);
            t = d.ToString("yyyy-MM-dd HH:mm:ss");

            DateTime d2 = Convert.ToDateTime(textBox3.Text);
            string t2 = d2.ToString("yyyy-MM-dd HH:mm:ss");

            SqlCommand cmd = new SqlCommand("Update PersonelIzın SET PersonelKod='" + textBox5.Text + "', BasTar='" + t + "',BitTar='" + t2 + "'WHERE PersonelKod= '" + textBox1.Text + "' ", baglantı);

            cmd.ExecuteNonQuery();
            izingoster("SELECT * FROM PersonelIzın");
            baglantı.Close();
            textBox5.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Menüler mn = new Menüler();
            mn.Show();
            this.Hide();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar !=8)
            {
                e.Handled = true;
            }
        }
    }
}
