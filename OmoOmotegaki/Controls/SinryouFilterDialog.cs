using OmoSeitokuEreceipt.SER;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OmoOmotegaki.Controls
{
    public partial class SinryouFilterDialog : Form
    {
        private const string 診療フィルターファイル_拡張子 = "sff";

        private SinryouFilter _filter;

        public SinryouFilter Filter
        {
            get { return _filter; }
            private set
            {
                _filter = value;
                this.UpdateFilterControls();
            }
        }

        public SinryouFilterDialog(string filterName)
        {
            InitializeComponent();
            InitControl();

            this.Filter = new SinryouFilter(filterName);
        }

        public SinryouFilterDialog(SinryouFilter baseFilter)
        {
            InitializeComponent();
            InitControl();

            this.Filter = new SinryouFilter(baseFilter);
        }

        private void InitControl()
        {
            foreach (ComboBox cmb in new[] { cmbByoMatchType, cmbSyoMatchType })
            {
                foreach (object i in Enum.GetValues(typeof(SinryouFilter.MatchType)))
                {
                    cmb.Items.Add(i.ToString());
                }
            }
        }


        /// <summary>
        /// コントロールの設定値をフィルターに反映
        /// </summary>
        private void UpdateFilterProperties()
        {
            SinryouFilter filter = this.Filter;


            string txt = txtFilterName.Text.Trim(' ', '　');
            filter.Title = txt;


            txt = txtByoumei.Text.Trim(' ', '　');
            filter.病名 = txt;
            filter.病名MatchType = GetMatchType(cmbByoMatchType);

            txt = txtSyochi.Text.Trim(' ', '　');
            filter.処置 = txt;
            filter.処置MatchType = GetMatchType(cmbSyoMatchType);

            if (sisikiInputControl1.HasData)
                filter.歯式 = sisikiInputControl1.歯式;
            else
                filter.歯式 = null;
            filter.歯種完全一致 = chkSisikiKanzen.Checked;
        }


        /// <summary>
        /// フィルターの設定値をコントロールに反映
        /// </summary>
        private void UpdateFilterControls()
        {
            SinryouFilter filter = this.Filter;

            txtFilterName.Text = filter.Title;

            if (filter.Has病名)
                txtByoumei.Text = filter.病名;
            cmbByoMatchType.SelectedItem = filter.病名MatchType.ToString();

            if (filter.Has処置)
                txtSyochi.Text = filter.処置;
            cmbSyoMatchType.SelectedItem = filter.処置MatchType.ToString();

            if (filter.Has歯式)
                sisikiInputControl1.歯式 = filter.歯式;
            chkSisikiKanzen.Checked = filter.歯種完全一致;
        }


        private SinryouFilter.MatchType GetMatchType(ComboBox cmb)
        {
            return (SinryouFilter.MatchType)Enum.Parse(
                typeof(SinryouFilter.MatchType), (string)cmb.SelectedItem);
        }


        private void SinryouFilterDialog_Load(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(delegate
            {
                this.BeginInvoke(new Action(delegate
                {
                    txtByoumei.Focus();
                }));
            })).Start();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.btnOK.Enabled) return;

            this.btnOK.Enabled = false;

            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                this.UpdateFilterProperties();

                this.Close();
            }
            catch (Exception ex)
            {
                this.btnOK.Enabled = true;

                throw ex;
            }
        }

        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnOK.PerformClick();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.UpdateFilterProperties();

            FileStream dest = null;

            using (var dlg = new SaveFileDialog())
            {
                string initdir = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "テンプレート");

                if (!Directory.Exists(initdir))
                    Directory.CreateDirectory(initdir);

                dlg.AddExtension = true;
                dlg.CheckPathExists = true;
                dlg.CreatePrompt = false;
                dlg.DefaultExt = ".xml";
                dlg.FileName = this.Filter.Title;
                dlg.Filter = string.Format("診療フィルターファイル (*.{0})|*.{0}|すべてのファイル (*.*)|*.*", 診療フィルターファイル_拡張子);
                dlg.InitialDirectory = initdir;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    dest = File.Open(dlg.FileName, FileMode.Create);
                }
            }

            if (dest != null)
            {
                try
                {
                    var xml = new XmlSerializer(this.Filter.GetType());
                    xml.Serialize(dest, this.Filter);
                }
                finally
                {
                    dest.Close();
                }
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            FileStream source = null;

            using (var dlg = new OpenFileDialog())
            {
                string initdir = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "テンプレート");

                if (!Directory.Exists(initdir))
                    Directory.CreateDirectory(initdir);

                dlg.AddExtension = true;
                dlg.CheckPathExists = true;
                dlg.DefaultExt = ".xml";
                dlg.Filter = string.Format("診療フィルターファイル (*.{0})|*.{0}|すべてのファイル (*.*)|*.*", 診療フィルターファイル_拡張子);
                dlg.InitialDirectory = initdir;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    source = File.Open(dlg.FileName, FileMode.Open,
                                        FileAccess.Read, FileShare.ReadWrite);
                }
            }

            if (source != null)
            {
                object filterObj;
                try
                {
                    var xml = new XmlSerializer(this.Filter.GetType());
                    filterObj = xml.Deserialize(source);
                }
                finally
                {
                    source.Close();
                }

                this.Filter = (SinryouFilter)filterObj;
            }
        }
    }
}
