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
namespace PianoTitles
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\scor.mdf;Integrated Security=True;Connect Timeout=30";
        DataTable dt = new DataTable(); int rr = 5; int p = 0; int alt = 50; int alt2 = 10;
        Image wrong = Properties.Resources.wrong;
        int corect, gresit = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\Geo\source\repos\PianoTitles\PianoTitles\Teardrop1.wav");
            player.Play();

            dt.Columns.Add(new DataColumn("1", typeof(string)));
            dt.Columns.Add(new DataColumn("2", typeof(string)));
            dt.Columns.Add(new DataColumn("3", typeof(string)));
            dt.Columns.Add(new DataColumn("4", typeof(string)));

            dataGridView1.DataSource = dt;
            dataGridView1.ClearSelection();

            dt.Rows.Add("", "", "", ""); dt.Rows.Add("", "", "", ""); dt.Rows.Add("", "", "", ""); dt.Rows.Add("", "", "", ""); dt.Rows.Add("", "", "", "");
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            { c.Width = 71; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dt.Rows.Add("", "", "", "");
            int rand = Convert.ToInt32(dt.Rows.Count - 1);
            formatcell(rand);
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            { c.Width = 71; }
            if (rr >= 5)
            {
                verificare(0);
                dataGridView1.Rows.RemoveAt(0);


            }
            rr++;

            if (corect == 10 + p)
            {
                if (timer1.Interval > 100)
                {
                    p = p + 10;
                    timer1.Interval = timer1.Interval - alt;
                }
                else
                { p = p + 10; timer1.Interval = timer1.Interval - alt2; }
            }
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            if (label2.Text == "10")
            {
                timer1.Stop();
                if (textBox1.Text == "")
                    textBox1.Text = "Anonymus";
                SqlCommand cmd;
                cmd = new SqlCommand("insert into scoruri(nume,scor) VALUES (@nm, @sc)", con);
                cmd.Parameters.AddWithValue("nm", textBox1.Text);
                cmd.Parameters.AddWithValue("sc", Convert.ToInt32(label1.Text));

                cmd.ExecuteNonQuery();
                DialogResult d = MessageBox.Show("Ai pierdut!", "Incepi un joc nou?", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    label1.Text = "0";
                    label2.Text = "0";
                    dt.Clear(); rr = 5; p = 0; alt = 50; alt2 = 10; gresit = 0; corect = 0;
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\Users\Geo\source\repos\PianoTitles\PianoTitles\Teardrop1.wav");
                    player.Play();

                    dataGridView1.DataSource = dt;
                    dataGridView1.ClearSelection();
                    timer1.Start();

                    dt.Rows.Add("", "", "", ""); dt.Rows.Add("", "", "", ""); dt.Rows.Add("", "", "", ""); dt.Rows.Add("", "", "", ""); dt.Rows.Add("", "", "", "");
                    foreach (DataGridViewColumn c in dataGridView1.Columns)
                    { c.Width = 71; }

                }
                else if (d == DialogResult.No)
                {
                    Application.Exit();
                }
            }
        }
        private void formatcell(int rand)
        {
            Random rnd = new Random();
            int cell = rnd.Next(0, 4);
            dataGridView1.Rows[rand].Cells[cell].Style.BackColor = Color.Black;
        }
        private void verificare(int rand)
        {
            for (int i = 0; i < 4; i++)
            {
                if (dataGridView1.Rows[rand].Cells[i].Style.BackColor == Color.Black)
                {
                    gresit++;
                    label2.Text = gresit.ToString();
                }
            }
        }
        private void formatcellalb(int rand, int col)
        {

            dataGridView1.Rows[rand].Cells[col].Style.BackColor = Color.White;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.Black)
            {
                formatcellalb(e.RowIndex, e.ColumnIndex);
                corect++;
                label1.Text = corect.ToString();
            }
            else
            {
                formatcellwrong(e.RowIndex, e.ColumnIndex);
                gresit++;
                label2.Text = gresit.ToString();

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            button1.Enabled = false;
        }

        private void formatcellwrong(int rand, int col)
        {

            dataGridView1.Rows[rand].Cells[col].Style.BackColor = Color.Red;

        }
    }
}
