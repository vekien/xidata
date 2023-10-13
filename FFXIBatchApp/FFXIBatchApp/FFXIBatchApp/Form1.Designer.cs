
namespace FFXIBatchApp
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
			this.label5 = new System.Windows.Forms.Label();
			this.GithubLink = new System.Windows.Forms.LinkLabel();
			this.label4 = new System.Windows.Forms.Label();
			this.FFXIModdingDiscordLink = new System.Windows.Forms.LinkLabel();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.PathFFXI = new System.Windows.Forms.TextBox();
			this.StartNpcExtract = new System.Windows.Forms.Button();
			this.LogBox = new System.Windows.Forms.RichTextBox();
			this.LogTimer = new System.Windows.Forms.Timer(this.components);
			this.StopEverything = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.StartZoneRoomsExtract = new System.Windows.Forms.Button();
			this.label10 = new System.Windows.Forms.Label();
			this.NoesisArgsZone = new System.Windows.Forms.TextBox();
			this.StartZoneExtract = new System.Windows.Forms.Button();
			this.label9 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label14 = new System.Windows.Forms.Label();
			this.NoesisArgsArmor = new System.Windows.Forms.TextBox();
			this.StartArmorExtract = new System.Windows.Forms.Button();
			this.label15 = new System.Windows.Forms.Label();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.label16 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.label11 = new System.Windows.Forms.Label();
			this.NoesisArgsWeapons = new System.Windows.Forms.TextBox();
			this.StartWeaponsExtract = new System.Windows.Forms.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.label13 = new System.Windows.Forms.Label();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.StartNpcCommonExtract = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.NoesisArgsNPC = new System.Windows.Forms.TextBox();
			this.label22 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.label19 = new System.Windows.Forms.Label();
			this.NoesisArgsAnimations = new System.Windows.Forms.TextBox();
			this.StartAnimationsExtract = new System.Windows.Forms.Button();
			this.label20 = new System.Windows.Forms.Label();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.label21 = new System.Windows.Forms.Label();
			this.tabPage6 = new System.Windows.Forms.TabPage();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.tabPage7 = new System.Windows.Forms.TabPage();
			this.EnableStopHook = new System.Windows.Forms.CheckBox();
			this.label24 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.PathNoesis = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.OpenOutputFolder = new System.Windows.Forms.Button();
			this.OpenLogFile = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.panel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabPage5.SuspendLayout();
			this.groupBox7.SuspendLayout();
			this.tabPage6.SuspendLayout();
			this.tabPage7.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.label5.Location = new System.Drawing.Point(178, 25);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(211, 20);
			this.label5.TabIndex = 4;
			this.label5.Text = "<-- Everything is open source!!";
			// 
			// GithubLink
			// 
			this.GithubLink.AutoSize = true;
			this.GithubLink.Location = new System.Drawing.Point(8, 25);
			this.GithubLink.Name = "GithubLink";
			this.GithubLink.Size = new System.Drawing.Size(158, 20);
			this.GithubLink.TabIndex = 3;
			this.GithubLink.TabStop = true;
			this.GithubLink.Text = "Github: vekien/xidata";
			this.GithubLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GithubLink_LinkClicked);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.label4.Location = new System.Drawing.Point(178, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(390, 20);
			this.label4.TabIndex = 2;
			this.label4.Text = "<-- If you need help, join the discord and message Vekien";
			// 
			// FFXIModdingDiscordLink
			// 
			this.FFXIModdingDiscordLink.AutoSize = true;
			this.FFXIModdingDiscordLink.Location = new System.Drawing.Point(8, 56);
			this.FFXIModdingDiscordLink.Name = "FFXIModdingDiscordLink";
			this.FFXIModdingDiscordLink.Size = new System.Drawing.Size(158, 20);
			this.FFXIModdingDiscordLink.TabIndex = 1;
			this.FFXIModdingDiscordLink.TabStop = true;
			this.FFXIModdingDiscordLink.Text = "FFXI Modding Discord";
			this.FFXIModdingDiscordLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.FFXIModdingDiscordLink_LinkClicked);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
			this.label3.ForeColor = System.Drawing.Color.Brown;
			this.label3.Location = new System.Drawing.Point(13, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(643, 57);
			this.label3.TabIndex = 0;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.ForestGreen;
			this.label2.Location = new System.Drawing.Point(13, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(277, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Final Fantasy XI   (click input to browse)";
			// 
			// PathFFXI
			// 
			this.PathFFXI.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PathFFXI.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.PathFFXI.Location = new System.Drawing.Point(17, 33);
			this.PathFFXI.Name = "PathFFXI";
			this.PathFFXI.Size = new System.Drawing.Size(594, 25);
			this.PathFFXI.TabIndex = 0;
			this.PathFFXI.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PathFFXI_SelectPath);
			// 
			// StartNpcExtract
			// 
			this.StartNpcExtract.BackColor = System.Drawing.Color.YellowGreen;
			this.StartNpcExtract.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartNpcExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.StartNpcExtract.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartNpcExtract.Location = new System.Drawing.Point(13, 87);
			this.StartNpcExtract.Name = "StartNpcExtract";
			this.StartNpcExtract.Size = new System.Drawing.Size(224, 39);
			this.StartNpcExtract.TabIndex = 0;
			this.StartNpcExtract.Text = "Start NPC Main Exporting";
			this.StartNpcExtract.UseVisualStyleBackColor = false;
			this.StartNpcExtract.Click += new System.EventHandler(this.StartNpcExtract_Click);
			// 
			// LogBox
			// 
			this.LogBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LogBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
			this.LogBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.LogBox.Font = new System.Drawing.Font("Consolas", 9F);
			this.LogBox.ForeColor = System.Drawing.SystemColors.Menu;
			this.LogBox.Location = new System.Drawing.Point(688, 112);
			this.LogBox.Margin = new System.Windows.Forms.Padding(2);
			this.LogBox.Name = "LogBox";
			this.LogBox.Size = new System.Drawing.Size(625, 602);
			this.LogBox.TabIndex = 2;
			this.LogBox.Text = "";
			// 
			// LogTimer
			// 
			this.LogTimer.Enabled = true;
			this.LogTimer.Interval = 500;
			this.LogTimer.Tick += new System.EventHandler(this.LogTimer_Tick);
			// 
			// StopEverything
			// 
			this.StopEverything.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.StopEverything.BackColor = System.Drawing.Color.Salmon;
			this.StopEverything.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StopEverything.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.StopEverything.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StopEverything.Location = new System.Drawing.Point(1151, 38);
			this.StopEverything.Name = "StopEverything";
			this.StopEverything.Size = new System.Drawing.Size(144, 40);
			this.StopEverything.TabIndex = 2;
			this.StopEverything.Text = "STOP";
			this.StopEverything.UseVisualStyleBackColor = false;
			this.StopEverything.Click += new System.EventHandler(this.StopEverything_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(-12, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(389, 106);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.tabControl1);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.panel1.Location = new System.Drawing.Point(12, 112);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(10);
			this.panel1.Size = new System.Drawing.Size(669, 602);
			this.panel1.TabIndex = 4;
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Controls.Add(this.tabPage6);
			this.tabControl1.Controls.Add(this.tabPage7);
			this.tabControl1.Location = new System.Drawing.Point(13, 175);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.Padding = new System.Drawing.Point(10, 5);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(643, 414);
			this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.tabControl1.TabIndex = 8;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.StartZoneRoomsExtract);
			this.tabPage1.Controls.Add(this.label10);
			this.tabPage1.Controls.Add(this.NoesisArgsZone);
			this.tabPage1.Controls.Add(this.StartZoneExtract);
			this.tabPage1.Controls.Add(this.label9);
			this.tabPage1.Controls.Add(this.groupBox4);
			this.tabPage1.Location = new System.Drawing.Point(4, 33);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(10);
			this.tabPage1.Size = new System.Drawing.Size(635, 377);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Zones";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// StartZoneRoomsExtract
			// 
			this.StartZoneRoomsExtract.BackColor = System.Drawing.Color.YellowGreen;
			this.StartZoneRoomsExtract.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartZoneRoomsExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.StartZoneRoomsExtract.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartZoneRoomsExtract.Location = new System.Drawing.Point(241, 87);
			this.StartZoneRoomsExtract.Name = "StartZoneRoomsExtract";
			this.StartZoneRoomsExtract.Size = new System.Drawing.Size(238, 39);
			this.StartZoneRoomsExtract.TabIndex = 13;
			this.StartZoneRoomsExtract.Text = "Start Zone Rooms Exporting";
			this.StartZoneRoomsExtract.UseVisualStyleBackColor = false;
			this.StartZoneRoomsExtract.Click += new System.EventHandler(this.StartZoneRoomsExtract_Click);
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(9, 316);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(133, 20);
			this.label10.TabIndex = 12;
			this.label10.Text = "Noesis Arguments";
			// 
			// NoesisArgsZone
			// 
			this.NoesisArgsZone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.NoesisArgsZone.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.NoesisArgsZone.Location = new System.Drawing.Point(13, 339);
			this.NoesisArgsZone.Name = "NoesisArgsZone";
			this.NoesisArgsZone.Size = new System.Drawing.Size(609, 25);
			this.NoesisArgsZone.TabIndex = 11;
			this.NoesisArgsZone.Text = "-ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11optimizegeo " +
    "-ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 0 -scale 100";
			// 
			// StartZoneExtract
			// 
			this.StartZoneExtract.BackColor = System.Drawing.Color.YellowGreen;
			this.StartZoneExtract.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartZoneExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.StartZoneExtract.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartZoneExtract.Location = new System.Drawing.Point(13, 87);
			this.StartZoneExtract.Name = "StartZoneExtract";
			this.StartZoneExtract.Size = new System.Drawing.Size(193, 39);
			this.StartZoneExtract.TabIndex = 9;
			this.StartZoneExtract.Text = "Start Zone Exporting";
			this.StartZoneExtract.UseVisualStyleBackColor = false;
			this.StartZoneExtract.Click += new System.EventHandler(this.StartZoneExtract_Click);
			// 
			// label9
			// 
			this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Font = new System.Drawing.Font("Verdana", 9.75F);
			this.label9.Location = new System.Drawing.Point(13, 139);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(609, 177);
			this.label9.TabIndex = 10;
			this.label9.Text = resources.GetString("label9.Text");
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.label8);
			this.groupBox4.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox4.Location = new System.Drawing.Point(13, 13);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox4.Size = new System.Drawing.Size(609, 58);
			this.groupBox4.TabIndex = 8;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Note";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Segoe UI", 10.5F);
			this.label8.ForeColor = System.Drawing.Color.Brown;
			this.label8.Location = new System.Drawing.Point(8, 26);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(282, 19);
			this.label8.TabIndex = 0;
			this.label8.Text = "This uses ?cmode - You can still use your PC.";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.label14);
			this.tabPage2.Controls.Add(this.NoesisArgsArmor);
			this.tabPage2.Controls.Add(this.StartArmorExtract);
			this.tabPage2.Controls.Add(this.label15);
			this.tabPage2.Controls.Add(this.groupBox6);
			this.tabPage2.Location = new System.Drawing.Point(4, 33);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(10);
			this.tabPage2.Size = new System.Drawing.Size(635, 377);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Armor";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// label14
			// 
			this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label14.AutoSize = true;
			this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(9, 316);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(133, 20);
			this.label14.TabIndex = 22;
			this.label14.Text = "Noesis Arguments";
			// 
			// NoesisArgsArmor
			// 
			this.NoesisArgsArmor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.NoesisArgsArmor.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.NoesisArgsArmor.Location = new System.Drawing.Point(13, 339);
			this.NoesisArgsArmor.Name = "NoesisArgsArmor";
			this.NoesisArgsArmor.Size = new System.Drawing.Size(609, 25);
			this.NoesisArgsArmor.TabIndex = 21;
			this.NoesisArgsArmor.Text = "-noanims -ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11opt" +
    "imizegeo -ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 270 -scale " +
    "60";
			// 
			// StartArmorExtract
			// 
			this.StartArmorExtract.BackColor = System.Drawing.Color.YellowGreen;
			this.StartArmorExtract.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartArmorExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.StartArmorExtract.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartArmorExtract.Location = new System.Drawing.Point(13, 87);
			this.StartArmorExtract.Name = "StartArmorExtract";
			this.StartArmorExtract.Size = new System.Drawing.Size(233, 39);
			this.StartArmorExtract.TabIndex = 19;
			this.StartArmorExtract.Text = "Start Armor Exporting";
			this.StartArmorExtract.UseVisualStyleBackColor = false;
			this.StartArmorExtract.Click += new System.EventHandler(this.StartArmorExtract_Click);
			// 
			// label15
			// 
			this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label15.BackColor = System.Drawing.Color.Transparent;
			this.label15.Font = new System.Drawing.Font("Verdana", 9.75F);
			this.label15.Location = new System.Drawing.Point(13, 139);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(609, 177);
			this.label15.TabIndex = 20;
			this.label15.Text = "This dumps out all armor into FBX format, you can edit the Noesis Args to your li" +
    "king below. \r\n";
			// 
			// groupBox6
			// 
			this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox6.Controls.Add(this.label16);
			this.groupBox6.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox6.Location = new System.Drawing.Point(13, 13);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox6.Size = new System.Drawing.Size(609, 58);
			this.groupBox6.TabIndex = 18;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Note";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Font = new System.Drawing.Font("Segoe UI", 10.5F);
			this.label16.ForeColor = System.Drawing.Color.Brown;
			this.label16.Location = new System.Drawing.Point(8, 26);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(282, 19);
			this.label16.TabIndex = 0;
			this.label16.Text = "This uses ?cmode - You can still use your PC.";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.label11);
			this.tabPage3.Controls.Add(this.NoesisArgsWeapons);
			this.tabPage3.Controls.Add(this.StartWeaponsExtract);
			this.tabPage3.Controls.Add(this.label12);
			this.tabPage3.Controls.Add(this.groupBox5);
			this.tabPage3.Location = new System.Drawing.Point(4, 33);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(10);
			this.tabPage3.Size = new System.Drawing.Size(635, 377);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Weapons";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(9, 316);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(133, 20);
			this.label11.TabIndex = 17;
			this.label11.Text = "Noesis Arguments";
			// 
			// NoesisArgsWeapons
			// 
			this.NoesisArgsWeapons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.NoesisArgsWeapons.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.NoesisArgsWeapons.Location = new System.Drawing.Point(13, 339);
			this.NoesisArgsWeapons.Name = "NoesisArgsWeapons";
			this.NoesisArgsWeapons.Size = new System.Drawing.Size(609, 25);
			this.NoesisArgsWeapons.TabIndex = 16;
			this.NoesisArgsWeapons.Text = "-noanims -ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11opt" +
    "imizegeo -ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 270 -scale " +
    "60";
			// 
			// StartWeaponsExtract
			// 
			this.StartWeaponsExtract.BackColor = System.Drawing.Color.YellowGreen;
			this.StartWeaponsExtract.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartWeaponsExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.StartWeaponsExtract.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartWeaponsExtract.Location = new System.Drawing.Point(13, 87);
			this.StartWeaponsExtract.Name = "StartWeaponsExtract";
			this.StartWeaponsExtract.Size = new System.Drawing.Size(233, 39);
			this.StartWeaponsExtract.TabIndex = 14;
			this.StartWeaponsExtract.Text = "Start Weapons Exporting";
			this.StartWeaponsExtract.UseVisualStyleBackColor = false;
			this.StartWeaponsExtract.Click += new System.EventHandler(this.StartWeaponsExtract_Click);
			// 
			// label12
			// 
			this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label12.BackColor = System.Drawing.Color.Transparent;
			this.label12.Font = new System.Drawing.Font("Verdana", 9.75F);
			this.label12.Location = new System.Drawing.Point(13, 139);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(609, 177);
			this.label12.TabIndex = 15;
			this.label12.Text = resources.GetString("label12.Text");
			// 
			// groupBox5
			// 
			this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox5.Controls.Add(this.label13);
			this.groupBox5.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox5.Location = new System.Drawing.Point(13, 13);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox5.Size = new System.Drawing.Size(609, 58);
			this.groupBox5.TabIndex = 13;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Note";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Segoe UI", 10.5F);
			this.label13.ForeColor = System.Drawing.Color.Brown;
			this.label13.Location = new System.Drawing.Point(8, 26);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(331, 19);
			this.label13.TabIndex = 0;
			this.label13.Text = "Controls your PC as a \"Open\" dialog needs handling.";
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.StartNpcCommonExtract);
			this.tabPage4.Controls.Add(this.label1);
			this.tabPage4.Controls.Add(this.NoesisArgsNPC);
			this.tabPage4.Controls.Add(this.label22);
			this.tabPage4.Controls.Add(this.StartNpcExtract);
			this.tabPage4.Controls.Add(this.groupBox3);
			this.tabPage4.Location = new System.Drawing.Point(4, 33);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(10);
			this.tabPage4.Size = new System.Drawing.Size(635, 377);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "NPC";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// StartNpcCommonExtract
			// 
			this.StartNpcCommonExtract.BackColor = System.Drawing.Color.YellowGreen;
			this.StartNpcCommonExtract.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartNpcCommonExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.StartNpcCommonExtract.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartNpcCommonExtract.Location = new System.Drawing.Point(272, 87);
			this.StartNpcCommonExtract.Name = "StartNpcCommonExtract";
			this.StartNpcCommonExtract.Size = new System.Drawing.Size(245, 39);
			this.StartNpcCommonExtract.TabIndex = 31;
			this.StartNpcCommonExtract.Text = "Start NPC Common Exporting";
			this.StartNpcCommonExtract.UseVisualStyleBackColor = false;
			this.StartNpcCommonExtract.Click += new System.EventHandler(this.StartNpcCommonExtract_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(9, 316);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(133, 20);
			this.label1.TabIndex = 30;
			this.label1.Text = "Noesis Arguments";
			// 
			// NoesisArgsNPC
			// 
			this.NoesisArgsNPC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.NoesisArgsNPC.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.NoesisArgsNPC.Location = new System.Drawing.Point(13, 339);
			this.NoesisArgsNPC.Name = "NoesisArgsNPC";
			this.NoesisArgsNPC.Size = new System.Drawing.Size(609, 25);
			this.NoesisArgsNPC.TabIndex = 29;
			this.NoesisArgsNPC.Text = "-ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11optimizegeo " +
    "-ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 0 -scale 120";
			// 
			// label22
			// 
			this.label22.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label22.BackColor = System.Drawing.Color.Transparent;
			this.label22.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label22.Location = new System.Drawing.Point(13, 139);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(609, 177);
			this.label22.TabIndex = 28;
			this.label22.Text = resources.GetString("label22.Text");
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox3.Location = new System.Drawing.Point(13, 13);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox3.Size = new System.Drawing.Size(609, 58);
			this.groupBox3.TabIndex = 7;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Note";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 10.5F);
			this.label7.ForeColor = System.Drawing.Color.Brown;
			this.label7.Location = new System.Drawing.Point(8, 26);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(359, 19);
			this.label7.TabIndex = 0;
			this.label7.Text = "This Automates Keyboard Inputs. Leave your PC running.";
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.label19);
			this.tabPage5.Controls.Add(this.NoesisArgsAnimations);
			this.tabPage5.Controls.Add(this.StartAnimationsExtract);
			this.tabPage5.Controls.Add(this.label20);
			this.tabPage5.Controls.Add(this.groupBox7);
			this.tabPage5.Location = new System.Drawing.Point(4, 33);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding(10);
			this.tabPage5.Size = new System.Drawing.Size(635, 377);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "Animations";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// label19
			// 
			this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label19.AutoSize = true;
			this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label19.Location = new System.Drawing.Point(9, 316);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(133, 20);
			this.label19.TabIndex = 27;
			this.label19.Text = "Noesis Arguments";
			// 
			// NoesisArgsAnimations
			// 
			this.NoesisArgsAnimations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.NoesisArgsAnimations.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.NoesisArgsAnimations.Location = new System.Drawing.Point(13, 339);
			this.NoesisArgsAnimations.Name = "NoesisArgsAnimations";
			this.NoesisArgsAnimations.Size = new System.Drawing.Size(609, 25);
			this.NoesisArgsAnimations.TabIndex = 26;
			this.NoesisArgsAnimations.Text = "-ff11bumpdir normals -ff11keepnames 3 -ff11noshiny -ff11hton 16 -ff11optimizegeo " +
    "-ff11keepnames -fbxtexrelonly -fbxtexext .png -rotate 180 0 270 -scale 120";
			// 
			// StartAnimationsExtract
			// 
			this.StartAnimationsExtract.BackColor = System.Drawing.Color.YellowGreen;
			this.StartAnimationsExtract.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartAnimationsExtract.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.StartAnimationsExtract.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.StartAnimationsExtract.Location = new System.Drawing.Point(13, 87);
			this.StartAnimationsExtract.Name = "StartAnimationsExtract";
			this.StartAnimationsExtract.Size = new System.Drawing.Size(233, 39);
			this.StartAnimationsExtract.TabIndex = 24;
			this.StartAnimationsExtract.Text = "Start Animations Exporting";
			this.StartAnimationsExtract.UseVisualStyleBackColor = false;
			this.StartAnimationsExtract.Click += new System.EventHandler(this.StartAnimationsExtract_Click);
			// 
			// label20
			// 
			this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label20.BackColor = System.Drawing.Color.Transparent;
			this.label20.Font = new System.Drawing.Font("Verdana", 9.75F);
			this.label20.Location = new System.Drawing.Point(13, 139);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(609, 168);
			this.label20.TabIndex = 25;
			this.label20.Text = resources.GetString("label20.Text");
			// 
			// groupBox7
			// 
			this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox7.Controls.Add(this.label21);
			this.groupBox7.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox7.Location = new System.Drawing.Point(13, 13);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox7.Size = new System.Drawing.Size(609, 58);
			this.groupBox7.TabIndex = 23;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Note";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Font = new System.Drawing.Font("Segoe UI", 10.5F);
			this.label21.ForeColor = System.Drawing.Color.Brown;
			this.label21.Location = new System.Drawing.Point(8, 26);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(331, 19);
			this.label21.TabIndex = 0;
			this.label21.Text = "Controls your PC as a \"Open\" dialog needs handling.";
			// 
			// tabPage6
			// 
			this.tabPage6.Controls.Add(this.label18);
			this.tabPage6.Controls.Add(this.label17);
			this.tabPage6.Location = new System.Drawing.Point(4, 33);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Padding = new System.Windows.Forms.Padding(10);
			this.tabPage6.Size = new System.Drawing.Size(635, 377);
			this.tabPage6.TabIndex = 5;
			this.tabPage6.Text = "Music/Sounds";
			this.tabPage6.UseVisualStyleBackColor = true;
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label18.Location = new System.Drawing.Point(13, 10);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(106, 25);
			this.label18.TabIndex = 22;
			this.label18.Text = "via PolUtils";
			// 
			// label17
			// 
			this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label17.BackColor = System.Drawing.Color.Transparent;
			this.label17.Font = new System.Drawing.Font("Verdana", 9.75F);
			this.label17.Location = new System.Drawing.Point(14, 48);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(608, 319);
			this.label17.TabIndex = 21;
			this.label17.Text = resources.GetString("label17.Text");
			// 
			// tabPage7
			// 
			this.tabPage7.Controls.Add(this.EnableStopHook);
			this.tabPage7.Controls.Add(this.label24);
			this.tabPage7.Controls.Add(this.label23);
			this.tabPage7.Controls.Add(this.PathNoesis);
			this.tabPage7.Controls.Add(this.label2);
			this.tabPage7.Controls.Add(this.label6);
			this.tabPage7.Controls.Add(this.PathFFXI);
			this.tabPage7.Location = new System.Drawing.Point(4, 33);
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.Padding = new System.Windows.Forms.Padding(10);
			this.tabPage7.Size = new System.Drawing.Size(635, 377);
			this.tabPage7.TabIndex = 6;
			this.tabPage7.Text = "Settings";
			this.tabPage7.UseVisualStyleBackColor = true;
			// 
			// EnableStopHook
			// 
			this.EnableStopHook.AutoSize = true;
			this.EnableStopHook.Checked = true;
			this.EnableStopHook.CheckState = System.Windows.Forms.CheckState.Checked;
			this.EnableStopHook.Location = new System.Drawing.Point(17, 180);
			this.EnableStopHook.Name = "EnableStopHook";
			this.EnableStopHook.Size = new System.Drawing.Size(263, 24);
			this.EnableStopHook.TabIndex = 9;
			this.EnableStopHook.Text = "Enable \"A to STOP\" Keyboard Hook";
			this.EnableStopHook.UseVisualStyleBackColor = true;
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.label24.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label24.Location = new System.Drawing.Point(14, 138);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(132, 15);
			this.label24.TabIndex = 8;
			this.label24.Text = "eg: X:\\FF11Tools\\Noesis";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.label23.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label23.Location = new System.Drawing.Point(14, 61);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(262, 15);
			this.label23.TabIndex = 7;
			this.label23.Text = "eg: E:\\SquareEnix\\SquareEnix\\FINAL FANTASY XI";
			// 
			// PathNoesis
			// 
			this.PathNoesis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PathNoesis.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.PathNoesis.Location = new System.Drawing.Point(17, 110);
			this.PathNoesis.Name = "PathNoesis";
			this.PathNoesis.Size = new System.Drawing.Size(594, 25);
			this.PathNoesis.TabIndex = 3;
			this.PathNoesis.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PathNoesis_SelectPath);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.ForestGreen;
			this.label6.Location = new System.Drawing.Point(13, 87);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(226, 20);
			this.label6.TabIndex = 2;
			this.label6.Text = "Noesis64 (click input to browse)";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.GithubLink);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.FFXIModdingDiscordLink);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(13, 76);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox1.Size = new System.Drawing.Size(639, 91);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Support";
			// 
			// OpenOutputFolder
			// 
			this.OpenOutputFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.OpenOutputFolder.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OpenOutputFolder.Location = new System.Drawing.Point(773, 38);
			this.OpenOutputFolder.Name = "OpenOutputFolder";
			this.OpenOutputFolder.Size = new System.Drawing.Size(183, 40);
			this.OpenOutputFolder.TabIndex = 5;
			this.OpenOutputFolder.Text = "Open Output Folder";
			this.OpenOutputFolder.UseVisualStyleBackColor = true;
			this.OpenOutputFolder.Click += new System.EventHandler(this.OpenOutputFolder_Click);
			// 
			// OpenLogFile
			// 
			this.OpenLogFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.OpenLogFile.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OpenLogFile.Location = new System.Drawing.Point(962, 38);
			this.OpenLogFile.Name = "OpenLogFile";
			this.OpenLogFile.Size = new System.Drawing.Size(146, 40);
			this.OpenLogFile.TabIndex = 6;
			this.OpenLogFile.Text = "Open Log File";
			this.OpenLogFile.UseVisualStyleBackColor = true;
			this.OpenLogFile.Click += new System.EventHandler(this.OpenLogFile_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::FFXIBatchApp.Properties.Resources.bg1;
			this.ClientSize = new System.Drawing.Size(1325, 726);
			this.Controls.Add(this.OpenLogFile);
			this.Controls.Add(this.OpenOutputFolder);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.StopEverything);
			this.Controls.Add(this.LogBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1000, 640);
			this.Name = "Form1";
			this.Text = "FFXI BATCH EXPORT";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Unload);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.panel1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			this.tabPage4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tabPage5.ResumeLayout(false);
			this.tabPage5.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			this.tabPage6.ResumeLayout(false);
			this.tabPage6.PerformLayout();
			this.tabPage7.ResumeLayout(false);
			this.tabPage7.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.RichTextBox LogBox;
		private System.Windows.Forms.Button StartNpcExtract;
		private System.Windows.Forms.Timer LogTimer;
		private System.Windows.Forms.Button StopEverything;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel FFXIModdingDiscordLink;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.LinkLabel GithubLink;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TextBox PathFFXI;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox PathNoesis;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button StartZoneExtract;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox NoesisArgsZone;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox NoesisArgsWeapons;
		private System.Windows.Forms.Button StartWeaponsExtract;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox NoesisArgsArmor;
		private System.Windows.Forms.Button StartArmorExtract;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.TextBox NoesisArgsAnimations;
		private System.Windows.Forms.Button StartAnimationsExtract;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox NoesisArgsNPC;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button StartNpcCommonExtract;
		private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Button StartZoneRoomsExtract;
		private System.Windows.Forms.Button OpenOutputFolder;
		private System.Windows.Forms.Button OpenLogFile;
		private System.Windows.Forms.CheckBox EnableStopHook;
	}
}

