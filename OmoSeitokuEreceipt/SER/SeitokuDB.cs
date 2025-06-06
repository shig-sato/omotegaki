using System;
using System.IO;
using System.Text;

namespace OmoSeitokuEreceipt
{
    //TODO SeitokuDBのDataSet化

    public sealed class SeitokuDB
    {
        private static readonly Encoding DEFAULT_ENCODING = Encoding.GetEncoding("Shift_JIS");

        public string[] Columns;

        /// <summary>
        /// [row, col]
        /// </summary>
        public string[,] Rows;

        public readonly string FilePath;
        public readonly Encoding Encoding;

        /// <summary>コンストラクタ</summary>
        /// <param name="path">DBファイルパス</param>
        /// <param name="encoding">DBファイルの文字エンコーディング</param>
        public SeitokuDB(string path, Encoding encoding)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            else if (!File.Exists(path))
            {
                throw new FileNotFoundException("ファイルが存在しません。", path);
            }

            this.FilePath = path;
            this.Encoding = encoding;

            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fs, encoding))
            {
                // DB定義の読み込み
                try
                {
                    int colCount;
                    int rowCount;

                    // 列数、行数を取得する。
                    if (GetColRowCount(out colCount, out rowCount, reader))
                    {
                        // 列定義
                        this.Columns = GetColumnDefinition(reader, colCount);

                        // 行データ
                        this.Rows = GetRowData(reader, colCount, rowCount);
                    }
                }
                catch (ApplicationException ex)
                {
                    throw new DBDefinitionErrorException(path, ex.Message + " (\"" + path + "\")");
                }
            }
        }

        /// <summary>コンストラクタ</summary>
        /// <param name="path">DBファイルパス</param>
        public SeitokuDB(string path)
            : this(path, DEFAULT_ENCODING)
        {
        }

        /// <summary>
        /// 列名から列のインデックスを取得する。
        /// </summary>
        /// <param name="colName"></param>
        /// <returns>存在しない場合 負の数値</returns>
        public int GetColumn(string colName)
        {
            string[] cols = this.Columns;

            for (int i = 0, len = cols.Length; i < len; ++i)
            {
                if (cols[i].Equals(colName))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 行と列名から値を取得する。
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <exception cref="ColumnNotFoundException">列が見つからない場合に発生する。</exception>
        public string GetValue(int row, string columnName)
        {
            int col = GetColumn(columnName);

            if (col < 0)
            {
                throw new ColumnNotFoundException(columnName);
            }

            return this.Rows[row, col];
        }

        /// <summary>
        /// 行と列から値を取得する。
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <returns></returns>
        /// <exception cref="ColumnNotFoundException">列が見つからない場合に発生する。</exception>
        /// <exception cref="System.IndexOutOfRangeException">行が範囲外の場合に発生する。</exception>
        public string GetValue(int row, int column)
        {
            if (column < 0 || this.Columns.Length <= column)
            {
                throw new ColumnIndexOutOfRangeException();
            }

            return this.Rows[row, column];
        }

        /// <summary>
        /// search が一致する列をもつ行インデックスを取得する。
        /// </summary>
        /// <param name="column">検索する列</param>
        /// <param name="search">検索する文字列</param>
        /// <param name="startIndex">開始位置。先頭は0</param>
        /// <returns>存在しない場合は 負の数値</returns>
        public int Find(int column, string search, int startIndex)
        {
            string[,] rows = this.Rows;
            for (int i = startIndex, len = rows.GetLength(0); i < len; ++i)
            {
                if (rows[i, column].Equals(search))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// search が含まれる列をもつ行インデックスを取得する。
        /// </summary>
        /// <param name="column"></param>
        /// <param name="search"></param>
        /// <param name="startIndex"></param>
        /// <returns>存在しない場合は 負の数値</returns>
        public int FindContains(int column, string search, int startIndex)
        {
            string[,] rows = this.Rows;
            for (int i = startIndex, len = rows.GetLength(0); i < len; ++i)
            {
                if (rows[i, column].Contains(search))
                {
                    return i;
                }
            }
            return -1;
        }



        private static bool GetColRowCount(out int colCount, out int rowCount, StreamReader input)
        {
            string line = input.ReadLine();

            if ((line == null) || !line.Contains(","))
            {
                throw new ApplicationException("SeitokuDB データベース定義行がありません。");
            }

            string[] split = line.Split(',');

            if ((split.Length != 2)
                || !int.TryParse(split[0], out rowCount)
                || !int.TryParse(split[1], out colCount)
                )
            {
                throw new ApplicationException("SeitokuDB データベース定義行の形式が異常です。");
            }

            rowCount -= 2; // 列定義分の行数(2行)を引く

            return true;
        }

        private static string[] GetColumnDefinition(StreamReader reader, int colCount)
        {
            var columns = new string[colCount];

            for (int i = 0; i < colCount; ++i)
            {
                string line = reader.ReadLine();

                if (line == null)
                {
                    throw new ApplicationException("SeitokuDB 定義と実際のカラム数が異なります。");
                }

                columns[i] = line.Substring(1, line.Length - 2);
            }

            return columns;
        }

        private static string[,] GetRowData(StreamReader reader, int colCount, int rowCount)
        {
            // 列定義と行データの間の空データ行をスキップ
            for (int i = 0; i < colCount; ++i)
            {
                if (reader.ReadLine() == null)
                {
                    throw new ApplicationException("SeitokuDB 空データ行がありません。");
                }
            }

            // 行データを取得
            string[,] rows = new string[rowCount, colCount];
            int col = 0;
            int row = 0;

            while (row < rowCount)
            {
                string line = reader.ReadLine();

                if (line == null)
                {
                    throw new ApplicationException("SeitokuDB 定義と実際の行数が異なります。");
                }

                // 両端のダブルクオーテーションを削除。
                int len = line.Length;
                rows[row, col] = (len < 2)
                                    ? ""
                                    : line.Substring(1, len - 2);

                // カラムインデックスをローテーション
                col = (col + 1) % colCount;

                // 行を進める
                if (col == 0)
                {
                    ++row;
                }
            }

            return rows;
        }
    }



    #region 例外クラス

    public sealed class ColumnNotFoundException : ApplicationException
    {
        public readonly string ColumnName;

        public ColumnNotFoundException(string columnName)
            : this(columnName, "Column not found \"" + columnName + "\".")
        {
        }

        public ColumnNotFoundException(string columnName, string message)
            : base(message)
        {
            this.ColumnName = columnName;
        }

        public ColumnNotFoundException(string columnName, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ColumnName = columnName;
        }
    }

    public sealed class ColumnIndexOutOfRangeException : ApplicationException
    {
        public ColumnIndexOutOfRangeException()
            : base("Column index is out of range.")
        {
        }

        public ColumnIndexOutOfRangeException(string message)
            : base(message)
        {
        }

        public ColumnIndexOutOfRangeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// データベースファイルから定義の取得に失敗した際に発生する例外。
    /// </summary>
    public sealed class DBDefinitionErrorException : ApplicationException
    {
        public readonly string DBFilePath;

        public DBDefinitionErrorException(string dbFilePath)
            : base("Can't get definition by SeitokuDB file. (\"" + dbFilePath + "\")")
        {
            this.DBFilePath = dbFilePath;
        }

        public DBDefinitionErrorException(string dbFilePath, string message)
            : base(message)
        {
            this.DBFilePath = dbFilePath;
        }
    }

    #endregion
}
