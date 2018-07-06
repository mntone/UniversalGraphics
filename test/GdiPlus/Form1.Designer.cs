namespace UniversalGraphics.Test
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

		#region Windows フォーム デザイナーで生成されたコード
		
		private void InitializeComponent()
		{
			this.delegateTypesComboBox = new System.Windows.Forms.ComboBox();
			this.delegatesLabel = new System.Windows.Forms.Label();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.SuspendLayout();
			// 
			// delegateTypesComboBox
			// 
			this.delegateTypesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.delegateTypesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.delegateTypesComboBox.FormattingEnabled = true;
			this.delegateTypesComboBox.Location = new System.Drawing.Point(131, 6);
			this.delegateTypesComboBox.Name = "delegateTypesComboBox";
			this.delegateTypesComboBox.Size = new System.Drawing.Size(657, 32);
			this.delegateTypesComboBox.TabIndex = 0;
			// 
			// delegatesLabel
			// 
			this.delegatesLabel.AutoSize = true;
			this.delegatesLabel.Location = new System.Drawing.Point(12, 9);
			this.delegatesLabel.Name = "delegatesLabel";
			this.delegatesLabel.Size = new System.Drawing.Size(113, 24);
			this.delegatesLabel.TabIndex = 1;
			this.delegatesLabel.Text = "Delegates:";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.delegatesLabel);
			this.Controls.Add(this.delegateTypesComboBox);
			this.Name = "Form1";
			this.Text = "UniversalGraphics.GdiPlus.Test";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox delegateTypesComboBox;
		private System.Windows.Forms.Label delegatesLabel;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
	}
}

