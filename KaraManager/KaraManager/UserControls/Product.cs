using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaraManager
{
    public partial class Product : UserControl
    {
        // Status
        private dynamic prod;
        private Room parent;
        private int evenId;
        private int count;

        public Product(Room parent, dynamic prodIn, int id, int c = 0)
        {
            InitializeComponent();
            prod = prodIn;
            evenId = id;
            count = c;
            this.parent = parent;
            binData();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void binData()
        {
            label1.Text = (string)prod.name;
            label2.Text = "Còn " + (string)prod.count + " cái";
            textBox1.Text = count.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            count++;
            textBox1.Text = count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            count--;
            textBox1.Text = count.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }
            else
            {
                count = int.Parse(textBox1.Text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            parent.UpdateEven((int)evenId, (int)prod.id, count);
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            parent.UpdateEven((int)evenId, (int)prod.id, 0);
            this.Hide();
        }

        private void Product_Load(object sender, EventArgs e)
        {

        }
    }
}
