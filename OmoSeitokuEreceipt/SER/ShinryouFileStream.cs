using System;
using System.IO;

namespace OmoSeitokuEreceipt.SER
{
    public sealed class ShinryouFileStream : IDisposable
    {
        private FileStream _fs;
        private BinaryReader _br;
        private bool _disposedValue;

        public ShinryouFileStream(Shinryoujo shinryoujo)
        {
            // UNDONE 診療録データ ハードコード
            string path = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, $"RANDA{shinryoujo.Key}.900");

            if (!File.Exists(path))
            {
                throw new Exception($"診療データファイルが存在しません。[{path}]");
            }

            _fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _br = new BinaryReader(_fs);//, Encoding.GetEncoding("Shift_JIS")))
        }

        public SinryouDataLoader GetSinryouDataLoader(KarteId karteId)
        {
            return new SinryouDataLoader(karteId, _fs, _br);
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
        ~ShinryouFileStream()
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
