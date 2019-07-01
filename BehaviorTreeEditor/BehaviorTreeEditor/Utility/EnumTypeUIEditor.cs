using BehaviorTreeEditor.UIControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BehaviorTreeEditor
{
    public class EnumTypeUIEditor : UITypeEditor
    {
        public EnumTypeUIEditor()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var edSvc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (edSvc != null)
            {
                EnumTypeUserControl enumTypeUserControl = new EnumTypeUserControl((string)value);
                edSvc.DropDownControl(enumTypeUserControl);
                value = enumTypeUserControl.EnumType;
            }
            return base.EditValue(context, provider, value);
        }
    }
}
