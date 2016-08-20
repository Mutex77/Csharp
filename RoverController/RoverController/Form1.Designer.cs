namespace RoverController
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
            this.pbStream = new System.Windows.Forms.PictureBox();
            this.btnTCPConnect = new System.Windows.Forms.Button();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.pbRightSliderTrack = new System.Windows.Forms.PictureBox();
            this.pbLeftSliderTrack = new System.Windows.Forms.PictureBox();
            this.pbLeftSlider = new System.Windows.Forms.PictureBox();
            this.pbRightSlider = new System.Windows.Forms.PictureBox();
            this.btnLEDToggle = new System.Windows.Forms.Button();
            this.btnVideoToggle = new System.Windows.Forms.Button();
            this.btnIRLEDToggle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbStream)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRightSliderTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeftSliderTrack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeftSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRightSlider)).BeginInit();
            this.SuspendLayout();
            // 
            // pbStream
            // 
            this.pbStream.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbStream.BackColor = System.Drawing.SystemColors.ControlText;
            this.pbStream.Location = new System.Drawing.Point(252, 74);
            this.pbStream.Name = "pbStream";
            this.pbStream.Size = new System.Drawing.Size(1280, 960);
            this.pbStream.TabIndex = 0;
            this.pbStream.TabStop = false;
            // 
            // btnTCPConnect
            // 
            this.btnTCPConnect.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnTCPConnect.Location = new System.Drawing.Point(398, 1053);
            this.btnTCPConnect.Name = "btnTCPConnect";
            this.btnTCPConnect.Size = new System.Drawing.Size(180, 73);
            this.btnTCPConnect.TabIndex = 3;
            this.btnTCPConnect.Text = "Connect";
            this.btnTCPConnect.UseVisualStyleBackColor = true;
            this.btnTCPConnect.Click += new System.EventHandler(this.btnTCPTest_Click);
            // 
            // btnShutdown
            // 
            this.btnShutdown.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnShutdown.BackColor = System.Drawing.Color.Gainsboro;
            this.btnShutdown.Location = new System.Drawing.Point(1206, 1053);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(180, 73);
            this.btnShutdown.TabIndex = 7;
            this.btnShutdown.Text = "Shutdown Robot";
            this.btnShutdown.UseVisualStyleBackColor = false;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // pbRightSliderTrack
            // 
            this.pbRightSliderTrack.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pbRightSliderTrack.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pbRightSliderTrack.Location = new System.Drawing.Point(1642, 300);
            this.pbRightSliderTrack.Name = "pbRightSliderTrack";
            this.pbRightSliderTrack.Size = new System.Drawing.Size(130, 600);
            this.pbRightSliderTrack.TabIndex = 8;
            this.pbRightSliderTrack.TabStop = false;
            // 
            // pbLeftSliderTrack
            // 
            this.pbLeftSliderTrack.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbLeftSliderTrack.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pbLeftSliderTrack.Location = new System.Drawing.Point(12, 300);
            this.pbLeftSliderTrack.Name = "pbLeftSliderTrack";
            this.pbLeftSliderTrack.Size = new System.Drawing.Size(130, 600);
            this.pbLeftSliderTrack.TabIndex = 9;
            this.pbLeftSliderTrack.TabStop = false;
            // 
            // pbLeftSlider
            // 
            this.pbLeftSlider.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbLeftSlider.BackColor = System.Drawing.SystemColors.HotTrack;
            this.pbLeftSlider.Location = new System.Drawing.Point(7, 558);
            this.pbLeftSlider.Name = "pbLeftSlider";
            this.pbLeftSlider.Size = new System.Drawing.Size(140, 85);
            this.pbLeftSlider.TabIndex = 10;
            this.pbLeftSlider.TabStop = false;
            // 
            // pbRightSlider
            // 
            this.pbRightSlider.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pbRightSlider.BackColor = System.Drawing.SystemColors.HotTrack;
            this.pbRightSlider.Location = new System.Drawing.Point(1637, 558);
            this.pbRightSlider.Name = "pbRightSlider";
            this.pbRightSlider.Size = new System.Drawing.Size(140, 85);
            this.pbRightSlider.TabIndex = 10;
            this.pbRightSlider.TabStop = false;
            // 
            // btnLEDToggle
            // 
            this.btnLEDToggle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnLEDToggle.BackColor = System.Drawing.Color.Gainsboro;
            this.btnLEDToggle.Location = new System.Drawing.Point(802, 1053);
            this.btnLEDToggle.Name = "btnLEDToggle";
            this.btnLEDToggle.Size = new System.Drawing.Size(180, 73);
            this.btnLEDToggle.TabIndex = 11;
            this.btnLEDToggle.Text = "LEDs";
            this.btnLEDToggle.UseVisualStyleBackColor = false;
            this.btnLEDToggle.Click += new System.EventHandler(this.btnLEDToggle_Click);
            // 
            // btnVideoToggle
            // 
            this.btnVideoToggle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnVideoToggle.BackColor = System.Drawing.Color.Gainsboro;
            this.btnVideoToggle.Location = new System.Drawing.Point(1004, 1053);
            this.btnVideoToggle.Name = "btnVideoToggle";
            this.btnVideoToggle.Size = new System.Drawing.Size(180, 73);
            this.btnVideoToggle.TabIndex = 12;
            this.btnVideoToggle.Text = "Video";
            this.btnVideoToggle.UseVisualStyleBackColor = false;
            this.btnVideoToggle.Click += new System.EventHandler(this.btnVideoToggle_Click);
            // 
            // btnIRLEDToggle
            // 
            this.btnIRLEDToggle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnIRLEDToggle.BackColor = System.Drawing.Color.Gainsboro;
            this.btnIRLEDToggle.Location = new System.Drawing.Point(600, 1053);
            this.btnIRLEDToggle.Name = "btnIRLEDToggle";
            this.btnIRLEDToggle.Size = new System.Drawing.Size(180, 73);
            this.btnIRLEDToggle.TabIndex = 13;
            this.btnIRLEDToggle.Text = "IR LEDs";
            this.btnIRLEDToggle.UseVisualStyleBackColor = false;
            this.btnIRLEDToggle.Click += new System.EventHandler(this.btnIRLEDToggle_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1784, 1161);
            this.Controls.Add(this.btnIRLEDToggle);
            this.Controls.Add(this.btnVideoToggle);
            this.Controls.Add(this.btnLEDToggle);
            this.Controls.Add(this.btnShutdown);
            this.Controls.Add(this.btnTCPConnect);
            this.Controls.Add(this.pbLeftSlider);
            this.Controls.Add(this.pbRightSlider);
            this.Controls.Add(this.pbRightSliderTrack);
            this.Controls.Add(this.pbLeftSliderTrack);
            this.Controls.Add(this.pbStream);
            this.Name = "Form1";
            this.Text = "Rover Controller";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pbStream)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRightSliderTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeftSliderTrack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLeftSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRightSlider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbStream;
        private System.Windows.Forms.Button btnTCPConnect;
        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.PictureBox pbRightSliderTrack;
        private System.Windows.Forms.PictureBox pbLeftSliderTrack;
        private System.Windows.Forms.PictureBox pbLeftSlider;
        private System.Windows.Forms.PictureBox pbRightSlider;
        private System.Windows.Forms.Button btnLEDToggle;
        private System.Windows.Forms.Button btnVideoToggle;
        private System.Windows.Forms.Button btnIRLEDToggle;
    }
}

