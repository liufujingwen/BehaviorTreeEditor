using System;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class EditEnumItemForm : Form
    {
        private Form m_Form;
        private CustomEnum m_CustomEnum;
        private EnumItem m_EnumItem;

        /// <summary>
        /// 创建修改枚举选项窗口
        /// </summary>
        /// <param name="form">AddEnumForm、EditEnumForm</param>
        /// <param name="enumItem">枚举项</param>
        public EditEnumItemForm(Form form, CustomEnum customEnum, EnumItem enumItem)
        {
            m_Form = form;
            m_CustomEnum = customEnum;
            m_EnumItem = enumItem;
            InitializeComponent();
        }

        private void ModifyEnumItemForm_Load(object sender, EventArgs e)
        {
            label3.Text = string.Format("修改{0}的枚举项", m_CustomEnum.EnumType);
            textBox1.Text = m_EnumItem.EnumStr;
            textBox2.Text = m_EnumItem.EnumValue.ToString();
            textBox3.Text = m_EnumItem.Describe;
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
            if (m_CustomEnum.ExistEnumStr(m_EnumItem.EnumStr, m_EnumItem))
            {
                MainForm.Instance.ShowMessage(string.Format("已存在枚举项:{0},请换一个枚举项字符", m_EnumItem.EnumStr));
                return;
            }

            //验证枚举值是已存在
            if (m_CustomEnum.ExistEnumValue(m_EnumItem.EnumValue, m_EnumItem))
            {
                MainForm.Instance.ShowMessage(string.Format("已存在枚举值:{0}，请换一个枚举值", m_EnumItem.EnumValue));
                return;
            }

            if (m_Form is AddEnumForm)
            {
                (m_Form as AddEnumForm).UpdateEnumItem(m_EnumItem);
            }
            else if (m_Form is EditEnumForm)
            {
                (m_Form as EditEnumForm).UpdateEnumItem(m_EnumItem);
            }

            this.Close();
        }
    }
}
