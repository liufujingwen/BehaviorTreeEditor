using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BehaviorTreeEditor.UIControls
{
    public partial class EnumItemUserControl : UserControl
    {
        ITypeDescriptorContext m_Context;
        private CustomEnum m_CustomEnum;
        private string m_EnumStr;

        public string EnumStr
        {
            get { return m_EnumStr; }
        }

        public EnumItemUserControl(ITypeDescriptorContext context, CustomEnum customEnum, string defaultValue)
        {
            m_Context = context;
            m_CustomEnum = customEnum;
            m_EnumStr = defaultValue;
            InitializeComponent();
        }

        private void EnumItemUserControl_Load(object sender, EventArgs e)
        {
            this.listBox1.DrawMode = DrawMode.OwnerDrawVariable;
            this.listBox1.DrawItem += new DrawItemEventHandler(ListBoxGroupRange_DrawItem);

            if (m_CustomEnum == null)
                return;

            //绑定
            listBox1.Items.Clear();
            if (m_CustomEnum.Enums.Count > 0)
            {
                int selectedIndex = 0;

                for (int i = 0; i < m_CustomEnum.Enums.Count; i++)
                {
                    EnumItem enumItem = m_CustomEnum.Enums[i];
                    listBox1.Items.Add(enumItem.EnumStr + " " + enumItem.EnumValue);
                    if (m_EnumStr == enumItem.EnumStr)
                        selectedIndex = i;
                }

                listBox1.SelectedIndex = selectedIndex;
            }
        }

        //自绘Item，使其视觉效果更好
        private void ListBoxGroupRange_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            if (e.Index >= 0)
            {
                StringFormat sStringFormat = new StringFormat();
                sStringFormat.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds, sStringFormat);
            }
            e.DrawFocusRectangle();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0 && m_CustomEnum.Enums.Count > 0)
            {
                EnumItem enumItem = m_CustomEnum.Enums[listBox1.SelectedIndex];
                m_EnumStr = enumItem.EnumStr;
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            //if (listBox1.SelectedIndex >= 0 && m_CustomEnum.Enums.Count > 0)
            //{
            //    EnumItem enumItem = m_CustomEnum.Enums[listBox1.SelectedIndex];
            //    if(m_EnumStr == enumItem.EnumStr)
            //    {
            //        Console.WriteLine("111");
            //    }
            //}
        }
    }
}
