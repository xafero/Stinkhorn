namespace Stinkhorn.Bureau.Controls
{
    partial class ResponseControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.propertyGrid1 = new AdamsLair.WinForms.PropertyEditing.PropertyGrid();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dragDropControl1 = new Stinkhorn.Bureau.Controls.DragDropControl();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(368, 289);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.AllowDrop = true;
            this.propertyGrid1.AutoScroll = true;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.ReadOnly = false;
            this.propertyGrid1.ShowNonPublic = false;
            this.propertyGrid1.Size = new System.Drawing.Size(360, 96);
            this.propertyGrid1.SortEditorsByName = true;
            this.propertyGrid1.SplitterPosition = 144;
            this.propertyGrid1.SplitterRatio = 0.4F;
            this.propertyGrid1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 19);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dragDropControl1);
            this.splitContainer1.Size = new System.Drawing.Size(360, 266);
            this.splitContainer1.SplitterDistance = 96;
            this.splitContainer1.TabIndex = 3;
            // 
            // dragDropControl1
            // 
            this.dragDropControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dragDropControl1.Location = new System.Drawing.Point(0, 0);
            this.dragDropControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dragDropControl1.Name = "dragDropControl1";
            this.dragDropControl1.Size = new System.Drawing.Size(360, 166);
            this.dragDropControl1.TabIndex = 2;
            // 
            // ResponseControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ResponseControl";
            this.Size = new System.Drawing.Size(368, 289);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private AdamsLair.WinForms.PropertyEditing.PropertyGrid propertyGrid1;
        private DragDropControl dragDropControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
