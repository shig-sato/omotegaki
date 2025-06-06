using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.International.JapaneseTextAlignment;

namespace Microsoft.Samples.JapaneseAlignementTextBox
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase")]

    partial class FormDemo : Form
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
            this.textBoxExJp2 = new Microsoft.Samples.JapaneseAlignementTextBox.JapaneseAlignementTextBox();
            this.textBoxExJp1 = new Microsoft.Samples.JapaneseAlignementTextBox.JapaneseAlignementTextBox();
            this.SuspendLayout();
            // 
            // textBoxExJp2
            // 
            this.textBoxExJp2.AligmentUnitInfoProvider = null;
            this.textBoxExJp2.ExtendedTextAlignment = Microsoft.International.JapaneseTextAlignment.TextAlignmentStyle.FullJustify;
            this.textBoxExJp2.LeftMargin = 1;
            this.textBoxExJp2.Location = new System.Drawing.Point(59, 77);
            this.textBoxExJp2.Name = "textBoxExJp2";
            this.textBoxExJp2.RightMargin = 1;
            this.textBoxExJp2.Size = new System.Drawing.Size(146, 20);
            this.textBoxExJp2.TabIndex = 1;
            // 
            // textBoxExJp1
            // 
            this.textBoxExJp1.AligmentUnitInfoProvider = null;
            this.textBoxExJp1.ExtendedTextAlignment = Microsoft.International.JapaneseTextAlignment.TextAlignmentStyle.Justify;
            this.textBoxExJp1.LeftMargin = 20;
            this.textBoxExJp1.Location = new System.Drawing.Point(59, 30);
            this.textBoxExJp1.Name = "textBoxExJp1";
            this.textBoxExJp1.RightMargin = 20;
            this.textBoxExJp1.Size = new System.Drawing.Size(146, 20);
            this.textBoxExJp1.TabIndex = 0;
            // 
            // FormDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 131);
            this.Controls.Add(this.textBoxExJp2);
            this.Controls.Add(this.textBoxExJp1);
            this.Name = "FormDemo";
            this.Text = "FormDemo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        public JapaneseAlignementTextBox textBoxExJp1;
        public JapaneseAlignementTextBox textBoxExJp2;

        #endregion
    }
}

