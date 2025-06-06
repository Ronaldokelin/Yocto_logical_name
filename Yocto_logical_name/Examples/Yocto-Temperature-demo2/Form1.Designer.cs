﻿namespace WindowsFormsApplication1
{
    partial class Form1
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
          this.InventoryTimer = new System.Windows.Forms.Timer(this.components);
          this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
          this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
          this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
          this.statusStrip1 = new System.Windows.Forms.StatusStrip();
          this.statusStrip1.SuspendLayout();
          this.SuspendLayout();
          // 
          // InventoryTimer
          // 
          this.InventoryTimer.Tick += new System.EventHandler(this.InventoryTimer_Tick);
          // 
          // RefreshTimer
          // 
          this.RefreshTimer.Interval = 1000;
          this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
          // 
          // toolStripStatusLabel1
          // 
          this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
          this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
          this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
          // 
          // toolStripStatusLabel2
          // 
          this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
          this.toolStripStatusLabel2.Size = new System.Drawing.Size(292, 17);
          this.toolStripStatusLabel2.Text = "Plug any Yocto device featuring a temperature sensor ";
          // 
          // statusStrip1
          // 
          this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2});
          this.statusStrip1.Location = new System.Drawing.Point(0, 285);
          this.statusStrip1.Name = "statusStrip1";
          this.statusStrip1.Size = new System.Drawing.Size(299, 22);
          this.statusStrip1.SizingGrip = false;
          this.statusStrip1.TabIndex = 8;
          this.statusStrip1.Text = "statusStrip1";
          // 
          // Form1
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(299, 307);
          this.Controls.Add(this.statusStrip1);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.Name = "Form1";
          this.Text = "Yocto-Temperature demo";
          this.Load += new System.EventHandler(this.Form1_Load);
          this.statusStrip1.ResumeLayout(false);
          this.statusStrip1.PerformLayout();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer InventoryTimer;
        private System.Windows.Forms.Timer RefreshTimer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}

