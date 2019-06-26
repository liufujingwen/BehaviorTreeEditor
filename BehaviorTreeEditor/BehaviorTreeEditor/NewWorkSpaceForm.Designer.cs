namespace BehaviorTreeEditor
{
    partial class NewWorkSpaceForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.workSpaceNameTB = new System.Windows.Forms.TextBox();
            this.workSpaceDirectoryTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.selectWorkSpaceDirectoryBTN = new System.Windows.Forms.Button();
            this.dataSaveDirectoryTB = new System.Windows.Forms.TextBox();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.enterBTN = new System.Windows.Forms.Button();
            this.selectDataSaveDirectoryBTN = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.describeTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "工作区名字：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "工作区位置：";
            // 
            // workSpaceNameTB
            // 
            this.workSpaceNameTB.Location = new System.Drawing.Point(135, 41);
            this.workSpaceNameTB.Name = "workSpaceNameTB";
            this.workSpaceNameTB.Size = new System.Drawing.Size(389, 21);
            this.workSpaceNameTB.TabIndex = 2;
            // 
            // workSpaceDirectoryTB
            // 
            this.workSpaceDirectoryTB.Location = new System.Drawing.Point(135, 71);
            this.workSpaceDirectoryTB.Name = "workSpaceDirectoryTB";
            this.workSpaceDirectoryTB.Size = new System.Drawing.Size(389, 21);
            this.workSpaceDirectoryTB.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "数据保存位置：";
            // 
            // selectWorkSpaceDirectoryBTN
            // 
            this.selectWorkSpaceDirectoryBTN.Location = new System.Drawing.Point(542, 69);
            this.selectWorkSpaceDirectoryBTN.Name = "selectWorkSpaceDirectoryBTN";
            this.selectWorkSpaceDirectoryBTN.Size = new System.Drawing.Size(61, 23);
            this.selectWorkSpaceDirectoryBTN.TabIndex = 5;
            this.selectWorkSpaceDirectoryBTN.Text = "浏览...";
            this.selectWorkSpaceDirectoryBTN.UseVisualStyleBackColor = true;
            this.selectWorkSpaceDirectoryBTN.Click += new System.EventHandler(this.selectWorkSpaceDirectoryBTN_Click);
            // 
            // dataSaveDirectoryTB
            // 
            this.dataSaveDirectoryTB.Location = new System.Drawing.Point(135, 103);
            this.dataSaveDirectoryTB.Name = "dataSaveDirectoryTB";
            this.dataSaveDirectoryTB.Size = new System.Drawing.Size(389, 21);
            this.dataSaveDirectoryTB.TabIndex = 6;
            // 
            // cancelBTN
            // 
            this.cancelBTN.Location = new System.Drawing.Point(196, 227);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(107, 28);
            this.cancelBTN.TabIndex = 7;
            this.cancelBTN.Text = "取消";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // enterBTN
            // 
            this.enterBTN.Location = new System.Drawing.Point(359, 227);
            this.enterBTN.Name = "enterBTN";
            this.enterBTN.Size = new System.Drawing.Size(107, 28);
            this.enterBTN.TabIndex = 8;
            this.enterBTN.Text = "确定";
            this.enterBTN.UseVisualStyleBackColor = true;
            this.enterBTN.Click += new System.EventHandler(this.enterBTN_Click);
            // 
            // selectDataSaveDirectoryBTN
            // 
            this.selectDataSaveDirectoryBTN.Location = new System.Drawing.Point(542, 101);
            this.selectDataSaveDirectoryBTN.Name = "selectDataSaveDirectoryBTN";
            this.selectDataSaveDirectoryBTN.Size = new System.Drawing.Size(61, 23);
            this.selectDataSaveDirectoryBTN.TabIndex = 9;
            this.selectDataSaveDirectoryBTN.Text = "浏览...";
            this.selectDataSaveDirectoryBTN.UseVisualStyleBackColor = true;
            this.selectDataSaveDirectoryBTN.Click += new System.EventHandler(this.selectDataSaveDirectoryBTN_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "请选择工作区位置";
            // 
            // folderBrowserDialog2
            // 
            this.folderBrowserDialog2.Description = "选择数据保存位置";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(79, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "描述：";
            // 
            // describeTB
            // 
            this.describeTB.Location = new System.Drawing.Point(135, 137);
            this.describeTB.Multiline = true;
            this.describeTB.Name = "describeTB";
            this.describeTB.Size = new System.Drawing.Size(389, 54);
            this.describeTB.TabIndex = 11;
            // 
            // NewWorkSpaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 267);
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
            this.Name = "NewWorkSpaceForm";
            this.Text = "新建工作区";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox workSpaceNameTB;
        private System.Windows.Forms.TextBox workSpaceDirectoryTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button selectWorkSpaceDirectoryBTN;
        private System.Windows.Forms.TextBox dataSaveDirectoryTB;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.Button enterBTN;
        private System.Windows.Forms.Button selectDataSaveDirectoryBTN;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox describeTB;
    }
}