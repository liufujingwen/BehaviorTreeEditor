using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Design;

namespace BehaviorTreeEditor
{
    public class CollectionEditor : IWindowsFormsEditorService, IServiceProvider, ITypeDescriptorContext
    {
        public static bool EditValue(IWin32Window owner, object component, string propertyName) 
        {
            PropertyDescriptor prop = TypeDescriptor.GetProperties(component)[propertyName];
            if (prop == null)
                throw new ArgumentException("PropertyName [" + propertyName + "] in object not found.");
            UITypeEditor editor = (UITypeEditor) prop.GetEditor(typeof(UITypeEditor));
            CollectionEditor ctx = new CollectionEditor(owner, component, prop);
            if (editor != null && editor.GetEditStyle(ctx) == UITypeEditorEditStyle.Modal)
            {
                object value = prop.GetValue(component);
                value = editor.EditValue(ctx, ctx, value);
                if (!prop.IsReadOnly)
                {
                    prop.SetValue(component, value);
                    return true;
                }
            }

            return false;
        }
        private readonly IWin32Window owner;
        private readonly object component;
        private readonly PropertyDescriptor property;
        private CollectionEditor(IWin32Window owner, object component, PropertyDescriptor property)
        {
            this.owner = owner;
            this.component = component;
            this.property = property;
        }

        public void CloseDropDown()
        {
            throw new NotImplementedException();
        }

        public void DropDownControl(System.Windows.Forms.Control control)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.Form dialog)
        {
            DialogResult result = dialog.ShowDialog(owner);
            if (result == DialogResult.OK)
            {
            }
             
            return result;
        }

        public object GetService(Type serviceType)
        {
            return serviceType == typeof(IWindowsFormsEditorService) ? this : null;
        }

        IContainer ITypeDescriptorContext.Container { get { return null; } }
        object ITypeDescriptorContext.Instance { get { return component; } }
        void ITypeDescriptorContext.OnComponentChanged() { }
        bool ITypeDescriptorContext.OnComponentChanging() { return true; }

        PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor { get { return property; } }
    }

}
