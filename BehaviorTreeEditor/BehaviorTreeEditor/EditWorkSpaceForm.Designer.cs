namespace BehaviorTreeEditor
{
    partial class EditWorkSpaceForm
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
            this.describeTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.selectDataSaveDirectoryBTN = new System.Windows.Forms.Button();
            this.enterBTN = new System.Windows.Forms.Button();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.dataSaveDirectoryTB = new System.Windows.Forms.TextBox();
            this.selectWorkSpaceDirectoryBTN = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.workSpaceDirectoryTB = new System.Windows.Forms.TextBox();
            this.workSpaceNameTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // describeTB
            // 
            this.describeTB.Location = new System.Drawing.Point(187, 153);
            this.describeTB.Margin = new System.Windows.Forms.Padding(4);
            this.describeTB.Multiline = true;
            this.describeTB.Name = "describeTB";
            this.describeTB.Size = new System.Drawing.Size(517, 66);
            this.describeTB.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 153);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 22;
            this.label4.Text = "描述：";
            // 
            // selectDataSaveDirectoryBTN
            // 
            this.selectDataSaveDirectoryBTN.Location = new System.Drawing.Point(730, 108);
            this.selectDataSaveDirectoryBTN.Margin = new System.Windows.Forms.Padding(4);
            this.selectDataSaveDirectoryBTN.Name = "selectDataSaveDirectoryBTN";
            this.selectDataSaveDirectoryBTN.Size = new System.Drawing.Size(81, 29);
            this.selectDataSaveDirectoryBTN.TabIndex = 21;
            this.selectDataSaveDirectoryBTN.Text = "浏览...";
            this.selectDataSaveDirectoryBTN.UseVisualStyleBackColor = true;
            this.selectDataSaveDirectoryBTN.Click += new System.EventHandler(this.selectDataSaveDirectoryBTN_Click);
            // 
            // enterBTN
            // 
            this.enterBTN.Location = new System.Drawing.Point(486, 266);
            this.enterBTN.Margin = new System.Windows.Forms.Padding(4);
            this.enterBTN.Name = "enterBTN";
            this.enterBTN.Size = new System.Drawing.Size(143, 35);
            this.enterBTN.TabIndex = 20;
            this.enterBTN.Text = "确定";
            this.enterBTN.UseVisualStyleBackColor = true;
            this.enterBTN.Click += new System.EventHandler(this.enterBTN_Click);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Location = new System.Drawing.Point(268, 266);
            this.cancelBTN.Margin = new System.Windows.Forms.Padding(4);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(143, 35);
            this.cancelBTN.TabIndex = 19;
            this.cancelBTN.Text = "取消";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // dataSaveDirectoryTB
            // 
            this.dataSaveDirectoryTB.Location = new System.Drawing.Point(187, 111);
            this.dataSaveDirectoryTB.Margin = new System.Windows.Forms.Padding(4);
            this.dataSaveDirectoryTB.Name = "dataSaveDirectoryTB";
            this.dataSaveDirectoryTB.Size = new System.Drawing.Size(517, 25);
            this.dataSaveDirectoryTB.TabIndex = 18;
            // 
            // selectWorkSpaceDirectoryBTN
            // 
            this.selectWorkSpaceDirectoryBTN.Location = new System.Drawing.Point(730, 68);
            this.selectWorkSpaceDirectoryBTN.Margin = new System.Windows.Forms.Padding(4);
            this.selectWorkSpaceDirectoryBTN.Name = "selectWorkSpaceDirectoryBTN";
            this.selectWorkSpaceDirectoryBTN.Size = new System.Drawing.Size(81, 29);
            this.selectWorkSpaceDirectoryBTN.TabIndex = 17;
            this.selectWorkSpaceDirectoryBTN.Text = "浏览...";
            this.selectWorkSpaceDirectoryBTN.UseVisualStyleBackColor = true;
            this.selectWorkSpaceDirectoryBTN.Click += new System.EventHandler(this.selectWorkSpaceDirectoryBTN_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 114);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "数据保存位置：";
            // 
            // workSpaceDirectoryTB
            // 
            this.workSpaceDirectoryTB.Location = new System.Drawing.Point(187, 71);
            this.workSpaceDirectoryTB.Margin = new System.Windows.Forms.Padding(4);
            this.workSpaceDirectoryTB.Name = "workSpaceDirectoryTB";
            this.workSpaceDirectoryTB.Size = new System.Drawing.Size(517, 25);
            this.workSpaceDirectoryTB.TabIndex = 15;
            // 
            // workSpaceNameTB
            // 
            this.workSpaceNameTB.Location = new System.Drawing.Point(187, 33);
            this.workSpaceNameTB.Margin = new System.Windows.Forms.Padding(4);
            this.workSpaceNameTB.Name = "workSpaceNameTB";
            this.workSpaceNameTB.Size = new System.Drawing.Size(517, 25);
            this.workSpaceNameTB.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "工作区位置：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "工作区名字：";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "请选择工作区位置";
            // 
            // folderBrowserDialog2
            // 
            this.folderBrowserDialog2.Description = "选择数据保存位置";
            // 
            // EditWorkSpaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 334);
            this.Controls.Add(this.describeTB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.selectDataSaveDirectoryBTN);
            this.Controls.Add(this.enterBTN);
            this.Controls.Add(this.cancelBTN);
            this.Controls.Add(this.dataSaveDirectoryTB);
            this.Controls.Add(this.selectWorkSpaceDirectoryBTN);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.workSpaceDirectoryTB);
            this.Controls.Add(this.workSpaceNameTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditWorkSpaceForm";
            this.Text = "编辑工作区";
            this.Load += new System.EventHandler(this.EditWorkSpaceForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox describeTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button selectDataSaveDirectoryBTN;
        private System.Windows.Forms.Button enterBTN;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.TextBox dataSaveDirectoryTB;
        private System.Windows.Forms.Button selectWorkSpaceDirectoryBTN;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox workSpaceDirectoryTB;
        private System.Windows.Forms.TextBox workSpaceNameTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
    }
}