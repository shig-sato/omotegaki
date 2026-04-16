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
using static OmoOmotegaki.Yahara.YaharaConverter;

namespace OmoOmotegaki.Yahara.Views
{
    public partial class OptionDialog : Form
    {
        static Dictionary<string, string> sinryoujoDict = new Dictionary<string, string>() {
            { "本院", "hon" },
            { "分院", "bun" },
        };

        internal YaharaConverter.ConverterOption ConverterOption
        {
            get; set;
        }

        public OptionDialog()
        {
            InitializeComponent();

            DialogResult = DialogResult.Cancel;

            txtInputFolder.Text = global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder;
            txtOutputFolder.Text = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "yahara_xml");

            chkZenKikan.Checked = true;
            chkZenKikan_CheckedChanged(this, EventArgs.Empty);
            radKarteAll.Checked = true;

            btnSelectInputFolder.Enabled = false;
            txtInputFolder.Enabled = false;

            UpdateControls();
            UpdateConverterOption();
        }

        private void UpdateConverterOption()
        {
            // 出力先フォルダ
            var outputFolder = new DirectoryInfo(txtOutputFolder.Text);
            var opt = new ConverterOption(outputFolder);

            if (chkShinryoujoHon.Checked)
                opt.ShinryoujoHon = new ShinryoujoItem(new Shinryoujo("Hon"));

            if (chkShinryoujoBun.Checked)
                opt.ShinryoujoBun = new ShinryoujoItem(new Shinryoujo("Bun"));

            // 期間指定
            opt.DateRange = chkZenKikan.Checked
                ? DateRange.All
                : new DateRange(
                    dateStart.Value.Date,
                    dateEnd.Value.Date.AddDays(1).AddMilliseconds(-1));

            // 出力カルテ
            if (radKarteAll.Checked)
            {
            }
            else if (radKarteLimit.Checked)
            {
                opt.StartKarteNumber = (int)numStartKarteNumber.Value;
                opt.Limit = (int)numLimit.Value;
            }
            else if (radKarteOne.Checked)
            {
                opt.Limit = 1;
                opt.OneKarte = new KarteId(
                    opt.ShinryoujoItems.First().Shinryoujo,
                    (int)numOneKarte.Value);
            }

            ConverterOption = opt;
        }

        private void UpdateControls()
        {
            numStartKarteNumber.Enabled = radKarteLimit.Checked;
            numLimit.Enabled = radKarteLimit.Checked;
            numOneKarte.Enabled = radKarteOne.Checked;
            dateStart.Enabled = dateEnd.Enabled = !chkZenKikan.Checked;

            if (radKarteOne.Checked)
            {
                if (chkShinryoujoHon.Checked && chkShinryoujoBun.Checked) chkShinryoujoBun.Checked = false;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            UpdateConverterOption();

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
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

        private void chkShinryoujoHon_CheckedChanged(object sender, EventArgs e)
        {
            if (radKarteOne.Checked)
            {
                if (chkShinryoujoHon.Checked) chkShinryoujoBun.Checked = false;
            }
            UpdateControls();
        }

        private void chkShinryoujoBun_CheckedChanged(object sender, EventArgs e)
        {
            if (radKarteOne.Checked)
            {
                if (chkShinryoujoBun.Checked) chkShinryoujoHon.Checked = false;
            }
            UpdateControls();
        }
    }
}
