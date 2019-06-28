namespace BehaviorTreeEditor
{
    partial class EnumForm
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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点0");
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加类ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑类ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.Gainsboro;
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ItemHeight = 25;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(2);
            this.treeView1.Name = "treeView1";
            treeNode2.Name = "节点0";
            treeNode2.Text = "节点0";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeView1.Size = new System.Drawing.Size(514, 450);
            this.treeView1.TabIndex = 1;
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加类ToolStripMenuItem,
            this.编辑类ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.重置ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 130);
            // 
            // 添加类ToolStripMenuItem
            // 
            this.添加类ToolStripMenuItem.Image = global::BehaviorTreeEditor.Properties.Resources._new;
            this.添加类ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.添加类ToolStripMenuItem.Name = "添加类ToolStripMenuItem";
            this.添加类ToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.添加类ToolStripMenuItem.Text = "添加枚举";
            this.添加类ToolStripMenuItem.Click += new System.EventHandler(this.添加类ToolStripMenuItem_Click);
            // 
            // 编辑类ToolStripMenuItem
            // 
            this.编辑类ToolStripMenuItem.Name = "编辑类ToolStripMenuItem";
            this.编辑类ToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.编辑类ToolStripMenuItem.Text = "编辑枚举";
            this.编辑类ToolStripMenuItem.Click += new System.EventHandler(this.编辑类ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Image = global::BehaviorTreeEditor.Properties.Resources.delete;
            this.删除ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 重置ToolStripMenuItem
            // 
            this.重置ToolStripMenuItem.Name = "重置ToolStripMenuItem";
            this.重置ToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.重置ToolStripMenuItem.Text = "删除全部";
            this.重置ToolStripMenuItem.Click += new System.EventHandler(this.重置ToolStripMenuItem_Click);
            // 
            // EnumForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 450);
            this.Controls.Add(this.treeView1);
            this.Name = "EnumForm";
            this.Text = "枚举查看窗口";
            this.Load += new System.EventHandler(this.EnumForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加类ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 编辑类ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重置ToolStripMenuItem;
    }
}