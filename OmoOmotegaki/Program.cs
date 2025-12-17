using OmoOmotegaki.Models;
using OmoOmotegaki.Threading;
using OmoSeitokuEreceipt.SER;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OmoOmotegaki
{
    static class Program
    {
        private const int TRANS_ACT_OPEN_NEW_FORM = 1;
        private const string IPC_PORT_NAME = "omotegaki_channel";
        private const string IPC_URI = "transmutex";

        public static readonly string Version = CreateVersionText();

        private static MyApplicationContext _appContext;
        private static bool _restarting;

        #region TempDir

        public static string TempDir
        {
            get; private set;
        }
        public static string GetTempFilePath(string fileName)
        {
            return Path.Combine(TempDir, fileName);
        }
        private static void CreateTempDir()
        {
            TempDir = Path.Combine(Path.GetTempPath(), "Omotegaki.tmp");
            if (Directory.Exists(TempDir)) { Directory.Delete(TempDir, true); }
            Directory.CreateDirectory(TempDir);
        }
        private static void DeleteTempDir()
        {
            if (Directory.Exists(TempDir)) { Directory.Delete(TempDir, true); }
        }

        #endregion

        public static int FormCount => _appContext.FormCount;

        [STAThread]
        static void Main(string[] args)
        {
            // ClickOnceではコマンドライン引数は使用できない。
            args = null;

            // 多重起動判定
            using (var trMutex = new TransactionMutex(false, Application.ProductName, IPC_PORT_NAME, IPC_URI))
            {
                if (trMutex.IsServer)
                {
                    // 新規起動

                    // 多重起動側からの通信イベントを登録
                    trMutex.Server.Transacted += _transaction_Transactioned;

                    // 起動パラメータ (コマンドライン引数の代替)
                    string parameterText = AppDomain.CurrentDomain.SetupInformation.ActivationArguments?.ActivationData?[0];
                    if (!string.IsNullOrWhiteSpace(parameterText))
                    {
                        foreach (string param in parameterText.Split('|'))
                        {
                            string arg = (param is null) ? "" : param.Trim();
                            if (arg.Length == 0) { continue; }

                            // "D=" データフォルダを設定
                            if (arg.Length > 2 &&
                                arg.StartsWith("D=", StringComparison.OrdinalIgnoreCase))
                            {
                                string dataFolderPath = arg.Substring(2);
                                global::OmoSeitokuEreceipt.Properties.Settings.Default.SetDataFolderSuppressConfirm(dataFolderPath);
                            }
                        }
                    }

                    RunApplication();
                }
                else
                {
                    // 多重起動

                    try
                    {
                        // 新規起動側に通信
                        trMutex.Client.Transact(new TransData(TRANS_ACT_OPEN_NEW_FORM));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(new StringBuilder()
                                        .AppendLine("既にソフトが起動中です。")
                                        .Append("エラー: ").Append(ex.Message)
                                        .ToString());
                    }
                }

            }

            if (_restarting)
            {
                OnRestart();
            }

            // アプリケーション終了
        }



        // Static Method

        /// <summary>
        /// すべてのフォームを終了し、アプリケーションを再起動する。
        /// </summary>
        public static void Restart()
        {
            if (!_restarting)
            {
                _restarting = true;
                CloseAllForm();
            }
        }

        private static void OnRestart()
        {
            string[] args = Environment.GetCommandLineArgs();

            // args[0] は 実行ファイルのパスなので除外し、それ以外の引数を連結
            var arguments = string.Join(" ", args, 1, args.Length - 1);
            try
            {
                System.Diagnostics.Process.Start(Application.ExecutablePath, arguments);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void RunApplication(string dataFolder = null)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolderChanged += Settings_DataFolderChanged;

            Settings_DataFolderChanged(global::OmoSeitokuEreceipt.Properties.Settings.Default, EventArgs.Empty);

            try
            {
                CreateTempDir();
                _appContext = new MyApplicationContext();
                // アプリケーションを実行
                Application.Run(_appContext);
            }
            finally
            {
                _appContext.Dispose();
                _appContext = null;

                DeleteTempDir();

                global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolderChanged -= Settings_DataFolderChanged;
            }
        }

        /// <summary>
        /// データフォルダーが変更された際に呼び出される。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Settings_DataFolderChanged(object sender, EventArgs e)
        {
        }


        // プロセス間通信
        /// <summary>
        /// 新規起動で実行中に多重起動側からの通信に応答する。
        /// </summary>
        /// <param name="e"></param>
        public static void _transaction_Transactioned(TransactionEventArgs e)
        {
            switch (e.Data.Action)
            {
                case TRANS_ACT_OPEN_NEW_FORM:
                    ShowNewForm();
                    break;
            }
        }

        public static void ShowNewForm()
        {
            _appContext.ShowNewForm();
        }

        public static void CloseAllForm()
        {
            _appContext.CloseAll();
        }


        /// <summary>
        /// ソフトウェアのバージョン文字列を作成する。
        /// </summary>
        /// <returns></returns>
        public static string CreateVersionText()
        {
            var sb = new StringBuilder(32);

            byte[] hash;
            using (FileStream fs = File.OpenRead(Application.ExecutablePath))
            {
                // MD5生成
                hash = System.Security.Cryptography.MD5.Create().ComputeHash(fs);
            }
            int i = 0;
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
                if (++i % 2 == 0)
                {
                    if (i == 6) break;
                    sb.Append('-');
                }
            }

            return sb.ToString();
        }

    }
}
