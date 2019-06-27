﻿namespace BehaviorTreeEditor
{
    partial class InputValueDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.enterBTN = new System.Windows.Forms.Button();
            this.cancerBTN = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(355, 489);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.cancerBTN);
            this.splitContainer1.Panel2.Controls.Add(this.enterBTN);
            this.splitContainer1.Size = new System.Drawing.Size(355, 541);
            this.splitContainer1.SplitterDistance = 489;
            this.splitContainer1.TabIndex = 1;
            // 
            // enterBTN
            // 
            this.enterBTN.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.enterBTN.Location = new System.Drawing.Point(268, 13);
            this.enterBTN.Name = "enterBTN";
            this.enterBTN.Size = new System.Drawing.Size(75, 23);
            this.enterBTN.TabIndex = 0;
            this.enterBTN.Text = "确定";
            this.enterBTN.UseVisualStyleBackColor = true;
            // 
            // cancerBTN
            // 
            this.cancerBTN.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancerBTN.Location = new System.Drawing.Point(177, 13);
            this.cancerBTN.Name = "cancerBTN";
            this.cancerBTN.Size = new System.Drawing.Size(75, 23);
            this.cancerBTN.TabIndex = 1;
            this.cancerBTN.Text = "取消";
            this.cancerBTN.UseVisualStyleBackColor = true;
            // 
            // InputValueDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 541);
            this.Controls.Add(this.splitContainer1);
            this.Name = "InputValueDialogForm";
            this.Text = "InputValueDialogForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button cancerBTN;
        private System.Windows.Forms.Button enterBTN;
    }
}