using OmoSeitokuEreceipt.SER;
using System;
using System.IO;
using System.Text;

namespace OmoOmotegaki.Models
{
    public sealed class KarteRepository
    {
        public static KarteData GetKarteData(KarteId karteId)
        {
            using FileStream fs = OpenKarteDataFile(karteId.Shinryoujo);
            using BinaryReader br = new BinaryReader(fs);
            long pos = (karteId.KarteNumber - 1) * KanjaData.SIZE;

            if (0 > pos || pos >= fs.Length)
            {
                throw new Exception("患者番号が範囲外です。");
            }

            fs.Seek(pos, SeekOrigin.Begin);

            return new KarteData(br);
        }

        public static FileStream OpenKarteDataFile(Shinryoujo shinryoujo)
        {
            string path = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, $"kanrc_2n.{shinryoujo.Key}");

            if (!File.Exists(path))
            {
                throw new Exception($"患者データファイルが存在しません。[{path}]");
            }

            return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }

        public static SinryouDataLoader GetShinryouDataLoader(KarteId karteId)
        {
            // UNDONE 診療録データ ハードコード
            string path = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, $"RANDA{karteId.Shinryoujo.Key}.900");

            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BinaryReader br = new BinaryReader(fs))//, Encoding.GetEncoding("Shift_JIS")))
            {
                return new SinryouDataLoader(karteId, fs, br);
            }
        }



        /// <summary>
        /// 現在のカルテの患者負担率を0以上1以下の範囲の値で取得する。
        /// </summary>
        /// <param name="hutanRitu">成功時は負担率が設定される。</param>
        /// <param name="error">エラーの場合、メッセージが設定される。</param>
        /// <param name="karte">カルテデータ</param>
        /// <param name="showInputBox">カルテに患者負担率が設定されていない場合に入力ボックスを出すかどうか。</param>
        /// <returns>成功した場合は true</returns>
        public static bool GetKanjaHutanritu(out double hutanRitu, out string error, KarteData karte, bool showInputBox)
        {
            if (karte.患者負担率.HasValue)
            {
                hutanRitu = karte.患者負担率.Value * 0.01;
                error = null;

                return true;
            }
            else
            {
                error = "患者負担率が設定されていません。";

                if (showInputBox)
                {
                    error = ShowKanjaHutanrituPrompt(out hutanRitu, 0.03);

                    return error is null;
                }
                else
                {
                    hutanRitu = 0;

                    return false;
                }
            }
        }
        /// <summary>
        /// 現在のカルテの患者負担率を0以上1以下の範囲の値で取得する。
        /// </summary>
        /// <param name="hutanRitu">成功時は負担率が設定される。</param>
        /// <param name="defaultHutanRitu">プロンプトで表示する患者負担率の初期値。キャンセル時に患者負担率に設定する。</param>
        /// <returns>成功した場合は null, エラー時はエラーメッセージ文字列。</returns>
        private static string ShowKanjaHutanrituPrompt(out double hutanRitu, double defaultHutanRitu)
        {
            string prompt = new StringBuilder()
                                .AppendLine("患者負担率が設定されていません。")
                                .AppendLine("負担率を百分率で入力してください。（パーセント(%)は不要です）")
                                .ToString();

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                                        prompt,
                                        "カルテ印刷 - 患者負担率入力",
                                        Math.Round(defaultHutanRitu * 100).ToString()
                                        );

            string error;

            if (string.IsNullOrEmpty(input))
            {
                // キャンセル
                error = "患者負担率の設定がキャンセルされました。";
            }
            else
            {
                try
                {
                    hutanRitu = int.Parse(Microsoft.VisualBasic.Strings.StrConv(
                                        input, Microsoft.VisualBasic.VbStrConv.Narrow)) * 0.01;

                    // 成功
                    return null;
                }
                catch (Exception ex)
                {
                    error = new StringBuilder().
                                    AppendLine("患者負担率が設定されていません。").
                                    AppendLine(ex.Message).
                                    ToString();
                }
            }

            // エラー

            hutanRitu = defaultHutanRitu;

            return error;

        }
    }
}
