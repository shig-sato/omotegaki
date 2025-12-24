using OmoSeitokuEreceipt.SER;
using System;
using System.Text;

namespace OmoOmotegaki.Models
{
    public static class KarteRepository
    {
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
