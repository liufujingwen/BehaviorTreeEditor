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
            BindEnum();
        }

        private void BindEnum()
        {
            treeView1.Nodes.Clear();

            for (int i = 0; i < m_NodeClasses.Enums.Count; i++)
            {
                CustomEnum customEnum = m_NodeClasses.Enums[i];
                TreeNode enumTreeNode = treeView1.Nodes.Add(customEnum.EnumType);
                enumTreeNode.Tag = customEnum;
                for (int j = 0; j < customEnum.Enums.Count; j++)
                {
                    EnumItem enumItem = customEnum.Enums[j];
                    TreeNode itemTreeNode = enumTreeNode.Nodes.Add(string.Format("{0}  ({1})", enumItem.EnumStr, enumItem.EnumValue));
                    itemTreeNode.Tag = enumItem;
                }
            }

            treeView1.ExpandAll();
        }

        private TreeNode FindTreeNode(object obj)
        {
            if (obj == null)
                return null;

            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                TreeNode treeNode_i = treeView1.Nodes[i];
                if (treeNode_i.Tag == obj)
                {
                    return treeNode_i;
                }
                else
                {
                    for (int ii = 0; ii < treeNode_i.Nodes.Count; ii++)
                    {
                        TreeNode treeNode_ii = treeNode_i.Nodes[ii];
                        if (treeNode_ii.Tag == obj)
                        {
                            return treeNode_ii;
                        }
                    }
                }
            }

            for (int i = 0; i < m_NodeClasses.Enums.Count; i++)
            {
                CustomEnum customEnum = m_NodeClasses.Enums[i];
                TreeNode enumTreeNode = treeView1.Nodes.Add(customEnum.EnumType);
                enumTreeNode.Tag = customEnum;
                for (int j = 0; j < customEnum.Enums.Count; j++)
                {
                    EnumItem enumItem = customEnum.Enums[j];
                    TreeNode itemTreeNode = enumTreeNode.Nodes.Add(string.Format("{0}  ({1})", enumItem.EnumStr, enumItem.EnumValue));
                    itemTreeNode.Tag = enumItem;
                }
            }

            return null;
        }

        private void 添加类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomEnum customEnum = new CustomEnum();
            InputValueDialogForm inputValueDialogForm = new InputValueDialogForm("添加枚举类型", customEnum);
            if (inputValueDialogForm.ShowDialog() == DialogResult.OK)
            {
                if (m_NodeClasses.AddEnum(customEnum))
                {
                    BindEnum();
                    TreeNode treeNode = FindTreeNode(customEnum);
                    treeView1.SelectedNode = treeNode;
                    MainForm.Instance.NodeClassDirty = true;
                    MainForm.Instance.ShowInfo(string.Format("添加枚举{0}成功 时间:{1}", customEnum, DateTime.Now));
                }
            }
        }

        private void 编辑类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            CustomEnum customEnum = null;

            if (treeView1.SelectedNode.Tag is CustomEnum)
            {
                customEnum = treeView1.SelectedNode.Tag as CustomEnum;
            }
            else if (treeView1.SelectedNode.Tag is EnumItem)
            {
                customEnum = treeView1.SelectedNode.Parent.Tag as CustomEnum;
            }

            if (customEnum == null)
                return;

            string oldEnumContent = XmlUtility.ObjectToString(customEnum);
            InputValueDialogForm inputValueDialogForm = new InputValueDialogForm("编辑枚举", customEnum);
            if (inputValueDialogForm.ShowDialog() == DialogResult.OK)
            {
                customEnum.Check();

                string newEnumContent = XmlUtility.ObjectToString(customEnum);
                if (newEnumContent != oldEnumContent)
                {
                    BindEnum();
                    TreeNode treeNode = FindTreeNode(customEnum);
                    treeView1.SelectedNode = treeNode;
                    MainForm.Instance.NodeClassDirty = true;
                    MainForm.Instance.ShowInfo(string.Format("修改枚举{0}成功 时间:{1}", customEnum, DateTime.Now));
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;

            if (treeView1.SelectedNode.Tag is CustomEnum)
            {
                CustomEnum customEnum = treeView1.SelectedNode.Tag as CustomEnum;
                TreeNode treeNode = FindTreeNode(customEnum);
                if (MessageBox.Show(string.Format("确定删除枚举类型{0}吗?", customEnum.EnumType), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (treeNode == null)
                        return;

                    treeView1.Nodes.Remove(treeNode);
                    MainForm.Instance.NodeClassDirty = true;
                    MainForm.Instance.ShowInfo(string.Format("删除枚举类型{0} 时间:{1}", customEnum.EnumType, DateTime.Now));
                }
            }
            else if (treeView1.SelectedNode.Tag is EnumItem)
            {
                CustomEnum customEnum = treeView1.SelectedNode.Parent.Tag as CustomEnum;
                EnumItem enumItem = treeView1.SelectedNode.Tag as EnumItem;
                TreeNode treeNode = FindTreeNode(enumItem);
                if (MessageBox.Show(string.Format("确定删除枚举{0}的{1}选项吗?", customEnum.EnumType, enumItem.EnumStr), "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (treeNode == null)
                        return;

                    treeView1.Nodes.Remove(treeNode);
                    MainForm.Instance.NodeClassDirty = true;
                    MainForm.Instance.ShowInfo(string.Format("删除枚举{0}的{1} 时间:{2}", customEnum.EnumType, enumItem.EnumStr, DateTime.Now));
                }
            }
        }

        private void 重置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定删除全部枚举吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                m_NodeClasses.Enums.Clear();
                treeView1.Nodes.Clear();
                MainForm.Instance.NodeClassDirty = true;
                MainForm.Instance.ShowInfo(string.Format("删除全部枚举 时间:{0}", DateTime.Now));
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip1.Show(this, e.Location);
                }
            }
        }
    }
}