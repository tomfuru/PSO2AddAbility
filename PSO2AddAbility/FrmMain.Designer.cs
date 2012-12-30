namespace PSO2AddAbility
{
    partial class FrmMain
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSynthesis = new System.Windows.Forms.Button();
            this.treeViewResult = new System.Windows.Forms.TreeView();
            this.tsmi価格設定 = new System.Windows.Forms.MenuStrip();
            this.tsmi設定 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConfigValue = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslText = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSynthesisNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numSynFee2 = new System.Windows.Forms.NumericUpDown();
            this.numSynFee3 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.waInputCurrent = new PSO2AddAbility.WeaponAbilityInputComboBox();
            this.waInputObjective = new PSO2AddAbility.WeaponAbilityInputComboBox();
            this.weaponAbilityInputButton1 = new PSO2AddAbility.WeaponAbilityInputCheckBox();
            this.tsmi価格設定.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSynFee2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSynFee3)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSynthesis
            // 
            this.btnSynthesis.Location = new System.Drawing.Point(199, 349);
            this.btnSynthesis.Name = "btnSynthesis";
            this.btnSynthesis.Size = new System.Drawing.Size(75, 23);
            this.btnSynthesis.TabIndex = 0;
            this.btnSynthesis.Text = "合成";
            this.btnSynthesis.UseVisualStyleBackColor = true;
            this.btnSynthesis.Click += new System.EventHandler(this.btnSynthesis_Click);
            // 
            // treeViewResult
            // 
            this.treeViewResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewResult.Location = new System.Drawing.Point(290, 32);
            this.treeViewResult.Name = "treeViewResult";
            this.treeViewResult.Size = new System.Drawing.Size(292, 475);
            this.treeViewResult.TabIndex = 1;
            this.treeViewResult.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewResult_BeforeExpand);
            // 
            // tsmi価格設定
            // 
            this.tsmi価格設定.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tsmi価格設定.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi設定,
            this.tsmiConfigValue});
            this.tsmi価格設定.Location = new System.Drawing.Point(0, 0);
            this.tsmi価格設定.Name = "tsmi価格設定";
            this.tsmi価格設定.Size = new System.Drawing.Size(595, 26);
            this.tsmi価格設定.TabIndex = 4;
            this.tsmi価格設定.Text = "menuStrip1";
            // 
            // tsmi設定
            // 
            this.tsmi設定.Name = "tsmi設定";
            this.tsmi設定.Size = new System.Drawing.Size(62, 22);
            this.tsmi設定.Text = "設定(C)";
            this.tsmi設定.Click += new System.EventHandler(this.tsmi設定_Click);
            // 
            // tsmiConfigValue
            // 
            this.tsmiConfigValue.Name = "tsmiConfigValue";
            this.tsmiConfigValue.Size = new System.Drawing.Size(86, 22);
            this.tsmiConfigValue.Text = "価格設定(&V)";
            this.tsmiConfigValue.Click += new System.EventHandler(this.tsmiConfigValue_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslText,
            this.tsslSynthesisNumber});
            this.statusStrip1.Location = new System.Drawing.Point(0, 509);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(595, 23);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslText
            // 
            this.tsslText.Name = "tsslText";
            this.tsslText.Size = new System.Drawing.Size(560, 18);
            this.tsslText.Spring = true;
            this.tsslText.Text = "...";
            this.tsslText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslSynthesisNumber
            // 
            this.tsslSynthesisNumber.Name = "tsslSynthesisNumber";
            this.tsslSynthesisNumber.Size = new System.Drawing.Size(20, 18);
            this.tsslSynthesisNumber.Text = "...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "合成費用";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "2つ";
            // 
            // numSynFee2
            // 
            this.numSynFee2.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSynFee2.Location = new System.Drawing.Point(36, 46);
            this.numSynFee2.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numSynFee2.Name = "numSynFee2";
            this.numSynFee2.Size = new System.Drawing.Size(61, 19);
            this.numSynFee2.TabIndex = 8;
            // 
            // numSynFee3
            // 
            this.numSynFee3.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSynFee3.Location = new System.Drawing.Point(129, 46);
            this.numSynFee3.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numSynFee3.Name = "numSynFee3";
            this.numSynFee3.Size = new System.Drawing.Size(61, 19);
            this.numSynFee3.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(106, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "3つ";
            // 
            // waInputCurrent
            // 
            this.waInputCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.waInputCurrent.IncludeGarbage = true;
            this.waInputCurrent.Label = "現在武器";
            this.waInputCurrent.Location = new System.Drawing.Point(12, 215);
            this.waInputCurrent.Name = "waInputCurrent";
            this.waInputCurrent.Size = new System.Drawing.Size(262, 128);
            this.waInputCurrent.TabIndex = 3;
            // 
            // waInputObjective
            // 
            this.waInputObjective.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.waInputObjective.IncludeGarbage = false;
            this.waInputObjective.Label = "目標武器";
            this.waInputObjective.Location = new System.Drawing.Point(12, 81);
            this.waInputObjective.Name = "waInputObjective";
            this.waInputObjective.Size = new System.Drawing.Size(262, 128);
            this.waInputObjective.TabIndex = 2;
            // 
            // weaponAbilityInputButton1
            // 
            this.weaponAbilityInputButton1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.weaponAbilityInputButton1.Label = "...";
            this.weaponAbilityInputButton1.Location = new System.Drawing.Point(12, 378);
            this.weaponAbilityInputButton1.Name = "weaponAbilityInputButton1";
            this.weaponAbilityInputButton1.Size = new System.Drawing.Size(262, 128);
            this.weaponAbilityInputButton1.TabIndex = 11;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 532);
            this.Controls.Add(this.weaponAbilityInputButton1);
            this.Controls.Add(this.numSynFee3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numSynFee2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeViewResult);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.waInputCurrent);
            this.Controls.Add(this.waInputObjective);
            this.Controls.Add(this.btnSynthesis);
            this.Controls.Add(this.tsmi価格設定);
            this.MainMenuStrip = this.tsmi価格設定;
            this.Name = "FrmMain";
            this.Text = "PSO2AddAbility";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.tsmi価格設定.ResumeLayout(false);
            this.tsmi価格設定.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSynFee2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSynFee3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSynthesis;
        private System.Windows.Forms.TreeView treeViewResult;
        private WeaponAbilityInputComboBox waInputObjective;
        private WeaponAbilityInputComboBox waInputCurrent;
        private System.Windows.Forms.MenuStrip tsmi価格設定;
        private System.Windows.Forms.ToolStripMenuItem tsmi設定;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslText;
        private System.Windows.Forms.ToolStripStatusLabel tsslSynthesisNumber;
        private System.Windows.Forms.ToolStripMenuItem tsmiConfigValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numSynFee2;
        private System.Windows.Forms.NumericUpDown numSynFee3;
        private System.Windows.Forms.Label label3;
        private WeaponAbilityInputCheckBox weaponAbilityInputButton1;
    }
}

