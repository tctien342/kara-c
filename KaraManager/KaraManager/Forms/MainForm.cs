using KaraManager.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaraManager
{
    public partial class MainForm : Form
    {
        // Init important variable
        public APIManager api;
        public KaraComponents cpms = new KaraComponents();

        // Init private variable
        private delegate void DoWorkDelegate(); // Do an job when form loaded

        // Init API variable
        public dynamic activeBill;
        public dynamic monthLy;

        // Init timmer for calling every time
        public Timer t = new Timer();



        public MainForm(APIManager apiM)
        {
            InitializeComponent();
            this.api = apiM; // Binding api class
        }

        // Function to init data when success login
        public async void InitForm()
        {  
            // Show form
            this.Show();
            // Getting important data
            activeBill = await api.GetActiveBill();
            monthLy = await api.GetMonth();

            // Setting timmer for make job calling every time
            t.Interval = 5000;
            t.Tick += new EventHandler(this.t_Tick);
            t.Start();

            // Render Data to form
            toolStripStatusLabel1.Text = "Nhân viên: " + api.userName;
            toolStripStatusLabel2.Text = "Mã NV: " + api.userId;
            UpdateTime();
            RenderStatic();
            RenderRooms();
        }

        // Logout function here
        public void Logout()
        {
            // Cleaning api class
            api.Logout();

            // Stop job
            t.Stop();

            // Hide this form
            this.Hide();

            // Show login
            Login l = new Login(this);
            l.Show();
        }

        // Render statistic screen here
        public void RenderStatic()
        {
            // Callculating data
            int activeRoom = activeBill.data.Count;
            int deActiveRoom = api.roomsI.Count - activeRoom;
            int sumActive = 0;

            // PROCESSING IT
            foreach(dynamic room in activeBill.data)
            {
                sumActive += (int)BillInfo(room).money;
            }

            // BUILD STAT
            Button activeR = cpms.MakeBtnStatic(Color.FromArgb(60, 193, 143), Color.White, "Phòng đang hát", activeRoom, "",true);
            Button deActiveR = cpms.MakeBtnStatic(Color.FromArgb(60, 193, 143), Color.White, "Phòng trống", deActiveRoom, "",true);
            Button sumActiveR = cpms.MakeBtnStatic(Color.FromArgb(60, 193, 143), Color.White, "Dự thu", sumActive, " đ");
            Button prodActive = cpms.MakeBtnStatic(Color.FromArgb(60, 193, 143), Color.White, "Số sản phẩm", api.prodUse.Count, " Cái");
            Button sumMonth = cpms.MakeBtnStatic(Color.FromArgb(60, 193, 143), Color.White, "Thu nhập tháng", (long)monthLy.data.value, " đ");

            // RENDER
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.Controls.Add(activeR);
            flowLayoutPanel1.Controls.Add(deActiveR);
            flowLayoutPanel1.Controls.Add(sumActiveR);
            flowLayoutPanel1.Controls.Add(prodActive);
            flowLayoutPanel1.Controls.Add(sumMonth);
            return;
        }

        // Render rooms screen here
        public void RenderRooms()
        {
            // Init array data
            List<Button> list = new List<Button>();
            List<int> activeID = new List<int>();

            // PROCESSING DATA TO ARRAY
            // Make active room button
            foreach (dynamic room in activeBill.data)
            {
                dynamic roomInfo = BillInfo(room);
                activeID.Add((int)room.room.id);
                Button roomAct = 
                    cpms.MakeBtnProd(Color.FromArgb(60, 193, 143), Color.White, (string)roomInfo.type.name, (int)roomInfo.money, " đ", -1, roomInfo.start.ToString("dd/MM HH:mm"));
                roomAct.Click += (sender, e) => OpenRoomActive(sender, e, roomInfo);
                list.Add(roomAct);
            }

            // Make deactived room button
            foreach (dynamic room in api.roomsI)
            {
                Console.WriteLine(room);
                if (!activeID.Contains((int)room.id))
                {
                    Button roomAct = cpms.MakeBtnStatic(Color.FromArgb(120, 120, 120), Color.White, (string)room.name, -1, "Chưa mở");
                    roomAct.Click += (sender, e) => OpenRoom(sender, e, room);
                    list.Add(roomAct);
                }
            }

            // RENDER IT
            flowLayoutPanel2.Controls.Clear();
            foreach (Button roomBtn in list)
            {
                flowLayoutPanel2.Controls.Add(roomBtn);
            }
            return;
        }

        // Fuction to open an active room
        public void OpenRoomActive(object sender, EventArgs e, dynamic billinfo)
        {
            // Stop the timer
            t.Stop();

            // Show room form
            Room room = new Room(this, billinfo);
            room.Show();
        }

        // Function to open an not actived room
        public void OpenRoom(object sender, EventArgs e, dynamic room)
        {
            // Stop the timer
            t.Stop();
            // Show the select time pack User Controle (usOpenRoom)
            usOpenRoom open = new usOpenRoom(this, room);
            this.Controls.Add(open);
            open.Left = (this.ClientSize.Width - open.Width) / 2;
            open.Top = (this.ClientSize.Height - open.Height) / 2;
            open.BringToFront();
        }


        // Fuction call when timepack selected, open room
        public async void OpeningRoom(int roomID, int timeProdId)
        {
            dynamic info = await api.PostMakeBill(roomID, timeProdId);
            dynamic billData = await api.GetBill((int)info.data.id);
            UpdateWithRen();
            OpenRoomActive(null, null, BillInfo(billData.data));
        }

        // Fuction making bill info from raw data server
        public dynamic BillInfo(dynamic roomData)
        {
            // Init variabale
            DateTime now = DateTime.Now;
            dynamic bill = new ExpandoObject();

            // Making return data
            bill.id = roomData.id;
            bill.staff = roomData.staff;
            bill.type = roomData.room;
            bill.start = DateTime.Parse((string)roomData.created_at);
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
            bill.hoursMoney = bill.timeSpent * (bill.moneyPerHour/60);
            bill.money = bill.prodmoney + bill.hoursMoney;

            // Log the room
            Console.WriteLine("ROOMS STATs: " + "TIME: " + bill.timeSpent + " PRODM: " + bill.prodmoney + " BILLM:" + bill.money);
            return bill;
        }

        // UnUsuage Function
        public void RenderRoom()
        {
            for (int i = 0; i < api.roomsI.Count; i++)
            {
                Button btn2 = new Button();
                btn2.Text = api.roomsI[i].name;
                btn2.AutoSize = true;
                btn2.Size= new Size(100, 100);
                flowLayoutPanel1.Controls.Add(btn2);
            }

        }

        // btn Resfresh render functio (unUsage)
        private void button1_Click(object sender, EventArgs e)
        {
            RenderStatic();
        }

        // On from loading
        private void Form1_Load(object sender, EventArgs e)
        {
            // Show an button loading to screen
            Button loading = cpms.MakeBtnStatic(Color.FromArgb(200, 200, 200), Color.White, "LOADING", -1, "Please wait");
            flowLayoutPanel1.Controls.Add(loading);

            // Calling loaded function;
            BeginInvoke(new DoWorkDelegate(Form1_Loaded));
        }

        // On form loaded function => show login function
        public void Form1_Loaded()
        {
            // Hide form
            this.Hide();
            // Show login form
            Login l = new Login(this);
            l.Show();
        }

        // Fuction to update state in an config time
        public void t_Tick(object sender, EventArgs e)
        {
            UpdateTime();
            UpdateWithRen();
        }

        // Update state function => getting date from server
        public async void UpdateVal()
        {
            activeBill = await api.GetActiveBill();
            monthLy = await api.GetMonth();
        }

        // Like UpdateVal but will re-render the screen
        public async void UpdateWithRen()
        {
            activeBill = await api.GetActiveBill();
            monthLy = await api.GetMonth();
            RenderStatic();
            RenderRooms();
        }

        // Function to update clock
        private void UpdateTime()
        {
            DateTime now = DateTime.Now;
            string H = now.Hour < 10 ? "0" + now.Hour.ToString() : now.Hour.ToString();
            string M = now.Minute < 10 ? "0" + now.Minute.ToString() : now.Minute.ToString();
            label1.Text = H + ":" + M;
        }

        // Render color for tabPage (unUsuage)
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabPage page = tabControl1.TabPages[e.Index];
            Color col = e.Index == 0 ? Color.Aqua : Color.Yellow;
            e.Graphics.FillRectangle(new SolidBrush(col), e.Bounds);

            Rectangle paddedBounds = e.Bounds;
            int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);
            TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, page.ForeColor);
        }

        // Function when click logout button
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }

        // Function to call an Search BillID userControll (TraCuu)
        private void billToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TraCuu open = new TraCuu("Tra cứu hóa đơn", api);
            this.Controls.Add(open);
            open.Left = (this.ClientSize.Width - open.Width) / 2;
            open.Top = (this.ClientSize.Height - open.Height) / 2;
            open.BringToFront();
        }
    }
}
