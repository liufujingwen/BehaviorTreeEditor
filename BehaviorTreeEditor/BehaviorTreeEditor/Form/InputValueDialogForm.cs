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
    public partial class InputValueDialogForm : Form
    {
        public InputValueDialogForm(String caption, Object obj)
        {
            InitializeComponent();

            Text = caption;
            propertyGrid1.SelectedObject = obj;
        }

        private void InputValueDialogForm1_Load(object sender, EventArgs e)
        {

        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            propertyGrid1.Refresh();
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }
    }
}
