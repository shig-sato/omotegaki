using System;
using System.IO;

namespace OmoSeitokuEreceipt.SER
{
    public sealed class KarteFileStream : IDisposable
    {
        private const int KARTE_NUM_MAX = 32766;

        public int MaxKarteNumber => maxKarteNumber ?? (maxKarteNumber = Math.Min(KARTE_NUM_MAX, (int)(_fs.Length / KanjaData.SIZE))).Value;
        private int? maxKarteNumber;

        private FileStream _fs;
        private BinaryReader _br;
        private bool _disposedValue;

        public KarteFileStream(Shinryoujo shinryoujo)
        {
            string path = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, $"kanrc_2n.{shinryoujo.Key}");

            if (!File.Exists(path))
            {
                throw new Exception($"患者データファイルが存在しません。[{path}]");
            }

            _fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _br = new BinaryReader(_fs);
        }

        public KarteData GetKarteData(int karteNumber)
        {
            long pos = (karteNumber - 1) * KanjaData.SIZE;
            if (0 > pos || pos >= _fs.Length)
            {
                throw new Exception("患者番号が範囲外です。");
            }
            _fs.Seek(pos, SeekOrigin.Begin);

            return new KarteData(_br);
        }

        #region IDisposable

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // マネージド状態を破棄します (マネージド オブジェクト)
                }

                // アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // 大きなフィールドを null に設定します
                _disposedValue = true;
                _br.Dispose();
                _br = null;
                _fs.Dispose();
                _fs = null;
            }
        }

        // 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        ~KarteFileStream()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable
    }

}
