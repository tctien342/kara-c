using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Dynamic;

namespace KaraManager.UserControls
{
    public partial class TraCuu : UserControl
    {
        private int billID = -1;
        private APIManager api;

        public TraCuu(string label, APIManager api)
        {
            InitializeComponent();
            this.api = api;
            label1.Text = label;
        }

        private void TraCuu_Load(object sender, EventArgs e)
        {

        }

        // On back button click
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Hide();
        }

        // On find button click
        private async void button1_Click(object sender, EventArgs e)
        {
            // Check if billID is filled
            if (billID == -1)
            {
                MessageBox.Show("Hãy nhập id hóa đơn!!!");
            }
            else
            {
                // Searching...
                label2.ForeColor = Color.BlueViolet;
                label2.Text = "Đang tìm...";
                // Calling search API
                dynamic data = await api.GetBill(billID);
                if (data == null)
                {
                    label2.ForeColor = Color.Red;
                    label2.Text = "Không tìm thấy hóa đơn !!!";
                }
                else
                {
                    if ((int)data.error_code == 0)
                    {
                        // Found BILL => making report to user
                        if (data.data.paid_at == null)
                        {
                            label2.ForeColor = Color.Red;
                            label2.Text = "Chưa thanh toán !!!";
                        }
                        else
                        {
                            dynamic billInfo = BillInfo(data.data);

                            Report r = new Report(billInfo, (string)billInfo.staff.name, billInfo.end);
                            r.Show();

                            // Hide user controll
                            this.Dispose();
                            this.Hide();
                        }
                    }
                    else
                    {
                        label2.ForeColor = Color.Red;
                        label2.Text = "Không tìm thấy hóa đơn !!!";
                    }
                }
            }
        }

        // On enter bill ID => check it
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, "[^0-9]"))
            {
                MessageBox.Show("Chỉ chấp nhận số.");
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
            }
            else
            {
                if(textBox1.Text != "")
                    billID = int.Parse(textBox1.Text);
            }
        }

        // Same as function in mainform
        public dynamic BillInfo(dynamic roomData)
        {
            DateTime now = DateTime.Now;
            dynamic bill = new ExpandoObject();
            bill.id = roomData.id;
            bill.staff = roomData.staff;
            bill.type = roomData.room;
            bill.start = DateTime.Parse((string)roomData.created_at);
            bill.end = DateTime.Parse((string)roomData.paid_at).ToString("dd'/'MM'/'yyyy");
            bill.timeSpent = now.Day * 24 * 60 + now.Hour * 60 + now.Minute - bill.start.Day * 24 * 60 - bill.start.Hour * 60 - bill.start.Minute;
            bill.prodmoney = 0;
            bill.produse = new List<dynamic>();
            foreach (dynamic even in roomData.evens)
            {
                if (even.prod.is_time == 0)
                {
                    bill.prodmoney += (int)even.number * (int)even.prod.value;
                    bill.produse.Add(even);
                }
                else
                {
                    bill.timePack = even;
                }
            }
            bill.moneyPerHour = (int)bill.timePack.prod.value * (float)bill.type.type.ratio;
            bill.hoursMoney = bill.timeSpent * (bill.moneyPerHour / 60);
            bill.money = bill.prodmoney + bill.hoursMoney;
            Console.WriteLine("ROOMS STATs: " + "TIME: " + bill.timeSpent + " PRODM: " + bill.prodmoney + " BILLM:" + bill.money);
            return bill;
        }
    }
}
