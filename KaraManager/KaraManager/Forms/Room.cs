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
    public partial class Room : Form
    {
        // Init class variable
        private MainForm dad;
        private dynamic billInfo;
        private KaraComponents cpms = new KaraComponents();

        // Room form constructor, need dad form and bill information
        public Room(MainForm form1, dynamic bill)
        {
            InitializeComponent();
            // Bind data
            dad = form1;
            billInfo = bill;
            
            // Start rendering
            this.Text = "Phòng " + bill.type.name;
            InitStat();
            InitProd();
        }

        private void Room_Load(object sender, EventArgs e){}

        // Rendering room info
        private void InitStat()
        {
            // Getting info
            string typeRoom = (string)billInfo.type.type.name;
            string typeTime = (string)billInfo.timePack.prod.name;
            string openTime = billInfo.start.ToString("dd/MM HH:mm");
            int money = (int)billInfo.money;

            // Make info
            Button btnTypeRoom = cpms.MakeBtnStatic(Color.FromArgb(60, 193, 143), Color.White, "Loại phòng", -1, typeRoom);
            Button btnTypeTime = cpms.MakeBtnStatic(Color.FromArgb(60, 193, 143), Color.White, "Gói giờ", -1, typeTime);
            Button btnOpenTime = cpms.MakeBtnStatic(Color.FromArgb(60, 193, 143), Color.White, "Giờ mở cửa", -1, openTime);
            Button btnMoney = cpms.MakeBtnStatic(Color.FromArgb(60, 193, 143), Color.White, "Thu nhập", money, " đ");

            // Render
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel2.Controls.Add(btnTypeRoom);
            flowLayoutPanel2.Controls.Add(btnTypeTime);
            flowLayoutPanel2.Controls.Add(btnOpenTime);
            flowLayoutPanel2.Controls.Add(btnMoney);
        }

        // Rendering room product
        private void InitProd()
        {
            // Init array variable
            List<int> prodID = new List<int>();
            List<Button> prodUse = new List<Button>();

            // Make used product btn
            foreach (dynamic prod in billInfo.produse)
            {
                prodID.Add((int)prod.prod.id);
                Button btnProd =
                    cpms.MakeBtnProd(Color.FromArgb(60, 193, 143), Color.White, (string)prod.prod.name, (int)prod.number * (int)prod.prod.value, " đ", (int)prod.number, " cái");
                btnProd.Click += (sender, e) => OpenProd(sender, e, prod.prod, (int)prod.id, (int)prod.number);
                prodUse.Add(btnProd);
            }

            // Make unUsed product btn
            foreach (dynamic prod in dad.api.prodUse)
            {
                if (!prodID.Contains((int)prod.id))
                {
                    Button btnProd =
                        cpms.MakeBtnProd(Color.FromArgb(150, 150, 150), Color.White, (string)prod.name, (int)prod.value, " đ", (int)prod.count, " cái");
                    btnProd.Click += (sender, e) => OpenProd(sender, e, prod, -1, 0);
                    prodUse.Add(btnProd);
                }
            }

            // Render it to layout
            flowLayoutPanel1.Controls.Clear();
            foreach (Button btn in prodUse)
            {
                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        // Update bill status and render it
        public async void UpdateEven(int evenId, int prodId, int number)
        {
            dynamic updateE;
            if (evenId != -1)
            {
                updateE = await dad.api.PostEditEven(evenId, prodId, number);
            }
            else
            {
                updateE = await dad.api.PostAddEven((int)billInfo.id, prodId, number);
            }
            Console.WriteLine(updateE);
            dad.UpdateVal();
            dynamic bill = await dad.api.GetBill((int)billInfo.id);
            billInfo = dad.BillInfo(bill.data);
            InitStat();
            InitProd();
        }

        // Open an product user controller (Product)
        private void OpenProd(object sender, EventArgs e, dynamic prod, int evenId, int count = 0)
        {
            Product prodControll = new Product(this, prod ,evenId, count);
            this.Controls.Add(prodControll);
            prodControll.Left = (this.ClientSize.Width - prodControll.Width) / 2;
            prodControll.Top = (this.ClientSize.Height - prodControll.Height) / 2;
            prodControll.BringToFront();
        }

        // When form closed
        private void Room_FormClosed(object sender, FormClosedEventArgs e)
        {   
            // Update dad state
            dad.RenderStatic();
            dad.RenderRooms();


            // Start dad job again
            dad.t.Start();
        }

        // On back click
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // On checkout button click
        private void button1_Click(object sender, EventArgs e)
        {
            validateUserEntry();
        }

        // Checkout Bill function
        private async void validateUserEntry()
        {
            // Initializes the variables to pass to the MessageBox.Show method.

            string message = "Chắc chắn thanh toán?";
            string caption = "Xác thực";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                // If user acceted then make an report to user
                Report r = new Report(billInfo, dad.api.userName, DateTime.Now.ToString("dd'/'MM'/'yyyy HH':'mm"));
                r.Show();
                dynamic res = await dad.api.PostPaidBill((int)billInfo.id, (long)billInfo.money);

                // Update dad state then close this form
                dad.UpdateWithRen();
                this.Close();
            }
            else
            {

            }
        }
    }
}
