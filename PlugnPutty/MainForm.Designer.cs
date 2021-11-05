namespace PlugnPutty
{
	partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.LogListBox = new System.Windows.Forms.ListBox();
            this.EnableCheckBox = new System.Windows.Forms.CheckBox();
            this.AutocloseCheckBox = new System.Windows.Forms.CheckBox();
            this.TrayNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.StartMinimizedCheckBox = new System.Windows.Forms.CheckBox();
            this.ReopenCheckBox = new System.Windows.Forms.CheckBox();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.AutostartCheckBox = new System.Windows.Forms.CheckBox();
            this.MainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // LogListBox
            // 
            this.LogListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTableLayoutPanel.SetColumnSpan(this.LogListBox, 5);
            this.LogListBox.FormattingEnabled = true;
            this.LogListBox.Location = new System.Drawing.Point(3, 26);
            this.LogListBox.Name = "LogListBox";
            this.LogListBox.ScrollAlwaysVisible = true;
            this.LogListBox.Size = new System.Drawing.Size(366, 121);
            this.LogListBox.TabIndex = 0;
            // 
            // EnableCheckBox
            // 
            this.EnableCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EnableCheckBox.AutoSize = true;
            this.EnableCheckBox.Location = new System.Drawing.Point(3, 3);
            this.EnableCheckBox.Name = "EnableCheckBox";
            this.EnableCheckBox.Size = new System.Drawing.Size(59, 17);
            this.EnableCheckBox.TabIndex = 1;
            this.EnableCheckBox.Text = "Enable";
            this.EnableCheckBox.UseVisualStyleBackColor = true;
            this.EnableCheckBox.CheckedChanged += new System.EventHandler(this.EnableCheckBox_CheckedChanged);
            // 
            // AutocloseCheckBox
            // 
            this.AutocloseCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AutocloseCheckBox.AutoSize = true;
            this.AutocloseCheckBox.Location = new System.Drawing.Point(68, 3);
            this.AutocloseCheckBox.Name = "AutocloseCheckBox";
            this.AutocloseCheckBox.Size = new System.Drawing.Size(73, 17);
            this.AutocloseCheckBox.TabIndex = 2;
            this.AutocloseCheckBox.Text = "Autoclose";
            this.AutocloseCheckBox.UseVisualStyleBackColor = true;
            this.AutocloseCheckBox.CheckedChanged += new System.EventHandler(this.AutocloseCheckBox_CheckedChanged);
            // 
            // TrayNotify
            // 
            this.TrayNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayNotify.Icon")));
            this.TrayNotify.Text = "notifyIcon1";
            this.TrayNotify.Visible = true;
            this.TrayNotify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayNotify_MouseDoubleClick);
            // 
            // StartMinimizedCheckBox
            // 
            this.StartMinimizedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartMinimizedCheckBox.AutoSize = true;
            this.StartMinimizedCheckBox.Location = new System.Drawing.Point(217, 3);
            this.StartMinimizedCheckBox.Name = "StartMinimizedCheckBox";
            this.StartMinimizedCheckBox.Size = new System.Drawing.Size(72, 17);
            this.StartMinimizedCheckBox.TabIndex = 3;
            this.StartMinimizedCheckBox.Text = "Minimized";
            this.StartMinimizedCheckBox.UseVisualStyleBackColor = true;
            this.StartMinimizedCheckBox.CheckedChanged += new System.EventHandler(this.StartMinimizedCheckBox_CheckedChanged);
            // 
            // ReopenCheckBox
            // 
            this.ReopenCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReopenCheckBox.AutoSize = true;
            this.ReopenCheckBox.Location = new System.Drawing.Point(147, 3);
            this.ReopenCheckBox.Name = "ReopenCheckBox";
            this.ReopenCheckBox.Size = new System.Drawing.Size(64, 17);
            this.ReopenCheckBox.TabIndex = 4;
            this.ReopenCheckBox.Text = "Reopen";
            this.ReopenCheckBox.UseVisualStyleBackColor = true;
            this.ReopenCheckBox.CheckedChanged += new System.EventHandler(this.ReopenCheckBox_CheckedChanged);
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTableLayoutPanel.ColumnCount = 5;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.MainTableLayoutPanel.Controls.Add(this.EnableCheckBox, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.LogListBox, 0, 1);
            this.MainTableLayoutPanel.Controls.Add(this.StartMinimizedCheckBox, 3, 0);
            this.MainTableLayoutPanel.Controls.Add(this.ReopenCheckBox, 2, 0);
            this.MainTableLayoutPanel.Controls.Add(this.AutocloseCheckBox, 1, 0);
            this.MainTableLayoutPanel.Controls.Add(this.AutostartCheckBox, 4, 0);
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(12, 12);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(372, 157);
            this.MainTableLayoutPanel.TabIndex = 5;
            // 
            // AutostartCheckBox
            // 
            this.AutostartCheckBox.AutoSize = true;
            this.AutostartCheckBox.Location = new System.Drawing.Point(295, 3);
            this.AutostartCheckBox.Name = "AutostartCheckBox";
            this.AutostartCheckBox.Size = new System.Drawing.Size(68, 17);
            this.AutostartCheckBox.TabIndex = 5;
            this.AutostartCheckBox.Text = "Autostart";
            this.AutostartCheckBox.UseVisualStyleBackColor = true;
            this.AutostartCheckBox.CheckedChanged += new System.EventHandler(this.AutostartCheckBox_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 181);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(412, 220);
            this.Name = "MainForm";
            this.Text = "PlugnPutty";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.MainTableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox LogListBox;
		private System.Windows.Forms.CheckBox EnableCheckBox;
		private System.Windows.Forms.CheckBox AutocloseCheckBox;
		private System.Windows.Forms.NotifyIcon TrayNotify;
		private System.Windows.Forms.CheckBox StartMinimizedCheckBox;
		private System.Windows.Forms.CheckBox ReopenCheckBox;
		private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
		private System.Windows.Forms.CheckBox AutostartCheckBox;
	}
}

