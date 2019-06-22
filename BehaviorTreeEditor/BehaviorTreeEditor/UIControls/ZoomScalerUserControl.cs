using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BehaviorTreeEditor.UIControls
{
    public partial class ZoomScalerUserControl : UserControl
    {
        //最小缩放值
        private float m_Min;
        //最大缩放值
        private float m_Max;
        //当前缩放
        private float m_ZoomScale;

        public ZoomScalerUserControl(float min, float max)
        {
            m_Min = min;
            m_Max = max;
            InitializeComponent();
            zoomBar.Minimum = 0;
            zoomBar.Maximum = (int)(m_Max * 100);
        }

        private void ZoomScalerUserControl_Load(object sender, EventArgs e)
        {
           
        }

        public void SetVisible(bool visible)
        {
            this.Visible = visible;
            this.timer1.Stop();
            if (visible)
                timer1.Start();
        }

        //设置当前缩放大小
        public void SetZoomScale(float zoomScale)
        {
            m_ZoomScale = zoomScale;
            m_ZoomScale = Mathf.Clamp(m_ZoomScale, m_Min, m_Max);
            zoom.Text = (int)(m_ZoomScale * 100) + "%";
            zoomBar.Value = (int)((m_ZoomScale) * 100);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SetVisible(false);
        }
    }
}
