namespace ProgramRestore
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
            this.bYSec = new System.Windows.Forms.Button();
            this.bYukle = new System.Windows.Forms.Button();
            this.tDosya = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bYSec
            // 
            this.bYSec.Location = new System.Drawing.Point(361, 44);
            this.bYSec.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bYSec.Name = "bYSec";
            this.bYSec.Size = new System.Drawing.Size(97, 31);
            this.bYSec.TabIndex = 0;
            this.bYSec.Text = "Yedek Seç";
            this.bYSec.UseVisualStyleBackColor = true;
            this.bYSec.Click += new System.EventHandler(this.bYSec_Click);
            // 
            // bYukle
            // 
            this.bYukle.Location = new System.Drawing.Point(361, 91);
            this.bYukle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bYukle.Name = "bYukle";
            this.bYukle.Size = new System.Drawing.Size(97, 55);
            this.bYukle.TabIndex = 1;
            this.bYukle.Text = "Yükle";
            this.bYukle.UseVisualStyleBackColor = true;
            this.bYukle.Click += new System.EventHandler(this.bYukle_Click);
            // 
            // tDosya
            // 
            this.tDosya.Location = new System.Drawing.Point(52, 44);
            this.tDosya.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tDosya.Name = "tDosya";
            this.tDosya.Size = new System.Drawing.Size(301, 26);
            this.tDosya.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 60);
            this.label1.TabIndex = 3;
            this.label1.Text = "Bu işlemi yaptığınız zaman mevcut\r\n veriler silinecek Yedeğinizdeki bilgiler \r\nyü" +
    "klenecek\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.IndianRed;
            this.label2.Location = new System.Drawing.Point(51, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(237, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "En Son Aldığınız Yedeği Seçiniz.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 163);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tDosya);
            this.Controls.Add(this.bYukle);
            this.Controls.Add(this.bYSec);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(528, 202);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(528, 202);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Barkodlu Satış Programı Yedekten Yükleme";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bYSec;
        private System.Windows.Forms.Button bYukle;
        private System.Windows.Forms.TextBox tDosya;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

