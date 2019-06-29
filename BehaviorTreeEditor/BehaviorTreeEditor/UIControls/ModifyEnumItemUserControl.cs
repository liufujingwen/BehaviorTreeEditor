using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BehaviorTreeEditor.UIControls
{
    public partial class ModifyEnumItemUserControl : UserControl
    {
        private EnumItem m_EnumItem;
        private EnumItemUserControl m_EnumItemUserControl;

        public ModifyEnumItemUserControl(EnumItemUserControl enumItemUserControl, EnumItem enumItem)
        {
            m_EnumItemUserControl = enumItemUserControl;
            m_EnumItem = enumItem;
            InitializeComponent();
        }

        private void ModifyEnumItemUserControl_Load(object sender, EventArgs e)
        {
            textBox1.Text = m_EnumItem.EnumStr;
            textBox2.Text = m_EnumItem.EnumValue.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            m_EnumItem.EnumStr = textBox1.Text.Trim();
            int value = m_EnumItem.EnumValue;
            if (!int.TryParse(textBox2.Text, out value))
            {
            }
            m_EnumItem.EnumValue = value;
            m_EnumItemUserControl.ChangeNormalMode();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            m_EnumItemUserControl.ChangeNormalMode();
        }
    }
}
