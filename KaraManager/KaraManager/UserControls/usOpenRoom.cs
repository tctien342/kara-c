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
    public partial class usOpenRoom : UserControl
    {
        // Init variable
        private MainForm dad;
        private dynamic room;
        private KaraComponents cpms = new KaraComponents();
        private dynamic selectTime;
        private FlatButton selectBtn;

        // Time select view constructor
        public usOpenRoom(MainForm form, dynamic room)
        {
            InitializeComponent();
            this.dad = form;
            this.room = room;
            InitInfo();
        }

        // Init Screen
        private void InitInfo()
        {
            // Init label
            label1.Text = (string)room.name;

            // Init time pack
            dynamic timePack = dad.api.prodTime;
            
            // Making time pack button
            foreach (dynamic time in timePack)
            {
                Button btnProd =
                        cpms.MakeBtnTime(Color.FromArgb(150, 150, 150), Color.White, (string)time.name, (long)((int)time.value*(float)room.type.ratio));
                btnProd.AutoSize = true;
                btnProd.Click += (sender, e) => TimeClick(sender, e, time);
                flowLayoutPanel1.Controls.Add(btnProd);
            }
        }

        // On time click
        private void TimeClick(object sender, EventArgs e, dynamic timeP)
        {
            // If has selected previous button
            if (selectBtn != null)
            {   
                // Changing to deactive color
                selectBtn.BackColor = Color.FromArgb(150, 150, 150);
                selectBtn.CurrentBackColor = Color.FromArgb(150, 150, 150);
            }

            // Active selected button
            selectBtn = sender as FlatButton;
            selectBtn.BackColor = Color.FromArgb(60, 193, 143);
            selectBtn.CurrentBackColor = Color.FromArgb(60, 193, 143);
            selectTime = timeP;
        }

        private void label2_Click(object sender, EventArgs e){}

        private void usOpenRoom_Load(object sender, EventArgs e){}

        // On cancel
        private void button2_Click(object sender, EventArgs e)
        {
            // Hide it
            this.Dispose();
            this.Hide();
            dad.t.Start();
        }

        // On create room click
        private void button1_Click(object sender, EventArgs e)
        {
            // Check if time pack selected
            if (selectBtn != null)
            {
                // if selected => open room
                dad.OpeningRoom((int)room.id, (int)selectTime.id);
                this.Dispose();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hãy chọn gói giá!!!");
            }
        }
    }
}
