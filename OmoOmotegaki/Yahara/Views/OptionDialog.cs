using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmoOmotegaki.Yahara.Views
{
    public partial class OptionDialog : Form
    {
        internal YaharaConverter.ConverterOption ConverterOption
        {
            get; set;
        }

        public OptionDialog()
        {
            InitializeComponent();

            this.DialogResult = DialogResult.Cancel;

            txtInputFolder.Text = global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder;
            txtOutputFolder.Text = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "yahara_xml");

            this.chkZenKikan.Checked = true;
            chkZenKikan_CheckedChanged(this, EventArgs.Empty);
            this.radKarteAll.Checked = true;



            var sinryoujoDict = new Dictionary<string, string> {
                { "本院", "hon" },
                { "分院", "bun" },
            };
            cmbSinryoujo.DataSource = new BindingSource(sinryoujoDict, null);
            cmbSinryoujo.DisplayMember = "Key";
            cmbSinryoujo.ValueMember = "Value";


            btnSelectInputFolder.Enabled = false;
            txtInputFolder.Enabled = false;

            UpdateControls();
            UpdateConverterOption();
        }

        private void UpdateConverterOption()
        {
            // 出力先フォルダ
            var outputFolder = new DirectoryInfo(txtOutputFolder.Text);
            var opt = new YaharaConverter.ConverterOption(outputFolder);

            // 期間指定
            opt.DateRange = chkZenKikan.Checked
                ? DateRange.All
                : new DateRange(
                    dateStart.Value.Date,
                    dateEnd.Value.Date.AddDays(1).AddMilliseconds(-1));

            // 出力カルテ
            if (this.radKarteAll.Checked)
            {
            }
            else if (radKarteLimit.Checked)
            {
                opt.Limit = (int)numLimit.Value;
            }
            else if (radKarteOne.Checked)
            {
                opt.Limit = 1;
                opt.OneKarte = new KarteId(
                    new Shinryoujo(cmbSinryoujo.SelectedValue.ToString()),
                    (int)numOneKarte.Value);
            }

            this.ConverterOption = opt;
        }

        private void UpdateControls()
        {
            numLimit.Enabled = radKarteLimit.Checked;
            numOneKarte.Enabled = cmbSinryoujo.Enabled = radKarteOne.Checked;
            dateStart.Enabled = dateEnd.Enabled = !chkZenKikan.Checked;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            UpdateConverterOption();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OptionDialog_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void chkZenKikan_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void btnSelectInputFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtInputFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSelectOutputFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
            {
                txtOutputFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void radKarteLimit_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void radKarteOne_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
    }
}
