using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Microsoft.Samples.JapaneseAlignementTextBox
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormDemo formDemo = new FormDemo();
            Application.Run(formDemo);

        }
    }
}