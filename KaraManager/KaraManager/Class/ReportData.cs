using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaraManager
{
    // Make an report Header data like billID - userID - Open - Close of the bill
    public class Bill3
    {
        public Bill3(string billID, string date, string dateOut, string staff, string room)
        {
            this.billID = billID;
            this.date = date;
            this.dateOut = dateOut;
            billStaff = staff;
            this.room = room;
        }

        private string billID;
        public string BillID
        {

            get { return billID; }

            set { billID = value; }

        }

        private string date;
        public string Date

        {
            get { return date; }

            set { date = value; }
        }

        private string dateOut;
        public string DateOut

        {
            get { return dateOut; }

            set { dateOut = value; }
        }

        private string billStaff;
        public string BillStaff

        {

            get { return billStaff; }

            set { billStaff = value; }

        }

        private string room;
        public string Room

        {

            get { return room; }

            set { room = value; }

        }
    }

    // Make an report body data like product used of the bill
    public class BillProdUsed
    {
        public BillProdUsed(string name, string dongia, string number, string tong)
        {
            this.name = name;

            this.dongia = dongia!=""? dongia + " đ" : "";

            this.number = number;

            this.tong = tong + " đ";
        }

        private string name;
        public string Name
        {

            get { return name; }

            set { name = value; }

        }

        private string dongia;
        public string Dongia

        {
            get { return dongia; }

            set { dongia = value; }
        }

        private string number;
        public string Number

        {

            get { return number; }

            set { number = value; }

        }

        private string tong;
        public string Tong

        {
            get { return tong; }

            set { tong = value; }
        }
    }
}
