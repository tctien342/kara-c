namespace KaraManager
{
    partial class Report
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report));
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.BillBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.billProdUsedBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.BillBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.billProdUsedBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer1.AutoScroll = true;
            this.reportViewer1.AutoSize = true;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.BillBindingSource;
            reportDataSource2.Name = "DataSet2";
            reportDataSource2.Value = this.billProdUsedBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "KaraManager.Hoadon.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(-1, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(362, 551);
            this.reportViewer1.TabIndex = 0;
            // 
            // BillBindingSource
            // 
            this.BillBindingSource.DataSource = typeof(KaraManager.Bill3);
            // 
            // billProdUsedBindingSource
            // 
            this.billProdUsedBindingSource.DataSource = typeof(KaraManager.BillProdUsed);
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(361, 552);
            this.Controls.Add(this.reportViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Report";
            this.Text = "Hóa đơn";
            this.Load += new System.EventHandler(this.Report_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BillBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.billProdUsedBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource BillBindingSource;
        private System.Windows.Forms.BindingSource billProdUsedBindingSource;
    }
}