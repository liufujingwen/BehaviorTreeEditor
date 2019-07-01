using BehaviorTreeEditor.UIControls;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace BehaviorTreeEditor
{

    public class EnumItemUIEditor : UITypeEditor
    {
        public EnumItemUIEditor()
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
                CustomEnum customEnum = null;
                if (context.Instance is EnumDeaultValue)
                {
                    EnumDeaultValue enumDeaultValue = context.Instance as EnumDeaultValue;
                    if (enumDeaultValue != null)
                    {
                        if (string.IsNullOrEmpty(enumDeaultValue.EnumType))
                        {
                            MainForm.Instance.ShowMessage("请先选择枚举类型");
                        }
                        else
                        {
                            customEnum = MainForm.Instance.NodeClasses.FindEnum(enumDeaultValue.EnumType);
                            if (customEnum == null)
                            {
                                MainForm.Instance.ShowMessage(string.Format("不存在枚举类型:{0},请先注册", enumDeaultValue.EnumType));
                            }
                        }
                    }
                }

                if (customEnum != null)
                {
                    EnumItemUserControl enumTypeUserControl = new EnumItemUserControl(customEnum, (string)value);
                    edSvc.DropDownControl(enumTypeUserControl);
                    value = enumTypeUserControl.EnumStr;
                }
            }
            return base.EditValue(context, provider, value);
        }
    }
}
