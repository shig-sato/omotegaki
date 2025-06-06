using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OmoSeitokuEreceipt.Properties
{
    // このクラスでは設定クラスでの特定のイベントを処理することができます:
    //  SettingChanging イベントは、設定値が変更される前に発生します。
    //  PropertyChanged イベントは、設定値が変更された後に発生します。
    //  SettingsLoaded イベントは、設定値が読み込まれた後に発生します。
    //  SettingsSaving イベントは、設定値が保存される前に発生します。


    [SettingsManageability(SettingsManageability.Roaming)]
    public sealed partial class Settings
    {
        #region クラス, デリゲート

        public delegate void ExPropertyChangedEventHandler(object sender, ExPropertyChangedEventArgs e);

        public sealed class ExPropertyChangedEventArgs : PropertyChangedEventArgs
        {
            public readonly string OldValue;
            public readonly string NewValue;

            public ExPropertyChangedEventArgs(string propertyName, string oldValue, string newValue)
                : base(propertyName)
            {
                OldValue = oldValue;
                NewValue = newValue;
            }
        }

        #endregion


        public const string PROPERTY_DATA_FOLDER = "DataFolder";

        public const string DEFAULT_INITIAL_FILENAME = "initial.900";
        public const string DEFAULT_INITIAL_FILENAME_PATTERN = "initial*.900";

        public const string ERECE_FOLDER_NAME = "EReceipt";

        private const string SYOCHI_DB_LIST_FILE_NAME = "S9X0401X.XXX";



        public event SettingChangingEventHandler DataFolderChanging;
        public event ExPropertyChangedEventHandler DataFolderChanged;

        private bool _suppressConfirmDataFolderChanging;


        /// <summary>
        /// 変更前のデータフォルダーパス
        /// </summary>
        public string PreviewDataFolder
        {
            get;
            private set;
        }

        public string EReceFolder
        {
            get;
            private set;
        }


        public Settings()
        {
            if (!string.IsNullOrEmpty(DataFolder))
            {
                UpdateEReceFolder();
            }
        }

        public void SetDataFolderSuppressConfirm(string dataFolder)
        {
            try
            {
                _suppressConfirmDataFolderChanging = true;
                DataFolder = dataFolder;
            }
            finally
            {
                _suppressConfirmDataFolderChanging = false;
            }
        }

        protected override void OnSettingChanging(object sender, SettingChangingEventArgs e)
        {
            // 基底関数呼び出し後に実行する処理。
            Action afterAction = null;

            if (!e.Cancel)
            {
                // データフォルダー
                if (PROPERTY_DATA_FOLDER.Equals(e.SettingName))
                {
                    bool reCall = false;

                    if (e.NewValue != null)
                    {
                        string newPath = e.NewValue.ToString();
                        string normalized = NormalizeDataFolderPath(newPath);

                        if (newPath.Equals(normalized))
                        {
                            if (!CanSetDataFolder(newPath))
                            {
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            // 新しいパスとそれを正規化したものが異なる場合
                            // 今回の変更をキャンセルする。
                            e.Cancel = true;

                            // キャンセル完了後に、正規化したパスを設定しなおす。
                            afterAction = delegate
                            {
                                DataFolder = normalized;
                            };

                            reCall = true;
                        }
                    }

                    if (!reCall)
                    {
                        OnDataFolderChanging(e);

                        if (!e.Cancel)
                        {
                            PreviewDataFolder = DataFolder;
                        }
                    }
                }
            }

            base.OnSettingChanging(sender, e);

            afterAction?.Invoke();
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);


            if (PROPERTY_DATA_FOLDER.Equals(e.PropertyName))
            {
                var ev = new ExPropertyChangedEventArgs(
                               propertyName: e.PropertyName,
                               oldValue: PreviewDataFolder,
                               newValue: DataFolder);

                OnDataFolderChanged(ev);
            }
        }



        private void OnDataFolderChanging(SettingChangingEventArgs e)
        {
            bool isFirstTime = string.IsNullOrEmpty(DataFolder);

            if (!_suppressConfirmDataFolderChanging && !isFirstTime)
            {
                var res = MessageBox.Show(
                                new StringBuilder()
                                    .AppendLine("データフォルダーの変更にはアプリケーションの再起動が必要です。")
                                    .AppendLine("すべてのウィンドウを終了します。")
                                    .ToString(),
                                "再起動チェック",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning,
                                MessageBoxDefaultButton.Button2);

                if (res != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }

            DataFolderChanging?.Invoke(this, e);
        }

        private void OnDataFolderChanged(ExPropertyChangedEventArgs e)
        {
            UpdateEReceFolder();

            DataFolderChanged?.Invoke(this, e);
        }

        private void UpdateEReceFolder()
        {
            EReceFolder = Path.Combine(DataFolder, ERECE_FOLDER_NAME) + Path.DirectorySeparatorChar;
        }



        /// <summary>
        /// データフォルダを新しい値に変更可能かどうか調べる。
        /// </summary>
        /// <param name="path">新しいデータフォルダーパス</param>
        /// <returns>変更可能な場合 true</returns>
        private bool CanSetDataFolder(string path)
        {
            try
            {
                ValidateDataFolder(path);
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message);

                return false;
            }

            // 既定の値と異なる場合、変更可能とする。

            return !path.Equals(DataFolder, StringComparison.OrdinalIgnoreCase);
        }




        /// <summary>
        /// データフォルダーパスを正規化する。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string NormalizeDataFolderPath(string path)
        {
            // 末尾がディレクトリ区切り文字ではない場合に付加する
            if ((!string.IsNullOrEmpty(path))
                && (path[path.Length - 1] != Path.DirectorySeparatorChar))
            {
                path += Path.DirectorySeparatorChar;
            }

            return path;
        }


        /// <summary>
        /// 指定したパスがデータフォルダーとして不適当な場合に例外を投げる。
        /// </summary>
        /// <param name="path"></param>
        public static void ValidateDataFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                throw new ApplicationException("データフォルダがありません。");
            }

            if (!File.Exists(Path.Combine(path, SYOCHI_DB_LIST_FILE_NAME)) ||
                !Directory.Exists(Path.Combine(path, ERECE_FOLDER_NAME)))
            {
                throw new ApplicationException("正しいデータフォルダではありません。");
            }
        }
    }
}
