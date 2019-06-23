namespace BehaviorTreeEditor.UIControls
{
    partial class ZoomScalerUserControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.zoom = new System.Windows.Forms.Label();
            this.zoomBar = new System.Windows.Forms.TrackBar();
            this.autoHideTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.zoomBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Zoom:";
            // 
            // zoom
            // 
            this.zoom.AutoSize = true;
            this.zoom.BackColor = System.Drawing.Color.Transparent;
            this.zoom.ForeColor = System.Drawing.Color.White;
            this.zoom.Location = new System.Drawing.Point(41, 8);
            this.zoom.Name = "zoom";
            this.zoom.Size = new System.Drawing.Size(29, 12);
            this.zoom.TabIndex = 2;
            this.zoom.Text = "100%";
            // 
            // zoomBar
            // 
            this.zoomBar.AutoSize = false;
            this.zoomBar.Enabled = false;
            this.zoomBar.Location = new System.Drawing.Point(76, 3);
            this.zoomBar.Name = "zoomBar";
            this.zoomBar.Size = new System.Drawing.Size(104, 22);
            this.zoomBar.TabIndex = 3;
            this.zoomBar.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // autoHideTimer
            // 
            this.autoHideTimer.Interval = 2000;
            this.autoHideTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ZoomScalerUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.zoomBar);
            this.Controls.Add(this.zoom);
            this.Controls.Add(this.label1);
            this.Name = "ZoomScalerUserControl";
            this.Size = new System.Drawing.Size(189, 28);
            this.Load += new System.EventHandler(this.ZoomScalerUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.zoomBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label zoom;
        private System.Windows.Forms.TrackBar zoomBar;
        private System.Windows.Forms.Timer autoHideTimer;
    }
}
