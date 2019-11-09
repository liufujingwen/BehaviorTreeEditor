namespace BehaviorTreeEditor.UIControls
{
    partial class SetVariableUserControl
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
            this.components = new System.ComponentModel.Container();
            this.CBB_ParameterName = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CBB_ParameterType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TB_ParameterValue = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // CBB_ParameterName
            // 
            this.CBB_ParameterName.FormattingEnabled = true;
            this.CBB_ParameterName.Location = new System.Drawing.Point(110, 57);
            this.CBB_ParameterName.Name = "CBB_ParameterName";
            this.CBB_ParameterName.Size = new System.Drawing.Size(250, 23);
            this.CBB_ParameterName.TabIndex = 11;
            this.CBB_ParameterName.SelectedIndexChanged += new System.EventHandler(this.CBB_ParameterName_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 10;
            this.label2.Text = "参数名称：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "参数类型：";
            // 
            // CBB_ParameterType
            // 
            this.CBB_ParameterType.FormattingEnabled = true;
            this.CBB_ParameterType.Items.AddRange(new object[] {
            "GlobalVar（全局变量）",
            "BehaviorTreeVar（行为树变量）",
            "ContextVar（上下文变量）"});
            this.CBB_ParameterType.Location = new System.Drawing.Point(110, 17);
            this.CBB_ParameterType.Name = "CBB_ParameterType";
            this.CBB_ParameterType.Size = new System.Drawing.Size(250, 23);
            this.CBB_ParameterType.TabIndex = 9;
            this.CBB_ParameterType.SelectedIndexChanged += new System.EventHandler(this.CBB_ParameterType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "参数值：";
            // 
            // TB_ParameterValue
            // 
            this.TB_ParameterValue.Location = new System.Drawing.Point(110, 100);
            this.TB_ParameterValue.Name = "TB_ParameterValue";
            this.TB_ParameterValue.Size = new System.Drawing.Size(250, 25);
            this.TB_ParameterValue.TabIndex = 13;
            this.TB_ParameterValue.TextChanged += new System.EventHandler(this.TB_ParameterValue_TextChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // SetVariableUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TB_ParameterValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CBB_ParameterName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CBB_ParameterType);
            this.Name = "SetVariableUserControl";
            this.Size = new System.Drawing.Size(421, 717);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CBB_ParameterName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CBB_ParameterType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TB_ParameterValue;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
