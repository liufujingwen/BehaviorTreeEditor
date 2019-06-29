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
    public partial class EnumForm : Form
    {
        NodeClasses m_NodeClasses;

        public EnumForm()
        {
            m_NodeClasses = MainForm.Instance.NodeClasses;
            InitializeComponent();
        }

        private void EnumForm_Load(object sender, EventArgs e)
        {
            ImageList il = new ImageList();
            //设置高度
            il.ImageSize = new Size(1, 20);
            //绑定listView控件
            listView1.SmallImageList = il;

            BindEnum();
        }

        private void BindEnum()
        {
            if (m_NodeClasses == null)
                return;

            listView1.Items.Clear();
            for (int i = 0; i < m_NodeClasses.Enums.Count; i++)
            {
                CustomEnum customEnum = m_NodeClasses.Enums[i];
                ListViewItem listViewItem = listView1.Items.Add(customEnum.EnumType);
                listViewItem.Tag = customEnum;
                string content = string.Empty;
                for (int j = 0; j < customEnum.Enums.Count; j++)
                {
                    EnumItem enumItem = customEnum.Enums[j];
                    content += string.Format("{0} = {1} {2}", enumItem.EnumStr, enumItem.EnumValue, j < customEnum.Enums.Count - 1 ? "," : string.Empty);
                }
                listViewItem.SubItems.Add(content);
                listViewItem.SubItems.Add(customEnum.Describe);
            }
        }

        //private TreeNode FindTreeNode(object obj)
        //{
        //    if (obj == null)
        //        return null;

        //    for (int i = 0; i < treeView1.Nodes.Count; i++)
        //    {
        //        TreeNode treeNode_i = treeView1.Nodes[i];
        //        if (treeNode_i.Tag == obj)
        //        {
        //            return treeNode_i;
        //        }
        //        else
        //        {
        //            for (int ii = 0; ii < treeNode_i.Nodes.Count; ii++)
        //            {
        //                TreeNode treeNode_ii = treeNode_i.Nodes[ii];
        //                if (treeNode_ii.Tag == obj)
        //                {
        //                    return treeNode_ii;
        //                }
        //            }
        //        }
        //    }

        //    for (int i = 0; i < m_NodeClasses.Enums.Count; i++)
        //    {
        //        CustomEnum customEnum = m_NodeClasses.Enums[i];
        //        TreeNode enumTreeNode = treeView1.Nodes.Add(customEnum.EnumType);
        //        enumTreeNode.Tag = customEnum;
        //        for (int j = 0; j < customEnum.Enums.Count; j++)
        //        {
        //            EnumItem enumItem = customEnum.Enums[j];
        //            TreeNode itemTreeNode = enumTreeNode.Nodes.Add(string.Format("{0}  ({1})", enumItem.EnumStr, enumItem.EnumValue));
        //            itemTreeNode.Tag = enumItem;
        //        }
        //    }

        //    return null;
        //}

        private void 添加类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CustomEnum customEnum = new CustomEnum();
            //InputValueDialogForm inputValueDialogForm = new InputValueDialogForm("添加枚举类型", customEnum);
            //if (inputValueDialogForm.ShowDialog() == DialogResult.OK)
            //{
            //    if (m_NodeClasses.AddEnum(customEnum))
            //    {
            //        BindEnum();
            //        TreeNode treeNode = FindTreeNode(customEnum);
            //        treeView1.SelectedNode = treeNode;
            //        MainForm.Instance.NodeClassDirty = true;
            //        MainForm.Instance.ShowInfo(string.Format("添加枚举{0}成功 时间:{1}", customEnum, DateTime.Now));
            //    }
            //}
        }

        private void 编辑类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (treeView1.SelectedNode == null)
            //    return;

            //CustomEnum customEnum = null;

            //if (treeView1.SelectedNode.Tag is CustomEnum)
            //{
            //    customEnum = treeView1.SelectedNode.Tag as CustomEnum;
            //}
            //else if (treeView1.SelectedNode.Tag is EnumItem)
            //{
            //    customEnum = treeView1.SelectedNode.Parent.Tag as CustomEnum;
            //}

            //if (customEnum == null)
            //    return;

            //string oldEnumContent = XmlUtility.ObjectToString(customEnum);
            //InputValueDialogForm inputValueDialogForm = new InputValueDialogForm("编辑枚举", customEnum);
            //if (inputValueDialogForm.ShowDialog() == DialogResult.OK)
            //{
            //    customEnum.Check();

            //    string newEnumContent = XmlUtility.ObjectToString(customEnum);
            //    if (newEnumContent != oldEnumContent)
            //    {
            //        BindEnum();
            //        TreeNode treeNode = FindTreeNode(customEnum);
            //        treeView1.SelectedNode = treeNode;
            //        MainForm.Instance.NodeClassDirty = true;
            //        MainForm.Instance.ShowInfo(string.Format("修改枚举{0}成功 时间:{1}", customEnum, DateTime.Now));
            //    }
            //}
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (treeView1.SelectedNode == null)
            //    return;

            //if (treeView1.SelectedNode.Tag is CustomEnum)
            //{
            //    CustomEnum customEnum = treeView1.SelectedNode.Tag as CustomEnum;
            //    TreeNode treeNode = FindTreeNode(customEnum);
            //    if (MessageBox.Show(string.Format("确定删除枚举类型{0}吗?", customEnum.EnumType), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //    {
            //        if (treeNode == null)
            //            return;

            //        treeView1.Nodes.Remove(treeNode);
            //        MainForm.Instance.NodeClassDirty = true;
            //        MainForm.Instance.ShowInfo(string.Format("删除枚举类型{0} 时间:{1}", customEnum.EnumType, DateTime.Now));
            //    }
            //}
            //else if (treeView1.SelectedNode.Tag is EnumItem)
            //{
            //    CustomEnum customEnum = treeView1.SelectedNode.Parent.Tag as CustomEnum;
            //    EnumItem enumItem = treeView1.SelectedNode.Tag as EnumItem;
            //    TreeNode treeNode = FindTreeNode(enumItem);
            //    if (MessageBox.Show(string.Format("确定删除枚举{0}的{1}选项吗?", customEnum.EnumType, enumItem.EnumStr), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //    {
            //        if (treeNode == null)
            //            return;

            //        treeView1.Nodes.Remove(treeNode);
            //        MainForm.Instance.NodeClassDirty = true;
            //        MainForm.Instance.ShowInfo(string.Format("删除枚举{0}的{1} 时间:{2}", customEnum.EnumType, enumItem.EnumStr, DateTime.Now));
            //    }
            //}
        }

        private void 重置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("确定删除全部枚举吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            //{
            //    m_NodeClasses.Enums.Clear();
            //    treeView1.Nodes.Clear();
            //    MainForm.Instance.NodeClassDirty = true;
            //    MainForm.Instance.ShowInfo(string.Format("删除全部枚举 时间:{0}", DateTime.Now));
            //}
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
                    InputValueDialogForm dlg = new InputValueDialogForm("编辑枚举", listView1.SelectedItems[0].Tag);
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        Exec("Refresh");
                    }
                }
                else
                {
                    Exec("New");
                }
                Exec("Refresh");
            }
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

        private void 删除ToolStripMenuItem_Click_1(object sender, EventArgs e)
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
                case "Save":
                    Save();
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

        private void Save()
        {
            MainForm.Instance.Exec(OperationType.Save);
        }

        public class EnumListContent
        {
            List<CustomEnum> m_DataList = new List<CustomEnum>();
            public List<CustomEnum> DataList { get { return m_DataList; } }
        }

        private void Copy()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                EnumListContent content = new EnumListContent();
                foreach (ListViewItem lvItem in listView1.SelectedItems)
                {
                    if (lvItem.Tag is CustomEnum)
                        content.DataList.Add((CustomEnum)lvItem.Tag);
                }

                if (content.DataList.Count > 0)
                    Clipboard.SetText(XmlUtility.ObjectToString(content));

                MainForm.Instance.ShowInfo("您复制了" + content.DataList.Count.ToString() + "个枚举！！！");
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
                EnumListContent content = (EnumListContent)XmlUtility.StringToObject<EnumListContent>(Clipboard.GetText());

                for (int i = 0; i < content.DataList.Count; i++)
                {
                    CustomEnum customEnum = content.DataList[i];
                    string enumType = customEnum.EnumType;
                    do
                    {
                        enumType += "_New";
                    }
                    while (m_NodeClasses.ExistEnumType(enumType));

                    customEnum.EnumType = enumType;
                    m_NodeClasses.AddEnum(customEnum);
                }
                MainForm.Instance.NodeClassDirty = true;
                Exec("Refresh");
                MainForm.Instance.ShowInfo("您粘贴了" + content.DataList.Count + "个枚举！！！");
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

                CustomEnum preEnum = m_NodeClasses.Enums[preIdx];
                CustomEnum selectedEnum = m_NodeClasses.Enums[selectIdx];

                m_NodeClasses.Enums[preIdx] = selectedEnum;
                m_NodeClasses.Enums[selectIdx] = preEnum;

                selectIdx = preIdx;
            }
            else
            {
                //最后一个不能往下交换
                if (selectIdx == listView1.Items.Count - 1)
                    return;

                int nextIdx = selectIdx + 1;

                CustomEnum preEnum = m_NodeClasses.Enums[nextIdx];
                CustomEnum selectedEnum = m_NodeClasses.Enums[selectIdx];

                m_NodeClasses.Enums[nextIdx] = selectedEnum;
                m_NodeClasses.Enums[selectIdx] = preEnum;

                selectIdx = nextIdx;
            }

            MainForm.Instance.NodeClassDirty = true;
            Exec("Refresh");
            MainForm.Instance.ShowInfo("交换成功 时间:" + DateTime.Now);
            listView1.Items[selectIdx].Selected = true;
        }

        private void Edit()
        {
            if (listView1.SelectedIndices.Count == 1)
            {
                int selectIdx = listView1.SelectedIndices[0];
                ListViewItem selectedItem = listView1.Items[0];
                CustomEnum customEnum = selectedItem.Tag as CustomEnum;
            }
            CollectionEditor.EditValue(this, m_NodeClasses.Enums, "Fields");
        }

        private void Delete()
        {
            if (listView1.SelectedIndices.Count == 1)
            {
                int selectIdx = listView1.SelectedIndices[0];
                ListViewItem selectedItem = listView1.Items[selectIdx];
                CustomEnum customEnum = selectedItem.Tag as CustomEnum;

                if (MessageBox.Show(string.Format("确定删除枚举类型{0}吗?", customEnum.EnumType), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    listView1.Items.RemoveAt(selectIdx);
                    m_NodeClasses.RemoveEnum(customEnum);
                    MainForm.Instance.NodeClassDirty = true;
                    MainForm.Instance.ShowInfo(string.Format("删除枚举类型{0} 时间:{1}", customEnum.EnumType, DateTime.Now));
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
        }
    }
}