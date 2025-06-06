using OmoOmotegaki.Models;
using OmoOmotegaki.Models.ShinryouCheckActions;
using OmoSeitoku;
using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Yaml.Serialization;


namespace OmoOmotegaki.Data
{
    /// <summary>
    /// クライアント共有設定
    /// </summary>
    public sealed class ClientShareSettings
    {
        private const string SETTINGS_FILE_NAME = "表書き共有設定.yaml";

        /// <summary>
        /// 設定変更イベントを無視する。
        /// </summary>
        public static bool IgnoreSettingsChangeEvent = false;

        #region Constructor

        public ClientShareSettings()
        {
        }  // 引数があるとシリアライズ出来ない

        #endregion Constructor

        /// <summary>
        /// セッティングが変更されると発生
        /// </summary>
        public event EventHandler SettingsChange;

        #region Static Property

        /// <summary>
        /// アプリケーションのセッティングを取得
        /// </summary>
        public static ClientShareSettings Instance
        {
            get
            {
                if (__property_instance == null)
                {
                    IgnoreSettingsChangeEvent = true;
                    try
                    {
                        __property_instance = Load();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                        __property_instance = new ClientShareSettings();
                    }
                    finally
                    {
                        IgnoreSettingsChangeEvent = false;
                    }
                }
                return __property_instance;
            }
        }

        private static ClientShareSettings __property_instance;

        public static string FilePath
        {
            get
            {
                string dataFolder = global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder;

                if (string.IsNullOrWhiteSpace(dataFolder))
                    throw new InvalidOperationException("[error: 9876e5af] データフォルダが未設定です。");

                return Path.Combine(
                    dataFolder,
                    SETTINGS_FILE_NAME);
            }
        }

        #endregion Static Property

        #region セッティング項目

        [Browsable(true)]
        [Category("診療表示"), DisplayName("設定テンプレート"), Description("診療表示画面の設定テンプレートを設定")]
        [Editor(typeof(DescriptiveCollectionEditor), typeof(UITypeEditor))]
        public List<SinryouViewerSettings> ShinryouViewerSettings
        {
            get => __property_SinryouViewerSettings;
            set
            {
                if (value == __property_SinryouViewerSettings) return;

                __property_SinryouViewerSettings = value;

                if (!IgnoreSettingsChangeEvent)
                {
                    PerformSettingsChange();
                }
            }
        }

        private List<SinryouViewerSettings> __property_SinryouViewerSettings = new List<SinryouViewerSettings>();



        [Browsable(true)]
        [Category("診療表示"), DisplayName("診療チェック"), Description("診療チェック対象のルールを設定")]
        [Editor(typeof(DescriptiveCollectionEditor), typeof(UITypeEditor))]
        public List<ShinryouCheckRule> ShinryouCheckSyochiList
        {
            get { return __property_ShinryouCheckSyochiList; }
            set
            {
                if (value == __property_ShinryouCheckSyochiList) return;

                __property_ShinryouCheckSyochiList = value;

                if (!IgnoreSettingsChangeEvent)
                {
                    PerformSettingsChange();
                }
            }
        }

        private List<ShinryouCheckRule> __property_ShinryouCheckSyochiList = new List<ShinryouCheckRule>();

        #endregion セッティング項目

        #region Static Method
        public static ClientShareSettings Load()
        {
            var serializer = new YamlSerializer();
            var settings = (ClientShareSettings)serializer.DeserializeFromFile(FilePath)[0];

            診療チェックリストに初期データが無ければ追加する(settings);

            return settings;
        }

        private static void 診療チェックリストに初期データが無ければ追加する(ClientShareSettings settings)
        {
            List<ShinryouCheckRule> list = settings.ShinryouCheckSyochiList ?? new List<ShinryouCheckRule>();

            if (!list.Any(p => p.ShochiId == 560))
            {
                list.Add(new ShinryouCheckRule(560)
                {
                    RuleType = ShinryouCheckRuleType.回数と前回算定日を表示
                });
            }

            if (!list.Any(p => p.ShochiId == 159))
            {
                list.Add(new ShinryouCheckRule(159)
                {
                    RuleType = ShinryouCheckRuleType.一定期間算定不可,
                    CheckAction = new ShinryouCheckAction一定期間算定不可()
                    {
                        算定ブロック単位 = 算定ブロック単位.顎,
                        算定不可期間 = new Term(0, 6, 0),
                    }
                });
            }

            if (!list.Any(p => p.ShochiId == 139))
            {
                list.Add(new ShinryouCheckRule(139)
                {
                    RuleType = ShinryouCheckRuleType.一定期間算定不可,
                    CheckAction = new ShinryouCheckAction一定期間算定不可()
                    {
                        算定ブロック単位 = 算定ブロック単位.顎,
                        算定不可期間 = new Term(0, 6, 0),
                        算定済チェック処置 = new[] { 141 },
                    }
                });
            }

            if (!list.Any(p => p.ShochiId == 73))
            {
                list.Add(new ShinryouCheckRule(73)
                {
                    RuleType = ShinryouCheckRuleType.算定可能回数,
                    CheckAction = new ShinryouCheckAction算定可能回数
                    {
                        算定ブロック単位 = 算定ブロック単位.歯区分,
                        同類算定処置 = new[] { 74 },
                    }
                });
            }

            if (!list.Any(p => p.ShochiId == 75))
            {
                list.Add(new ShinryouCheckRule(75)
                {
                    RuleType = ShinryouCheckRuleType.算定可能回数検査,
                    CheckAction = new ShinryouCheckAction算定可能回数検査
                    {
                        算定ブロック単位 = 算定ブロック単位.歯区分,
                        同類算定処置 = new[] { 76, 77 },
                        リセット処置 = new[] { 24, 25, 26 },
                    }
                });
            }

            if (!list.Any(p => p.ShochiId == 778))
            {
                list.Add(new ShinryouCheckRule(778)
                {
                    RuleType = ShinryouCheckRuleType.算定可能回数,
                    CheckAction = new ShinryouCheckAction算定可能回数
                    {
                        算定ブロック単位 = 算定ブロック単位.歯区分,
                    }
                });
            }

            if (!list.Any(p => p.ShochiId == 779))
            {
                list.Add(new ShinryouCheckRule(779)
                {
                    RuleType = ShinryouCheckRuleType.算定可能回数,
                    CheckAction = new ShinryouCheckAction算定可能回数
                    {
                        算定ブロック単位 = 算定ブロック単位.歯区分,
                    }
                });
            }

            settings.ShinryouCheckSyochiList = list;
        }

        #endregion

        /// <summary>
        /// 設定の編集フォームを開く
        /// </summary>
        public void ShowForm(IWin32Window owner)
        {
            SettingsForm.Instance.ShowDialog(owner);
            SettingsForm.Instance.Activate();
        }

        /// <summary>
        /// セッティングをファイルに保存する
        /// </summary>
        public void Save()
        {
            var serializer = new YamlSerializer();

            serializer.SerializeToFile(FilePath, Instance);
        }

        /// <summary>
        /// セッティングをファイルから読み込む
        /// </summary>

        /// <summary>
        /// セッティング変更後イベントを発生させる。
        /// </summary>
        public void PerformSettingsChange()
        {
            SettingsChange?.Invoke(Instance, new EventArgs());
        }

        /// <summary>
        /// セッティング フォーム
        /// </summary>
        private sealed class SettingsForm : Form
        {
            private readonly PropertyGrid _propertyGrid = new PropertyGrid();

            private SettingsForm()
            {
                Text = "設定";
                Font = new Font(Font.FontFamily, 13f);
                _propertyGrid.Dock = DockStyle.Fill;
                Controls.Add(_propertyGrid);
                _propertyGrid.SelectedObject = ClientShareSettings.Instance;

                ClientShareSettings.Instance.SettingsChange += InternalReload;

                MinimumSize = Size;
                Size = new Size(640, 500);
            }

            ~SettingsForm()
            {
                ClientShareSettings.Instance.SettingsChange -= InternalReload;
            }

            public static SettingsForm Instance
            {
                get
                {
                    if (__property_instance == null || !__property_instance.Created)
                        __property_instance = new SettingsForm();
                    return __property_instance;
                }
            }

            private static SettingsForm __property_instance;

            public static void Reload()
            {
                Instance.InternalReload(null, null);
            }

            private void InternalReload(object sender, EventArgs e)
            {
                _propertyGrid.RefreshTabs(PropertyTabScope.Component);
                _propertyGrid.Refresh();
            }
        }
    }
}
