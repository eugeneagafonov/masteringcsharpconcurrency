namespace ConcurrencyBook.Samples
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
			this._images = new System.Windows.Forms.ImageList(this.components);
			this._loadSyncBtn = new System.Windows.Forms.Button();
			this._resultPanel = new System.Windows.Forms.Panel();
			this._timeLabel = new System.Windows.Forms.Label();
			this._loadTaskBtn = new System.Windows.Forms.Button();
			this._loasAsyncBtn = new System.Windows.Forms.Button();
			this._iteratorBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _images
			// 
			this._images.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this._images.ImageSize = new System.Drawing.Size(192, 108);
			this._images.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// _loadSyncBtn
			// 
			this._loadSyncBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._loadSyncBtn.Location = new System.Drawing.Point(414, 12);
			this._loadSyncBtn.Name = "_loadSyncBtn";
			this._loadSyncBtn.Size = new System.Drawing.Size(147, 23);
			this._loadSyncBtn.TabIndex = 1;
			this._loadSyncBtn.Text = "Load synchronously";
			this._loadSyncBtn.UseVisualStyleBackColor = true;
			this._loadSyncBtn.Click += new System.EventHandler(this._loadSyncBtn_Click);
			// 
			// _resultPanel
			// 
			this._resultPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._resultPanel.AutoScroll = true;
			this._resultPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this._resultPanel.Location = new System.Drawing.Point(12, 12);
			this._resultPanel.Name = "_resultPanel";
			this._resultPanel.Size = new System.Drawing.Size(396, 616);
			this._resultPanel.TabIndex = 2;
			// 
			// _timeLabel
			// 
			this._timeLabel.AutoSize = true;
			this._timeLabel.Location = new System.Drawing.Point(12, 631);
			this._timeLabel.Name = "_timeLabel";
			this._timeLabel.Size = new System.Drawing.Size(33, 13);
			this._timeLabel.TabIndex = 3;
			this._timeLabel.Text = "Time:";
			// 
			// _loadTaskBtn
			// 
			this._loadTaskBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._loadTaskBtn.Location = new System.Drawing.Point(414, 41);
			this._loadTaskBtn.Name = "_loadTaskBtn";
			this._loadTaskBtn.Size = new System.Drawing.Size(147, 23);
			this._loadTaskBtn.TabIndex = 1;
			this._loadTaskBtn.Text = "Load using TPL";
			this._loadTaskBtn.UseVisualStyleBackColor = true;
			this._loadTaskBtn.Click += new System.EventHandler(this._loadTaskBtn_Click);
			// 
			// _loasAsyncBtn
			// 
			this._loasAsyncBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._loasAsyncBtn.Location = new System.Drawing.Point(414, 70);
			this._loasAsyncBtn.Name = "_loasAsyncBtn";
			this._loasAsyncBtn.Size = new System.Drawing.Size(147, 23);
			this._loasAsyncBtn.TabIndex = 1;
			this._loasAsyncBtn.Text = "Load using async";
			this._loasAsyncBtn.UseVisualStyleBackColor = true;
			this._loasAsyncBtn.Click += new System.EventHandler(this._loadAsyncBtn_Click);
			// 
			// _iteratorBtn
			// 
			this._iteratorBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._iteratorBtn.Location = new System.Drawing.Point(414, 99);
			this._iteratorBtn.Name = "_iteratorBtn";
			this._iteratorBtn.Size = new System.Drawing.Size(147, 23);
			this._iteratorBtn.TabIndex = 1;
			this._iteratorBtn.Text = "Load using iterator";
			this._iteratorBtn.UseVisualStyleBackColor = true;
			this._iteratorBtn.Click += new System.EventHandler(this._iteratorBtn_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(573, 653);
			this.Controls.Add(this._timeLabel);
			this.Controls.Add(this._resultPanel);
			this.Controls.Add(this._iteratorBtn);
			this.Controls.Add(this._loasAsyncBtn);
			this.Controls.Add(this._loadTaskBtn);
			this.Controls.Add(this._loadSyncBtn);
			this.Name = "MainForm";
			this.Text = "Bing Wallpaper Sample";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ImageList _images;
		private System.Windows.Forms.Button _loadSyncBtn;
		private System.Windows.Forms.Panel _resultPanel;
		private System.Windows.Forms.Label _timeLabel;
		private System.Windows.Forms.Button _loadTaskBtn;
		private System.Windows.Forms.Button _loasAsyncBtn;
		private System.Windows.Forms.Button _iteratorBtn;
	}
}

