using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KaraManager
{
    // Init class for extra components
    public class KaraComponents
    {
        // Init some basic style config
        static Size btnS = new Size(150, 150);
        static Size btnSmall = new Size(100, 100);
        static Font h1 = new Font("Roboto", 12);
        static Font h2 = new Font("Roboto", 10);
        static Font h3 = new Font("Roboto", 8);
        static Font h1B = new Font("Roboto", 12, FontStyle.Bold);
        static Font sH1B = new Font("Roboto", 24, FontStyle.Bold);
        dynamic n = System.Environment.NewLine;

        // Init function to generate components

        // Return an stat button in statistic screen
        public Button MakeBtnStatic(Color clrB, Color clrT, String name, long value = -1, string extras = "", bool Big = false)
        {
            FlatButton result = new FlatButton(clrB);
            result.FlatStyle = FlatStyle.Flat;
            result.Size = btnS;
            result.Font = h1;
            result.ForeColor = clrT;
            result.Text = name;
            result.subText = (value > -1 ? CoverInt(value) : "") + extras;
            result.subFont = Big ? sH1B : h1B;
            return result;
        }

        // Make an product button in room screen
        public Button MakeBtnProd(Color clrB, Color clrT, String name, long value = -1, string extras = "", long value2 = -1, string extras2 = "", bool Big = false)
        {
            FlatButtonProd result = new FlatButtonProd(clrB);
            result.FlatStyle = FlatStyle.Flat;
            result.Size = btnS;
            result.Font = h1;
            result.ForeColor = clrT;
            result.Text = name;
            result.subText = (value > -1 ? CoverInt(value) : "") + extras;
            result.use = (value2 > -1 ? CoverInt(value2) : "") + extras2;
            result.subFont = Big ? sH1B : h1B;
            result.useFont = h2;
            return result;
        }

        // Make an select time prod button in open room
        public Button MakeBtnTime(Color clrB, Color clrT, String name, long value = -1, string extras = " đ")
        {
            FlatButton result = new FlatButton(clrB);
            result.FlatStyle = FlatStyle.Flat;
            result.Size = btnSmall;
            result.Font = h3;
            result.ForeColor = clrT;
            result.Text = name;
            result.subText = (value > -1 ? CoverInt(value) : "") + extras;
            result.subFont = h2;
            return result;
        }


        // Extra function
        private string CoverInt(long number)
        {
            return number.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("de"));
        }
    }

    // FlatButton with has 2 line
    public class FlatButton : Button
    {
        public string subText;
        public Font subFont;
        public FlatButton(Color initC)
        {
            BackColor = initC;
            ForeColor = Color.White;
            Size = new Size(150, 150);
            CurrentBackColor = BackColor;
        }

        public Color CurrentBackColor;

        private Color onHoverBackColor = Color.DarkOrchid;
        public Color OnHoverBackColor
        {
            get { return onHoverBackColor; }
            set { onHoverBackColor = value; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            CurrentBackColor = onHoverBackColor;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            CurrentBackColor = BackColor;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            CurrentBackColor = Color.RoyalBlue;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            CurrentBackColor = BackColor;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.FillRectangle(new SolidBrush(CurrentBackColor), 0, 0, Width, Height);
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(pevent.Graphics, Text, Font, new Point(Width + 3, Height / 4), ForeColor, flags);
            TextRenderer.DrawText(pevent.Graphics, subText, subFont, new Point(Width + 3, Height / 2), ForeColor, flags);
        }
    }


    // Flatbutton with has 3 line
    public class FlatButtonProd : Button
    {
        public string subText;
        public string use;
        public Font subFont;
        public Font useFont;
        public FlatButtonProd(Color initC)
        {
            BackColor = initC;
            ForeColor = Color.White;
            Size = new Size(150, 150);
            CurrentBackColor = BackColor;
        }

        public Color CurrentBackColor;

        private Color onHoverBackColor = Color.DarkOrchid;
        public Color OnHoverBackColor
        {
            get { return onHoverBackColor; }
            set { onHoverBackColor = value; Invalidate(); }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            CurrentBackColor = onHoverBackColor;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            CurrentBackColor = BackColor;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            CurrentBackColor = Color.RoyalBlue;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            CurrentBackColor = BackColor;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.FillRectangle(new SolidBrush(CurrentBackColor), 0, 0, Width, Height);
            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(pevent.Graphics, Text, Font, new Point(Width + 3, Height / 4), ForeColor, flags);
            TextRenderer.DrawText(pevent.Graphics, subText, subFont, new Point(Width + 3, Height / 2), ForeColor, flags);
            TextRenderer.DrawText(pevent.Graphics, use, useFont, new Point(Width + 3, 3*Height / 4), ForeColor, flags);
        }
    }
}
