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
    public partial class EnumTypeUserControl : UserControl
    {
        private string m_EnumType;
        private NodeClasses m_NodeClasses;

        public string EnumType
        {
            get { return m_EnumType; }
        }

        public EnumTypeUserControl(string enumType)
        {
            m_EnumType = enumType;
            InitializeComponent();
        }

        private void EnumTypeUserControl_Load(object sender, EventArgs e)
        {
            this.listBox1.DrawMode = DrawMode.OwnerDrawVariable;
            this.listBox1.DrawItem += new DrawItemEventHandler(ListBoxGroupRange_DrawItem);

            m_NodeClasses = MainForm.Instance.NodeClasses;

            if (m_NodeClasses == null)
                return;

            //绑定

            listBox1.Items.Clear();
            if (m_NodeClasses.Enums.Count > 0)
            {
                int selectedIndex = 0;

                for (int i = 0; i < m_NodeClasses.Enums.Count; i++)
                {
                    string tempEnumName = m_NodeClasses.Enums[i].EnumType;
                    listBox1.Items.Add(tempEnumName);
                    if (m_EnumType == tempEnumName)
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
            if (listBox1.SelectedIndex >= 0 && m_NodeClasses.Enums.Count > 0)
            {
                CustomEnum customEnum = m_NodeClasses.Enums[listBox1.SelectedIndex];
                m_EnumType = customEnum.EnumType;
            }
        }
    }
}
