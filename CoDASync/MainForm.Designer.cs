namespace CoDASync
{
    partial class CoDASyncWindow
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
            this.CorvusControlGroup = new System.Windows.Forms.GroupBox();
            this.ExecListingButton = new System.Windows.Forms.Button();
            this.ListingFilename = new System.Windows.Forms.TextBox();
            this.CorvusConfigurationBox = new System.Windows.Forms.GroupBox();
            this.ConnectPortButton = new System.Windows.Forms.Button();
            this.BaudRateTextBox = new System.Windows.Forms.TextBox();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.OriginSettingsBox = new System.Windows.Forms.GroupBox();
            this.MoveToPositionButton = new System.Windows.Forms.Button();
            this.OriginY = new System.Windows.Forms.NumericUpDown();
            this.MoveToOriginButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.OriginX = new System.Windows.Forms.NumericUpDown();
            this.SetOriginButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.OriginZ = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.RelativeMoveGroupBox = new System.Windows.Forms.GroupBox();
            this.moveAllAxes = new System.Windows.Forms.Button();
            this.moveNegativeZ = new System.Windows.Forms.Button();
            this.movePositiveZ = new System.Windows.Forms.Button();
            this.moveNegativeY = new System.Windows.Forms.Button();
            this.movePositiveY = new System.Windows.Forms.Button();
            this.moveNegativeX = new System.Windows.Forms.Button();
            this.movePositiveX = new System.Windows.Forms.Button();
            this.RMoveY = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.RMoveX = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.RMoveZ = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.SendCommandButton = new System.Windows.Forms.Button();
            this.VenusCommandBox = new System.Windows.Forms.TextBox();
            this.NIDAQmxControlBox = new System.Windows.Forms.GroupBox();
            this.NIDAQmxConfigurationBox = new System.Windows.Forms.GroupBox();
            this.ChannelTextBox = new System.Windows.Forms.TextBox();
            this.DeviceNameTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.CorvusStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ConfigureNIDAQmxButton = new System.Windows.Forms.Button();
            this.CorvusControlGroup.SuspendLayout();
            this.CorvusConfigurationBox.SuspendLayout();
            this.OriginSettingsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OriginY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OriginX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OriginZ)).BeginInit();
            this.RelativeMoveGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RMoveY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RMoveX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RMoveZ)).BeginInit();
            this.NIDAQmxControlBox.SuspendLayout();
            this.NIDAQmxConfigurationBox.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CorvusControlGroup
            // 
            this.CorvusControlGroup.Controls.Add(this.ExecListingButton);
            this.CorvusControlGroup.Controls.Add(this.ListingFilename);
            this.CorvusControlGroup.Controls.Add(this.CorvusConfigurationBox);
            this.CorvusControlGroup.Controls.Add(this.OriginSettingsBox);
            this.CorvusControlGroup.Controls.Add(this.RelativeMoveGroupBox);
            this.CorvusControlGroup.Controls.Add(this.SendCommandButton);
            this.CorvusControlGroup.Controls.Add(this.VenusCommandBox);
            this.CorvusControlGroup.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.CorvusControlGroup.Location = new System.Drawing.Point(12, 12);
            this.CorvusControlGroup.Name = "CorvusControlGroup";
            this.CorvusControlGroup.Size = new System.Drawing.Size(337, 655);
            this.CorvusControlGroup.TabIndex = 0;
            this.CorvusControlGroup.TabStop = false;
            this.CorvusControlGroup.Text = "Corvus Control";
            // 
            // ExecListingButton
            // 
            this.ExecListingButton.Location = new System.Drawing.Point(6, 123);
            this.ExecListingButton.Name = "ExecListingButton";
            this.ExecListingButton.Size = new System.Drawing.Size(98, 23);
            this.ExecListingButton.TabIndex = 16;
            this.ExecListingButton.Text = "Exec Listing";
            this.ExecListingButton.UseVisualStyleBackColor = true;
            // 
            // ListingFilename
            // 
            this.ListingFilename.Location = new System.Drawing.Point(110, 125);
            this.ListingFilename.Name = "ListingFilename";
            this.ListingFilename.Size = new System.Drawing.Size(221, 20);
            this.ListingFilename.TabIndex = 17;
            this.ListingFilename.Text = "Not implemented (yet)";
            // 
            // CorvusConfigurationBox
            // 
            this.CorvusConfigurationBox.Controls.Add(this.ConnectPortButton);
            this.CorvusConfigurationBox.Controls.Add(this.BaudRateTextBox);
            this.CorvusConfigurationBox.Controls.Add(this.PortTextBox);
            this.CorvusConfigurationBox.Controls.Add(this.label8);
            this.CorvusConfigurationBox.Controls.Add(this.label7);
            this.CorvusConfigurationBox.Location = new System.Drawing.Point(7, 20);
            this.CorvusConfigurationBox.Name = "CorvusConfigurationBox";
            this.CorvusConfigurationBox.Size = new System.Drawing.Size(324, 73);
            this.CorvusConfigurationBox.TabIndex = 15;
            this.CorvusConfigurationBox.TabStop = false;
            this.CorvusConfigurationBox.Text = "Configure";
            // 
            // ConnectPortButton
            // 
            this.ConnectPortButton.Location = new System.Drawing.Point(228, 40);
            this.ConnectPortButton.Name = "ConnectPortButton";
            this.ConnectPortButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectPortButton.TabIndex = 4;
            this.ConnectPortButton.Text = "Connect";
            this.ConnectPortButton.UseVisualStyleBackColor = true;
            this.ConnectPortButton.Click += new System.EventHandler(this.ConnectPortButton_Click);
            // 
            // BaudRateTextBox
            // 
            this.BaudRateTextBox.Location = new System.Drawing.Point(86, 42);
            this.BaudRateTextBox.Name = "BaudRateTextBox";
            this.BaudRateTextBox.Size = new System.Drawing.Size(100, 20);
            this.BaudRateTextBox.TabIndex = 3;
            this.BaudRateTextBox.Text = "57600";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(86, 16);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(100, 20);
            this.PortTextBox.TabIndex = 2;
            this.PortTextBox.Text = "COM6";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Baud Rate";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Port";
            // 
            // OriginSettingsBox
            // 
            this.OriginSettingsBox.Controls.Add(this.MoveToPositionButton);
            this.OriginSettingsBox.Controls.Add(this.OriginY);
            this.OriginSettingsBox.Controls.Add(this.MoveToOriginButton);
            this.OriginSettingsBox.Controls.Add(this.label4);
            this.OriginSettingsBox.Controls.Add(this.OriginX);
            this.OriginSettingsBox.Controls.Add(this.SetOriginButton);
            this.OriginSettingsBox.Controls.Add(this.label5);
            this.OriginSettingsBox.Controls.Add(this.OriginZ);
            this.OriginSettingsBox.Controls.Add(this.label6);
            this.OriginSettingsBox.Location = new System.Drawing.Point(6, 347);
            this.OriginSettingsBox.Name = "OriginSettingsBox";
            this.OriginSettingsBox.Size = new System.Drawing.Size(325, 100);
            this.OriginSettingsBox.TabIndex = 14;
            this.OriginSettingsBox.TabStop = false;
            this.OriginSettingsBox.Text = "Origin Settings";
            // 
            // MoveToPositionButton
            // 
            this.MoveToPositionButton.Location = new System.Drawing.Point(244, 68);
            this.MoveToPositionButton.Name = "MoveToPositionButton";
            this.MoveToPositionButton.Size = new System.Drawing.Size(75, 23);
            this.MoveToPositionButton.TabIndex = 9;
            this.MoveToPositionButton.Text = "MV to Pos";
            this.MoveToPositionButton.UseVisualStyleBackColor = true;
            this.MoveToPositionButton.Click += new System.EventHandler(this.MoveToPositionButton_Click);
            // 
            // OriginY
            // 
            this.OriginY.DecimalPlaces = 3;
            this.OriginY.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.OriginY.Location = new System.Drawing.Point(66, 45);
            this.OriginY.Maximum = new decimal(new int[] {
            16383,
            0,
            0,
            0});
            this.OriginY.Minimum = new decimal(new int[] {
            16383,
            0,
            0,
            -2147483648});
            this.OriginY.Name = "OriginY";
            this.OriginY.Size = new System.Drawing.Size(140, 20);
            this.OriginY.TabIndex = 1;
            // 
            // MoveToOriginButton
            // 
            this.MoveToOriginButton.Location = new System.Drawing.Point(244, 42);
            this.MoveToOriginButton.Name = "MoveToOriginButton";
            this.MoveToOriginButton.Size = new System.Drawing.Size(75, 23);
            this.MoveToOriginButton.TabIndex = 8;
            this.MoveToOriginButton.Text = "MV to Origin";
            this.MoveToOriginButton.UseVisualStyleBackColor = true;
            this.MoveToOriginButton.Click += new System.EventHandler(this.MoveToOriginButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "z mm";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // OriginX
            // 
            this.OriginX.DecimalPlaces = 3;
            this.OriginX.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.OriginX.Location = new System.Drawing.Point(66, 19);
            this.OriginX.Maximum = new decimal(new int[] {
            16383,
            0,
            0,
            0});
            this.OriginX.Minimum = new decimal(new int[] {
            16383,
            0,
            0,
            -2147483648});
            this.OriginX.Name = "OriginX";
            this.OriginX.Size = new System.Drawing.Size(140, 20);
            this.OriginX.TabIndex = 0;
            // 
            // SetOriginButton
            // 
            this.SetOriginButton.Location = new System.Drawing.Point(244, 16);
            this.SetOriginButton.Name = "SetOriginButton";
            this.SetOriginButton.Size = new System.Drawing.Size(75, 23);
            this.SetOriginButton.TabIndex = 7;
            this.SetOriginButton.Text = "Set Origin";
            this.SetOriginButton.UseVisualStyleBackColor = true;
            this.SetOriginButton.Click += new System.EventHandler(this.SetOriginButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "y mm";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // OriginZ
            // 
            this.OriginZ.DecimalPlaces = 3;
            this.OriginZ.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.OriginZ.Location = new System.Drawing.Point(66, 71);
            this.OriginZ.Maximum = new decimal(new int[] {
            16383,
            0,
            0,
            0});
            this.OriginZ.Minimum = new decimal(new int[] {
            16383,
            0,
            0,
            -2147483648});
            this.OriginZ.Name = "OriginZ";
            this.OriginZ.Size = new System.Drawing.Size(140, 20);
            this.OriginZ.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "x mm";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RelativeMoveGroupBox
            // 
            this.RelativeMoveGroupBox.Controls.Add(this.moveAllAxes);
            this.RelativeMoveGroupBox.Controls.Add(this.moveNegativeZ);
            this.RelativeMoveGroupBox.Controls.Add(this.movePositiveZ);
            this.RelativeMoveGroupBox.Controls.Add(this.moveNegativeY);
            this.RelativeMoveGroupBox.Controls.Add(this.movePositiveY);
            this.RelativeMoveGroupBox.Controls.Add(this.moveNegativeX);
            this.RelativeMoveGroupBox.Controls.Add(this.movePositiveX);
            this.RelativeMoveGroupBox.Controls.Add(this.RMoveY);
            this.RelativeMoveGroupBox.Controls.Add(this.label3);
            this.RelativeMoveGroupBox.Controls.Add(this.RMoveX);
            this.RelativeMoveGroupBox.Controls.Add(this.label2);
            this.RelativeMoveGroupBox.Controls.Add(this.RMoveZ);
            this.RelativeMoveGroupBox.Controls.Add(this.label1);
            this.RelativeMoveGroupBox.Location = new System.Drawing.Point(6, 152);
            this.RelativeMoveGroupBox.Name = "RelativeMoveGroupBox";
            this.RelativeMoveGroupBox.Size = new System.Drawing.Size(325, 189);
            this.RelativeMoveGroupBox.TabIndex = 6;
            this.RelativeMoveGroupBox.TabStop = false;
            this.RelativeMoveGroupBox.Text = "Relative Move";
            // 
            // moveAllAxes
            // 
            this.moveAllAxes.Location = new System.Drawing.Point(244, 101);
            this.moveAllAxes.Name = "moveAllAxes";
            this.moveAllAxes.Size = new System.Drawing.Size(75, 23);
            this.moveAllAxes.TabIndex = 13;
            this.moveAllAxes.Text = "All axes";
            this.moveAllAxes.UseVisualStyleBackColor = true;
            this.moveAllAxes.Click += new System.EventHandler(this.RelativeMove_Click);
            // 
            // moveNegativeZ
            // 
            this.moveNegativeZ.Location = new System.Drawing.Point(174, 159);
            this.moveNegativeZ.Name = "moveNegativeZ";
            this.moveNegativeZ.Size = new System.Drawing.Size(28, 23);
            this.moveNegativeZ.TabIndex = 12;
            this.moveNegativeZ.Text = "z-";
            this.moveNegativeZ.UseVisualStyleBackColor = true;
            this.moveNegativeZ.Click += new System.EventHandler(this.RelativeMove_Click);
            // 
            // movePositiveZ
            // 
            this.movePositiveZ.Location = new System.Drawing.Point(174, 101);
            this.movePositiveZ.Name = "movePositiveZ";
            this.movePositiveZ.Size = new System.Drawing.Size(28, 23);
            this.movePositiveZ.TabIndex = 11;
            this.movePositiveZ.Text = "z+";
            this.movePositiveZ.UseVisualStyleBackColor = true;
            this.movePositiveZ.Click += new System.EventHandler(this.RelativeMove_Click);
            // 
            // moveNegativeY
            // 
            this.moveNegativeY.Location = new System.Drawing.Point(87, 159);
            this.moveNegativeY.Name = "moveNegativeY";
            this.moveNegativeY.Size = new System.Drawing.Size(28, 23);
            this.moveNegativeY.TabIndex = 10;
            this.moveNegativeY.Text = "y-";
            this.moveNegativeY.UseVisualStyleBackColor = true;
            this.moveNegativeY.Click += new System.EventHandler(this.RelativeMove_Click);
            // 
            // movePositiveY
            // 
            this.movePositiveY.Location = new System.Drawing.Point(87, 101);
            this.movePositiveY.Name = "movePositiveY";
            this.movePositiveY.Size = new System.Drawing.Size(28, 23);
            this.movePositiveY.TabIndex = 9;
            this.movePositiveY.Text = "y+";
            this.movePositiveY.UseVisualStyleBackColor = true;
            this.movePositiveY.Click += new System.EventHandler(this.RelativeMove_Click);
            // 
            // moveNegativeX
            // 
            this.moveNegativeX.Location = new System.Drawing.Point(62, 130);
            this.moveNegativeX.Name = "moveNegativeX";
            this.moveNegativeX.Size = new System.Drawing.Size(28, 23);
            this.moveNegativeX.TabIndex = 8;
            this.moveNegativeX.Text = "x-";
            this.moveNegativeX.UseVisualStyleBackColor = true;
            this.moveNegativeX.Click += new System.EventHandler(this.RelativeMove_Click);
            // 
            // movePositiveX
            // 
            this.movePositiveX.Location = new System.Drawing.Point(111, 130);
            this.movePositiveX.Name = "movePositiveX";
            this.movePositiveX.Size = new System.Drawing.Size(28, 23);
            this.movePositiveX.TabIndex = 7;
            this.movePositiveX.Text = "x+";
            this.movePositiveX.UseVisualStyleBackColor = true;
            this.movePositiveX.Click += new System.EventHandler(this.RelativeMove_Click);
            // 
            // RMoveY
            // 
            this.RMoveY.DecimalPlaces = 3;
            this.RMoveY.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.RMoveY.Location = new System.Drawing.Point(62, 45);
            this.RMoveY.Maximum = new decimal(new int[] {
            16383,
            0,
            0,
            0});
            this.RMoveY.Minimum = new decimal(new int[] {
            16383,
            0,
            0,
            -2147483648});
            this.RMoveY.Name = "RMoveY";
            this.RMoveY.Size = new System.Drawing.Size(140, 20);
            this.RMoveY.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "z mm";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RMoveX
            // 
            this.RMoveX.DecimalPlaces = 3;
            this.RMoveX.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.RMoveX.Location = new System.Drawing.Point(62, 19);
            this.RMoveX.Maximum = new decimal(new int[] {
            16383,
            0,
            0,
            0});
            this.RMoveX.Minimum = new decimal(new int[] {
            16383,
            0,
            0,
            -2147483648});
            this.RMoveX.Name = "RMoveX";
            this.RMoveX.Size = new System.Drawing.Size(140, 20);
            this.RMoveX.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "y mm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // RMoveZ
            // 
            this.RMoveZ.DecimalPlaces = 3;
            this.RMoveZ.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.RMoveZ.Location = new System.Drawing.Point(62, 71);
            this.RMoveZ.Maximum = new decimal(new int[] {
            16383,
            0,
            0,
            0});
            this.RMoveZ.Minimum = new decimal(new int[] {
            16383,
            0,
            0,
            -2147483648});
            this.RMoveZ.Name = "RMoveZ";
            this.RMoveZ.Size = new System.Drawing.Size(140, 20);
            this.RMoveZ.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "x mm";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // SendCommandButton
            // 
            this.SendCommandButton.Location = new System.Drawing.Point(6, 97);
            this.SendCommandButton.Name = "SendCommandButton";
            this.SendCommandButton.Size = new System.Drawing.Size(98, 23);
            this.SendCommandButton.TabIndex = 0;
            this.SendCommandButton.Text = "Send Command";
            this.SendCommandButton.UseVisualStyleBackColor = true;
            this.SendCommandButton.Click += new System.EventHandler(this.SendCommandButton_Click);
            // 
            // VenusCommandBox
            // 
            this.VenusCommandBox.Location = new System.Drawing.Point(110, 99);
            this.VenusCommandBox.Name = "VenusCommandBox";
            this.VenusCommandBox.Size = new System.Drawing.Size(221, 20);
            this.VenusCommandBox.TabIndex = 1;
            this.VenusCommandBox.Text = "0 0 0 setpos";
            this.VenusCommandBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VenusCommandBox_KeyDown);
            // 
            // NIDAQmxControlBox
            // 
            this.NIDAQmxControlBox.Controls.Add(this.NIDAQmxConfigurationBox);
            this.NIDAQmxControlBox.Location = new System.Drawing.Point(355, 12);
            this.NIDAQmxControlBox.Name = "NIDAQmxControlBox";
            this.NIDAQmxControlBox.Size = new System.Drawing.Size(322, 655);
            this.NIDAQmxControlBox.TabIndex = 1;
            this.NIDAQmxControlBox.TabStop = false;
            this.NIDAQmxControlBox.Text = "NI-DAQmx Control";
            // 
            // NIDAQmxConfigurationBox
            // 
            this.NIDAQmxConfigurationBox.Controls.Add(this.ConfigureNIDAQmxButton);
            this.NIDAQmxConfigurationBox.Controls.Add(this.ChannelTextBox);
            this.NIDAQmxConfigurationBox.Controls.Add(this.DeviceNameTextBox);
            this.NIDAQmxConfigurationBox.Controls.Add(this.label10);
            this.NIDAQmxConfigurationBox.Controls.Add(this.label9);
            this.NIDAQmxConfigurationBox.Location = new System.Drawing.Point(6, 20);
            this.NIDAQmxConfigurationBox.Name = "NIDAQmxConfigurationBox";
            this.NIDAQmxConfigurationBox.Size = new System.Drawing.Size(310, 73);
            this.NIDAQmxConfigurationBox.TabIndex = 0;
            this.NIDAQmxConfigurationBox.TabStop = false;
            this.NIDAQmxConfigurationBox.Text = "Configure";
            // 
            // ChannelTextBox
            // 
            this.ChannelTextBox.Location = new System.Drawing.Point(75, 42);
            this.ChannelTextBox.Name = "ChannelTextBox";
            this.ChannelTextBox.Size = new System.Drawing.Size(100, 20);
            this.ChannelTextBox.TabIndex = 8;
            this.ChannelTextBox.Text = "0 - 6";
            // 
            // DeviceNameTextBox
            // 
            this.DeviceNameTextBox.Location = new System.Drawing.Point(75, 16);
            this.DeviceNameTextBox.Name = "DeviceNameTextBox";
            this.DeviceNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.DeviceNameTextBox.TabIndex = 7;
            this.DeviceNameTextBox.Text = "Dev2";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Channels";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Device";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CorvusStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 657);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(925, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // CorvusStatusLabel
            // 
            this.CorvusStatusLabel.Name = "CorvusStatusLabel";
            this.CorvusStatusLabel.Size = new System.Drawing.Size(88, 17);
            this.CorvusStatusLabel.Text = "Not configured";
            // 
            // ConfigureNIDAQmxButton
            // 
            this.ConfigureNIDAQmxButton.Location = new System.Drawing.Point(203, 40);
            this.ConfigureNIDAQmxButton.Name = "ConfigureNIDAQmxButton";
            this.ConfigureNIDAQmxButton.Size = new System.Drawing.Size(75, 23);
            this.ConfigureNIDAQmxButton.TabIndex = 9;
            this.ConfigureNIDAQmxButton.Text = "Configure";
            this.ConfigureNIDAQmxButton.UseVisualStyleBackColor = true;
			this.ConfigureNIDAQmxButton.Click += new System.EventHandler(this.ConfigureNIDAQmxButton_Click);
            // 
            // CoDASyncWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 679);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.NIDAQmxControlBox);
            this.Controls.Add(this.CorvusControlGroup);
            this.Name = "CoDASyncWindow";
            this.Text = "CoDASync";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownHandler);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpHandler);
            this.CorvusControlGroup.ResumeLayout(false);
            this.CorvusControlGroup.PerformLayout();
            this.CorvusConfigurationBox.ResumeLayout(false);
            this.CorvusConfigurationBox.PerformLayout();
            this.OriginSettingsBox.ResumeLayout(false);
            this.OriginSettingsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OriginY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OriginX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OriginZ)).EndInit();
            this.RelativeMoveGroupBox.ResumeLayout(false);
            this.RelativeMoveGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RMoveY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RMoveX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RMoveZ)).EndInit();
            this.NIDAQmxControlBox.ResumeLayout(false);
            this.NIDAQmxConfigurationBox.ResumeLayout(false);
            this.NIDAQmxConfigurationBox.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox CorvusControlGroup;
        private System.Windows.Forms.Button SendCommandButton;
        private System.Windows.Forms.TextBox VenusCommandBox;
        private System.Windows.Forms.NumericUpDown RMoveX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown RMoveZ;
        private System.Windows.Forms.NumericUpDown RMoveY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox RelativeMoveGroupBox;
        private System.Windows.Forms.Button moveAllAxes;
        private System.Windows.Forms.Button moveNegativeZ;
        private System.Windows.Forms.Button movePositiveZ;
        private System.Windows.Forms.Button moveNegativeY;
        private System.Windows.Forms.Button movePositiveY;
        private System.Windows.Forms.Button moveNegativeX;
        private System.Windows.Forms.Button movePositiveX;
        private System.Windows.Forms.GroupBox OriginSettingsBox;
        private System.Windows.Forms.NumericUpDown OriginY;
        private System.Windows.Forms.Button MoveToOriginButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown OriginX;
        private System.Windows.Forms.Button SetOriginButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown OriginZ;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox CorvusConfigurationBox;
        private System.Windows.Forms.Button ConnectPortButton;
        private System.Windows.Forms.TextBox BaudRateTextBox;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button ExecListingButton;
        private System.Windows.Forms.TextBox ListingFilename;
        private System.Windows.Forms.GroupBox NIDAQmxControlBox;
        private System.Windows.Forms.Button MoveToPositionButton;
        private System.Windows.Forms.GroupBox NIDAQmxConfigurationBox;
        private System.Windows.Forms.Button ConfigureNIDAQmxButton;
        private System.Windows.Forms.TextBox ChannelTextBox;
        private System.Windows.Forms.TextBox DeviceNameTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel CorvusStatusLabel;
    }
}