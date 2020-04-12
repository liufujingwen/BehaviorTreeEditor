namespace BehaviorTreeEditor
{
    partial class AddDefineForm
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
            this.nodeTypeCBB = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.describeTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.classTypeTB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.enterBTN = new System.Windows.Forms.Button();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.粘贴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBB_CheckField = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.categoryTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listViewFields = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nodeIconCBB = new System.Windows.Forms.ComboBox();
            this.nodeIconPB = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nodeIconPB)).BeginInit();
            this.SuspendLayout();
            // 
            // nodeTypeCBB
            // 
            this.nodeTypeCBB.FormattingEnabled = true;
            this.nodeTypeCBB.Location = new System.Drawing.Point(48, 77);
            this.nodeTypeCBB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.nodeTypeCBB.Name = "nodeTypeCBB";
            this.nodeTypeCBB.Size = new System.Drawing.Size(343, 20);
            this.nodeTypeCBB.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 78);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "类型：";
            // 
            // describeTB
            // 
            this.describeTB.Location = new System.Drawing.Point(49, 133);
            this.describeTB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.describeTB.Multiline = true;
            this.describeTB.Name = "describeTB";
            this.describeTB.Size = new System.Drawing.Size(384, 56);
            this.describeTB.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 136);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "描述：";
            // 
            // classTypeTB
            // 
            this.classTypeTB.Location = new System.Drawing.Point(49, 18);
            this.classTypeTB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.classTypeTB.Name = "classTypeTB";
            this.classTypeTB.Size = new System.Drawing.Size(343, 21);
            this.classTypeTB.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "类名：";
            // 
            // enterBTN
            // 
            this.enterBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.enterBTN.Location = new System.Drawing.Point(547, 529);
            this.enterBTN.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.enterBTN.Name = "enterBTN";
            this.enterBTN.Size = new System.Drawing.Size(84, 26);
            this.enterBTN.TabIndex = 5;
            this.enterBTN.Text = "确定";
            this.enterBTN.UseVisualStyleBackColor = true;
            this.enterBTN.Click += new System.EventHandler(this.enterBTN_Click);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBTN.Location = new System.Drawing.Point(430, 529);
            this.cancelBTN.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(84, 26);
            this.cancelBTN.TabIndex = 4;
            this.cancelBTN.Text = "取消";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制ToolStripMenuItem,
            this.粘贴ToolStripMenuItem,
            this.toolStripSeparator1,
            this.选项ToolStripMenuItem,
            this.新建ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 120);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.复制ToolStripMenuItem.Text = "复制";
            this.复制ToolStripMenuItem.Click += new System.EventHandler(this.复制ToolStripMenuItem_Click);
            // 
            // 粘贴ToolStripMenuItem
            // 
            this.粘贴ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.粘贴ToolStripMenuItem.Name = "粘贴ToolStripMenuItem";
            this.粘贴ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.粘贴ToolStripMenuItem.Text = "粘贴";
            this.粘贴ToolStripMenuItem.Click += new System.EventHandler(this.粘贴ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(97, 6);
            // 
            // 选项ToolStripMenuItem
            // 
            this.选项ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem";
            this.选项ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.选项ToolStripMenuItem.Text = "选项";
            this.选项ToolStripMenuItem.Click += new System.EventHandler(this.选项ToolStripMenuItem_Click);
            // 
            // 新建ToolStripMenuItem
            // 
            this.新建ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.新建ToolStripMenuItem.Name = "新建ToolStripMenuItem";
            this.新建ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.新建ToolStripMenuItem.Text = "新建";
            this.新建ToolStripMenuItem.Click += new System.EventHandler(this.新建ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.CBB_CheckField);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.labelTB);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.categoryTB);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.nodeTypeCBB);
            this.groupBox1.Controls.Add(this.classTypeTB);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.describeTB);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(9, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(624, 217);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // CBB_CheckField
            // 
            this.CBB_CheckField.AutoSize = true;
            this.CBB_CheckField.Location = new System.Drawing.Point(538, 20);
            this.CBB_CheckField.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.CBB_CheckField.Name = "CBB_CheckField";
            this.CBB_CheckField.Size = new System.Drawing.Size(15, 14);
            this.CBB_CheckField.TabIndex = 11;
            this.CBB_CheckField.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(444, 21);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "是否检测字段：";
            // 
            // labelTB
            // 
            this.labelTB.Location = new System.Drawing.Point(48, 46);
            this.labelTB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelTB.Name = "labelTB";
            this.labelTB.Size = new System.Drawing.Size(343, 21);
            this.labelTB.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 49);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "标签：";
            // 
            // categoryTB
            // 
            this.categoryTB.Location = new System.Drawing.Point(49, 102);
            this.categoryTB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.categoryTB.Name = "categoryTB";
            this.categoryTB.Size = new System.Drawing.Size(343, 21);
            this.categoryTB.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 105);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "类别：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listViewFields);
            this.groupBox2.Location = new System.Drawing.Point(9, 245);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(624, 266);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "字段预览(右键展示操作菜单)";
            // 
            // listViewFields
            // 
            this.listViewFields.BackColor = System.Drawing.SystemColors.Window;
            this.listViewFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listViewFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFields.FullRowSelect = true;
            this.listViewFields.GridLines = true;
            this.listViewFields.HideSelection = false;
            this.listViewFields.LabelEdit = true;
            this.listViewFields.Location = new System.Drawing.Point(2, 16);
            this.listViewFields.Name = "listViewFields";
            this.listViewFields.Size = new System.Drawing.Size(620, 248);
            this.listViewFields.TabIndex = 2;
            this.listViewFields.UseCompatibleStateImageBehavior = false;
            this.listViewFields.View = System.Windows.Forms.View.Details;
            this.listViewFields.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewFields_MouseDown);
            this.listViewFields.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.listViewFields_PreviewKeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "字段名";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "标签";
            this.columnHeader2.Width = 151;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "字段类型";
            this.columnHeader3.Width = 159;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "默认值";
            this.columnHeader4.Width = 209;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "描述";
            this.columnHeader5.Width = 300;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nodeIconCBB);
            this.groupBox3.Controls.Add(this.nodeIconPB);
            this.groupBox3.Location = new System.Drawing.Point(446, 49);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(161, 100);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "图标";
            // 
            // nodeIconCBB
            // 
            this.nodeIconCBB.FormattingEnabled = true;
            this.nodeIconCBB.Location = new System.Drawing.Point(21, 18);
            this.nodeIconCBB.Name = "nodeIconCBB";
            this.nodeIconCBB.Size = new System.Drawing.Size(121, 20);
            this.nodeIconCBB.TabIndex = 1;
            this.nodeIconCBB.SelectedIndexChanged += new System.EventHandler(this.nodeIconCBB_SelectedIndexChanged);
            // 
            // nodeIconPB
            // 
            this.nodeIconPB.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nodeIconPB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nodeIconPB.Location = new System.Drawing.Point(21, 44);
            this.nodeIconPB.Name = "nodeIconPB";
            this.nodeIconPB.Size = new System.Drawing.Size(121, 50);
            this.nodeIconPB.TabIndex = 0;
            this.nodeIconPB.TabStop = false;
            // 
            // AddDefineForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(641, 572);
            this.Controls.Add(this.enterBTN);
            this.Controls.Add(this.cancelBTN);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "AddDefineForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加节点";
            this.Load += new System.EventHandler(this.AddDefineForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nodeIconPB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox describeTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox classTypeTB;
        private System.Windows.Forms.Button enterBTN;
        private System.Windows.Forms.Button cancelBTN;
        private System.Windows.Forms.ComboBox nodeTypeCBB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 粘贴ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 选项ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listViewFields;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TextBox categoryTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox labelTB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox CBB_CheckField;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox nodeIconCBB;
        private System.Windows.Forms.PictureBox nodeIconPB;
    }
}