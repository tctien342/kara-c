using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaraManager
{
    public partial class Login : Form
    {
        private APIManager api;
        private MainForm dad;
        public Login(MainForm form)
        {
            InitializeComponent();
            dad = form;
            api = form.api;
            textBox1.Text = "staff@gmail.com";
            textBox2.Text = "123456";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "") {
                label4.Text = "Hãy điền đủ thông tin !!!";
            }
            else{
                label4.ForeColor = Color.BlueViolet;
                label4.Text = "Đang đăng nhập...";
            }
            int state = await api.InitClass(textBox1.Text, textBox2.Text);
            if (state == -1)
            {
                label4.ForeColor = Color.Red;
                label4.Text = "Sai thông tin đăng nhập !!!";
            }
            else
            {
                this.Hide();
                this.dad.InitForm();
            }
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.dad.Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
