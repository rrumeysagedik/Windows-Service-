using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NETA
{
    public partial class Menüler : Form
    {
        public Menüler()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Tatiltakipp tt = new Tatiltakipp();
            tt.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SifreGüncellenecek sfr = new SifreGüncellenecek();
            sfr.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PersonelBilgi prs = new PersonelBilgi();
            prs.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Personelizin pi = new Personelizin();
            pi.Show();
            this.Hide();
        }
    }
}
