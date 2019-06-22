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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ContentUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.DoubleBuffered = true;
            this.Name = "ContentUserControl";
            this.Size = new System.Drawing.Size(660, 368);
            this.Load += new System.EventHandler(this.ContentUserControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ContentUserControl_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ContentUserControl_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ContentUserControl_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContentUserControl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ContentUserControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ContentUserControl_MouseUp);
            this.Resize += new System.EventHandler(this.ContentUserControl_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
    }
}
