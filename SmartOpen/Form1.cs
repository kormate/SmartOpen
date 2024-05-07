using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SmartOpen.DataClass;


namespace SmartOpen
{
    public partial class Form1 : Form
    {
        private DataClass dataClass;
        private List<DataClass.Dolgozo> dolgozok;
        public Form1()
        {

            InitializeComponent();
            

            dataClass = new DataClass();
 
            LoadEmps();

            TimeNow();

            dataGridView1.CellClick += dataGridView1_CellClick;

            buttonReg.Enabled = false;

            textBoxVez.TextChanged += TextBoxes_TextChanged;
            textBoxKere.TextChanged += TextBoxes_TextChanged;
            textBoxEmail.TextChanged += TextBoxes_TextChanged;
            textBoxBeosztas.TextChanged += TextBoxes_TextChanged;
            textBoxJelszo.TextChanged += TextBoxes_TextChanged;

            
        }

        private void TimeNow()
        {
            DateTime now = DateTime.Now;

            string labelText = now.ToString("yyyy.MM.dd.");

            labelDatum.Text = labelText;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                labelNev.Text = row.Cells["fname"].Value.ToString();
                labelMunkakor.Text = row.Cells["fmunkakor"].Value.ToString();
                labelStatusz.Text = row.Cells["fstatusz"].Value.ToString();
            }
        }

        private void LoadEmps()
        {

            dolgozok = dataClass.GetDataFromDataBase();

            
            dataGridView1.DataSource = dolgozok;
            dataGridView1.Columns["fpw"].Visible = false;
            dataGridView1.Columns["fsalt"].Visible = false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void regisztrációToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBoxKereses_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void buttonKeres_Click(object sender, EventArgs e)
        {
            string keresettNev = textBoxKereses.Text.Trim();
            List<SmartOpen.DataClass.Dolgozo> keresettDolgozok = dolgozok.Where(dolgozo => dolgozo.fname.ToLower().Contains(keresettNev.ToLower())).ToList();
            if (keresettDolgozok.Count > 0)
            {
                SmartOpen.DataClass.Dolgozo talaltDolgozo = keresettDolgozok[0];
                labelNev.Text = talaltDolgozo.fname;
                labelMunkakor.Text = talaltDolgozo.fmunkakor;
                labelStatusz.Text = talaltDolgozo.fstatusz;



                dataGridView1.DataSource = null;
                dataGridView1.DataSource = keresettDolgozok;
                dataGridView1.Columns["fpw"].Visible = false;
                dataGridView1.Columns["fsalt"].Visible = false;
            }
            else
            {

                LoadEmps();
            }


        }

        private void buttonTorles_Click(object sender, EventArgs e)
        {
            labelNev.Text = "";
            labelMunkakor.Text = "";
            labelStatusz.Text = "";
            textBoxKereses.Clear();
            LoadEmps();
        }

        private void TextBoxes_TextChanged(object sender, EventArgs e)
        {
            bool MindenKitoltve = !string.IsNullOrWhiteSpace(textBoxVez.Text) &&
                                  !string.IsNullOrWhiteSpace(textBoxKere.Text) &&
                                  !string.IsNullOrWhiteSpace(textBoxEmail.Text) &&
                                  !string.IsNullOrWhiteSpace(textBoxBeosztas.Text) &&
                                  !string.IsNullOrWhiteSpace(textBoxJelszo.Text);
            buttonReg.Enabled = MindenKitoltve;
        }
        private void buttonReg_Click(object sender, EventArgs e)
        {
            string salt = PwHasher.GenSalt(16);
            string saltedPassw = PwHasher.HashPw(textBoxJelszo.Text, salt);

            SmartOpen.DataClass.Dolgozo ujDolgozo = new SmartOpen.DataClass.Dolgozo();
            ujDolgozo.fname = textBoxVez.Text +' '+ textBoxKere.Text;
            ujDolgozo.fmail = textBoxEmail.Text;
            ujDolgozo.fmunkakor = textBoxBeosztas.Text;
            ujDolgozo.fpw = saltedPassw;
            ujDolgozo.fsalt = salt;

            dataClass = new DataClass();
            dataClass.AddEmps(ujDolgozo);

            textBoxVez.Clear();
            textBoxKere.Clear();
            textBoxEmail.Clear();
            textBoxBeosztas.Clear();
            textBoxJelszo.Clear();

            LoadEmps();

            MessageBox.Show("Sikeres regisztráció!");

            
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
