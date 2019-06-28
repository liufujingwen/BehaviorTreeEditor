using System;
using System.Windows.Forms;

namespace BehaviorTreeEditor
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            AgentDesigner agent = new AgentDesigner();
            //FieldDesigner field = new FieldDesigner();

            //field.FieldType = FieldType.IntFieldType;
            //IntFieldDesigner intField = new IntFieldDesigner();
            //intField.FieldName = "测试节点";
            //intField.Value = 100;
            //field.Field = intField;
            //agent.Fields.Add(field);
            //propertyGrid1.SelectedObject = agent;
        }
    }
}
