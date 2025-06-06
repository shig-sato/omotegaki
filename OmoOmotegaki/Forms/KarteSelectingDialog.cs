using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OmoOmotegaki.Forms
{
    public partial class KarteSelectingDialog : Form
    {
        public int[] KarteNumbers
        {
            get { return _numbers.ToArray(); }
        }

        private List<int> _numbers = new List<int>();

        public KarteSelectingDialog()
        {
            InitializeComponent();
        }

        private void UpdateListBox()
        {
            _listbox1.Items.Clear();
            _listbox1.Items.AddRange(_numbers.Cast<object>().ToArray());
        }


        #region EventHandler

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var inputValue = txtInput.Text;
                int inputNumber;
                if (int.TryParse(inputValue, out inputNumber) && !_numbers.Contains(inputNumber))
                {
                    _numbers.Add(inputNumber);
                    UpdateListBox();
                    txtInput.Clear();
                    txtInput.Focus();
                }
            }
        }

        private void _listbox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var removes = _listbox1.SelectedItems.Cast<int>();

                if (removes.Count() == 0) return;

                _numbers.RemoveAll(p => removes.Contains(p));
                UpdateListBox();
            }
        }

        private void KarteSelectingDialog_Shown(object sender, EventArgs e)
        {
            txtInput.Focus();
        }

        private void btnLoadXcllKrbn_Click(object sender, EventArgs e)
        {
            // UNDONE xcllファイル ハードコード
            string path = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "xcllkrbn.csv");
            if (!File.Exists(path))
            {
                MessageBox.Show($"ファイルが見つかりません。{Environment.NewLine}path: {path}");
                return;
            }

            if (0 < _numbers.Count)
            {
                var result = MessageBox.Show("現在入力されている番号がクリアされます。", "読み込みの続行確認",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);

                if (result != DialogResult.OK)
                {
                    return;
                }
            }

            _numbers.Clear();

            foreach (string line in File.ReadAllLines(path).Where(line => (0 < line.Length)))
            {
                int num;
                if (int.TryParse(line, out num))
                {
                    _numbers.Add(num);
                }
            }

            UpdateListBox();
            txtInput.Clear();
            txtInput.Focus();
        }

        #endregion
    }
}
