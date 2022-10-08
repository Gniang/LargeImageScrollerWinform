namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.tlpPlots = new System.Windows.Forms.TableLayoutPanel();
            this.tlpOffset = new System.Windows.Forms.TableLayoutPanel();
            this.txtOffsetX = new WinFormsApp1.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.tlpOffset.SuspendLayout();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 433);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(800, 17);
            this.hScrollBar1.TabIndex = 1;
            // 
            // tlpPlots
            // 
            this.tlpPlots.ColumnCount = 1;
            this.tlpPlots.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPlots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPlots.Location = new System.Drawing.Point(0, 45);
            this.tlpPlots.Name = "tlpPlots";
            this.tlpPlots.RowCount = 1;
            this.tlpPlots.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPlots.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPlots.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPlots.Size = new System.Drawing.Size(800, 388);
            this.tlpPlots.TabIndex = 2;
            // 
            // tlpOffset
            // 
            this.tlpOffset.ColumnCount = 3;
            this.tlpOffset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpOffset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tlpOffset.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 573F));
            this.tlpOffset.Controls.Add(this.txtOffsetX, 1, 0);
            this.tlpOffset.Controls.Add(this.label1, 0, 0);
            this.tlpOffset.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpOffset.Location = new System.Drawing.Point(0, 0);
            this.tlpOffset.Name = "tlpOffset";
            this.tlpOffset.RowCount = 1;
            this.tlpOffset.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpOffset.Size = new System.Drawing.Size(800, 45);
            this.tlpOffset.TabIndex = 3;
            // 
            // txtOffsetX
            // 
            this.txtOffsetX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOffsetX.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtOffsetX.Location = new System.Drawing.Point(97, 3);
            this.txtOffsetX.Name = "txtOffsetX";
            this.txtOffsetX.Size = new System.Drawing.Size(127, 39);
            this.txtOffsetX.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "オフセット[m]";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tlpPlots);
            this.Controls.Add(this.tlpOffset);
            this.Controls.Add(this.hScrollBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tlpOffset.ResumeLayout(false);
            this.tlpOffset.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private HScrollBar hScrollBar1;
        private TableLayoutPanel tlpPlots;
        private TableLayoutPanel tlpOffset;
        private TextBoxEx txtOffsetX;
        private Label label1;
    }
}