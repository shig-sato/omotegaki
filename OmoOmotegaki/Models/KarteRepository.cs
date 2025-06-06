using OmoSeitokuEreceipt.SER;
using System;
using System.IO;

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
    }
}
