namespace ImageTemplate
{
    partial class MainApplication
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
            this.label9 = new System.Windows.Forms.Label();
            this.cboxView2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tboxK = new System.Windows.Forms.TextBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tboxSize = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudMaskSize = new System.Windows.Forms.NumericUpDown();
            this.tboxFilePath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGaussSigma = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboxView1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.isGauss = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numericBlend = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaskSize)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBlend)).BeginInit();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(673, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 16);
            this.label9.TabIndex = 50;
            this.label9.Text = "View";
            // 
            // cboxView2
            // 
            this.cboxView2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboxView2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxView2.FormattingEnabled = true;
            this.cboxView2.Location = new System.Drawing.Point(722, 12);
            this.cboxView2.Name = "cboxView2";
            this.cboxView2.Size = new System.Drawing.Size(576, 21);
            this.cboxView2.TabIndex = 47;
            this.cboxView2.SelectionChangeCommitted += new System.EventHandler(this.selectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 573);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 19);
            this.label7.TabIndex = 45;
            this.label7.Text = "Original Image";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(970, 649);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 43;
            this.label1.Text = "K Parameter";
            // 
            // tboxK
            // 
            this.tboxK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tboxK.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxK.Location = new System.Drawing.Point(1064, 646);
            this.tboxK.Name = "tboxK";
            this.tboxK.Size = new System.Drawing.Size(148, 23);
            this.tboxK.TabIndex = 44;
            this.tboxK.Text = "0.8";
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnApply.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(676, 597);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(98, 72);
            this.btnApply.TabIndex = 34;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOpen.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpen.Location = new System.Drawing.Point(549, 597);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(105, 72);
            this.btnOpen.TabIndex = 31;
            this.btnOpen.Text = "Open Image";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(796, 610);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 16);
            this.label3.TabIndex = 35;
            this.label3.Text = "Mask Size";
            // 
            // tboxSize
            // 
            this.tboxSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tboxSize.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxSize.Location = new System.Drawing.Point(95, 646);
            this.tboxSize.Name = "tboxSize";
            this.tboxSize.ReadOnly = true;
            this.tboxSize.Size = new System.Drawing.Size(448, 23);
            this.tboxSize.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(796, 649);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 16);
            this.label4.TabIndex = 37;
            this.label4.Text = "Gauss Sigma";
            // 
            // nudMaskSize
            // 
            this.nudMaskSize.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.nudMaskSize.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMaskSize.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudMaskSize.Location = new System.Drawing.Point(890, 608);
            this.nudMaskSize.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nudMaskSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudMaskSize.Name = "nudMaskSize";
            this.nudMaskSize.Size = new System.Drawing.Size(57, 23);
            this.nudMaskSize.TabIndex = 38;
            this.nudMaskSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // tboxFilePath
            // 
            this.tboxFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tboxFilePath.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxFilePath.Location = new System.Drawing.Point(95, 608);
            this.tboxFilePath.Name = "tboxFilePath";
            this.tboxFilePath.ReadOnly = true;
            this.tboxFilePath.Size = new System.Drawing.Size(448, 23);
            this.tboxFilePath.TabIndex = 39;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 611);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 16);
            this.label5.TabIndex = 40;
            this.label5.Text = "File Name";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 649);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 16);
            this.label6.TabIndex = 41;
            this.label6.Text = "Image Size";
            // 
            // txtGaussSigma
            // 
            this.txtGaussSigma.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtGaussSigma.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGaussSigma.Location = new System.Drawing.Point(890, 646);
            this.txtGaussSigma.Name = "txtGaussSigma";
            this.txtGaussSigma.Size = new System.Drawing.Size(57, 23);
            this.txtGaussSigma.TabIndex = 42;
            this.txtGaussSigma.Text = "0.8";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Location = new System.Drawing.Point(676, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(626, 525);
            this.panel2.TabIndex = 30;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(617, 514);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(631, 514);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMinSize = new System.Drawing.Size(1, 1);
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(12, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(642, 525);
            this.panel1.TabIndex = 29;
            // 
            // cboxView1
            // 
            this.cboxView1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxView1.FormattingEnabled = true;
            this.cboxView1.Location = new System.Drawing.Point(56, 12);
            this.cboxView1.Name = "cboxView1";
            this.cboxView1.Size = new System.Drawing.Size(592, 21);
            this.cboxView1.TabIndex = 48;
            this.cboxView1.SelectionChangeCommitted += new System.EventHandler(this.selectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(9, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 16);
            this.label8.TabIndex = 49;
            this.label8.Text = "View";
            // 
            // isGauss
            // 
            this.isGauss.AutoSize = true;
            this.isGauss.Location = new System.Drawing.Point(932, 578);
            this.isGauss.Name = "isGauss";
            this.isGauss.Size = new System.Drawing.Size(15, 14);
            this.isGauss.TabIndex = 51;
            this.isGauss.UseVisualStyleBackColor = true;
            this.isGauss.CheckedChanged += new System.EventHandler(this.isGauss_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(796, 576);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 16);
            this.label2.TabIndex = 52;
            this.label2.Text = "Gauss Smooth";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(970, 610);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 16);
            this.label10.TabIndex = 54;
            this.label10.Text = "Blend (%)";
            // 
            // numericBlend
            // 
            this.numericBlend.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.numericBlend.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericBlend.Location = new System.Drawing.Point(1064, 609);
            this.numericBlend.Name = "numericBlend";
            this.numericBlend.Size = new System.Drawing.Size(57, 23);
            this.numericBlend.TabIndex = 55;
            this.numericBlend.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // MainApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1314, 681);
            this.Controls.Add(this.numericBlend);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.isGauss);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cboxView1);
            this.Controls.Add(this.cboxView2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tboxK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGaussSigma);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tboxFilePath);
            this.Controls.Add(this.nudMaskSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tboxSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "MainApplication";
            this.Text = "MainApplication";
            this.Load += new System.EventHandler(this.MainApplication_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudMaskSize)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericBlend)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboxView2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tboxK;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tboxSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudMaskSize;
        private System.Windows.Forms.TextBox tboxFilePath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGaussSigma;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboxView1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox isGauss;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericBlend;
    }
}