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
    public partial class AddEnumItemForm : Form
    {
        private CustomEnum m_CustomEnum;
        private EnumItem m_EnumItem = new EnumItem();
        private Form m_Form;

        /// <summary>
        /// 创建添加枚举项窗口
        /// </summary>
        /// <param name="form">由哪个窗口触发，(AddEnumForm、EditEnumForm)</param>
        /// <param name="customEnum"></param>
        public AddEnumItemForm(Form form, CustomEnum customEnum)
        {
            m_Form = form;
            m_CustomEnum = customEnum;
            InitializeComponent();
        }

        private void AddEnumItemForm_Load(object sender, EventArgs e)
        {
            label3.Text = string.Format("为枚举类型:{0},添加枚举项", m_CustomEnum.EnumType);
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void enterBTN_Click(object sender, EventArgs e)
        {
            m_EnumItem.EnumStr = textBox1.Text.Trim();

            //验证枚举值
            int value = m_EnumItem.EnumValue;
            if (!int.TryParse(textBox2.Text, out value))
            {
                MainForm.Instance.ShowMessage("枚举值请填写整数");
                return;
            }
            m_EnumItem.EnumValue = value;

            //枚举项描述
            m_EnumItem.Describe = textBox3.Text.Trim();

            //验证枚举项是否为空
            if (string.IsNullOrEmpty(m_EnumItem.EnumStr))
            {
                MainForm.Instance.ShowMessage("枚举项不能为空");
                return;
            }

            //验证枚举选项是否已存在
            if (m_CustomEnum.ExistEnumStr(m_EnumItem.EnumStr))
            {
                MainForm.Instance.ShowMessage(string.Format("已存在枚举项:{0},请换一个枚举项字符", m_EnumItem.EnumStr));
                return;
            }

            //验证枚举值是已存在
            if (m_CustomEnum.ExistEnumValue(m_EnumItem.EnumValue))
            {
                MainForm.Instance.ShowMessage(string.Format("已存在枚举值:{0}，请换一个枚举值", m_EnumItem.EnumValue));
                return;
            }

            m_CustomEnum.AddEnumItem(m_EnumItem);

            if (m_Form is AddEnumForm)
            {
                (m_Form as AddEnumForm).AddEnumItem(m_EnumItem);
            }
            else if (m_Form is EditEnumForm)
            {
                (m_Form as EditEnumForm).AddEnumItem(m_EnumItem);
            }

            this.Close();
        }
    }
}
