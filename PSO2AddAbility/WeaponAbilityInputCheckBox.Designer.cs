namespace PSO2AddAbility
{
    partial class WeaponAbilityInputCheckBox
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblText = new System.Windows.Forms.Label();
            this.pnlDisp = new System.Windows.Forms.Panel();
            this.vscrPanel = new System.Windows.Forms.VScrollBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(10, 7);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(11, 12);
            this.lblText.TabIndex = 9;
            this.lblText.Text = "...";
            // 
            // pnlDisp
            // 
            this.pnlDisp.AutoSize = true;
            this.pnlDisp.Location = new System.Drawing.Point(-1, -1);
            this.pnlDisp.Name = "pnlDisp";
            this.pnlDisp.Size = new System.Drawing.Size(246, 108);
            this.pnlDisp.TabIndex = 10;
            // 
            // vscrPanel
            // 
            this.vscrPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.vscrPanel.Location = new System.Drawing.Point(243, 0);
            this.vscrPanel.Name = "vscrPanel";
            this.vscrPanel.Size = new System.Drawing.Size(18, 105);
            this.vscrPanel.TabIndex = 0;
            this.vscrPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vscrPanel_Scroll);
            this.vscrPanel.ValueChanged += new System.EventHandler(this.vscrPanel_ValueChanged);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.vscrPanel);
            this.panel2.Controls.Add(this.pnlDisp);
            this.panel2.Location = new System.Drawing.Point(-1, 21);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(263, 107);
            this.panel2.TabIndex = 11;
            // 
            // WeaponAbilityInputCheckBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lblText);
            this.Name = "WeaponAbilityInputCheckBox";
            this.Size = new System.Drawing.Size(262, 128);
            this.Load += new System.EventHandler(this.WeaponAbilityInputCheckBox_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Panel pnlDisp;
        private System.Windows.Forms.VScrollBar vscrPanel;
        private System.Windows.Forms.Panel panel2;

    }
}
