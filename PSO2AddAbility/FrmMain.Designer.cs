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
            this.components = new System.ComponentModel.Container();
            this.btnSynthesis = new System.Windows.Forms.Button();
            this.treeViewResult = new System.Windows.Forms.TreeView();
            this.menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.waInputCurrent = new PSO2AddAbility.WeaponAbilityInput();
            this.waInputObjective = new PSO2AddAbility.WeaponAbilityInput();
            this.SuspendLayout();
            // 
            // btnSynthesis
            // 
            this.btnSynthesis.Location = new System.Drawing.Point(199, 300);
            this.btnSynthesis.Name = "btnSynthesis";
            this.btnSynthesis.Size = new System.Drawing.Size(75, 23);
            this.btnSynthesis.TabIndex = 0;
            this.btnSynthesis.Text = "合成";
            this.btnSynthesis.UseVisualStyleBackColor = true;
            this.btnSynthesis.Click += new System.EventHandler(this.btnSynthesis_Click);
            // 
            // treeViewResult
            // 
            this.treeViewResult.Location = new System.Drawing.Point(290, 12);
            this.treeViewResult.Name = "treeViewResult";
            this.treeViewResult.Size = new System.Drawing.Size(292, 490);
            this.treeViewResult.TabIndex = 1;
            this.treeViewResult.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewResult_BeforeExpand);
            // 
            // menu
            // 
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(61, 4);
            // 
            // waInputCurrent
            // 
            this.waInputCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.waInputCurrent.IncludeGarbage = true;
            this.waInputCurrent.Label = "現在武器";
            this.waInputCurrent.Location = new System.Drawing.Point(12, 146);
            this.waInputCurrent.Name = "waInputCurrent";
            this.waInputCurrent.Size = new System.Drawing.Size(262, 128);
            this.waInputCurrent.TabIndex = 3;
            // 
            // waInputObjective
            // 
            this.waInputObjective.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.waInputObjective.IncludeGarbage = false;
            this.waInputObjective.Label = "目標武器";
            this.waInputObjective.Location = new System.Drawing.Point(12, 12);
            this.waInputObjective.Name = "waInputObjective";
            this.waInputObjective.Size = new System.Drawing.Size(262, 128);
            this.waInputObjective.TabIndex = 2;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 507);
            this.Controls.Add(this.waInputCurrent);
            this.Controls.Add(this.waInputObjective);
            this.Controls.Add(this.treeViewResult);
            this.Controls.Add(this.btnSynthesis);
            this.Name = "FrmMain";
            this.Text = "PSO2AddAbility";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSynthesis;
        private System.Windows.Forms.TreeView treeViewResult;
        private System.Windows.Forms.ContextMenuStrip menu;
        private WeaponAbilityInput waInputObjective;
        private WeaponAbilityInput waInputCurrent;
    }
}

