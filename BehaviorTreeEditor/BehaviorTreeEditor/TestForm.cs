using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
