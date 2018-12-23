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
    public partial class Report : Form
    {
        // Init Report state
        private dynamic billInfo;
        private string staffName;
        private string end;
        // Report constuctor need bill info an staff name
        public Report(dynamic billInfo, string staffName, string end)
        {
            InitializeComponent();
            this.billInfo = billInfo;
            this.staffName = staffName;
            this.end = end;
            InitBill();
        }

        // Render the bill
        public void InitBill()
        {
            // Make Header like bill id - staff name - open an closed
            string roomHead = (string)billInfo.type.name + " - " + (string)billInfo.type.type.name;
            Bill3 aBill = 
                new Bill3((string)billInfo.id, billInfo.start.ToString("dd'/'MM'/'yyyy HH':'mm"), end, staffName, roomHead);
            BillBindingSource.DataSource = aBill;

            // Make Body like usage product
            List<BillProdUsed> listp = new List<BillProdUsed>();

            // Making product time
            int hours = (int)(billInfo.timeSpent / 60);
            int minutes = (int)(billInfo.timeSpent % 60);
            string timeSl = "";
            if (hours > 0)
            {
                timeSl += hours.ToString() + " giờ ";
            }
            timeSl += minutes + " phút";
            listp.Add(new BillProdUsed("Giờ hát", CoverInt((long)billInfo.moneyPerHour), timeSl, CoverInt((long)billInfo.hoursMoney)));

            // Making product money
            foreach (dynamic prod in billInfo.produse)
            {
                listp.Add(new BillProdUsed((string)prod.prod.name, CoverInt((long)prod.prod.value), (string)prod.number, CoverInt(((int)prod.prod.value*(int)prod.number))));
            }
            
            // Making sum money
            listp.Add(new BillProdUsed("Tổng", "", "", CoverInt((long)billInfo.money)));
            
            // Render it
            billProdUsedBindingSource.DataSource = listp;
        }

        private void Report_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        // Function to add dot to an number
        private string CoverInt(long number)
        {
            return number.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
        }
    }
}
