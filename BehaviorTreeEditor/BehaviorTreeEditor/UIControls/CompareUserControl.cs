using System;
using System.Windows.Forms;

namespace BehaviorTreeEditor.UIControls
{
    public partial class CompareUserControl : UserControl
    {
        private NodeDesigner m_Node;
        private bool m_BindState = false;

        public CompareUserControl()
        {
            InitializeComponent();
        }

        public void SetNode(NodeDesigner node)
        {
            m_Node = node;
            BindNode();
        }

        private void BindNode()
        {
            m_BindState = false;

            CBB_LeftCompareType.Items.Clear();
            CBB_RightCompareType.Items.Clear();

            CBB_RightParameter.Items.Clear();
            CBB_CompareType.Items.Clear();

            CustomEnum variableCustomEnum = MainForm.Instance.NodeTemplate.FindEnum("VariableType");
            for (int i = 0; i < variableCustomEnum.Enums.Count; i++)
            {
                EnumItem enumItem = variableCustomEnum.Enums[i];
                CBB_LeftCompareType.Items.Add(enumItem.EnumStr);
                CBB_RightCompareType.Items.Add(enumItem.EnumStr);
            }

            //绑定左边参数类型
            FieldDesigner leftFieldDesigner = m_Node.FindFieldByName("LeftType");
            EnumFieldDesigner leftEnumFieldDesigner = leftFieldDesigner.Field as EnumFieldDesigner;
            CBB_LeftCompareType.SelectedIndex = leftEnumFieldDesigner.ValueIndex;

            //绑定右边参数类型
            FieldDesigner rightFieldDesigner = m_Node.FindFieldByName("RightType");
            EnumFieldDesigner rightEnumFieldDesigner = rightFieldDesigner.Field as EnumFieldDesigner;
            CBB_RightCompareType.SelectedIndex = rightEnumFieldDesigner.ValueIndex;

            //绑定左边参数名
            BindLeftParameter(leftEnumFieldDesigner.Value);

            //绑定右边参数
            BindRightParameter(rightEnumFieldDesigner.Value);

            //绑定比较类型
            CustomEnum compareCustomEnum = MainForm.Instance.NodeTemplate.FindEnum("CompareType");
            for (int i = 0; i < compareCustomEnum.Enums.Count; i++)
            {
                EnumItem enumItem = compareCustomEnum.Enums[i];
                CBB_CompareType.Items.Add(enumItem.EnumStr);
            }
            FieldDesigner compareTypeFieldDesigner = m_Node.FindFieldByName("CompareType");
            EnumFieldDesigner compareEnumFieldDesigner = compareTypeFieldDesigner.Field as EnumFieldDesigner;
            CBB_CompareType.SelectedIndex = compareEnumFieldDesigner.ValueIndex;

            m_BindState = true;
        }

        //改变左边参数类型
        private void CBB_LeftCompareType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_BindState)
                return;

            FieldDesigner leftFieldDesigner = m_Node.FindFieldByName("LeftType");
            EnumFieldDesigner leftEnumFieldDesigner = leftFieldDesigner.Field as EnumFieldDesigner;

            CustomEnum leftNodeType = MainForm.Instance.NodeTemplate.FindEnum("VariableType");
            EnumItem enumItem = leftNodeType.FindEnum(CBB_LeftCompareType.Text.Trim());

            leftEnumFieldDesigner.Value = enumItem.EnumStr;

            //重置参数名
            FieldDesigner leftLeftParameter = m_Node.FindFieldByName("LeftParameter");
            StringFieldDesigner leftStringFieldDesigner = leftLeftParameter.Field as StringFieldDesigner;
            leftStringFieldDesigner.Value = null;

            BindLeftParameter(leftEnumFieldDesigner.Value);
        }

        //改变右边参数类型
        private void CBB_RightCompareType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_BindState)
                return;

            FieldDesigner rightFieldDesigner = m_Node.FindFieldByName("RightType");
            EnumFieldDesigner rightEnumFieldDesigner = rightFieldDesigner.Field as EnumFieldDesigner;

            CustomEnum leftNodeType = MainForm.Instance.NodeTemplate.FindEnum("VariableType");
            EnumItem enumItem = leftNodeType.FindEnum(CBB_RightCompareType.Text.Trim());

            rightEnumFieldDesigner.Value = enumItem.EnumStr;

            //重置参数名
            FieldDesigner rightRightParameter = m_Node.FindFieldByName("RightParameter");
            StringFieldDesigner rightStringFieldDesigner = rightRightParameter.Field as StringFieldDesigner;
            rightStringFieldDesigner.Value = null;

            BindRightParameter(rightEnumFieldDesigner.Value);
        }

        //绑定左边参数名
        private void BindLeftParameter(string varType)
        {
            CBB_LeftParameter.Items.Clear();

            if (varType == "GlobalVar")
            {
                var variable = MainForm.Instance.BehaviorTreeData.GlobalVariable;
                for (int i = 0; i < variable.VariableFields.Count; i++)
                {
                    VariableFieldDesigner variableFieldDesigner = variable.VariableFields[i];
                    CBB_LeftParameter.Items.Add(variableFieldDesigner.VariableFieldName);
                }
            }
            else if (varType == "BehaviorTreeVar")
            {
                BehaviorTreeDesigner behaviorTreeDesigner = MainForm.Instance.SelectedBehaviorTree;
                var variable = behaviorTreeDesigner.BehaviorTreeVariableFields;
                if (variable != null)
                {
                    for (int i = 0; i < variable.Count; i++)
                    {
                        VariableFieldDesigner variableFieldDesigner = variable[i];
                        CBB_LeftParameter.Items.Add(variableFieldDesigner.VariableFieldName);
                    }
                }
            }
            else if (varType == "ContextVar")
            {
                var variable = MainForm.Instance.BehaviorTreeData.ContextVariable;
                for (int i = 0; i < variable.VariableFields.Count; i++)
                {
                    VariableFieldDesigner variableFieldDesigner = variable.VariableFields[i];
                    CBB_LeftParameter.Items.Add(variableFieldDesigner.VariableFieldName);
                }
            }
            else if (varType == "Const")
            {
                //todo
            }

            FieldDesigner leftLeftParameter = m_Node.FindFieldByName("LeftParameter");
            StringFieldDesigner leftStringFieldDesigner = leftLeftParameter.Field as StringFieldDesigner;
            CBB_LeftParameter.Text = leftStringFieldDesigner.Value;
        }

        //绑定右边参数名
        private void BindRightParameter(string varType)
        {
            CBB_RightParameter.Items.Clear();

            if (varType == "GlobalVar")
            {
                var variable = MainForm.Instance.BehaviorTreeData.GlobalVariable;
                for (int i = 0; i < variable.VariableFields.Count; i++)
                {
                    VariableFieldDesigner variableFieldDesigner = variable.VariableFields[i];
                    CBB_RightParameter.Items.Add(variableFieldDesigner.VariableFieldName);
                }
            }
            else if (varType == "BehaviorTreeVar")
            {
                BehaviorTreeDesigner behaviorTreeDesigner = MainForm.Instance.SelectedBehaviorTree;
                var variable = behaviorTreeDesigner.BehaviorTreeVariableFields;
                if (variable != null)
                {
                    for (int i = 0; i < variable.Count; i++)
                    {
                        VariableFieldDesigner variableFieldDesigner = variable[i];
                        CBB_RightParameter.Items.Add(variableFieldDesigner.VariableFieldName);
                    }
                }
            }
            else if (varType == "ContextVar")
            {
                var variable = MainForm.Instance.BehaviorTreeData.ContextVariable;
                for (int i = 0; i < variable.VariableFields.Count; i++)
                {
                    VariableFieldDesigner variableFieldDesigner = variable.VariableFields[i];
                    CBB_RightParameter.Items.Add(variableFieldDesigner.VariableFieldName);
                }
            }
            else if (varType == "Const")
            {
                //todo
            }

            FieldDesigner rightRightParameter = m_Node.FindFieldByName("RightParameter");
            StringFieldDesigner rightStringFieldDesigner = rightRightParameter.Field as StringFieldDesigner;
            CBB_RightParameter.Text = rightStringFieldDesigner.Value;
        }

        private void CBB_LeftParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = CBB_LeftParameter.Text.Trim();
            FieldDesigner leftRightParameter = m_Node.FindFieldByName("LeftParameter");
            StringFieldDesigner leftStringFieldDesigner = leftRightParameter.Field as StringFieldDesigner;
            leftStringFieldDesigner.Value = value;
        }

        private void CBB_RightParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = CBB_RightParameter.Text.Trim();
            FieldDesigner rightRightParameter = m_Node.FindFieldByName("RightParameter");
            StringFieldDesigner rightStringFieldDesigner = rightRightParameter.Field as StringFieldDesigner;
            rightStringFieldDesigner.Value = value;
        }

        private void CBB_CompareType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_BindState)
                return;

            CustomEnum compareCustomEnum = MainForm.Instance.NodeTemplate.FindEnum("CompareType");
            EnumItem enumItem = compareCustomEnum.FindEnum(CBB_CompareType.Text.Trim());

            FieldDesigner compareFieldDesigner = m_Node.FindFieldByName("CompareType");
            EnumFieldDesigner compareEnumFieldDesigner = compareFieldDesigner.Field as EnumFieldDesigner;
            compareEnumFieldDesigner.Value = enumItem.EnumStr;
        }
    }
}