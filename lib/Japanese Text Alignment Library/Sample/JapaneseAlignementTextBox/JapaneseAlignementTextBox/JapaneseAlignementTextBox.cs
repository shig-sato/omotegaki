using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.International.JapaneseTextAlignment;

namespace Microsoft.Samples.JapaneseAlignementTextBox
{
    /// <summary>
    /// Sample TextBox on windows form which supports Japanese Text Alignment.
    /// </summary>
    public class JapaneseAlignementTextBox : TextBox
    {
        private TextAlignmentStyleInfo textAlignmentStyleInfo;
        private IAlignmentUnitInfoProvider alignmentUnitInfoProvider;

        /// <summary>
        /// Constructor sets the basic alignment style information of the textBox.
        /// </summary>
        public JapaneseAlignementTextBox()
        {
            textAlignmentStyleInfo.LeftMargin = 1;
            textAlignmentStyleInfo.RightMargin = 1;
        }

        /// <summary>
        /// Gets or sets text alignment info.
        /// </summary>
        public TextAlignmentStyle ExtendedTextAlignment
        {
            get
            {
                return textAlignmentStyleInfo.Style;
            }
            set
            {
                textAlignmentStyleInfo.Style = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets left margin.
        /// </summary>
        public int LeftMargin
        {
            get
            {
                return textAlignmentStyleInfo.LeftMargin;
            }
            set
            {
                textAlignmentStyleInfo.LeftMargin = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets right margin.
        /// </summary>
        public int RightMargin
        {
            get
            {
                return textAlignmentStyleInfo.RightMargin;
            }
            set
            {
                textAlignmentStyleInfo.RightMargin = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets and sets alignment unit provider.
        /// </summary>
        public IAlignmentUnitInfoProvider AligmentUnitInfoProvider
        {
            get
            {
                return alignmentUnitInfoProvider;
            }
            set
            {
                alignmentUnitInfoProvider = value;
                Refresh();
            }
        }

        /// <summary>
        /// Raises the GotFocus event to set the TextBox¡¯s style.
        /// </summary>
        /// <param name="e">A <see cref="EventArgs"/> that contains the event data. </param>
        protected override void OnGotFocus(EventArgs e)
        {
            this.SetStyle(ControlStyles.UserPaint, false);
        }

        /// <summary>
        /// Raises the LostFocus" event. 
        /// </summary>
        /// <param name="e">A <see cref="EventArgs"/> that contains the event data. </param>
        protected override void OnLostFocus(EventArgs e)
        {
            // Paint by the userPaint and use the alignment painting.
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data. </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Draws the textBox using the alignment sytle.
            Utility.DrawJapaneseString(
                e.Graphics,
                this.Text,
                this.Font,
                this.ForeColor,
            new Rectangle(
                    ClientRectangle.Left + 1,
                    ClientRectangle.Top + 1,
                    ClientRectangle.Width - 2,
                    ClientRectangle.Height - 2),
                textAlignmentStyleInfo,
                alignmentUnitInfoProvider);
        }
    }
}
