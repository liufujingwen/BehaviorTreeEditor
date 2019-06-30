namespace BehaviorTreeEditor.UIControls
{
    partial class ContentUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentUserControl));
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.nodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.连线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上移ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下移ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNodeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.centerItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transitionContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除连线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeContextMenuStrip.SuspendLayout();
            this.viewContextMenuStrip.SuspendLayout();
            this.transitionContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 20;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // nodeContextMenuStrip
            // 
            this.nodeContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.nodeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.连线ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.上移ToolStripMenuItem,
            this.下移ToolStripMenuItem,
            this.toolStripSeparator1,
            this.复制ToolStripMenuItem});
            this.nodeContextMenuStrip.Name = "nodeContextMenuStrip";
            this.nodeContextMenuStrip.Size = new System.Drawing.Size(105, 140);
            // 
            // 连线ToolStripMenuItem
            // 
            this.连线ToolStripMenuItem.Image = global::BehaviorTreeEditor.Properties.Resources.TransitionIcon;
            this.连线ToolStripMenuItem.Name = "连线ToolStripMenuItem";
            this.连线ToolStripMenuItem.Size = new System.Drawing.Size(104, 26);
            this.连线ToolStripMenuItem.Text = "连线";
            this.连线ToolStripMenuItem.Click += new System.EventHandler(this.连线ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Image = global::BehaviorTreeEditor.Properties.Resources.delete;
            this.删除ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(104, 26);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 上移ToolStripMenuItem
            // 
            this.上移ToolStripMenuItem.Image = global::BehaviorTreeEditor.Properties.Resources.UpIcon;
            this.上移ToolStripMenuItem.Name = "上移ToolStripMenuItem";
            this.上移ToolStripMenuItem.Size = new System.Drawing.Size(104, 26);
            this.上移ToolStripMenuItem.Text = "上移";
            // 
            // 下移ToolStripMenuItem
            // 
            this.下移ToolStripMenuItem.Image = global::BehaviorTreeEditor.Properties.Resources.DownIcon;
            this.下移ToolStripMenuItem.Name = "下移ToolStripMenuItem";
            this.下移ToolStripMenuItem.Size = new System.Drawing.Size(104, 26);
            this.下移ToolStripMenuItem.Text = "下移";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(101, 6);
            // 
            // 复制ToolStripMenuItem
            // 
            this.复制ToolStripMenuItem.Image = global::BehaviorTreeEditor.Properties.Resources.copy;
            this.复制ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.复制ToolStripMenuItem.Name = "复制ToolStripMenuItem";
            this.复制ToolStripMenuItem.Size = new System.Drawing.Size(104, 26);
            this.复制ToolStripMenuItem.Text = "复制";
            // 
            // viewContextMenuStrip
            // 
            this.viewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.viewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNodeItem,
            this.toolStripSeparator2,
            this.centerItem});
            this.viewContextMenuStrip.Name = "nodeContextMenuStrip";
            this.viewContextMenuStrip.Size = new System.Drawing.Size(129, 62);
            // 
            // addNodeItem
            // 
            this.addNodeItem.Name = "addNodeItem";
            this.addNodeItem.Size = new System.Drawing.Size(128, 26);
            this.addNodeItem.Text = "添加节点";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(125, 6);
            // 
            // centerItem
            // 
            this.centerItem.Image = ((System.Drawing.Image)(resources.GetObject("centerItem.Image")));
            this.centerItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.centerItem.Name = "centerItem";
            this.centerItem.Size = new System.Drawing.Size(128, 26);
            this.centerItem.Text = "居中";
            this.centerItem.Click += new System.EventHandler(this.centerItem_Click);
            // 
            // transitionContextMenuStrip
            // 
            this.transitionContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.transitionContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除连线ToolStripMenuItem});
            this.transitionContextMenuStrip.Name = "transitionContextMenuStrip";
            this.transitionContextMenuStrip.Size = new System.Drawing.Size(129, 30);
            // 
            // 删除连线ToolStripMenuItem
            // 
            this.删除连线ToolStripMenuItem.Image = global::BehaviorTreeEditor.Properties.Resources.delete;
            this.删除连线ToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除连线ToolStripMenuItem.Name = "删除连线ToolStripMenuItem";
            this.删除连线ToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.删除连线ToolStripMenuItem.Text = "删除连线";
            this.删除连线ToolStripMenuItem.Click += new System.EventHandler(this.删除连线ToolStripMenuItem_Click);
            // 
            // ContentUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.DoubleBuffered = true;
            this.Name = "ContentUserControl";
            this.Size = new System.Drawing.Size(660, 368);
            this.Load += new System.EventHandler(this.ContentUserControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ContentUserControl_Paint);
            this.Enter += new System.EventHandler(this.ContentUserControl_Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ContentUserControl_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ContentUserControl_KeyUp);
            this.Leave += new System.EventHandler(this.ContentUserControl_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContentUserControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ContentUserControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ContentUserControl_MouseUp);
            this.Resize += new System.EventHandler(this.ContentUserControl_Resize);
            this.nodeContextMenuStrip.ResumeLayout(false);
            this.viewContextMenuStrip.ResumeLayout(false);
            this.transitionContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.ContextMenuStrip nodeContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 连线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 上移ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下移ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 复制ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip viewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addNodeItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem centerItem;
        private System.Windows.Forms.ContextMenuStrip transitionContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 删除连线ToolStripMenuItem;
    }
}
