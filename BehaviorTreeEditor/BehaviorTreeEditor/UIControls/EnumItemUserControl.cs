﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BehaviorTreeEditor.UIControls
{
    public partial class EnumItemUserControl : UserControl
    {
        private EnumItem m_EnumItem;
        private ModifyEnumItemUserControl m_ModifyEnumItemUserControl;
        private Form m_Form;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form">由哪个From触发了编辑，（AddEnumForm、EditEnumForm）</param>
        /// <param name="enumItem"></param>
        public EnumItemUserControl(Form form, EnumItem enumItem)
        {
            m_Form = form;
            m_EnumItem = enumItem;
            InitializeComponent();
        }
        private void EnumItemUserControl_Load(object sender, EventArgs e)
        {
            m_ModifyEnumItemUserControl = new ModifyEnumItemUserControl(this, m_EnumItem);
        }

        //修改
        private void button1_Click(object sender, EventArgs e)
        {
            ChangeModifyMode();
        }

        //删除
        private void button2_Click(object sender, EventArgs e)
        {

        }

        //正常模式
        public void ChangeNormalMode()
        {
            this.Controls.Remove(m_ModifyEnumItemUserControl);
        }

        //修改模式
        public void ChangeModifyMode()
        {
            this.Controls.Add(m_ModifyEnumItemUserControl);
        }



    }
}