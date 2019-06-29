using BehaviorTreeEditor.UIControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class AddEnumForm : Form
    {
        private EnumForm m_EnumForm;
        private CustomEnum m_CustomEnum = new CustomEnum();
        private List<EnumItemUserControl> m_EnumItemControlList = new List<EnumItemUserControl>();

        public AddEnumForm(EnumForm enumForm)
        {
            m_EnumForm = enumForm;
            InitializeComponent();
        }

        private void AddEnumForm_Load(object sender, EventArgs e)
        {
            BindEnum();
        }

        public void BindEnum()
        {
            m_EnumItemControlList.Clear();
            for (int i = 0; i < m_CustomEnum.Enums.Count; i++)
            {
                EnumItem enumItem = m_CustomEnum.Enums[i];
                EnumItemUserControl enumItemUserControl = new EnumItemUserControl(this, enumItem);
                enumItemUserControl.Width = splitContainer1.Panel1.Width - 20;
                enumItemUserControl.Location = new Point(0, i * (enumItemUserControl.Height + 2));
                splitContainer1.Panel1.Controls.Add(enumItemUserControl);
                m_EnumItemControlList.Add(enumItemUserControl);
            }

            CheckShowScrollBar();
        }

        public EnumItemUserControl GetEnumItemUserControl(EnumItem enumItem)
        {
            if (enumItem == null)
                return null;

            for (int i = 0; i < m_EnumItemControlList.Count; i++)
            {
                EnumItemUserControl enumItemUserControl = m_EnumItemControlList[i];
                if (enumItemUserControl == null)
                    continue;
                if (enumItemUserControl.Tag == enumItem)
                    return enumItemUserControl;
            }

            return null;
        }

        public void RemoveEnumItem(EnumItem enumItem)
        {
            if (enumItem == null)
                return;

            m_CustomEnum.Remove(enumItem.EnumStr);

            EnumItemUserControl enumItemUserControl = GetEnumItemUserControl(enumItem);
            if (enumItemUserControl == null)
                return;

            m_EnumItemControlList.Remove(enumItemUserControl);

            for (int i = 0; i < m_EnumItemControlList.Count; i++)
            {
                EnumItemUserControl tempUserControl = m_EnumItemControlList[i];
                if (tempUserControl == null)
                    continue;
                tempUserControl.Location = new Point(0, i * (enumItemUserControl.Height + 2));
            }

            CheckShowScrollBar();
        }

        public void AddEnumItem(EnumItem enumItem)
        {
            if (enumItem == null)
                return;

            EnumItemUserControl enumItemUserControl = new EnumItemUserControl(this, enumItem);
            enumItemUserControl.Width = splitContainer1.Panel1.Width - 20;
            enumItemUserControl.Location = new Point(0, m_EnumItemControlList.Count * (enumItemUserControl.Height + 2));
            splitContainer1.Panel1.Controls.Add(enumItemUserControl);
            m_EnumItemControlList.Add(enumItemUserControl);

            CheckShowScrollBar();
        }

        public void CheckShowScrollBar()
        {
            if (m_EnumItemControlList.Count == 0)
                return;

            int height = m_EnumItemControlList.Count * m_EnumItemControlList[0].Height + 2;

            if (height > splitContainer1.Panel1.Height)
            {
                splitContainer1.Panel1.AutoScroll = true;
            }
            else
            {
                splitContainer1.Panel1.AutoScroll = false;
            }

            for (int i = 0; i < m_EnumItemControlList.Count; i++)
            {
                EnumItemUserControl enumItemUserControl = m_EnumItemControlList[i];
                enumItemUserControl.Width = splitContainer1.Panel1.AutoScroll ? splitContainer1.Panel1.Width - 20 : splitContainer1.Panel1.Width;
            }
        }

        public void ChangeNormalMode()
        {
            for (int i = 0; i < m_EnumItemControlList.Count; i++)
            {
                EnumItemUserControl enumItemUserControl = m_EnumItemControlList[i];
                if (enumItemUserControl == null)
                    continue;
                enumItemUserControl.ChangeNormalMode();
            }
        }

        private void addEnumItemBTN_Click(object sender, EventArgs e)
        {
            AddEnumItemForm addEnumItemForm = new AddEnumItemForm(this, m_CustomEnum);
            addEnumItemForm.ShowDialog();
        }

        private void enterBTN_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MainForm.Instance.ShowMessage("枚举类型为空,请填写枚举类型");
                return;
            }

            if (m_CustomEnum.Enums.Count == 0)
            {
                MainForm.Instance.ShowMessage("枚举项为0,请添加至少一个枚举项");
                return;
            }

            m_CustomEnum.EnumType = textBox1.Text.Trim();
            m_CustomEnum.Describe = textBox2.Text.Trim();

            if (!MainForm.Instance.NodeClasses.AddEnum(m_CustomEnum))
            {
                return;
            }

            MainForm.Instance.NodeClassDirty = true;
            m_EnumForm.AddEnum(m_CustomEnum);
            this.Close();
        }
    }
}
