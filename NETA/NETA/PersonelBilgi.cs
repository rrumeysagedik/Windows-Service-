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
    public partial class PersonelBilgi : Form
    {
        public PersonelBilgi()
        {
            InitializeComponent();
        }
        SqlConnection baglantı = new SqlConnection("Data Source=DESKTOP-DLHJ0EV; Initial Catalog=Personel;User Id=sa;Password=1qa2WS3ed");
        public void PersonelGoster(string personel)
        {
            SqlDataAdapter da = new SqlDataAdapter(personel, baglantı);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void PersonelBilgi_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PersonelGoster("SELECT * FROM Personel");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Personel(PersonelAdSoyad,PersonelKod,Email) VALUES(@personeladi,@personelkod,@personelmail)", baglantı);
            cmd.Parameters.AddWithValue("@personeladi", textBox1.Text);
            cmd.Parameters.AddWithValue("@personelkod", textBox2.Text);
            cmd.Parameters.AddWithValue("@personelmail", textBox3.Text);
            cmd.ExecuteNonQuery();
            PersonelGoster("SELECT * FROM Personel");
            baglantı.Close();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            PersonelGoster("SELECT * FROM Personel");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Satırı Silmek İstediğinize Emin misiniz? ","Satır Silindi.", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
            {
                baglantı.Open();
                SqlCommand command = new SqlCommand("DELETE FROM Personel  WHERE  pID= '"+txtPID.Text+"'", baglantı);
                command.ExecuteNonQuery();
                baglantı.Close();
                PersonelGoster("SELECT * FROM Personel");
            }
            else
            {
                MessageBox.Show("Satır Silinmedi.", "Satırı sil.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Personel WHERE PersonelKod like '%" + textBox5.Text + "%'", baglantı);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            baglantı.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilialan = dataGridView1.SelectedCells[0].RowIndex;
            string PersonelAdSoyad = dataGridView1.Rows[secilialan].Cells[1].Value.ToString();
            string PersonelKod = dataGridView1.Rows[secilialan].Cells[2].Value.ToString();
            string Email = dataGridView1.Rows[secilialan].Cells[3].Value.ToString();
            string pid = dataGridView1.Rows[secilialan].Cells[0].Value.ToString();

            textBox1.Text = PersonelAdSoyad;
            textBox2.Text = PersonelKod;
            textBox3.Text = Email;
            txtPID.Text = pid;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            baglantı.Open();
            SqlCommand cmd = new SqlCommand(" UPDATE Personel SET PersonelKod ='" + textBox2.Text + "',Email='" + textBox3.Text + "',PersonelAdSoyad='" + textBox1.Text + "' WHERE pID = "+txtPID.Text+"", baglantı);
            cmd.ExecuteNonQuery();
            PersonelGoster("SELECT * FROM Personel");
            baglantı.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

            Menüler mn = new Menüler();
            mn.Show();
            this.Hide();
        }

        private void txtPID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
