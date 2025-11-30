namespace CricketService.Desktop
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Button btnStartAll;
        private Button btnStartFrontend;
        private Button btnStartApi;
        private Button btnStopAll;
        private Button btnStopFrontend;
        private Button btnStopApi;
        private Button btnExit;
        private Label lblTitle;
        private Label lblDescription;
        private Label lblStatus;
        private Panel panelHeader;
        private Panel panelButtons;
        private Panel panelFooter;
        private Label lblVersion;
        private ProgressBar progressBar;
        private CheckBox chkDevMode;

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
            this.btnStartAll = new Button();
            this.btnStartFrontend = new Button();
            this.btnStartApi = new Button();
            this.btnStopAll = new Button();
            this.btnStopFrontend = new Button();
            this.btnStopApi = new Button();
            this.btnExit = new Button();
            this.lblTitle = new Label();
            this.lblDescription = new Label();
            this.lblStatus = new Label();
            this.panelHeader = new Panel();
            this.panelButtons = new Panel();
            this.panelFooter = new Panel();
            this.lblVersion = new Label();
            this.progressBar = new ProgressBar();
            this.chkDevMode = new CheckBox();
            this.panelHeader.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = Color.FromArgb(33, 150, 243);
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Controls.Add(this.lblDescription);
            this.panelHeader.Dock = DockStyle.Top;
            this.panelHeader.Location = new Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new Size(584, 90);
            this.panelHeader.TabIndex = 0;
            
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(280, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "🏏 CricketService Launcher";
            
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblDescription.ForeColor = Color.FromArgb(230, 230, 230);
            this.lblDescription.Location = new Point(25, 55);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(350, 20);
            this.lblDescription.TabIndex = 1;
            this.lblDescription.Text = "Choose how to start your Cricket Service application";
            
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = Color.Transparent;
            this.panelButtons.Controls.Add(this.btnStartAll);
            this.panelButtons.Controls.Add(this.btnStopAll);
            this.panelButtons.Controls.Add(this.btnStartFrontend);
            this.panelButtons.Controls.Add(this.btnStopFrontend);
            this.panelButtons.Controls.Add(this.btnStartApi);
            this.panelButtons.Controls.Add(this.btnStopApi);
            this.panelButtons.Dock = DockStyle.Fill;
            this.panelButtons.Location = new Point(0, 90);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Padding = new Padding(30, 30, 30, 20);
            this.panelButtons.Size = new Size(584, 230);
            this.panelButtons.TabIndex = 1;
            
            // 
            // btnStartAll
            // 
            this.btnStartAll.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.btnStartAll.BackColor = Color.FromArgb(76, 175, 80);
            this.btnStartAll.FlatAppearance.BorderSize = 0;
            this.btnStartAll.FlatAppearance.MouseOverBackColor = Color.FromArgb(102, 187, 106);
            this.btnStartAll.FlatStyle = FlatStyle.Flat;
            this.btnStartAll.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnStartAll.ForeColor = Color.White;
            this.btnStartAll.Location = new Point(30, 30);
            this.btnStartAll.Name = "btnStartAll";
            this.btnStartAll.Size = new Size(410, 50);
            this.btnStartAll.TabIndex = 0;
            this.btnStartAll.Text = "🚀 Start All Services";
            this.btnStartAll.UseVisualStyleBackColor = false;
            this.btnStartAll.Click += new EventHandler(this.btnStartAll_Click);
            this.btnStartAll.MouseEnter += (s, e) => {
                this.btnStartAll.BackColor = Color.FromArgb(102, 187, 106);
            };
            this.btnStartAll.MouseLeave += (s, e) => {
                this.btnStartAll.BackColor = Color.FromArgb(76, 175, 80);
            };
            
            // 
            // btnStopAll
            // 
            this.btnStopAll.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnStopAll.BackColor = Color.FromArgb(244, 67, 54);
            this.btnStopAll.FlatAppearance.BorderSize = 0;
            this.btnStopAll.FlatAppearance.MouseOverBackColor = Color.FromArgb(229, 57, 53);
            this.btnStopAll.FlatStyle = FlatStyle.Flat;
            this.btnStopAll.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnStopAll.ForeColor = Color.White;
            this.btnStopAll.Location = new Point(450, 30);
            this.btnStopAll.Name = "btnStopAll";
            this.btnStopAll.Size = new Size(104, 50);
            this.btnStopAll.TabIndex = 1;
            this.btnStopAll.Text = "⏹ Stop";
            this.btnStopAll.UseVisualStyleBackColor = false;
            this.btnStopAll.Click += new EventHandler(this.btnStopAll_Click);
            this.btnStopAll.MouseEnter += (s, e) => {
                this.btnStopAll.BackColor = Color.FromArgb(229, 57, 53);
            };
            this.btnStopAll.MouseLeave += (s, e) => {
                this.btnStopAll.BackColor = Color.FromArgb(244, 67, 54);
            };
            
            // 
            // btnStartFrontend
            // 
            this.btnStartFrontend.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.btnStartFrontend.BackColor = Color.FromArgb(33, 150, 243);
            this.btnStartFrontend.FlatAppearance.BorderSize = 0;
            this.btnStartFrontend.FlatAppearance.MouseOverBackColor = Color.FromArgb(66, 165, 245);
            this.btnStartFrontend.FlatStyle = FlatStyle.Flat;
            this.btnStartFrontend.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnStartFrontend.ForeColor = Color.White;
            this.btnStartFrontend.Location = new Point(30, 95);
            this.btnStartFrontend.Name = "btnStartFrontend";
            this.btnStartFrontend.Size = new Size(410, 50);
            this.btnStartFrontend.TabIndex = 2;
            this.btnStartFrontend.Text = "🎨 Start Frontend Only";
            this.btnStartFrontend.UseVisualStyleBackColor = false;
            this.btnStartFrontend.Click += new EventHandler(this.btnStartFrontend_Click);
            this.btnStartFrontend.MouseEnter += (s, e) => {
                this.btnStartFrontend.BackColor = Color.FromArgb(66, 165, 245);
            };
            this.btnStartFrontend.MouseLeave += (s, e) => {
                this.btnStartFrontend.BackColor = Color.FromArgb(33, 150, 243);
            };
            
            // 
            // btnStopFrontend
            // 
            this.btnStopFrontend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnStopFrontend.BackColor = Color.FromArgb(244, 67, 54);
            this.btnStopFrontend.FlatAppearance.BorderSize = 0;
            this.btnStopFrontend.FlatAppearance.MouseOverBackColor = Color.FromArgb(229, 57, 53);
            this.btnStopFrontend.FlatStyle = FlatStyle.Flat;
            this.btnStopFrontend.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnStopFrontend.ForeColor = Color.White;
            this.btnStopFrontend.Location = new Point(450, 95);
            this.btnStopFrontend.Name = "btnStopFrontend";
            this.btnStopFrontend.Size = new Size(104, 50);
            this.btnStopFrontend.TabIndex = 3;
            this.btnStopFrontend.Text = "⏹ Stop";
            this.btnStopFrontend.UseVisualStyleBackColor = false;
            this.btnStopFrontend.Click += new EventHandler(this.btnStopFrontend_Click);
            this.btnStopFrontend.MouseEnter += (s, e) => {
                this.btnStopFrontend.BackColor = Color.FromArgb(229, 57, 53);
            };
            this.btnStopFrontend.MouseLeave += (s, e) => {
                this.btnStopFrontend.BackColor = Color.FromArgb(244, 67, 54);
            };
            
            // 
            // btnStartApi
            // 
            this.btnStartApi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.btnStartApi.BackColor = Color.FromArgb(255, 152, 0);
            this.btnStartApi.FlatAppearance.BorderSize = 0;
            this.btnStartApi.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 183, 77);
            this.btnStartApi.FlatStyle = FlatStyle.Flat;
            this.btnStartApi.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnStartApi.ForeColor = Color.White;
            this.btnStartApi.Location = new Point(30, 160);
            this.btnStartApi.Name = "btnStartApi";
            this.btnStartApi.Size = new Size(410, 50);
            this.btnStartApi.TabIndex = 4;
            this.btnStartApi.Text = "⚙️ Start API Only";
            this.btnStartApi.UseVisualStyleBackColor = false;
            this.btnStartApi.Click += new EventHandler(this.btnStartApi_Click);
            this.btnStartApi.MouseEnter += (s, e) => {
                this.btnStartApi.BackColor = Color.FromArgb(255, 183, 77);
            };
            this.btnStartApi.MouseLeave += (s, e) => {
                this.btnStartApi.BackColor = Color.FromArgb(255, 152, 0);
            };
            
            // 
            // btnStopApi
            // 
            this.btnStopApi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnStopApi.BackColor = Color.FromArgb(244, 67, 54);
            this.btnStopApi.FlatAppearance.BorderSize = 0;
            this.btnStopApi.FlatAppearance.MouseOverBackColor = Color.FromArgb(229, 57, 53);
            this.btnStopApi.FlatStyle = FlatStyle.Flat;
            this.btnStopApi.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnStopApi.ForeColor = Color.White;
            this.btnStopApi.Location = new Point(450, 160);
            this.btnStopApi.Name = "btnStopApi";
            this.btnStopApi.Size = new Size(104, 50);
            this.btnStopApi.TabIndex = 5;
            this.btnStopApi.Text = "⏹ Stop";
            this.btnStopApi.UseVisualStyleBackColor = false;
            this.btnStopApi.Click += new EventHandler(this.btnStopApi_Click);
            this.btnStopApi.MouseEnter += (s, e) => {
                this.btnStopApi.BackColor = Color.FromArgb(229, 57, 53);
            };
            this.btnStopApi.MouseLeave += (s, e) => {
                this.btnStopApi.BackColor = Color.FromArgb(244, 67, 54);
            };
            
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = Color.FromArgb(250, 250, 250);
            this.panelFooter.BorderStyle = BorderStyle.FixedSingle;
            this.panelFooter.Controls.Add(this.lblStatus);
            this.panelFooter.Controls.Add(this.progressBar);
            this.panelFooter.Controls.Add(this.btnExit);
            this.panelFooter.Controls.Add(this.lblVersion);
            this.panelFooter.Controls.Add(this.chkDevMode);
            this.panelFooter.Dock = DockStyle.Bottom;
            this.panelFooter.Location = new Point(0, 270);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new Size(584, 50);
            this.panelFooter.TabIndex = 2;
            
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblStatus.ForeColor = Color.FromArgb(76, 76, 76);
            this.lblStatus.Location = new Point(15, 15);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new Size(90, 19);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Status: Ready";
            
            // 
            // progressBar
            // 
            this.progressBar.Location = new Point(120, 17);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new Size(200, 15);
            this.progressBar.Style = ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 1;
            this.progressBar.Visible = false;
            
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            this.lblVersion.ForeColor = Color.FromArgb(150, 150, 150);
            this.lblVersion.Location = new Point(450, 20);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new Size(45, 13);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "v1.0.0";
            
            // 
            // btnExit
            // 
            this.btnExit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.btnExit.BackColor = Color.FromArgb(244, 67, 54);
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = FlatStyle.Flat;
            this.btnExit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            this.btnExit.ForeColor = Color.White;
            this.btnExit.Location = new Point(510, 10);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(60, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.btnExit.MouseEnter += (s, e) => this.btnExit.BackColor = Color.FromArgb(229, 57, 53);
            this.btnExit.MouseLeave += (s, e) => this.btnExit.BackColor = Color.FromArgb(244, 67, 54);
            
            // 
            // chkDevMode
            // 
            this.chkDevMode.AutoSize = true;
            this.chkDevMode.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.chkDevMode.ForeColor = Color.FromArgb(76, 76, 76);
            this.chkDevMode.Location = new Point(330, 15);
            this.chkDevMode.Name = "chkDevMode";
            this.chkDevMode.Size = new Size(110, 19);
            this.chkDevMode.TabIndex = 4;
            this.chkDevMode.Text = "Show Terminals";
            this.chkDevMode.UseVisualStyleBackColor = true;
            this.chkDevMode.CheckedChanged += new EventHandler(this.chkDevMode_CheckedChanged);
            
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(240, 248, 255);
            this.ClientSize = new Size(584, 320);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelFooter);
            this.Name = "MainForm";
            this.Text = "CricketService Launcher";
            this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}