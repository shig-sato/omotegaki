using System;
using System.Windows.Forms;

namespace OmoSeitoku.Controls
{
    public partial class InputDialog : Form
    {
        public string Message
        {
            get => inputDialogControl1.Message;
            set => inputDialogControl1.Message = value;
        }

        public string Value
        {
            get => inputDialogControl1.Text;
            set => inputDialogControl1.Text = value;
        }

        public bool MultiLine
        {
            get => inputDialogControl1.MultiLine;
            set => inputDialogControl1.MultiLine = value;
        }

        public InputDialog()
        {
            InitializeComponent();

            inputDialogControl1.OKButtonClick += OKButton_Click;
            inputDialogControl1.CancelButtonClick += CancelButton_Click;
            Shown += delegate { inputDialogControl1.Focus(); };
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// テキスト入力ダイアログを表示する。キャンセル時はnullを返す。
        /// </summary>
        public static string Show(string prompt, bool multiline = false)
        {
            return Show(prompt, string.Empty, null, multiline);
        }

        /// <summary>
        /// テキスト入力ダイアログを表示する。キャンセル時はnullを返す。
        /// </summary>
        public static string Show(string prompt, string defaultText, bool multiline = false)
        {
            return Show(prompt, defaultText, null, multiline);
        }

        /// <summary>
        /// テキスト入力ダイアログを表示する。キャンセル時はnullを返す。
        /// </summary>
        public static string Show(string prompt, string defaultText, IWin32Window owner, bool multiline = false)
        {
            string result = null;
            using (var d = new InputDialog()
            {
                Message = prompt,
                MultiLine = multiline,
                Value = defaultText,
            })
            {
                if (d.ShowDialog(owner) == DialogResult.OK)
                {
                    result = d.Value;
                }
            }
            return result;
        }
    }
}
