namespace SerialGUISample
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serial = new System.IO.Ports.SerialPort(this.components);
            this.getIOtimer = new System.Windows.Forms.Timer(this.components);
            this.InputBox1 = new System.Windows.Forms.TextBox();
            this.OutputBox1 = new System.Windows.Forms.NumericUpDown();
            this.Send1 = new System.Windows.Forms.Button();
            this.Send2 = new System.Windows.Forms.Button();
            this.Get1 = new System.Windows.Forms.Button();
            this.Get2 = new System.Windows.Forms.Button();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.InputBox2 = new System.Windows.Forms.TextBox();
            this.OutputBox2 = new System.Windows.Forms.NumericUpDown();
            this.StartStopButton = new System.Windows.Forms.Button();
            this.AutoRefresh = new System.Windows.Forms.CheckBox();
            this.outputLabel1 = new System.Windows.Forms.Label();
            this.outputLabel2 = new System.Windows.Forms.Label();
            this.inputLabel1 = new System.Windows.Forms.Label();
            this.inputLabel2 = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.panelOutputs = new System.Windows.Forms.Panel();
            this.sliderValue2 = new System.Windows.Forms.Label();
            this.sliderValue1 = new System.Windows.Forms.Label();
            this.Slider2 = new System.Windows.Forms.TrackBar();
            this.Slider1 = new System.Windows.Forms.TrackBar();
            this.panelInputs = new System.Windows.Forms.Panel();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.panelSpeedometers = new System.Windows.Forms.Panel();
            this.speedometerLabel2 = new System.Windows.Forms.Label();
            this.speedometerLabel1 = new System.Windows.Forms.Label();
            this.speedometer2 = new SerialGUISample.Speedometer();
            this.speedometer1 = new SerialGUISample.Speedometer();
            this.panelStopwatch = new System.Windows.Forms.Panel();
            this.stopwatchTitleLabel = new System.Windows.Forms.Label();
            this.resetStopwatchButton = new System.Windows.Forms.Button();
            this.stopwatchLabel = new System.Windows.Forms.Label();
            this.panelCartDisplay = new System.Windows.Forms.Panel();
            this.cartDisplayLabel = new System.Windows.Forms.Label();
            this.cartDisplay = new SerialGUISample.CartDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.OutputBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputBox2)).BeginInit();
            this.panelOutputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Slider2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Slider1)).BeginInit();
            this.panelInputs.SuspendLayout();
            this.panelStatus.SuspendLayout();
            this.panelSpeedometers.SuspendLayout();
            this.panelStopwatch.SuspendLayout();
            this.panelCartDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // serial
            // 
            this.serial.PortName = "COM3";
            // 
            // getIOtimer
            // 
            this.getIOtimer.Enabled = true;
            this.getIOtimer.Interval = 10;
            this.getIOtimer.Tick += new System.EventHandler(this.getIOtimer_Tick);
            // 
            // InputBox1
            // 
            this.InputBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.InputBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.InputBox1.ForeColor = System.Drawing.Color.White;
            this.InputBox1.Location = new System.Drawing.Point(15, 40);
            this.InputBox1.Name = "InputBox1";
            this.InputBox1.Size = new System.Drawing.Size(250, 27);
            this.InputBox1.TabIndex = 0;
            this.InputBox1.Text = "○ Not Detected";
            this.InputBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OutputBox1
            // 
            this.OutputBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.OutputBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutputBox1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.OutputBox1.ForeColor = System.Drawing.Color.White;
            this.OutputBox1.Location = new System.Drawing.Point(15, 55);
            this.OutputBox1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.OutputBox1.Minimum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.OutputBox1.Name = "OutputBox1";
            this.OutputBox1.Size = new System.Drawing.Size(120, 32);
            this.OutputBox1.TabIndex = 3;
            this.OutputBox1.Value = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.OutputBox1.ValueChanged += new System.EventHandler(this.OutputBox1_ValueChanged);
            // 
            // Send1
            // 
            this.Send1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.Send1.FlatAppearance.BorderSize = 0;
            this.Send1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Send1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.Send1.ForeColor = System.Drawing.Color.White;
            this.Send1.Location = new System.Drawing.Point(540, 51);
            this.Send1.Name = "Send1";
            this.Send1.Size = new System.Drawing.Size(90, 40);
            this.Send1.TabIndex = 4;
            this.Send1.Text = "SEND";
            this.Send1.UseVisualStyleBackColor = false;
            this.Send1.Click += new System.EventHandler(this.Send1_Click);
            // 
            // Send2
            // 
            this.Send2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.Send2.FlatAppearance.BorderSize = 0;
            this.Send2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Send2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.Send2.ForeColor = System.Drawing.Color.White;
            this.Send2.Location = new System.Drawing.Point(540, 131);
            this.Send2.Name = "Send2";
            this.Send2.Size = new System.Drawing.Size(90, 36);
            this.Send2.TabIndex = 4;
            this.Send2.Text = "SEND";
            this.Send2.UseVisualStyleBackColor = false;
            this.Send2.Click += new System.EventHandler(this.Send2_Click);
            // 
            // Get1
            // 
            this.Get1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(58)))), ((int)(((byte)(183)))));
            this.Get1.FlatAppearance.BorderSize = 0;
            this.Get1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Get1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.Get1.ForeColor = System.Drawing.Color.White;
            this.Get1.Location = new System.Drawing.Point(275, 38);
            this.Get1.Name = "Get1";
            this.Get1.Size = new System.Drawing.Size(85, 32);
            this.Get1.TabIndex = 4;
            this.Get1.Text = "READ";
            this.Get1.UseVisualStyleBackColor = false;
            this.Get1.Visible = false;
            this.Get1.Click += new System.EventHandler(this.Get1_Click);
            // 
            // Get2
            // 
            this.Get2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(58)))), ((int)(((byte)(183)))));
            this.Get2.FlatAppearance.BorderSize = 0;
            this.Get2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Get2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.Get2.ForeColor = System.Drawing.Color.White;
            this.Get2.Location = new System.Drawing.Point(275, 88);
            this.Get2.Name = "Get2";
            this.Get2.Size = new System.Drawing.Size(85, 32);
            this.Get2.TabIndex = 4;
            this.Get2.Text = "READ";
            this.Get2.UseVisualStyleBackColor = false;
            this.Get2.Visible = false;
            this.Get2.Click += new System.EventHandler(this.Get2_Click);
            // 
            // statusBox
            // 
            this.statusBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(125)))), ((int)(((byte)(139)))));
            this.statusBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.statusBox.ForeColor = System.Drawing.Color.White;
            this.statusBox.Location = new System.Drawing.Point(15, 45);
            this.statusBox.Multiline = true;
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(620, 30);
            this.statusBox.TabIndex = 5;
            this.statusBox.Text = "Disconnected";
            this.statusBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // InputBox2
            // 
            this.InputBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.InputBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputBox2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.InputBox2.ForeColor = System.Drawing.Color.White;
            this.InputBox2.Location = new System.Drawing.Point(15, 90);
            this.InputBox2.Name = "InputBox2";
            this.InputBox2.Size = new System.Drawing.Size(250, 27);
            this.InputBox2.TabIndex = 0;
            this.InputBox2.Text = "○ Not Detected";
            this.InputBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OutputBox2
            // 
            this.OutputBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.OutputBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutputBox2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.OutputBox2.ForeColor = System.Drawing.Color.White;
            this.OutputBox2.Location = new System.Drawing.Point(15, 135);
            this.OutputBox2.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.OutputBox2.Name = "OutputBox2";
            this.OutputBox2.Size = new System.Drawing.Size(120, 29);
            this.OutputBox2.TabIndex = 3;
            this.OutputBox2.ValueChanged += new System.EventHandler(this.OutputBox2_ValueChanged);
            // 
            // StartStopButton
            // 
            this.StartStopButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.StartStopButton.FlatAppearance.BorderSize = 0;
            this.StartStopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartStopButton.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.StartStopButton.ForeColor = System.Drawing.Color.White;
            this.StartStopButton.Location = new System.Drawing.Point(20, 80);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(650, 65);
            this.StartStopButton.TabIndex = 6;
            this.StartStopButton.Text = "▶ START";
            this.StartStopButton.UseVisualStyleBackColor = false;
            this.StartStopButton.Click += new System.EventHandler(this.StartStop_Click);
            // 
            // AutoRefresh
            // 
            this.AutoRefresh.AutoSize = true;
            this.AutoRefresh.Checked = true;
            this.AutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoRefresh.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.AutoRefresh.ForeColor = System.Drawing.Color.White;
            this.AutoRefresh.Location = new System.Drawing.Point(15, 12);
            this.AutoRefresh.Name = "AutoRefresh";
            this.AutoRefresh.Size = new System.Drawing.Size(121, 24);
            this.AutoRefresh.TabIndex = 7;
            this.AutoRefresh.Text = "Auto Refresh";
            this.AutoRefresh.UseVisualStyleBackColor = true;
            this.AutoRefresh.CheckedChanged += new System.EventHandler(this.AutoRefresh_CheckedChanged);
            // 
            // outputLabel1
            // 
            this.outputLabel1.AutoSize = true;
            this.outputLabel1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.outputLabel1.ForeColor = System.Drawing.Color.White;
            this.outputLabel1.Location = new System.Drawing.Point(12, 20);
            this.outputLabel1.Name = "outputLabel1";
            this.outputLabel1.Size = new System.Drawing.Size(220, 25);
            this.outputLabel1.TabIndex = 8;
            this.outputLabel1.Text = "Motor Speed (127-255):";
            // 
            // outputLabel2
            // 
            this.outputLabel2.AutoSize = true;
            this.outputLabel2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.outputLabel2.ForeColor = System.Drawing.Color.White;
            this.outputLabel2.Location = new System.Drawing.Point(12, 105);
            this.outputLabel2.Name = "outputLabel2";
            this.outputLabel2.Size = new System.Drawing.Size(260, 21);
            this.outputLabel2.TabIndex = 8;
            this.outputLabel2.Text = "Turning Speed (0-MotorDiff):";
            // 
            // inputLabel1
            // 
            this.inputLabel1.AutoSize = true;
            this.inputLabel1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.inputLabel1.ForeColor = System.Drawing.Color.White;
            this.inputLabel1.Location = new System.Drawing.Point(12, 15);
            this.inputLabel1.Name = "inputLabel1";
            this.inputLabel1.Size = new System.Drawing.Size(62, 20);
            this.inputLabel1.TabIndex = 8;
            this.inputLabel1.Text = "Input 1:";
            // 
            // inputLabel2
            // 
            this.inputLabel2.AutoSize = true;
            this.inputLabel2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.inputLabel2.ForeColor = System.Drawing.Color.White;
            this.inputLabel2.Location = new System.Drawing.Point(12, 65);
            this.inputLabel2.Name = "inputLabel2";
            this.inputLabel2.Size = new System.Drawing.Size(62, 20);
            this.inputLabel2.TabIndex = 8;
            this.inputLabel2.Text = "Input 2:";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(200, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(347, 41);
            this.titleLabel.TabIndex = 9;
            this.titleLabel.Text = "🤖 Robot Controller Pro";
            // 
            // panelOutputs
            // 
            this.panelOutputs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panelOutputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOutputs.Controls.Add(this.sliderValue2);
            this.panelOutputs.Controls.Add(this.sliderValue1);
            this.panelOutputs.Controls.Add(this.Slider2);
            this.panelOutputs.Controls.Add(this.Slider1);
            this.panelOutputs.Controls.Add(this.outputLabel1);
            this.panelOutputs.Controls.Add(this.outputLabel2);
            this.panelOutputs.Controls.Add(this.OutputBox1);
            this.panelOutputs.Controls.Add(this.OutputBox2);
            this.panelOutputs.Controls.Add(this.Send1);
            this.panelOutputs.Controls.Add(this.Send2);
            this.panelOutputs.Location = new System.Drawing.Point(20, 165);
            this.panelOutputs.Name = "panelOutputs";
            this.panelOutputs.Size = new System.Drawing.Size(650, 190);
            this.panelOutputs.TabIndex = 10;
            // 
            // sliderValue2
            // 
            this.sliderValue2.AutoSize = true;
            this.sliderValue2.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.sliderValue2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(0)))));
            this.sliderValue2.Location = new System.Drawing.Point(485, 137);
            this.sliderValue2.Name = "sliderValue2";
            this.sliderValue2.Size = new System.Drawing.Size(21, 25);
            this.sliderValue2.TabIndex = 11;
            this.sliderValue2.Text = "0";
            // 
            // sliderValue1
            // 
            this.sliderValue1.AutoSize = true;
            this.sliderValue1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.sliderValue1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(0)))));
            this.sliderValue1.Location = new System.Drawing.Point(485, 57);
            this.sliderValue1.Name = "sliderValue1";
            this.sliderValue1.Size = new System.Drawing.Size(39, 25);
            this.sliderValue1.TabIndex = 11;
            this.sliderValue1.Text = "127";
            // 
            // Slider2
            // 
            this.Slider2.Location = new System.Drawing.Point(145, 131);
            this.Slider2.Maximum = 128;
            this.Slider2.Name = "Slider2";
            this.Slider2.Size = new System.Drawing.Size(330, 45);
            this.Slider2.TabIndex = 10;
            this.Slider2.TickFrequency = 10;
            this.Slider2.Scroll += new System.EventHandler(this.Slider2_Scroll);
            // 
            // Slider1
            // 
            this.Slider1.Location = new System.Drawing.Point(145, 51);
            this.Slider1.Maximum = 255;
            this.Slider1.Minimum = 127;
            this.Slider1.Name = "Slider1";
            this.Slider1.Size = new System.Drawing.Size(330, 45);
            this.Slider1.TabIndex = 10;
            this.Slider1.TickFrequency = 10;
            this.Slider1.Value = 127;
            this.Slider1.Scroll += new System.EventHandler(this.Slider1_Scroll);
            // 
            // panelInputs
            // 
            this.panelInputs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panelInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInputs.Controls.Add(this.inputLabel1);
            this.panelInputs.Controls.Add(this.inputLabel2);
            this.panelInputs.Controls.Add(this.InputBox1);
            this.panelInputs.Controls.Add(this.InputBox2);
            this.panelInputs.Controls.Add(this.Get1);
            this.panelInputs.Controls.Add(this.Get2);
            this.panelInputs.Location = new System.Drawing.Point(20, 375);
            this.panelInputs.Name = "panelInputs";
            this.panelInputs.Size = new System.Drawing.Size(380, 140);
            this.panelInputs.TabIndex = 11;
            // 
            // panelStatus
            // 
            this.panelStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStatus.Controls.Add(this.AutoRefresh);
            this.panelStatus.Controls.Add(this.statusBox);
            this.panelStatus.Location = new System.Drawing.Point(20, 650);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(650, 90);
            this.panelStatus.TabIndex = 12;
            // 
            // panelSpeedometers
            // 
            this.panelSpeedometers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panelSpeedometers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSpeedometers.Controls.Add(this.speedometerLabel2);
            this.panelSpeedometers.Controls.Add(this.speedometerLabel1);
            this.panelSpeedometers.Controls.Add(this.speedometer2);
            this.panelSpeedometers.Controls.Add(this.speedometer1);
            this.panelSpeedometers.Location = new System.Drawing.Point(420, 375);
            this.panelSpeedometers.Name = "panelSpeedometers";
            this.panelSpeedometers.Size = new System.Drawing.Size(250, 140);
            this.panelSpeedometers.TabIndex = 13;
            // 
            // speedometerLabel2
            // 
            this.speedometerLabel2.AutoSize = true;
            this.speedometerLabel2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.speedometerLabel2.ForeColor = System.Drawing.Color.White;
            this.speedometerLabel2.Location = new System.Drawing.Point(145, 10);
            this.speedometerLabel2.Name = "speedometerLabel2";
            this.speedometerLabel2.Size = new System.Drawing.Size(64, 19);
            this.speedometerLabel2.TabIndex = 8;
            this.speedometerLabel2.Text = "Motor 2";
            // 
            // speedometerLabel1
            // 
            this.speedometerLabel1.AutoSize = true;
            this.speedometerLabel1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.speedometerLabel1.ForeColor = System.Drawing.Color.White;
            this.speedometerLabel1.Location = new System.Drawing.Point(30, 10);
            this.speedometerLabel1.Name = "speedometerLabel1";
            this.speedometerLabel1.Size = new System.Drawing.Size(64, 19);
            this.speedometerLabel1.TabIndex = 8;
            this.speedometerLabel1.Text = "Motor 1";
            // 
            // speedometer2
            // 
            this.speedometer2.Location = new System.Drawing.Point(130, 35);
            this.speedometer2.Name = "speedometer2";
            this.speedometer2.Size = new System.Drawing.Size(100, 100);
            this.speedometer2.Speed = 0;
            this.speedometer2.TabIndex = 0;
            // 
            // speedometer1
            // 
            this.speedometer1.Location = new System.Drawing.Point(15, 35);
            this.speedometer1.Name = "speedometer1";
            this.speedometer1.Size = new System.Drawing.Size(100, 100);
            this.speedometer1.Speed = 0;
            this.speedometer1.TabIndex = 0;
            // 
            // panelStopwatch
            // 
            this.panelStopwatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panelStopwatch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelStopwatch.Controls.Add(this.stopwatchTitleLabel);
            this.panelStopwatch.Controls.Add(this.resetStopwatchButton);
            this.panelStopwatch.Controls.Add(this.stopwatchLabel);
            this.panelStopwatch.Location = new System.Drawing.Point(20, 535);
            this.panelStopwatch.Name = "panelStopwatch";
            this.panelStopwatch.Size = new System.Drawing.Size(650, 100);
            this.panelStopwatch.TabIndex = 14;
            // 
            // stopwatchTitleLabel
            // 
            this.stopwatchTitleLabel.AutoSize = true;
            this.stopwatchTitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.stopwatchTitleLabel.ForeColor = System.Drawing.Color.White;
            this.stopwatchTitleLabel.Location = new System.Drawing.Point(12, 12);
            this.stopwatchTitleLabel.Name = "stopwatchTitleLabel";
            this.stopwatchTitleLabel.Size = new System.Drawing.Size(127, 21);
            this.stopwatchTitleLabel.TabIndex = 8;
            this.stopwatchTitleLabel.Text = "⏱ Lap Timer:";
            // 
            // resetStopwatchButton
            // 
            this.resetStopwatchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.resetStopwatchButton.FlatAppearance.BorderSize = 0;
            this.resetStopwatchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetStopwatchButton.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.resetStopwatchButton.ForeColor = System.Drawing.Color.White;
            this.resetStopwatchButton.Location = new System.Drawing.Point(480, 38);
            this.resetStopwatchButton.Name = "resetStopwatchButton";
            this.resetStopwatchButton.Size = new System.Drawing.Size(150, 50);
            this.resetStopwatchButton.TabIndex = 1;
            this.resetStopwatchButton.Text = "🔄 RESET";
            this.resetStopwatchButton.UseVisualStyleBackColor = false;
            this.resetStopwatchButton.Click += new System.EventHandler(this.ResetStopwatch_Click);
            // 
            // stopwatchLabel
            // 
            this.stopwatchLabel.Font = new System.Drawing.Font("Consolas", 40F, System.Drawing.FontStyle.Bold);
            this.stopwatchLabel.ForeColor = System.Drawing.Color.White;
            this.stopwatchLabel.Location = new System.Drawing.Point(15, 38);
            this.stopwatchLabel.Name = "stopwatchLabel";
            this.stopwatchLabel.Size = new System.Drawing.Size(450, 55);
            this.stopwatchLabel.TabIndex = 0;
            this.stopwatchLabel.Text = "00:00.0";
            this.stopwatchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelCartDisplay
            // 
            this.panelCartDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.panelCartDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCartDisplay.Controls.Add(this.cartDisplayLabel);
            this.panelCartDisplay.Controls.Add(this.cartDisplay);
            this.panelCartDisplay.Location = new System.Drawing.Point(690, 80);
            this.panelCartDisplay.Name = "panelCartDisplay";
            this.panelCartDisplay.Size = new System.Drawing.Size(580, 660);
            this.panelCartDisplay.TabIndex = 15;
            // 
            // cartDisplayLabel
            // 
            this.cartDisplayLabel.AutoSize = true;
            this.cartDisplayLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.cartDisplayLabel.ForeColor = System.Drawing.Color.White;
            this.cartDisplayLabel.Location = new System.Drawing.Point(210, 15);
            this.cartDisplayLabel.Name = "cartDisplayLabel";
            this.cartDisplayLabel.Size = new System.Drawing.Size(159, 30);
            this.cartDisplayLabel.TabIndex = 1;
            this.cartDisplayLabel.Text = "🚗 Cart View";
            // 
            // cartDisplay
            // 
            this.cartDisplay.IsRunning = false;
            this.cartDisplay.Location = new System.Drawing.Point(15, 55);
            this.cartDisplay.Motor1Speed = 127;
            this.cartDisplay.Motor2Speed = 127;
            this.cartDisplay.Name = "cartDisplay";
            this.cartDisplay.Size = new System.Drawing.Size(550, 590);
            this.cartDisplay.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(1280, 760);
            this.Controls.Add(this.panelCartDisplay);
            this.Controls.Add(this.panelStopwatch);
            this.Controls.Add(this.panelSpeedometers);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.panelInputs);
            this.Controls.Add(this.panelOutputs);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.StartStopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Robot Control Pro - Curtin University";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.OutputBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputBox2)).EndInit();
            this.panelOutputs.ResumeLayout(false);
            this.panelOutputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Slider2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Slider1)).EndInit();
            this.panelInputs.ResumeLayout(false);
            this.panelInputs.PerformLayout();
            this.panelStatus.ResumeLayout(false);
            this.panelStatus.PerformLayout();
            this.panelSpeedometers.ResumeLayout(false);
            this.panelSpeedometers.PerformLayout();
            this.panelStopwatch.ResumeLayout(false);
            this.panelStopwatch.PerformLayout();
            this.panelCartDisplay.ResumeLayout(false);
            this.panelCartDisplay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer getIOtimer;
        private System.Windows.Forms.TextBox InputBox1;
        private System.Windows.Forms.NumericUpDown OutputBox1;
        private System.IO.Ports.SerialPort serial;
        private System.Windows.Forms.Button Send1;
        private System.Windows.Forms.Button Send2;
        private System.Windows.Forms.Button Get1;
        private System.Windows.Forms.Button Get2;
        private System.Windows.Forms.TextBox statusBox;
        private System.Windows.Forms.TextBox InputBox2;
        private System.Windows.Forms.NumericUpDown OutputBox2;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.CheckBox AutoRefresh;
        private System.Windows.Forms.Label outputLabel1;
        private System.Windows.Forms.Label outputLabel2;
        private System.Windows.Forms.Label inputLabel1;
        private System.Windows.Forms.Label inputLabel2;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel panelOutputs;
        private System.Windows.Forms.Panel panelInputs;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.TrackBar Slider1;
        private System.Windows.Forms.TrackBar Slider2;
        private System.Windows.Forms.Label sliderValue1;
        private System.Windows.Forms.Label sliderValue2;
        private System.Windows.Forms.Panel panelSpeedometers;
        private Speedometer speedometer1;
        private Speedometer speedometer2;
        private System.Windows.Forms.Label speedometerLabel1;
        private System.Windows.Forms.Label speedometerLabel2;
        private System.Windows.Forms.Panel panelStopwatch;
        private System.Windows.Forms.Label stopwatchLabel;
        private System.Windows.Forms.Button resetStopwatchButton;
        private System.Windows.Forms.Label stopwatchTitleLabel;
        private System.Windows.Forms.Panel panelCartDisplay;
        private System.Windows.Forms.Label cartDisplayLabel;
        private CartDisplay cartDisplay;
    }
}