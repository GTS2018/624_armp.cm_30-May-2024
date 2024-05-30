namespace qp.com.qa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2To = new System.Windows.Forms.Label();
            this.label1From = new System.Windows.Forms.Label();
            this.dateTimePicker2To = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1From = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton2test = new System.Windows.Forms.RadioButton();
            this.radioButton1live = new System.Windows.Forms.RadioButton();
            this.button1exit = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label5server = new System.Windows.Forms.Label();
            this.label4datainserted = new System.Windows.Forms.Label();
            this.label3datacoll = new System.Windows.Forms.Label();
            this.label2linkcoll = new System.Windows.Forms.Label();
            this.label1pno = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1timer = new System.Windows.Forms.Label();
            this.txtUrlName = new System.Windows.Forms.TextBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(12, 94);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(924, 434);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            this.webBrowser1.NewWindow += new System.ComponentModel.CancelEventHandler(this.webBrowser1_NewWindow);
            this.webBrowser1.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.webBrowser1_ProgressChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.label2To);
            this.panel1.Controls.Add(this.label1From);
            this.panel1.Controls.Add(this.dateTimePicker2To);
            this.panel1.Controls.Add(this.dateTimePicker1From);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.radioButton2test);
            this.panel1.Controls.Add(this.radioButton1live);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(866, 54);
            this.panel1.TabIndex = 1;
            // 
            // label2To
            // 
            this.label2To.AutoSize = true;
            this.label2To.Location = new System.Drawing.Point(400, 20);
            this.label2To.Name = "label2To";
            this.label2To.Size = new System.Drawing.Size(20, 13);
            this.label2To.TabIndex = 12;
            this.label2To.Text = "To";
            this.label2To.Visible = false;
            // 
            // label1From
            // 
            this.label1From.AutoSize = true;
            this.label1From.Location = new System.Drawing.Point(232, 20);
            this.label1From.Name = "label1From";
            this.label1From.Size = new System.Drawing.Size(30, 13);
            this.label1From.TabIndex = 11;
            this.label1From.Text = "From";
            // 
            // dateTimePicker2To
            // 
            this.dateTimePicker2To.CustomFormat = "dd-MM-yyyy";
            this.dateTimePicker2To.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2To.Location = new System.Drawing.Point(432, 17);
            this.dateTimePicker2To.Name = "dateTimePicker2To";
            this.dateTimePicker2To.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker2To.TabIndex = 10;
            this.dateTimePicker2To.Visible = false;
            // 
            // dateTimePicker1From
            // 
            this.dateTimePicker1From.CustomFormat = "dd-MM-yyyy";
            this.dateTimePicker1From.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1From.Location = new System.Drawing.Point(268, 17);
            this.dateTimePicker1From.Name = "dateTimePicker1From";
            this.dateTimePicker1From.Size = new System.Drawing.Size(120, 20);
            this.dateTimePicker1From.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Lime;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Green;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(763, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "&Go";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButton2test
            // 
            this.radioButton2test.AutoSize = true;
            this.radioButton2test.Location = new System.Drawing.Point(106, 19);
            this.radioButton2test.Name = "radioButton2test";
            this.radioButton2test.Size = new System.Drawing.Size(80, 17);
            this.radioButton2test.TabIndex = 3;
            this.radioButton2test.TabStop = true;
            this.radioButton2test.Text = "Test Server";
            this.radioButton2test.UseVisualStyleBackColor = true;
            this.radioButton2test.CheckedChanged += new System.EventHandler(this.radioButton2test_CheckedChanged);
            // 
            // radioButton1live
            // 
            this.radioButton1live.AutoSize = true;
            this.radioButton1live.Location = new System.Drawing.Point(21, 19);
            this.radioButton1live.Name = "radioButton1live";
            this.radioButton1live.Size = new System.Drawing.Size(79, 17);
            this.radioButton1live.TabIndex = 2;
            this.radioButton1live.TabStop = true;
            this.radioButton1live.Text = "Live Server";
            this.radioButton1live.UseVisualStyleBackColor = true;
            this.radioButton1live.CheckedChanged += new System.EventHandler(this.radioButton1live_CheckedChanged);
            // 
            // button1exit
            // 
            this.button1exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1exit.BackColor = System.Drawing.Color.Red;
            this.button1exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1exit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.button1exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1exit.Location = new System.Drawing.Point(894, 23);
            this.button1exit.Name = "button1exit";
            this.button1exit.Size = new System.Drawing.Size(42, 32);
            this.button1exit.TabIndex = 0;
            this.button1exit.Text = "&Exit";
            this.button1exit.UseVisualStyleBackColor = false;
            this.button1exit.Click += new System.EventHandler(this.button1exit_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 596);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(948, 28);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.TextChanged += new System.EventHandler(this.statusStrip1_TextChanged);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(500, 22);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 23);
            this.toolStripStatusLabel1.Text = "Status";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label5server);
            this.panel2.Controls.Add(this.label4datainserted);
            this.panel2.Controls.Add(this.label3datacoll);
            this.panel2.Controls.Add(this.label2linkcoll);
            this.panel2.Controls.Add(this.label1pno);
            this.panel2.Location = new System.Drawing.Point(12, 535);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(924, 54);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(616, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "ExpireData : 0";
            this.label1.Visible = false;
            // 
            // label5server
            // 
            this.label5server.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label5server.AutoSize = true;
            this.label5server.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5server.Location = new System.Drawing.Point(785, 19);
            this.label5server.Name = "label5server";
            this.label5server.Size = new System.Drawing.Size(112, 15);
            this.label5server.TabIndex = 4;
            this.label5server.Text = "Selected Server ";
            // 
            // label4datainserted
            // 
            this.label4datainserted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4datainserted.AutoSize = true;
            this.label4datainserted.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4datainserted.Location = new System.Drawing.Point(462, 7);
            this.label4datainserted.Name = "label4datainserted";
            this.label4datainserted.Size = new System.Drawing.Size(118, 15);
            this.label4datainserted.TabIndex = 3;
            this.label4datainserted.Text = "DuplicateData : 0";
            this.label4datainserted.Visible = false;
            // 
            // label3datacoll
            // 
            this.label3datacoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3datacoll.AutoSize = true;
            this.label3datacoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3datacoll.Location = new System.Drawing.Point(312, 6);
            this.label3datacoll.Name = "label3datacoll";
            this.label3datacoll.Size = new System.Drawing.Size(93, 15);
            this.label3datacoll.TabIndex = 2;
            this.label3datacoll.Text = "InsertData : 0";
            this.label3datacoll.Visible = false;
            // 
            // label2linkcoll
            // 
            this.label2linkcoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2linkcoll.AutoSize = true;
            this.label2linkcoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2linkcoll.Location = new System.Drawing.Point(136, 6);
            this.label2linkcoll.Name = "label2linkcoll";
            this.label2linkcoll.Size = new System.Drawing.Size(114, 15);
            this.label2linkcoll.TabIndex = 1;
            this.label2linkcoll.Text = "LinkCollected : 0";
            this.label2linkcoll.Visible = false;
            // 
            // label1pno
            // 
            this.label1pno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1pno.AutoSize = true;
            this.label1pno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1pno.Location = new System.Drawing.Point(3, 6);
            this.label1pno.Name = "label1pno";
            this.label1pno.Size = new System.Drawing.Size(78, 15);
            this.label1pno.TabIndex = 0;
            this.label1pno.Text = "PageNo : 0";
            this.label1pno.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1timer
            // 
            this.label1timer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1timer.AutoSize = true;
            this.label1timer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1timer.Location = new System.Drawing.Point(24, 494);
            this.label1timer.Name = "label1timer";
            this.label1timer.Size = new System.Drawing.Size(158, 25);
            this.label1timer.TabIndex = 4;
            this.label1timer.Text = "100 % Loading...";
            this.label1timer.Visible = false;
            // 
            // txtUrlName
            // 
            this.txtUrlName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrlName.Location = new System.Drawing.Point(12, 72);
            this.txtUrlName.Name = "txtUrlName";
            this.txtUrlName.Size = new System.Drawing.Size(924, 20);
            this.txtUrlName.TabIndex = 5;
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(948, 624);
            this.Controls.Add(this.txtUrlName);
            this.Controls.Add(this.label1timer);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.button1exit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "armp.cm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1exit;
        private System.Windows.Forms.RadioButton radioButton2test;
        private System.Windows.Forms.RadioButton radioButton1live;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5server;
        private System.Windows.Forms.Label label4datainserted;
        private System.Windows.Forms.Label label3datacoll;
        private System.Windows.Forms.Label label2linkcoll;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1timer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TextBox txtUrlName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1pno;
        private System.Windows.Forms.Label label2To;
        private System.Windows.Forms.Label label1From;
        private System.Windows.Forms.DateTimePicker dateTimePicker2To;
        private System.Windows.Forms.DateTimePicker dateTimePicker1From;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label1;
    }
}

