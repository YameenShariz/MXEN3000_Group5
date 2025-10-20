// Curtin University
// Mechatronics Engineering
// Serial GUI - Dual Speed Control with Real-Time Cart Visualization
// This application provides a visual interface to control a line-following robot
// with real-time feedback through speedometers and cart visualization

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;

namespace SerialGUISample
{
    public partial class Form1 : Form
    {
        // ========================== GLOBAL VARIABLES ==========================

        // Serial communication flag
        bool runSerial = true;

        // Robot running state
        bool isRunning = false;

        // Sensor input states (0 = black background, 1 = white line)
        int Input1 = 0;
        int Input2 = 0;

        // Motor speed values received from Arduino (0-255)
        int Motor1Speed = 0;
        int Motor2Speed = 0;

        // Serial communication buffers (4 bytes: START, COMMAND, DATA, CHECKSUM)
        byte[] Outputs = new byte[4];
        byte[] Inputs = new byte[4];

        // Stopwatch for lap timing
        private Stopwatch stopwatch = new Stopwatch();
        private Timer stopwatchTimer = new Timer();

        // ========================== PROTOCOL CONSTANTS ==========================

        const byte START = 255;              // Start byte for serial packets
        const byte ZERO = 0;                 // Zero constant
        const byte INPUT1 = 0;               // Command ID for sensor 1
        const byte INPUT2 = 1;               // Command ID for sensor 2
        const byte SPEED_OUTPUT = 2;         // Command ID for motor speed
        const byte ONOFF_CONTROL = 3;        // Command ID for on/off control
        const byte MOTOR1_SPEED = 4;         // Command ID for motor 1 feedback
        const byte MOTOR2_SPEED = 5;         // Command ID for motor 2 feedback
        const byte TURNING_SPEED = 6;        // Command ID for turning speed

        // ========================== CONSTRUCTOR ==========================

        public Form1()
        {
            InitializeComponent();

            // Configure stopwatch timer (updates every 100ms)
            stopwatchTimer.Interval = 100;
            stopwatchTimer.Tick += StopwatchTimer_Tick;

            // Configure Motor Speed control (Slider 1)
            OutputBox1.Minimum = 127;        // Idle speed
            OutputBox1.Maximum = 255;        // Maximum speed
            OutputBox1.DecimalPlaces = 0;
            OutputBox1.Increment = 1M;
            OutputBox1.Value = 127;

            // Configure Turning Speed control (Slider 2)
            OutputBox2.Minimum = 0;
            OutputBox2.Maximum = 128;
            OutputBox2.DecimalPlaces = 0;
            OutputBox2.Increment = 1M;
            OutputBox2.Value = 0;
            OutputBox2.Visible = true;

            // Show turning speed controls
            Send2.Visible = true;
            outputLabel2.Visible = true;
            Slider2.Visible = true;
            sliderValue2.Visible = true;

            // Configure motor speed slider
            Slider1.Minimum = 127;
            Slider1.Maximum = 255;
            Slider1.Value = 127;
            Slider1.TickFrequency = 10;

            // Configure turning speed slider
            Slider2.Minimum = 0;
            Slider2.Maximum = 128;
            Slider2.Value = 0;
            Slider2.TickFrequency = 10;

            // Configure status display
            statusBox.Text = "Disconnected";
            statusBox.ReadOnly = true;
            statusBox.TextAlign = HorizontalAlignment.Center;

            // Configure sensor input displays (read-only)
            InputBox1.ReadOnly = true;
            InputBox1.TextAlign = HorizontalAlignment.Center;
            InputBox2.ReadOnly = true;
            InputBox2.TextAlign = HorizontalAlignment.Center;

            // Initialize stopwatch display
            stopwatchLabel.Text = "00:00.0";

            // Refresh custom controls
            speedometer1.Invalidate();
            speedometer2.Invalidate();
            cartDisplay.Invalidate();

            // Attempt to open serial port
            if (runSerial)
            {
                if (!serial.IsOpen)
                {
                    try
                    {
                        serial.Open();
                        statusBox.Text = "Connected - Ready";
                        statusBox.ForeColor = Color.White;
                        statusBox.BackColor = Color.FromArgb(46, 125, 50);  // Green
                    }
                    catch (Exception ex)
                    {
                        statusBox.Text = $"ERROR: {ex.Message}";
                        statusBox.ForeColor = Color.White;
                        statusBox.BackColor = Color.FromArgb(198, 40, 40);  // Red
                    }
                }
            }
        }

        // ========================== STOPWATCH METHODS ==========================

        /// <summary>
        /// Updates the stopwatch display every 100ms
        /// </summary>
        private void StopwatchTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            stopwatchLabel.Text = $"{elapsed.Minutes:00}:{elapsed.Seconds:00}.{elapsed.Milliseconds / 100}";
        }

        // ========================== SERIAL COMMUNICATION ==========================

        /// <summary>
        /// Sends a command to the Arduino with checksum validation
        /// </summary>
        /// <param name="PORT">Command type (SPEED_OUTPUT, TURNING_SPEED, etc.)</param>
        /// <param name="DATA">Data value to send (0-255)</param>
        private void sendIO(byte PORT, byte DATA)
        {
            Outputs[0] = START;                      // Start byte (255)
            Outputs[1] = PORT;                       // Command byte
            Outputs[2] = DATA;                       // Data byte
            Outputs[3] = (byte)(START + PORT + DATA); // Checksum

            if (serial.IsOpen)
                serial.Write(Outputs, 0, 4);
        }

        // ========================== UI HELPER METHODS ==========================

        /// <summary>
        /// Generates a green color based on slider value percentage
        /// </summary>
        private Color GetSliderColor(int value, int max)
        {
            float percent = (float)value / max;
            int g = 100 + (int)(155 * percent);
            return Color.FromArgb(0, g, 0);
        }

        // ========================== SLIDER EVENT HANDLERS ==========================

        /// <summary>
        /// Motor Speed slider change handler
        /// </summary>
        private void Slider1_Scroll(object sender, EventArgs e)
        {
            OutputBox1.Value = Slider1.Value;
            sliderValue1.Text = $"{Slider1.Value}";
            sliderValue1.ForeColor = GetSliderColor(Slider1.Value, 255);

            // Update turning speed limits based on motor speed
            UpdateTurningSpeedLimits();

            // Send command immediately if robot is running
            if (isRunning)
            {
                SendSpeedCommand();
            }
        }

        /// <summary>
        /// Turning Speed slider change handler
        /// </summary>
        private void Slider2_Scroll(object sender, EventArgs e)
        {
            OutputBox2.Value = Slider2.Value;
            sliderValue2.Text = $"{Slider2.Value}";
            sliderValue2.ForeColor = GetSliderColor(Slider2.Value, (int)OutputBox1.Value);

            // Send command immediately if robot is running
            if (isRunning)
            {
                SendTurningSpeedCommand();
            }
        }

        /// <summary>
        /// Updates the maximum turning speed based on current motor speed
        /// Prevents motor speed from going below idle (127)
        /// </summary>
        private void UpdateTurningSpeedLimits()
        {
            int motorSpeed = (int)OutputBox1.Value;
            int newMax = motorSpeed - 127;  // Maximum turning = motor speed - idle speed

            Slider2.Maximum = newMax;
            OutputBox2.Maximum = newMax;

            // Clamp current value if it exceeds new maximum
            if (Slider2.Value > newMax)
            {
                Slider2.Value = newMax;
                OutputBox2.Value = newMax;
            }

            sliderValue2.Text = $"{Slider2.Value}";
            sliderValue2.ForeColor = GetSliderColor(Slider2.Value, newMax);
        }

        /// <summary>
        /// Refreshes the cart visualization display
        /// </summary>
        private void UpdateCartDisplay()
        {
            cartDisplay.Motor1Speed = Motor1Speed;
            cartDisplay.Motor2Speed = Motor2Speed;
            cartDisplay.IsRunning = isRunning;
            cartDisplay.Invalidate();
        }

        // ========================== NUMERIC BOX EVENT HANDLERS ==========================

        /// <summary>
        /// Motor Speed numeric box change handler
        /// </summary>
        private void OutputBox1_ValueChanged(object sender, EventArgs e)
        {
            int value = (int)OutputBox1.Value;

            Slider1.Value = value;
            sliderValue1.Text = $"{value}";
            sliderValue1.ForeColor = GetSliderColor(value, 255);

            UpdateTurningSpeedLimits();

            if (isRunning)
            {
                SendSpeedCommand();
            }
        }

        /// <summary>
        /// Turning Speed numeric box change handler
        /// </summary>
        private void OutputBox2_ValueChanged(object sender, EventArgs e)
        {
            int value = (int)OutputBox2.Value;

            Slider2.Value = value;
            sliderValue2.Text = $"{value}";
            sliderValue2.ForeColor = GetSliderColor(value, (int)OutputBox1.Value);

            if (isRunning)
            {
                SendTurningSpeedCommand();
            }
        }

        // ========================== COMMAND SENDING METHODS ==========================

        /// <summary>
        /// Sends motor speed command to Arduino
        /// </summary>
        private void SendSpeedCommand()
        {
            int hwValue = (int)OutputBox1.Value;
            if (hwValue < 127) hwValue = 127;  // Enforce minimum (idle speed)
            if (hwValue > 255) hwValue = 255;  // Enforce maximum

            sendIO(SPEED_OUTPUT, (byte)hwValue);
        }

        /// <summary>
        /// Sends turning speed command to Arduino
        /// </summary>
        private void SendTurningSpeedCommand()
        {
            int hwValue = (int)OutputBox2.Value;
            if (hwValue < 0) hwValue = 0;
            int maxAllowed = (int)OutputBox1.Value - 127;
            if (hwValue > maxAllowed) hwValue = maxAllowed;

            sendIO(TURNING_SPEED, (byte)hwValue);
        }

        // ========================== BUTTON EVENT HANDLERS ==========================

        /// <summary>
        /// Start/Stop button handler - toggles robot operation
        /// </summary>
        private void StartStop_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                // STARTING ROBOT
                SendSpeedCommand();
                SendTurningSpeedCommand();

                sendIO(ONOFF_CONTROL, 1);  // Send ON command
                isRunning = true;
                StartStopButton.Text = "⏹ STOP";
                StartStopButton.BackColor = Color.FromArgb(198, 40, 40);  // Red
                StartStopButton.ForeColor = Color.White;

                int motorSpeed = (int)OutputBox1.Value;
                int turnSpeed = (int)OutputBox2.Value;
                statusBox.Text = $"Robot Running | Speed: {motorSpeed} | Turn: {turnSpeed} ✓";
                statusBox.BackColor = Color.FromArgb(46, 125, 50);  // Green
                statusBox.ForeColor = Color.White;

                // Start stopwatch
                stopwatch.Restart();
                stopwatchTimer.Start();
                stopwatchLabel.ForeColor = Color.FromArgb(76, 175, 80);  // Light green

                UpdateCartDisplay();
            }
            else
            {
                // STOPPING ROBOT
                sendIO(ONOFF_CONTROL, 0);  // Send OFF command
                isRunning = false;
                StartStopButton.Text = "▶ START";
                StartStopButton.BackColor = Color.FromArgb(46, 125, 50);  // Green
                StartStopButton.ForeColor = Color.White;

                // Stop stopwatch
                stopwatch.Stop();
                stopwatchTimer.Stop();
                stopwatchLabel.ForeColor = Color.FromArgb(255, 152, 0);  // Orange

                TimeSpan finalTime = stopwatch.Elapsed;
                statusBox.Text = $"Robot Stopped ⏸ | Time: {finalTime.Minutes:00}:{finalTime.Seconds:00}.{finalTime.Milliseconds / 100}";
                statusBox.BackColor = Color.FromArgb(255, 152, 0);  // Orange
                statusBox.ForeColor = Color.White;

                UpdateCartDisplay();
            }
        }

        /// <summary>
        /// Reset stopwatch button handler
        /// </summary>
        private void ResetStopwatch_Click(object sender, EventArgs e)
        {
            stopwatch.Reset();
            stopwatchLabel.Text = "00:00.0";
            stopwatchLabel.ForeColor = Color.White;
            statusBox.Text = "Stopwatch Reset";
            statusBox.BackColor = Color.FromArgb(96, 125, 139);  // Gray-blue
        }

        /// <summary>
        /// Manual send button for motor speed
        /// </summary>
        private void Send1_Click(object sender, EventArgs e)
        {
            SendSpeedCommand();

            int hwValue = (int)OutputBox1.Value;
            statusBox.Text = $"Motor Speed Set: {hwValue}/255";
            statusBox.BackColor = Color.FromArgb(33, 150, 243);  // Blue
            statusBox.ForeColor = Color.White;
        }

        /// <summary>
        /// Manual send button for turning speed
        /// </summary>
        private void Send2_Click(object sender, EventArgs e)
        {
            SendTurningSpeedCommand();

            int hwValue = (int)OutputBox2.Value;
            statusBox.Text = $"Turning Speed Set: {hwValue}";
            statusBox.BackColor = Color.FromArgb(33, 150, 243);  // Blue
            statusBox.ForeColor = Color.White;
        }

        /// <summary>
        /// Get button handlers (sensors update automatically via timer)
        /// </summary>
        private void Get1_Click(object sender, EventArgs e)
        {
            statusBox.Text = "Sensors updating automatically ♻";
            statusBox.BackColor = Color.FromArgb(103, 58, 183);  // Purple
        }

        private void Get2_Click(object sender, EventArgs e)
        {
            statusBox.Text = "Sensors updating automatically ♻";
            statusBox.BackColor = Color.FromArgb(103, 58, 183);  // Purple
        }

        // ========================== SERIAL RECEIVE HANDLER ==========================

        /// <summary>
        /// Timer event that continuously reads incoming serial data from Arduino
        /// Processes sensor states and motor speed feedback
        /// </summary>
        private void getIOtimer_Tick(object sender, EventArgs e)
        {
            // Check if serial port is open and has data available
            if (serial.IsOpen && serial.BytesToRead >= 4)
            {
                Inputs[0] = (byte)serial.ReadByte();  // Read start byte

                // Verify start byte
                if (Inputs[0] == START)
                {
                    Inputs[1] = (byte)serial.ReadByte();  // Command
                    Inputs[2] = (byte)serial.ReadByte();  // Data
                    Inputs[3] = (byte)serial.ReadByte();  // Checksum

                    // Validate checksum
                    byte checkSum = (byte)(Inputs[0] + Inputs[1] + Inputs[2]);

                    if (Inputs[3] == checkSum)
                    {
                        // Process command based on type
                        switch (Inputs[1])
                        {
                            case INPUT1:
                                // Sensor 1 state update
                                Input1 = Inputs[2];
                                InputBox1.Text = (Input1 == 1) ? "● WHITE LINE" : "○ Black Background";
                                InputBox1.BackColor = (Input1 == 1) ? Color.White : Color.Black;
                                InputBox1.ForeColor = (Input1 == 1) ? Color.Black : Color.White;
                                break;

                            case INPUT2:
                                // Sensor 2 state update
                                Input2 = Inputs[2];
                                InputBox2.Text = (Input2 == 1) ? "● WHITE LINE" : "○ Black Background";
                                InputBox2.BackColor = (Input2 == 1) ? Color.White : Color.Black;
                                InputBox2.ForeColor = (Input2 == 1) ? Color.Black : Color.White;
                                break;

                            case MOTOR1_SPEED:
                                // Motor 1 speed feedback (0-255 converted to 0-100%)
                                Motor1Speed = Inputs[2];
                                int motor1Percent = (int)Math.Round(Motor1Speed / 2.55);
                                speedometer1.Speed = motor1Percent;
                                speedometer1.Invalidate();
                                UpdateCartDisplay();
                                break;

                            case MOTOR2_SPEED:
                                // Motor 2 speed feedback (0-255 converted to 0-100%)
                                Motor2Speed = Inputs[2];
                                int motor2Percent = (int)Math.Round(Motor2Speed / 2.55);
                                speedometer2.Speed = motor2Percent;
                                speedometer2.Invalidate();
                                UpdateCartDisplay();
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Auto-refresh checkbox handler
        /// </summary>
        private void AutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            getIOtimer.Enabled = AutoRefresh.Checked;
            if (AutoRefresh.Checked)
            {
                statusBox.Text = "Auto-refresh enabled ♻";
            }
            else
            {
                statusBox.Text = "Auto-refresh disabled";
            }
        }
    }

    // ========================== CUSTOM SPEEDOMETER CONTROL ==========================

    /// <summary>
    /// Custom control that displays a circular speedometer gauge (0-100%)
    /// </summary>
    public class Speedometer : Control
    {
        private int speed = 0;

        public int Speed
        {
            get { return speed; }
            set
            {
                speed = Math.Max(0, Math.Min(100, value));  // Clamp to 0-100
                Invalidate();  // Trigger repaint
            }
        }

        public Speedometer()
        {
            // Enable double buffering to prevent flicker
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;  // Smooth edges

            // Calculate dimensions
            int centerX = Width / 2;
            int centerY = Height / 2;
            int radius = Math.Min(Width, Height) / 2 - 10;

            // Draw outer circle background
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(45, 45, 45)))
            {
                g.FillEllipse(bgBrush, centerX - radius, centerY - radius, radius * 2, radius * 2);
            }

            // Draw colored arc segments (270 degrees total, starting at 135°)
            using (Pen arcPen = new Pen(Color.Empty, 8))
            {
                for (int i = 0; i <= 100; i += 2)
                {
                    // Calculate color: red (0%) -> yellow (50%) -> green (100%)
                    Color segmentColor;
                    if (i <= 50)
                    {
                        int r = 255;
                        int gr = (int)(i * 5.1);
                        segmentColor = Color.FromArgb(r, gr, 0);  // Red to Yellow
                    }
                    else
                    {
                        int r = (int)(255 - ((i - 50) * 5.1));
                        segmentColor = Color.FromArgb(r, 255, 0);  // Yellow to Green
                    }

                    // Dim inactive segments
                    arcPen.Color = i <= speed ? segmentColor : Color.FromArgb(60, 60, 60);
                    float startAngle = 135 + (i * 2.7f);  // Map 0-100 to 135-405 degrees
                    g.DrawArc(arcPen, centerX - radius + 4, centerY - radius + 4, (radius - 4) * 2, (radius - 4) * 2, startAngle, 2.7f);
                }
            }

            // Draw inner circle
            int innerRadius = radius - 20;
            using (SolidBrush innerBrush = new SolidBrush(Color.FromArgb(35, 35, 35)))
            {
                g.FillEllipse(innerBrush, centerX - innerRadius, centerY - innerRadius, innerRadius * 2, innerRadius * 2);
            }

            // Draw speed value text
            using (Font speedFont = new Font("Segoe UI", 18, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                string speedText = speed.ToString();
                SizeF textSize = g.MeasureString(speedText, speedFont);
                g.DrawString(speedText, speedFont, textBrush, centerX - textSize.Width / 2, centerY - textSize.Height / 2);
            }

            // Draw percentage symbol
            using (Font labelFont = new Font("Segoe UI", 8, FontStyle.Regular))
            using (SolidBrush labelBrush = new SolidBrush(Color.LightGray))
            {
                string label = "%";
                SizeF labelSize = g.MeasureString(label, labelFont);
                g.DrawString(label, labelFont, labelBrush, centerX - labelSize.Width / 2, centerY + 12);
            }
        }
    }

    // ========================== CUSTOM CART DISPLAY CONTROL ==========================

    /// <summary>
    /// Custom control that displays a visual representation of the robot cart
    /// Shows motor speeds, wheel colors, direction arrows, and movement status
    /// </summary>
    public class CartDisplay : Control
    {
        private int motor1Speed = 127;
        private int motor2Speed = 127;
        private bool isRunning = false;

        public int Motor1Speed
        {
            get { return motor1Speed; }
            set { motor1Speed = value; }
        }

        public int Motor2Speed
        {
            get { return motor2Speed; }
            set { motor2Speed = value; }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        public CartDisplay()
        {
            // Enable double buffering to prevent flicker
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int centerX = Width / 2;
            int centerY = Height / 2;

            // Define cart dimensions
            int cartWidth = 240;
            int cartHeight = 360;
            Rectangle cartRect = new Rectangle(centerX - cartWidth / 2, centerY - cartHeight / 2, cartWidth, cartHeight);

            // Draw shadow for depth effect
            Rectangle shadowRect = new Rectangle(cartRect.X + 8, cartRect.Y + 8, cartRect.Width, cartRect.Height);
            using (GraphicsPath shadowPath = GetRoundedRect(shadowRect, 20))
            {
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                {
                    g.FillPath(shadowBrush, shadowPath);
                }
            }

            // Draw cart body with gradient (blue when running, gray when stopped)
            using (GraphicsPath path = GetRoundedRect(cartRect, 20))
            {
                Color cartColor1 = isRunning ? Color.FromArgb(33, 150, 243) : Color.FromArgb(70, 70, 70);
                Color cartColor2 = isRunning ? Color.FromArgb(21, 101, 192) : Color.FromArgb(50, 50, 50);

                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    cartRect, cartColor1, cartColor2, LinearGradientMode.Vertical))
                {
                    g.FillPath(gradientBrush, path);
                }

                using (Pen cartPen = new Pen(Color.FromArgb(200, 200, 200), 4))
                {
                    g.DrawPath(cartPen, path);
                }
            }

            // Draw "FRONT" label
            using (Font frontFont = new Font("Segoe UI", 20, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(180, 255, 193, 7)))
            {
                string frontText = "FRONT";
                SizeF textSize = g.MeasureString(frontText, frontFont);
                float textX = centerX - textSize.Width / 2;
                float textY = centerY - cartHeight / 2 + 25;

                Rectangle labelRect = new Rectangle((int)textX - 8, (int)textY - 4, (int)textSize.Width + 16, (int)textSize.Height + 8);
                using (GraphicsPath labelPath = GetRoundedRect(labelRect, 6))
                {
                    g.FillPath(bgBrush, labelPath);
                }
                g.DrawString(frontText, frontFont, textBrush, textX, textY);
            }

            // Draw line sensors (two circles at front)
            int sensorSize = 20;
            int sensorY = centerY - cartHeight / 2 + 70;

            // Left sensor
            Rectangle leftSensorRect = new Rectangle(centerX - 60, sensorY, sensorSize, sensorSize);
            using (LinearGradientBrush sensorBrush = new LinearGradientBrush(
                leftSensorRect, Color.FromArgb(180, 180, 180), Color.FromArgb(120, 120, 120), LinearGradientMode.Vertical))
            {
                g.FillEllipse(sensorBrush, leftSensorRect);
            }
            using (Pen sensorPen = new Pen(Color.FromArgb(100, 100, 100), 2))
            {
                g.DrawEllipse(sensorPen, leftSensorRect);
            }

            // Right sensor
            Rectangle rightSensorRect = new Rectangle(centerX + 40, sensorY, sensorSize, sensorSize);
            using (LinearGradientBrush sensorBrush = new LinearGradientBrush(
                rightSensorRect, Color.FromArgb(180, 180, 180), Color.FromArgb(120, 120, 120), LinearGradientMode.Vertical))
            {
                g.FillEllipse(sensorBrush, rightSensorRect);
            }
            using (Pen sensorPen = new Pen(Color.FromArgb(100, 100, 100), 2))
            {
                g.DrawEllipse(sensorPen, rightSensorRect);
            }

            // Draw directional arrow (shows movement direction)
            int speedDiff = motor2Speed - motor1Speed;
            DrawDirectionalArrow(g, centerX, centerY - 30, speedDiff, isRunning);

            // Draw wheels with speed indicators
            int wheelWidth = 45;
            int wheelHeight = 140;
            int wheelOffsetX = cartWidth / 2 + 15;

            // Left wheel
            Rectangle leftWheel = new Rectangle(centerX - wheelOffsetX - wheelWidth, centerY - wheelHeight / 2, wheelWidth, wheelHeight);
            DrawWheel(g, leftWheel, motor1Speed, true);

            // Right wheel
            Rectangle rightWheel = new Rectangle(centerX + wheelOffsetX, centerY - wheelHeight / 2, wheelWidth, wheelHeight);
            DrawWheel(g, rightWheel, motor2Speed, false);

            // Draw "BACK" label
            using (Font backFont = new Font("Segoe UI", 20, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(180, 244, 67, 54)))
            {
                string backText = "BACK";
                SizeF textSize = g.MeasureString(backText, backFont);
                float textX = centerX - textSize.Width / 2;
                float textY = centerY + cartHeight / 2 - 50;

                Rectangle labelRect = new Rectangle((int)textX - 8, (int)textY - 4, (int)textSize.Width + 16, (int)textSize.Height + 8);
                using (GraphicsPath labelPath = GetRoundedRect(labelRect, 6))
                {
                    g.FillPath(bgBrush, labelPath);
                }
                g.DrawString(backText, backFont, textBrush, textX, textY);
            }

            // Draw speed indicators on wheels
            DrawSpeedIndicator(g, leftWheel, motor1Speed, "L");
            DrawSpeedIndicator(g, rightWheel, motor2Speed, "R");

            // Draw movement status text when running
            if (isRunning)
            {
                string statusText = GetMovementStatus(motor1Speed, motor2Speed);
                using (Font statusFont = new Font("Segoe UI", 18, FontStyle.Bold))
                using (SolidBrush statusBrush = new SolidBrush(Color.FromArgb(76, 175, 80)))
                {
                    SizeF statusSize = g.MeasureString(statusText, statusFont);
                    g.DrawString(statusText, statusFont, statusBrush,
                        centerX - statusSize.Width / 2, cartRect.Bottom + 30);
                }
            }
        }

        /// <summary>
        /// Creates a rounded rectangle path for smooth corners
        /// </summary>
        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Draws a wheel with color based on speed
        /// Green = forward, Red = reverse, Gray = idle
        /// </summary>
        private void DrawWheel(Graphics g, Rectangle wheelRect, int speed, bool isLeft)
        {
            Color wheelColor;

            if (speed == 127)
            {
                // Idle speed - gray
                wheelColor = Color.FromArgb(60, 60, 60);
            }
            else if (speed > 127)
            {
                // Forward - green gradient based on speed
                float speedPercent = (speed - 127) / 128f;
                int greenValue = (int)(100 + (155 * speedPercent));
                wheelColor = Color.FromArgb(0, greenValue, 0);
            }
            else
            {
                // Reverse - red gradient based on speed
                float speedPercent = (127 - speed) / 127f;
                int redValue = (int)(100 + (155 * speedPercent));
                wheelColor = Color.FromArgb(redValue, 0, 0);
            }

            // Override color to gray if robot is not running
            if (!isRunning)
            {
                wheelColor = Color.FromArgb(60, 60, 60);
            }

            // Draw wheel with gradient
            using (GraphicsPath wheelPath = GetRoundedRect(wheelRect, 8))
            {
                Color wheelColor2 = Color.FromArgb(wheelColor.R / 2, wheelColor.G / 2, wheelColor.B / 2);
                using (LinearGradientBrush wheelBrush = new LinearGradientBrush(
                    wheelRect, wheelColor, wheelColor2, LinearGradientMode.Vertical))
                {
                    g.FillPath(wheelBrush, wheelPath);
                }

                using (Pen wheelPen = new Pen(Color.White, 3))
                {
                    g.DrawPath(wheelPen, wheelPath);
                }
            }

            // Draw motion lines to indicate direction of rotation
            if (isRunning && speed != 127)
            {
                using (Pen motionPen = new Pen(Color.White, 2))
                {
                    int lineCount = 5;
                    int lineSpacing = wheelRect.Height / (lineCount + 1);

                    for (int i = 1; i <= lineCount; i++)
                    {
                        int y = wheelRect.Top + (i * lineSpacing);
                        int lineLength = 10;

                        if (speed > 127)
                        {
                            // Forward motion lines
                            g.DrawLine(motionPen,
                                wheelRect.Left + 8, y,
                                wheelRect.Left + 8 + lineLength, y);
                        }
                        else
                        {
                            // Reverse motion lines
                            g.DrawLine(motionPen,
                                wheelRect.Right - 8, y,
                                wheelRect.Right - 8 - lineLength, y);
                        }
                    }
                }
            }

            // Draw tire treads
            int tireCount = 4;
            int tireSpacing = wheelRect.Height / (tireCount + 1);
            using (Pen tirePen = new Pen(Color.FromArgb(40, 40, 40), 2))
            {
                for (int i = 1; i <= tireCount; i++)
                {
                    int y = wheelRect.Top + (i * tireSpacing);
                    g.DrawLine(tirePen, wheelRect.Left + 5, y, wheelRect.Right - 5, y);
                }
            }
        }

        /// <summary>
        /// Draws speed value text on the wheel
        /// </summary>
        private void DrawSpeedIndicator(Graphics g, Rectangle wheelRect, int speed, string label)
        {
            using (Font speedFont = new Font("Segoe UI", 11, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                string speedText = $"{label}\n{speed}";
                SizeF textSize = g.MeasureString(speedText, speedFont);

                float textX = wheelRect.X + (wheelRect.Width - textSize.Width) / 2;
                float textY = wheelRect.Y + (wheelRect.Height - textSize.Height) / 2;

                // Draw semi-transparent background for readability
                using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0)))
                {
                    Rectangle bgRect = new Rectangle((int)textX - 4, (int)textY - 2, (int)textSize.Width + 8, (int)textSize.Height + 4);
                    using (GraphicsPath bgPath = GetRoundedRect(bgRect, 4))
                    {
                        g.FillPath(bgBrush, bgPath);
                    }
                }

                g.DrawString(speedText, speedFont, textBrush, textX, textY);
            }
        }

        /// <summary>
        /// Draws directional arrow showing robot movement direction
        /// Straight arrow = forward, curved = turning
        /// </summary>
        private void DrawDirectionalArrow(Graphics g, int centerX, int centerY, int speedDiff, bool running)
        {
            if (!running)
            {
                // Draw gray straight arrow when stopped
                using (Pen arrowPen = new Pen(Color.Gray, 4))
                {
                    arrowPen.EndCap = LineCap.ArrowAnchor;
                    arrowPen.CustomEndCap = new AdjustableArrowCap(8, 8);
                    g.DrawLine(arrowPen, centerX, centerY + 15, centerX, centerY - 35);
                }
                return;
            }

            Color arrowColor = Color.FromArgb(76, 175, 80);  // Green when running

            if (Math.Abs(speedDiff) < 5)
            {
                // Going straight - draw straight arrow
                using (Pen arrowPen = new Pen(arrowColor, 6))
                {
                    arrowPen.EndCap = LineCap.ArrowAnchor;
                    arrowPen.CustomEndCap = new AdjustableArrowCap(10, 10);
                    g.DrawLine(arrowPen, centerX, centerY + 20, centerX, centerY - 40);
                }
            }
            else
            {
                // Turning - draw curved arrow
                float curvature = Math.Min(Math.Abs(speedDiff) / 25f, 4f);
                int direction = speedDiff > 0 ? -1 : 1;  // -1 = left, 1 = right

                using (Pen arrowPen = new Pen(arrowColor, 6))
                {
                    arrowPen.EndCap = LineCap.ArrowAnchor;
                    arrowPen.CustomEndCap = new AdjustableArrowCap(10, 10);

                    // Create bezier curve for smooth turn indication
                    PointF p1 = new PointF(centerX, centerY + 20);
                    PointF p2 = new PointF(centerX + (direction * curvature * 15), centerY - 10);
                    PointF p3 = new PointF(centerX + (direction * curvature * 20), centerY - 40);

                    g.DrawBezier(arrowPen, p1, p2, p2, p3);
                }
            }
        }

        /// <summary>
        /// Returns movement status text based on motor speeds
        /// </summary>
        private string GetMovementStatus(int m1, int m2)
        {
            if (m1 == 127 && m2 == 127) return "⏸ STOPPED";
            if (Math.Abs(m2 - m1) < 5) return "⬆ STRAIGHT";
            if (m2 > m1) return "↰ TURNING LEFT";
            if (m1 > m2) return "↱ TURNING RIGHT";
            return "● MOVING";
        }
    }
}