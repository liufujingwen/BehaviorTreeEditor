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
    public partial class InputValueDialogForm : Form
    {
        public InputValueDialogForm(String caption, Object obj)
        {
            InitializeComponent();

            Text = caption;
            propertyGrid1.SelectedObject = obj;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            propertyGrid1.Refresh();
        }

        private void cancerBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
