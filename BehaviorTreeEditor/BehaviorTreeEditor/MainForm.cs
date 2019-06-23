using BehaviorTreeEditor.UIControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class MainForm : Form
    {

        private ContentUserControl m_ContentUserControl;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            m_ContentUserControl = new ContentUserControl();
            m_ContentUserControl.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(m_ContentUserControl);
        }

        private void MainForm_Leave(object sender, EventArgs e)
        {
            Console.WriteLine("KeyUp：");
        }
    }
}
