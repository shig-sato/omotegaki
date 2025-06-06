using OmoSeitokuEreceipt.SER;
using System;
using System.Windows.Forms;

namespace OmoOmotegaki.Forms
{
    internal partial class KartePrintSettingDialog : Form
    {
        public KartePrintDesign.Settings Settings
        {
            get { return __prop__Settings; }
            set
            {
                __prop__Settings = new KartePrintDesign.Settings(value);

                OnSettingsChanged();
            }
        }


        // プロパティのみから参照
        private KartePrintDesign.Settings __prop__Settings;



        public KartePrintSettingDialog()
        {
            InitializeComponent();




            // 保険診療コンボボックス
            var hokenVals = Enum.GetNames(typeof(SER_保険診療));
            var hokenItems = new string[hokenVals.Length];
            for (int i = hokenVals.Length - 1; 0 <= i; --i)
            {
                hokenItems[i] = hokenVals[i];
            }
            cmbHokenSinryou.Items.AddRange(hokenItems);
            cmbHokenSinryou.SelectedIndex = 0;

            // 未来院請求コンボボックス
            var miraiinVals = Enum.GetNames(typeof(SER_未来院請求));
            var miraiinItems = new string[miraiinVals.Length];
            for (int i = miraiinVals.Length - 1; 0 <= i; --i)
            {
                miraiinItems[i] = miraiinVals[i];
            }
            cmbMiraiinSeikyuu.Items.AddRange(miraiinItems);
            cmbMiraiinSeikyuu.SelectedIndex = 0;

            chkShowByoumei.Checked = true;
            chkSSSSyochiName.Checked = true;
            chkSSSCheckPrint.Checked = false;
        }



        private KartePrintDesign.Settings CreateSettings()
        {
            var s = this.Settings;

            s = (s == null)
                ? new KartePrintDesign.Settings()
                : new KartePrintDesign.Settings(s);


            // 保険診療
            s.Hoken = (SER_保険診療)Enum.Parse(
                                    typeof(SER_保険診療),
                                    (string)cmbHokenSinryou.SelectedItem);

            // 未来院請求
            s.Miraiin = (SER_未来院請求)Enum.Parse(
                                    typeof(SER_未来院請求),
                                    (string)cmbMiraiinSeikyuu.SelectedItem);

            // カルテ番号
            s.ShowKarteNumber = chkShowKarteNumber.Checked;
            // 患者名
            s.ShowKarteName = chkShowKarteName.Checked;
            // カルテ期間
            s.ShowKarteKikan = chkShowKarteKikan.Checked;

            // 病名表示
            s.ShowByoumei = chkShowByoumei.Checked;
            // 診療録作成ソフトの処置名で印字する
            s.SSSSyochiName = chkSSSSyochiName.Checked;
            // 診療録作成ソフトのチェック印刷を印字する
            s.SSSCheckPrint = chkSSSCheckPrint.Checked;

            return s;
        }


        private void OnSettingsChanged()
        {
            KartePrintDesign.Settings newSettings = this.Settings ?? new KartePrintDesign.Settings();

            if (newSettings.Hoken.HasValue)
            {
                cmbHokenSinryou.SelectedItem = newSettings.Hoken.Value.ToString();
            }

            if (newSettings.Miraiin.HasValue)
            {
                cmbMiraiinSeikyuu.SelectedItem = newSettings.Miraiin.Value.ToString();
            }

            chkShowKarteNumber.Checked = newSettings.ShowKarteNumber;
            chkShowKarteName.Checked = newSettings.ShowKarteName;
            chkShowKarteKikan.Checked = newSettings.ShowKarteKikan;

            chkShowByoumei.Checked = newSettings.ShowByoumei;
            chkSSSSyochiName.Checked = newSettings.SSSSyochiName;
            chkSSSCheckPrint.Checked = newSettings.SSSCheckPrint;
        }



        // イベントハンドラ

        private void KartePrintSettingDialog_Load(object sender, EventArgs e)
        {
            OnSettingsChanged();

            //UNDONE カルテ一括印刷 自費・未来院の切り替えボタン実装
            {
                cmbHokenSinryou.SelectedItem = SER_保険診療.保険診療と自費診療.ToString();
                cmbHokenSinryou.Enabled = false;

                cmbMiraiinSeikyuu.SelectedItem = SER_未来院請求.通常請求と未来院請求.ToString();
                cmbMiraiinSeikyuu.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Settings = CreateSettings();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
