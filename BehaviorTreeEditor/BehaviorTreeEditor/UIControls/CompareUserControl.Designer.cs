namespace BehaviorTreeEditor.UIControls
{
    partial class CompareUserControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.CBB_LeftCompareType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CBB_CompareType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBB_LeftParameter = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CBB_RightParameter = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.CBB_RightCompareType = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "参数类型：";
            // 
            // CBB_LeftCompareType
            // 
            this.CBB_LeftCompareType.FormattingEnabled = true;
            this.CBB_LeftCompareType.Location = new System.Drawing.Point(98, 34);
            this.CBB_LeftCompareType.Name = "CBB_LeftCompareType";
            this.CBB_LeftCompareType.Size = new System.Drawing.Size(259, 23);
            this.CBB_LeftCompareType.TabIndex = 1;
            this.CBB_LeftCompareType.SelectedIndexChanged += new System.EventHandler(this.CBB_LeftCompareType_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "参数名称：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "比较类型：";
            // 
            // CBB_CompareType
            // 
            this.CBB_CompareType.FormattingEnabled = true;
            this.CBB_CompareType.Location = new System.Drawing.Point(104, 38);
            this.CBB_CompareType.Name = "CBB_CompareType";
            this.CBB_CompareType.Size = new System.Drawing.Size(259, 23);
            this.CBB_CompareType.TabIndex = 5;
            this.CBB_CompareType.SelectedIndexChanged += new System.EventHandler(this.CBB_CompareType_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CBB_LeftParameter);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.CBB_LeftCompareType);
            this.groupBox1.Location = new System.Drawing.Point(12, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(369, 120);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "左边参数";
            // 
            // CBB_LeftParameter
            // 
            this.CBB_LeftParameter.FormattingEnabled = true;
            this.CBB_LeftParameter.Location = new System.Drawing.Point(98, 74);
            this.CBB_LeftParameter.Name = "CBB_LeftParameter";
            this.CBB_LeftParameter.Size = new System.Drawing.Size(259, 23);
            this.CBB_LeftParameter.TabIndex = 3;
            this.CBB_LeftParameter.SelectedIndexChanged += new System.EventHandler(this.CBB_LeftParameter_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CBB_RightParameter);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.CBB_RightCompareType);
            this.groupBox2.Location = new System.Drawing.Point(12, 293);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(369, 120);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "右边参数";
            // 
            // CBB_RightParameter
            // 
            this.CBB_RightParameter.FormattingEnabled = true;
            this.CBB_RightParameter.Location = new System.Drawing.Point(98, 74);
            this.CBB_RightParameter.Name = "CBB_RightParameter";
            this.CBB_RightParameter.Size = new System.Drawing.Size(259, 23);
            this.CBB_RightParameter.TabIndex = 4;
            this.CBB_RightParameter.SelectedIndexChanged += new System.EventHandler(this.CBB_RightParameter_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "参数名称：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "参数类型：";
            // 
            // CBB_RightCompareType
            // 
            this.CBB_RightCompareType.FormattingEnabled = true;
            this.CBB_RightCompareType.Location = new System.Drawing.Point(98, 34);
            this.CBB_RightCompareType.Name = "CBB_RightCompareType";
            this.CBB_RightCompareType.Size = new System.Drawing.Size(259, 23);
            this.CBB_RightCompareType.TabIndex = 1;
            this.CBB_RightCompareType.SelectedIndexChanged += new System.EventHandler(this.CBB_RightCompareType_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.CBB_CompareType);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(12, 165);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(369, 88);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            // 
            // CompareUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CompareUserControl";
            this.Size = new System.Drawing.Size(395, 704);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CBB_LeftCompareType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CBB_CompareType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CBB_RightCompareType;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox CBB_LeftParameter;
        private System.Windows.Forms.ComboBox CBB_RightParameter;
    }
}
