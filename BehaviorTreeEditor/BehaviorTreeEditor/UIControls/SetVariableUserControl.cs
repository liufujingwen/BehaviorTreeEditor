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
    public partial class SetVariableUserControl : UserControl
    {
        private NodeDesigner m_Node;
        private bool m_BindState = false;

        public SetVariableUserControl()
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

            //绑定参数类型
            FieldDesigner parameterType = m_Node.FindFieldByName("ParameterType");
            EnumFieldDesigner parameterTypeEnumFieldDesigner = parameterType.Field as EnumFieldDesigner;
            CBB_ParameterType.SelectedIndex = parameterTypeEnumFieldDesigner.ValueIndex;

            //绑定参数名
            BindParameter(parameterTypeEnumFieldDesigner.GetEnumItemByIndex(CBB_ParameterType.SelectedIndex).EnumStr);

            //绑定参数值
            BindParameterValue();

            m_BindState = true;
        }

        private void CBB_ParameterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_BindState)
                return;

            //置空变量名
            FieldDesigner parameterName = m_Node.FindFieldByName("ParameterName");
            StringFieldDesigner parameterNameStringFieldDesigner = parameterName.Field as StringFieldDesigner;
            parameterNameStringFieldDesigner.Value = null;

            FieldDesigner parameterType = m_Node.FindFieldByName("ParameterType");
            EnumFieldDesigner parameterTypeEnumFieldDesigner = parameterType.Field as EnumFieldDesigner;

            BindParameter(parameterTypeEnumFieldDesigner.GetEnumItemByIndex(CBB_ParameterType.SelectedIndex).EnumStr);
        }

        private void CBB_ParameterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!m_BindState)
                return;
        }

        //绑定左边参数名
        private void BindParameter(string varType)
        {
            CBB_ParameterName.Items.Clear();

            if (varType == "GlobalVar")
            {
                var variable = MainForm.Instance.BehaviorTreeData.GlobalVariable;
                for (int i = 0; i < variable.VariableFields.Count; i++)
                {
                    VariableFieldDesigner variableFieldDesigner = variable.VariableFields[i];
                    CBB_ParameterName.Items.Add(variableFieldDesigner.VariableFieldName);
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
                        CBB_ParameterName.Items.Add(variableFieldDesigner.VariableFieldName);
                    }
                }
            }
            else if (varType == "ContextVar")
            {
                var variable = MainForm.Instance.BehaviorTreeData.ContextVariable;
                for (int i = 0; i < variable.VariableFields.Count; i++)
                {
                    VariableFieldDesigner variableFieldDesigner = variable.VariableFields[i];
                    CBB_ParameterName.Items.Add(variableFieldDesigner.VariableFieldName);
                }
            }

            FieldDesigner parameterName = m_Node.FindFieldByName("ParameterName");
            StringFieldDesigner parameterNameStringFieldDesigner = parameterName.Field as StringFieldDesigner;
            CBB_ParameterName.Text = parameterNameStringFieldDesigner.Value;
        }

        private void BindParameterValue()
        {
            //绑定参数值
            FieldDesigner parameterValue = m_Node.FindFieldByName("ParameterValue");
            BaseFieldDesigner parameterValueBaseFieldDesigner = parameterValue.Field;

            string value = null;
            if (parameterValueBaseFieldDesigner is IntFieldDesigner)
            {
                value = (parameterValueBaseFieldDesigner as IntFieldDesigner).Value.ToString();
            }
            else if (parameterValueBaseFieldDesigner is LongFieldDesigner)
            {
                value = (parameterValueBaseFieldDesigner as LongFieldDesigner).Value.ToString();
            }
            else if (parameterValueBaseFieldDesigner is FloatFieldDesigner)
            {
                value = (parameterValueBaseFieldDesigner as FloatFieldDesigner).Value.ToString();
            }
            else if (parameterValueBaseFieldDesigner is DoubleFieldDesigner)
            {
                value = (parameterValueBaseFieldDesigner as DoubleFieldDesigner).Value.ToString();
            }
            else if (parameterValueBaseFieldDesigner is StringFieldDesigner)
            {
                value = (parameterValueBaseFieldDesigner as StringFieldDesigner).Value;
            }
            else if (parameterValueBaseFieldDesigner is BooleanFieldDesigner)
            {
                value = (parameterValueBaseFieldDesigner as BooleanFieldDesigner).Value.ToString();
            }
            else
            {
                //todo...
            }

            TB_ParameterValue.Text = value;
        }

        private void TB_ParameterValue_TextChanged(object sender, EventArgs e)
        {
            if (!m_BindState)
                return;
        }

        //设置变量值
        private void SetParameterValue(string inputValue)
        {
            


        }


        private void CheckError()
        {
            //现在找到参数类型（作用域）
            FieldDesigner parameterType = m_Node.FindFieldByName("ParameterType");
            EnumFieldDesigner parameterTypeEnumFieldDesigner = parameterType.Field as EnumFieldDesigner;
            string varType = parameterTypeEnumFieldDesigner.GetEnumItemByIndex(CBB_ParameterType.SelectedIndex).EnumStr;

            //找到参数名
            FieldDesigner parameterName = m_Node.FindFieldByName("ParameterName");
            StringFieldDesigner parameterNameStringFieldDesigner = parameterName.Field as StringFieldDesigner;
            CBB_ParameterName.Text = parameterNameStringFieldDesigner.Value;

            if (string.IsNullOrEmpty(parameterNameStringFieldDesigner.Value))
            {
                errorProvider1.SetError(CBB_ParameterName, "请选择参数名称");
                return;
            }


        }
    }
}
