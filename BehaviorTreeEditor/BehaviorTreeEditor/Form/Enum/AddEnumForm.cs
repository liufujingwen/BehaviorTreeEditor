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

        public AddEnumForm(EnumForm enumForm)
        {
            m_EnumForm = enumForm;
            InitializeComponent();
        }

        private void AddEnumForm_Load(object sender, EventArgs e)
        {
            BindEnum();
        }

        private ListViewItem GetListViewItem(object obj)
        {
            if (obj == null)
                return null;

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                ListViewItem listViewItem = listView1.Items[i];
                if (listViewItem.Tag == obj)
                    return listViewItem;
            }

            return null;
        }

        private void listView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Exec("Copy");
                        break;
                    case Keys.V:
                        Exec("Paste");
                        break;
                    case Keys.Up:
                        Exec("Swap", true);
                        break;
                    case Keys.Down:
                        Exec("Swap", false);
                        break;
                    case Keys.S:
                        Exec("Save", false);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F12:
                        Exec("Edit");
                        break;
                    case Keys.N:
                        Exec("New");
                        break;
                    case Keys.Delete:
                        Exec("Delete");
                        break;
                }
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Tag = listView1;
                    contextMenuStrip1.Show(listView1, e.Location);
                }
            }
            else if (e.Clicks == 2)
            {
                if (listView1.SelectedItems.Count == 1)
                {
                    Exec("Edit");
                }
                else
                {
                    Exec("New");
                }
            }
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("Copy");
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("Paste");
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("New");
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("Edit");
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exec("Delete");
        }

        public bool Exec(string cmd, params object[] args)
        {
            if (string.IsNullOrEmpty(cmd))
                return false;

            switch (cmd)
            {
                case "Refresh":
                    BindEnum();
                    break;
                case "Copy":
                    Copy();
                    break;
                case "Paste":
                    Paste();
                    break;
                case "Swap":
                    Swap((bool)args[0]);
                    break;
                case "Edit":
                    Edit();
                    break;
                case "Delete":
                    Delete();
                    break;
                case "New":
                    New();
                    break;
            }

            return true;
        }

        private void BindEnum()
        {
            listView1.Items.Clear();
            for (int i = 0; i < m_CustomEnum.Enums.Count; i++)
            {
                EnumItem enumItem = m_CustomEnum.Enums[i];
                ListViewItem listViewItem = listView1.Items.Add(enumItem.EnumStr);
                listViewItem.Tag = enumItem;
                listViewItem.SubItems.Add(enumItem.EnumValue.ToString());
                listViewItem.SubItems.Add(enumItem.Describe);
            }
        }

        public class EnumItemListContent
        {
            private List<EnumItem> m_DataList = new List<EnumItem>();
            public List<EnumItem> DataList { get { return m_DataList; } }
        }

        private void Copy()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                EnumItemListContent content = new EnumItemListContent();
                foreach (ListViewItem lvItem in listView1.SelectedItems)
                {
                    if (lvItem.Tag is EnumItem)
                        content.DataList.Add((EnumItem)lvItem.Tag);
                }

                if (content.DataList.Count > 0)
                    Clipboard.SetText(XmlUtility.ObjectToString(content));

                MainForm.Instance.ShowInfo("您复制了" + content.DataList.Count.ToString() + "个枚举选项！！！");
            }
            else
            {
                MainForm.Instance.ShowInfo("您必须选择至少一个进行复制！！！");
                MainForm.Instance.ShowMessage("您必须选择至少一个进行复制！！！", "警告");
            }
        }

        private void Paste()
        {
            try
            {
                EnumItemListContent content = XmlUtility.StringToObject<EnumItemListContent>(Clipboard.GetText());

                for (int i = 0; i < content.DataList.Count; i++)
                {
                    EnumItem customEnum = content.DataList[i];
                    string enumStr = customEnum.EnumStr;
                    do
                    {
                        enumStr += "_New";
                    }
                    while (m_CustomEnum.ExistEnumStr(enumStr));

                    int enumValue = customEnum.EnumValue;
                    do
                    {
                        enumValue++;
                    }
                    while (m_CustomEnum.ExistEnumValue(enumValue));

                    customEnum.EnumStr = enumStr;
                    customEnum.EnumValue = enumValue;

                    m_CustomEnum.AddEnumItem(customEnum);
                }

                Exec("Refresh");
                MainForm.Instance.ShowInfo("您粘贴了" + content.DataList.Count + "个枚举选项！！！");
            }
            catch (Exception ex)
            {
                MainForm.Instance.ShowInfo("无法进行粘贴，错误信息：" + ex.Message);
                MainForm.Instance.ShowMessage("无法进行粘贴，错误信息：" + ex.Message, "警告");
            }
        }

        private void Swap(bool up)
        {
            if (listView1.SelectedIndices.Count > 1)
            {
                MainForm.Instance.ShowInfo("请选择一条记录进行交换");
                MainForm.Instance.ShowMessage("请选择一条记录进行交换", "警告");
                return;
            }

            int selectIdx = listView1.SelectedIndices[0];
            if (up)
            {
                //第一个不能往上交换
                if (selectIdx == 0)
                    return;

                int preIdx = selectIdx - 1;

                EnumItem preEnum = m_CustomEnum.Enums[preIdx];
                EnumItem selectedEnum = m_CustomEnum.Enums[selectIdx];

                m_CustomEnum.Enums[preIdx] = selectedEnum;
                m_CustomEnum.Enums[selectIdx] = preEnum;

                selectIdx = preIdx;
            }
            else
            {
                //最后一个不能往下交换
                if (selectIdx == listView1.Items.Count - 1)
                    return;

                int nextIdx = selectIdx + 1;

                EnumItem preEnum = m_CustomEnum.Enums[nextIdx];
                EnumItem selectedEnum = m_CustomEnum.Enums[selectIdx];

                m_CustomEnum.Enums[nextIdx] = selectedEnum;
                m_CustomEnum.Enums[selectIdx] = preEnum;

                selectIdx = nextIdx;
            }

            //MainForm.Instance.NodeClassDirty = true;
            Exec("Refresh");
            MainForm.Instance.ShowInfo("交换成功 时间:" + DateTime.Now);
            listView1.Items[selectIdx].Selected = true;
        }

        private void Edit()
        {
            if (listView1.SelectedIndices.Count == 1)
            {
                int selectIdx = listView1.SelectedIndices[0];
                ListViewItem selectedItem = listView1.Items[selectIdx];
                EnumItem enumItem = selectedItem.Tag as EnumItem;
                EditEnumItemForm editEnumForm = new EditEnumItemForm(this, m_CustomEnum, enumItem);
                editEnumForm.ShowDialog();
            }
        }

        private void Delete()
        {
            if (listView1.SelectedIndices.Count == 1)
            {
                int selectIdx = listView1.SelectedIndices[0];
                ListViewItem selectedItem = listView1.Items[selectIdx];
                EnumItem enumItem = selectedItem.Tag as EnumItem;

                if (MessageBox.Show(string.Format("确定删除枚举选项{0}吗?", enumItem.EnumStr), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    listView1.Items.RemoveAt(selectIdx);
                    m_CustomEnum.Remove(enumItem.EnumStr);
                    MainForm.Instance.ShowInfo(string.Format("删除枚举选项{0} 时间:{1}", enumItem.EnumStr, DateTime.Now));
                    if (listView1.Items.Count > selectIdx)
                        listView1.Items[selectIdx].Selected = true;
                }
            }
            else if (listView1.SelectedIndices.Count == 0)
            {
                MainForm.Instance.ShowInfo("请选择一条记录进行删除");
            }
            else
            {
                MainForm.Instance.ShowInfo("无法一次删除多个记录");
            }
        }

        private void New()
        {
            AddEnumItemForm addEnumItemForm = new AddEnumItemForm(this, m_CustomEnum);
            addEnumItemForm.ShowDialog();
        }

        public void AddEnumItem(EnumItem enumItem)
        {
            if (enumItem == null)
                return;

            Exec("Refresh");

            ListViewItem listViewItem = GetListViewItem(enumItem);
            if (listViewItem != null)
                listViewItem.Selected = true;
        }

        public void UpdateEnumItem(EnumItem enumItem)
        {
            if (enumItem == null)
                return;

            Exec("Refresh");

            ListViewItem listViewItem = GetListViewItem(enumItem);
            if (listViewItem != null)
                listViewItem.Selected = true;
        }

        private void cancelBTN_Click(object sender, EventArgs e)
        {
            this.Close();
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

            VerifyInfo verifyEnum = m_CustomEnum.VerifyEnum();
            if (verifyEnum.HasError)
            {
                MainForm.Instance.ShowMessage(verifyEnum.Msg);
                return;
            }

            if (!MainForm.Instance.NodeClasses.AddEnum(m_CustomEnum))
            {
                return;
            }

            m_EnumForm.AddEnum(m_CustomEnum);
            this.Close();
        }
    }
}
