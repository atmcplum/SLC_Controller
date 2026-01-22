namespace SLC_Controller {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.cbSimulationMode = new System.Windows.Forms.CheckBox();
            this.tbDelay = new System.Windows.Forms.TextBox();
            this.lbMaxDelay = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbCh1 = new System.Windows.Forms.Label();
            this.lbCh2 = new System.Windows.Forms.Label();
            this.lbCh3 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lbCh4 = new System.Windows.Forms.Label();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnSet2 = new System.Windows.Forms.Button();
            this.btnSet3 = new System.Windows.Forms.Button();
            this.btnSet4 = new System.Windows.Forms.Button();
            this.cbIP = new System.Windows.Forms.ComboBox();
            this.tbCycleTime = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnSet1 = new System.Windows.Forms.Button();
            this.lbIsConn = new System.Windows.Forms.Label();
            this.cbMode1 = new System.Windows.Forms.ComboBox();
            this.cbMode2 = new System.Windows.Forms.ComboBox();
            this.cbMode3 = new System.Windows.Forms.ComboBox();
            this.cbMode4 = new System.Windows.Forms.ComboBox();
            this.tbIma1 = new System.Windows.Forms.TextBox();
            this.tbWus1 = new System.Windows.Forms.TextBox();
            this.tbIma2 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbDus1 = new System.Windows.Forms.TextBox();
            this.tbIma3 = new System.Windows.Forms.TextBox();
            this.tbIma4 = new System.Windows.Forms.TextBox();
            this.tbWus2 = new System.Windows.Forms.TextBox();
            this.tbWus3 = new System.Windows.Forms.TextBox();
            this.tbWus4 = new System.Windows.Forms.TextBox();
            this.tbDus2 = new System.Windows.Forms.TextBox();
            this.tbDus3 = new System.Windows.Forms.TextBox();
            this.tbDus4 = new System.Windows.Forms.TextBox();
            this.btnTrigger1 = new System.Windows.Forms.Button();
            this.btnTrigger2 = new System.Windows.Forms.Button();
            this.btnTrigger4 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSetAll = new System.Windows.Forms.Button();
            this.btnTrigger3 = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblClock = new System.Windows.Forms.Label();
            this.clockTimer = new System.Windows.Forms.Timer(this.components);
            this.runTimer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbSimulationMode
            // 
            this.cbSimulationMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.cbSimulationMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.cbSimulationMode.FlatAppearance.BorderSize = 0;
            this.cbSimulationMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSimulationMode.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold);
            this.cbSimulationMode.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.cbSimulationMode.Location = new System.Drawing.Point(12, 44);
            this.cbSimulationMode.Name = "cbSimulationMode";
            this.cbSimulationMode.Size = new System.Drawing.Size(96, 27);
            this.cbSimulationMode.TabIndex = 27;
            this.cbSimulationMode.Text = "SIMULATION";
            this.cbSimulationMode.UseVisualStyleBackColor = false;
            this.cbSimulationMode.CheckedChanged += new System.EventHandler(this.cbSimulationMode_CheckedChanged);
            // 
            // tbDelay
            // 
            this.tbDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDelay.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDelay.ForeColor = System.Drawing.Color.White;
            this.tbDelay.Location = new System.Drawing.Point(750, 40);
            this.tbDelay.Name = "tbDelay";
            this.tbDelay.Size = new System.Drawing.Size(43, 23);
            this.tbDelay.TabIndex = 25;
            this.tbDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbDelay.TextChanged += new System.EventHandler(this.tbDelay_TextChanged);
            // 
            // lbMaxDelay
            // 
            this.lbMaxDelay.AutoSize = true;
            this.lbMaxDelay.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMaxDelay.ForeColor = System.Drawing.Color.Red;
            this.lbMaxDelay.Location = new System.Drawing.Point(632, 42);
            this.lbMaxDelay.Name = "lbMaxDelay";
            this.lbMaxDelay.Size = new System.Drawing.Size(99, 19);
            this.lbMaxDelay.TabIndex = 22;
            this.lbMaxDelay.Text = "MAX: Value";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label13.Location = new System.Drawing.Point(661, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 42);
            this.label13.TabIndex = 14;
            this.label13.Text = "D (us)";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label12.Location = new System.Drawing.Point(557, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(98, 42);
            this.label12.TabIndex = 13;
            this.label12.Text = "W (us)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label11.Location = new System.Drawing.Point(453, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(98, 42);
            this.label11.TabIndex = 12;
            this.label11.Text = "I (mA)";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label10.Location = new System.Drawing.Point(33, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(414, 42);
            this.label10.TabIndex = 11;
            this.label10.Text = "MODE";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 42);
            this.label1.TabIndex = 1;
            this.label1.Text = "CH";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbCh1
            // 
            this.lbCh1.AutoSize = true;
            this.lbCh1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCh1.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCh1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lbCh1.Location = new System.Drawing.Point(3, 42);
            this.lbCh1.Name = "lbCh1";
            this.lbCh1.Size = new System.Drawing.Size(24, 42);
            this.lbCh1.TabIndex = 2;
            this.lbCh1.Text = "1";
            this.lbCh1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbCh2
            // 
            this.lbCh2.AutoSize = true;
            this.lbCh2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCh2.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCh2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lbCh2.Location = new System.Drawing.Point(3, 84);
            this.lbCh2.Name = "lbCh2";
            this.lbCh2.Size = new System.Drawing.Size(24, 42);
            this.lbCh2.TabIndex = 3;
            this.lbCh2.Text = "2";
            this.lbCh2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbCh3
            // 
            this.lbCh3.AutoSize = true;
            this.lbCh3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCh3.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCh3.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lbCh3.Location = new System.Drawing.Point(3, 126);
            this.lbCh3.Name = "lbCh3";
            this.lbCh3.Size = new System.Drawing.Size(24, 42);
            this.lbCh3.TabIndex = 4;
            this.lbCh3.Text = "3";
            this.lbCh3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Lime;
            this.label17.Location = new System.Drawing.Point(795, 41);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(27, 19);
            this.label17.TabIndex = 21;
            this.label17.Text = "ms";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(725, 42);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(36, 19);
            this.label16.TabIndex = 20;
            this.label16.Text = "D= ";
            // 
            // lbCh4
            // 
            this.lbCh4.AutoSize = true;
            this.lbCh4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCh4.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCh4.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.lbCh4.Location = new System.Drawing.Point(3, 168);
            this.lbCh4.Name = "lbCh4";
            this.lbCh4.Size = new System.Drawing.Size(24, 43);
            this.lbCh4.TabIndex = 5;
            this.lbCh4.Text = "4";
            this.lbCh4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSetting
            // 
            this.btnSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetting.FlatAppearance.BorderSize = 0;
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetting.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetting.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnSetting.Location = new System.Drawing.Point(832, 0);
            this.btnSetting.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(106, 42);
            this.btnSetting.TabIndex = 0;
            this.btnSetting.Text = "SETTING";
            this.btnSetting.UseVisualStyleBackColor = false;
            // 
            // btnSet2
            // 
            this.btnSet2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnSet2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSet2.FlatAppearance.BorderSize = 0;
            this.btnSet2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet2.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet2.ForeColor = System.Drawing.Color.White;
            this.btnSet2.Location = new System.Drawing.Point(765, 87);
            this.btnSet2.Name = "btnSet2";
            this.btnSet2.Size = new System.Drawing.Size(64, 36);
            this.btnSet2.TabIndex = 10;
            this.btnSet2.Text = "SET";
            this.btnSet2.UseVisualStyleBackColor = false;
            this.btnSet2.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnSet3
            // 
            this.btnSet3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnSet3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSet3.FlatAppearance.BorderSize = 0;
            this.btnSet3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet3.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet3.ForeColor = System.Drawing.Color.White;
            this.btnSet3.Location = new System.Drawing.Point(765, 129);
            this.btnSet3.Name = "btnSet3";
            this.btnSet3.Size = new System.Drawing.Size(64, 36);
            this.btnSet3.TabIndex = 10;
            this.btnSet3.Text = "SET";
            this.btnSet3.UseVisualStyleBackColor = false;
            this.btnSet3.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnSet4
            // 
            this.btnSet4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnSet4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSet4.FlatAppearance.BorderSize = 0;
            this.btnSet4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet4.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet4.ForeColor = System.Drawing.Color.White;
            this.btnSet4.Location = new System.Drawing.Point(765, 171);
            this.btnSet4.Name = "btnSet4";
            this.btnSet4.Size = new System.Drawing.Size(64, 37);
            this.btnSet4.TabIndex = 10;
            this.btnSet4.Text = "SET";
            this.btnSet4.UseVisualStyleBackColor = false;
            this.btnSet4.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // cbIP
            // 
            this.cbIP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.cbIP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbIP.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIP.ForeColor = System.Drawing.Color.White;
            this.cbIP.FormattingEnabled = true;
            this.cbIP.Items.AddRange(new object[] {
            "172.28.37.100",
            "172.28.37.101",
            "172.28.37.102",
            "172.28.37.103",
            "172.28.37.104",
            "172.28.37.105",
            "172.28.37.106",
            "172.28.37.107",
            "172.28.37.108",
            "172.28.37.109",
            "172.28.38.101"});
            this.cbIP.Location = new System.Drawing.Point(578, 12);
            this.cbIP.Name = "cbIP";
            this.cbIP.Size = new System.Drawing.Size(133, 26);
            this.cbIP.TabIndex = 26;
            // 
            // tbCycleTime
            // 
            this.tbCycleTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbCycleTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCycleTime.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCycleTime.ForeColor = System.Drawing.Color.White;
            this.tbCycleTime.Location = new System.Drawing.Point(750, 16);
            this.tbCycleTime.Name = "tbCycleTime";
            this.tbCycleTime.Size = new System.Drawing.Size(43, 23);
            this.tbCycleTime.TabIndex = 24;
            this.tbCycleTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbCycleTime.TextChanged += new System.EventHandler(this.tbCycleTime_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Lime;
            this.label15.Location = new System.Drawing.Point(795, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(27, 19);
            this.label15.TabIndex = 23;
            this.label15.Text = "ms";
            // 
            // btnSet1
            // 
            this.btnSet1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnSet1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSet1.FlatAppearance.BorderSize = 0;
            this.btnSet1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet1.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSet1.ForeColor = System.Drawing.Color.White;
            this.btnSet1.Location = new System.Drawing.Point(765, 45);
            this.btnSet1.Name = "btnSet1";
            this.btnSet1.Size = new System.Drawing.Size(64, 36);
            this.btnSet1.TabIndex = 10;
            this.btnSet1.Text = "SET";
            this.btnSet1.UseVisualStyleBackColor = false;
            this.btnSet1.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // lbIsConn
            // 
            this.lbIsConn.AutoSize = true;
            this.lbIsConn.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIsConn.ForeColor = System.Drawing.Color.Red;
            this.lbIsConn.Location = new System.Drawing.Point(114, 14);
            this.lbIsConn.Name = "lbIsConn";
            this.lbIsConn.Size = new System.Drawing.Size(130, 22);
            this.lbIsConn.TabIndex = 18;
            this.lbIsConn.Text = "DISCONNECTED";
            // 
            // cbMode1
            // 
            this.cbMode1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMode1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.cbMode1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMode1.ForeColor = System.Drawing.Color.White;
            this.cbMode1.FormattingEnabled = true;
            this.cbMode1.Items.AddRange(new object[] {
            "Off",
            "Pulse",
            "Continuous"});
            this.cbMode1.Location = new System.Drawing.Point(33, 52);
            this.cbMode1.Name = "cbMode1";
            this.cbMode1.Size = new System.Drawing.Size(414, 22);
            this.cbMode1.TabIndex = 15;
            this.cbMode1.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // cbMode2
            // 
            this.cbMode2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMode2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.cbMode2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMode2.ForeColor = System.Drawing.Color.White;
            this.cbMode2.FormattingEnabled = true;
            this.cbMode2.Items.AddRange(new object[] {
            "Off",
            "Pulse",
            "Continuous"});
            this.cbMode2.Location = new System.Drawing.Point(33, 94);
            this.cbMode2.Name = "cbMode2";
            this.cbMode2.Size = new System.Drawing.Size(414, 22);
            this.cbMode2.TabIndex = 15;
            this.cbMode2.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // cbMode3
            // 
            this.cbMode3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMode3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.cbMode3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMode3.ForeColor = System.Drawing.Color.White;
            this.cbMode3.FormattingEnabled = true;
            this.cbMode3.Items.AddRange(new object[] {
            "Off",
            "Pulse",
            "Continuous"});
            this.cbMode3.Location = new System.Drawing.Point(33, 136);
            this.cbMode3.Name = "cbMode3";
            this.cbMode3.Size = new System.Drawing.Size(414, 22);
            this.cbMode3.TabIndex = 15;
            this.cbMode3.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // cbMode4
            // 
            this.cbMode4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbMode4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.cbMode4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMode4.ForeColor = System.Drawing.Color.White;
            this.cbMode4.FormattingEnabled = true;
            this.cbMode4.Items.AddRange(new object[] {
            "Off",
            "Pulse",
            "Continuous"});
            this.cbMode4.Location = new System.Drawing.Point(33, 178);
            this.cbMode4.Name = "cbMode4";
            this.cbMode4.Size = new System.Drawing.Size(414, 22);
            this.cbMode4.TabIndex = 15;
            this.cbMode4.SelectedIndexChanged += new System.EventHandler(this.cbMode_SelectedIndexChanged);
            // 
            // tbIma1
            // 
            this.tbIma1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbIma1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbIma1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIma1.ForeColor = System.Drawing.Color.White;
            this.tbIma1.Location = new System.Drawing.Point(453, 52);
            this.tbIma1.Name = "tbIma1";
            this.tbIma1.Size = new System.Drawing.Size(98, 22);
            this.tbIma1.TabIndex = 16;
            this.tbIma1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbWus1
            // 
            this.tbWus1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbWus1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbWus1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbWus1.ForeColor = System.Drawing.Color.White;
            this.tbWus1.Location = new System.Drawing.Point(557, 52);
            this.tbWus1.Name = "tbWus1";
            this.tbWus1.Size = new System.Drawing.Size(98, 22);
            this.tbWus1.TabIndex = 16;
            this.tbWus1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbIma2
            // 
            this.tbIma2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbIma2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbIma2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIma2.ForeColor = System.Drawing.Color.White;
            this.tbIma2.Location = new System.Drawing.Point(453, 94);
            this.tbIma2.Name = "tbIma2";
            this.tbIma2.Size = new System.Drawing.Size(98, 22);
            this.tbIma2.TabIndex = 16;
            this.tbIma2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(716, 18);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 19);
            this.label14.TabIndex = 19;
            this.label14.Text = "CT= ";
            // 
            // tbDus1
            // 
            this.tbDus1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbDus1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbDus1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDus1.ForeColor = System.Drawing.Color.White;
            this.tbDus1.Location = new System.Drawing.Point(661, 52);
            this.tbDus1.Name = "tbDus1";
            this.tbDus1.Size = new System.Drawing.Size(98, 22);
            this.tbDus1.TabIndex = 16;
            this.tbDus1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbIma3
            // 
            this.tbIma3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbIma3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbIma3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIma3.ForeColor = System.Drawing.Color.White;
            this.tbIma3.Location = new System.Drawing.Point(453, 136);
            this.tbIma3.Name = "tbIma3";
            this.tbIma3.Size = new System.Drawing.Size(98, 22);
            this.tbIma3.TabIndex = 16;
            this.tbIma3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbIma4
            // 
            this.tbIma4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbIma4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbIma4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIma4.ForeColor = System.Drawing.Color.White;
            this.tbIma4.Location = new System.Drawing.Point(453, 178);
            this.tbIma4.Name = "tbIma4";
            this.tbIma4.Size = new System.Drawing.Size(98, 22);
            this.tbIma4.TabIndex = 16;
            this.tbIma4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbWus2
            // 
            this.tbWus2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbWus2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbWus2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbWus2.ForeColor = System.Drawing.Color.White;
            this.tbWus2.Location = new System.Drawing.Point(557, 94);
            this.tbWus2.Name = "tbWus2";
            this.tbWus2.Size = new System.Drawing.Size(98, 22);
            this.tbWus2.TabIndex = 16;
            this.tbWus2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbWus3
            // 
            this.tbWus3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbWus3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbWus3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbWus3.ForeColor = System.Drawing.Color.White;
            this.tbWus3.Location = new System.Drawing.Point(557, 136);
            this.tbWus3.Name = "tbWus3";
            this.tbWus3.Size = new System.Drawing.Size(98, 22);
            this.tbWus3.TabIndex = 16;
            this.tbWus3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbWus4
            // 
            this.tbWus4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbWus4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbWus4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbWus4.ForeColor = System.Drawing.Color.White;
            this.tbWus4.Location = new System.Drawing.Point(557, 178);
            this.tbWus4.Name = "tbWus4";
            this.tbWus4.Size = new System.Drawing.Size(98, 22);
            this.tbWus4.TabIndex = 16;
            this.tbWus4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDus2
            // 
            this.tbDus2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbDus2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbDus2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDus2.ForeColor = System.Drawing.Color.White;
            this.tbDus2.Location = new System.Drawing.Point(661, 94);
            this.tbDus2.Name = "tbDus2";
            this.tbDus2.Size = new System.Drawing.Size(98, 22);
            this.tbDus2.TabIndex = 16;
            this.tbDus2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDus3
            // 
            this.tbDus3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbDus3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbDus3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDus3.ForeColor = System.Drawing.Color.White;
            this.tbDus3.Location = new System.Drawing.Point(661, 136);
            this.tbDus3.Name = "tbDus3";
            this.tbDus3.Size = new System.Drawing.Size(98, 22);
            this.tbDus3.TabIndex = 16;
            this.tbDus3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDus4
            // 
            this.tbDus4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbDus4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tbDus4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbDus4.ForeColor = System.Drawing.Color.White;
            this.tbDus4.Location = new System.Drawing.Point(661, 178);
            this.tbDus4.Name = "tbDus4";
            this.tbDus4.Size = new System.Drawing.Size(98, 22);
            this.tbDus4.TabIndex = 16;
            this.tbDus4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnTrigger1
            // 
            this.btnTrigger1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnTrigger1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTrigger1.FlatAppearance.BorderSize = 0;
            this.btnTrigger1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrigger1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrigger1.ForeColor = System.Drawing.Color.White;
            this.btnTrigger1.Location = new System.Drawing.Point(835, 45);
            this.btnTrigger1.Name = "btnTrigger1";
            this.btnTrigger1.Size = new System.Drawing.Size(100, 36);
            this.btnTrigger1.TabIndex = 0;
            this.btnTrigger1.Text = "TRIG";
            this.btnTrigger1.UseVisualStyleBackColor = false;
            this.btnTrigger1.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // btnTrigger2
            // 
            this.btnTrigger2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnTrigger2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTrigger2.FlatAppearance.BorderSize = 0;
            this.btnTrigger2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrigger2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrigger2.ForeColor = System.Drawing.Color.White;
            this.btnTrigger2.Location = new System.Drawing.Point(835, 87);
            this.btnTrigger2.Name = "btnTrigger2";
            this.btnTrigger2.Size = new System.Drawing.Size(100, 36);
            this.btnTrigger2.TabIndex = 17;
            this.btnTrigger2.Text = "TRIG";
            this.btnTrigger2.UseVisualStyleBackColor = false;
            this.btnTrigger2.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // btnTrigger4
            // 
            this.btnTrigger4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnTrigger4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTrigger4.FlatAppearance.BorderSize = 0;
            this.btnTrigger4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrigger4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrigger4.ForeColor = System.Drawing.Color.White;
            this.btnTrigger4.Location = new System.Drawing.Point(835, 171);
            this.btnTrigger4.Name = "btnTrigger4";
            this.btnTrigger4.Size = new System.Drawing.Size(100, 37);
            this.btnTrigger4.TabIndex = 19;
            this.btnTrigger4.Text = "TRIG";
            this.btnTrigger4.UseVisualStyleBackColor = false;
            this.btnTrigger4.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 420F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.btnSetAll, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label13, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label12, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label11, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbCh1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbCh2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbCh3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbCh4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnSetting, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSet1, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSet2, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSet3, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSet4, 5, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbMode1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbMode2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbMode3, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbMode4, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbIma1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbWus1, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbDus1, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbIma2, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbIma3, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbIma4, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbWus2, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbWus3, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbWus4, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbDus2, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbDus3, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbDus4, 4, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnTrigger1, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTrigger2, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnTrigger3, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnTrigger4, 6, 4);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 75);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(938, 211);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // btnSetAll
            // 
            this.btnSetAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnSetAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetAll.FlatAppearance.BorderSize = 0;
            this.btnSetAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetAll.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetAll.ForeColor = System.Drawing.Color.White;
            this.btnSetAll.Location = new System.Drawing.Point(765, 3);
            this.btnSetAll.Name = "btnSetAll";
            this.btnSetAll.Size = new System.Drawing.Size(64, 36);
            this.btnSetAll.TabIndex = 20;
            this.btnSetAll.Text = "ALL";
            this.btnSetAll.UseVisualStyleBackColor = false;
            this.btnSetAll.Click += new System.EventHandler(this.btnSetAll_Click);
            // 
            // btnTrigger3
            // 
            this.btnTrigger3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnTrigger3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTrigger3.FlatAppearance.BorderSize = 0;
            this.btnTrigger3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrigger3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrigger3.ForeColor = System.Drawing.Color.White;
            this.btnTrigger3.Location = new System.Drawing.Point(835, 129);
            this.btnTrigger3.Name = "btnTrigger3";
            this.btnTrigger3.Size = new System.Drawing.Size(100, 36);
            this.btnTrigger3.TabIndex = 18;
            this.btnTrigger3.Text = "TRIG";
            this.btnTrigger3.UseVisualStyleBackColor = false;
            this.btnTrigger3.Click += new System.EventHandler(this.btnTrigger_Click);
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnTest.Location = new System.Drawing.Point(827, 12);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(123, 57);
            this.btnTest.TabIndex = 16;
            this.btnTest.Text = "TEST";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(55)))), ((int)(((byte)(66)))));
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(96, 27);
            this.btnConnect.TabIndex = 11;
            this.btnConnect.Text = "CONNECT";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLog.ForeColor = System.Drawing.Color.White;
            this.rtbLog.Location = new System.Drawing.Point(12, 298);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbLog.Size = new System.Drawing.Size(938, 88);
            this.rtbLog.TabIndex = 28;
            this.rtbLog.Text = "";
            this.rtbLog.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(55)))));
            this.panel1.Controls.Add(this.lblClock);
            this.panel1.Location = new System.Drawing.Point(12, 392);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(938, 25);
            this.panel1.TabIndex = 29;
            // 
            // lblClock
            // 
            this.lblClock.Font = new System.Drawing.Font("Consolas", 9F);
            this.lblClock.ForeColor = System.Drawing.Color.Aqua;
            this.lblClock.Location = new System.Drawing.Point(787, 0);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(151, 24);
            this.lblClock.TabIndex = 0;
            this.lblClock.Text = "label2";
            this.lblClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clockTimer
            // 
            this.clockTimer.Tick += new System.EventHandler(this.clockTimer_Tick);
            // 
            // runTimer
            // 
            this.runTimer.Tick += new System.EventHandler(this.runTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(33)))), ((int)(((byte)(44)))));
            this.ClientSize = new System.Drawing.Size(963, 426);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.cbSimulationMode);
            this.Controls.Add(this.tbDelay);
            this.Controls.Add(this.lbMaxDelay);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.cbIP);
            this.Controls.Add(this.tbCycleTime);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lbIsConn);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "SLC_LightController";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbSimulationMode;
        private System.Windows.Forms.TextBox tbDelay;
        private System.Windows.Forms.Label lbMaxDelay;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbCh1;
        private System.Windows.Forms.Label lbCh2;
        private System.Windows.Forms.Label lbCh3;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lbCh4;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button btnSet2;
        private System.Windows.Forms.Button btnSet3;
        private System.Windows.Forms.Button btnSet4;
        private System.Windows.Forms.ComboBox cbIP;
        private System.Windows.Forms.TextBox tbCycleTime;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnSet1;
        private System.Windows.Forms.Label lbIsConn;
        private System.Windows.Forms.ComboBox cbMode1;
        private System.Windows.Forms.ComboBox cbMode2;
        private System.Windows.Forms.ComboBox cbMode3;
        private System.Windows.Forms.ComboBox cbMode4;
        private System.Windows.Forms.TextBox tbIma1;
        private System.Windows.Forms.TextBox tbWus1;
        private System.Windows.Forms.TextBox tbIma2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbDus1;
        private System.Windows.Forms.TextBox tbIma3;
        private System.Windows.Forms.TextBox tbIma4;
        private System.Windows.Forms.TextBox tbWus2;
        private System.Windows.Forms.TextBox tbWus3;
        private System.Windows.Forms.TextBox tbWus4;
        private System.Windows.Forms.TextBox tbDus2;
        private System.Windows.Forms.TextBox tbDus3;
        private System.Windows.Forms.TextBox tbDus4;
        private System.Windows.Forms.Button btnTrigger1;
        private System.Windows.Forms.Button btnTrigger2;
        private System.Windows.Forms.Button btnTrigger4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnTrigger3;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSetAll;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.Timer clockTimer;
        private System.Windows.Forms.Timer runTimer;
    }
}