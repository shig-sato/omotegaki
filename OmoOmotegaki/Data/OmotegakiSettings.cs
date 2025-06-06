using OmoSeitokuEreceipt.SER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Yaml.Serialization;

namespace OmoOmotegaki.Data
{
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "settings")]
    public sealed class OmotegakiSettings
    {
        private const string SETTINGS_FILE_NAME = "settings.yaml";

        /// <summary>
        /// 設定変更イベントを無視する。
        /// </summary>
        public static bool IgnoreSettingsChangeEvent = false;

        #region Constructor

        public OmotegakiSettings()
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
        public static OmotegakiSettings Instance
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

                        __property_instance = new OmotegakiSettings();
                    }
                    finally
                    {
                        IgnoreSettingsChangeEvent = false;
                    }
                }
                return __property_instance;
            }
        }

        private static OmotegakiSettings __property_instance;

        public static string FilePath
        {
            get
            {
                var productDataDir = Path.GetDirectoryName(Application.UserAppDataPath);
                return Path.Combine(productDataDir, SETTINGS_FILE_NAME);
            }
        }

        #endregion

        #region セッティング項目

        [Category("(全般)"), DisplayName("アップデートURL"), Description("アップデート情報を取得するURL")]
        public string UpdateUrl
        {
            get { return __property_UpdateUrl; }
            set
            {
                string old = __property_UpdateUrl;
                __property_UpdateUrl = value;

                if (!IgnoreSettingsChangeEvent && !string.Equals(old, value))
                {
                    PerformSettingsChange();
                }
            }
        }

        private string __property_UpdateUrl;

        //[YamlSerialize(YamlSerializeMethod.Never)]
        //[Browsable(false)]
        //[Category("(全般)"), DisplayName("最新のデータフォルダー"), Description("最新のデータフォルダー (recedata フォルダー) のフォルダーパス")]
        //public string LatestDataFolder
        //{
        //    get { return _DataFolderHistory.Count == 0 ? null : _DataFolderHistory[0]; }
        //}
        [Browsable(true)]
        [Category("(全般)"), Description("使用したデータフォルダーの履歴")]
        public string[] DataFolderHistory
        {
            get
            {
                string[] hist = __property_DataFolderHistory;
                if (hist is null)
                {
                    __property_DataFolderHistory = hist = Array.Empty<string>();
                }
                return hist;
            }
            set
            {
                string[] old = __property_DataFolderHistory;
                __property_DataFolderHistory = value;

                if (!IgnoreSettingsChangeEvent && !object.Equals(old, value))
                {
                    PerformSettingsChange();
                }
            }
        }

        private string[] __property_DataFolderHistory;

        [Browsable(false)]
        [Category("診療読込"), Description("初診期間内の同一歯式の診療を統合する。")]
        public bool ShishikiTougou
        {
            get { return __property_ShishikiTougou; }
            set
            {
                bool old = __property_ShishikiTougou;
                __property_ShishikiTougou = value;

                if (!IgnoreSettingsChangeEvent && old != value)
                {
                    PerformSettingsChange();
                }
            }
        }

        private bool __property_ShishikiTougou = true;

        [Category("ウィンドウ"), DisplayName("位置とサイズを記憶"), Description("終了時のウィンドウ位置とサイズを次回起動時に復帰する。")]
        public bool RestoreWindowState
        {
            get => __property_RestoreWindowState;
            set
            {
                bool old = __property_RestoreWindowState;
                __property_RestoreWindowState = value;

                if (!IgnoreSettingsChangeEvent && old != value)
                {
                    PerformSettingsChange();
                }
            }
        }

        private bool __property_RestoreWindowState = false;

        [Browsable(true)]
        [Category("ウィンドウ"), DisplayName("起動時の表示方法"), Description("起動時のウィンドウ表示方法。「位置とサイズを記憶」がTrueの場合は無視されます。")]
        public FormWindowState WindowState
        {
            get => __property_WindowState;
            set
            {
                FormWindowState old = __property_WindowState;
                __property_WindowState = value;

                if (!IgnoreSettingsChangeEvent && old != value)
                {
                    PerformSettingsChange();
                }
            }
        }

        private FormWindowState __property_WindowState = FormWindowState.Maximized;

        [Browsable(false)]
        [Category("ウィンドウ"), DisplayName("起動時の表示位置"), Description("ウィンドウ表示位置")]
        public Point DesktopLocation
        {
            get => __property_DesktopLocation;
            set
            {
                if (value == __property_DesktopLocation) return;

                __property_DesktopLocation = value;

                if (!IgnoreSettingsChangeEvent)
                {
                    PerformSettingsChange();
                }
            }
        }

        private Point __property_DesktopLocation;

        [Browsable(false)]
        [Category("ウィンドウ"), DisplayName("起動時のサイズ"), Description("ウィンドウサイズ")]
        public Size WindowSize
        {
            get => __property_WindowSize;
            set
            {
                if (value == __property_WindowSize) return;

                __property_WindowSize = value;

                if (!IgnoreSettingsChangeEvent)
                {
                    PerformSettingsChange();
                }
            }
        }

        private Size __property_WindowSize;

        #endregion セッティング項目

        private string FiltersFolderPath
        {
            get
            {
                string filtersFolderPath = __prop_FiltersFolderPath;
                if (filtersFolderPath is null)
                {
                    string productDataDir = Path.GetDirectoryName(Application.UserAppDataPath);
                    __prop_FiltersFolderPath = filtersFolderPath = Path.Combine(productDataDir, "SinryouFilter");
                }
                return filtersFolderPath;
            }
        }

        private string __prop_FiltersFolderPath;

        #region Static Method

        /// <summary>
        /// セッティングをファイルから読み込む
        /// </summary>
        public static OmotegakiSettings Load()
        {
            var serializer = new YamlSerializer();
            var settings = (OmotegakiSettings)serializer.DeserializeFromFile(FilePath)[0];

            return settings;
        }

        #endregion

        public List<SinryouFilter> GetFilterList(KarteId karteId)
        {
            string dir = FiltersFolderPath;
            if (Directory.Exists(dir))
            {
                string path = Path.Combine(dir, $"{karteId.Shinryoujo.Key}{karteId.KarteNumber}.yaml");
                if (File.Exists(path))
                {
                    try
                    {
                        return (List<SinryouFilter>)new YamlSerializer().DeserializeFromFile(path)[0];
                    }
                    catch (Exception e)
                    {
                        try { File.Delete(path); } catch { }
                        Console.WriteLine(e);
                    }
                }
            }
            return null;
        }

        public void SetFilterList(KarteId karteId, List<SinryouFilter> filters)
        {
            string dir = FiltersFolderPath;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string path = Path.Combine(dir, $"{karteId.Shinryoujo.Key}{karteId.KarteNumber}.yaml");
            if (filters == null)
            {
                File.Delete(path);
            }
            else
            {
                new YamlSerializer().SerializeToFile(path, filters);
            }
        }

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
        /// セッティング変更後イベントを発生させる。
        /// </summary>
        public void PerformSettingsChange()
        {
            SettingsChange?.Invoke(this, new EventArgs());
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
                _propertyGrid.SelectedObject = OmotegakiSettings.Instance;

                OmotegakiSettings.Instance.SettingsChange += InternalReload;

                MinimumSize = Size;
                Size = new Size(640, 500);
            }

            ~SettingsForm()
            {
                OmotegakiSettings.Instance.SettingsChange -= InternalReload;
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