using System;
using System.Collections.Generic;
using System.IO;

namespace OmoSeitokuEreceipt.SER
{
    public sealed class SyochiKikanDb
    {
        private List<SyochiDb> _syochiDbs;

        public bool TryFind(DateTime sinryoubi, out SyochiDb syochiDb)
        {
            if (_syochiDbs == null)
            {
                _syochiDbs = LoadAllSyochiDb();
            }

            for (int i = 0; i < _syochiDbs.Count; i++)
            {
                syochiDb = _syochiDbs[i];
                if (syochiDb.IsIn(sinryoubi))
                {
                    return true;
                }
            }
            syochiDb = null;
            return false;
        }

        private static List<SyochiDb> LoadAllSyochiDb()
        {
            const int COL_開始期日 = 1;
            const int COL_失効期日 = 2;
            const int COL_ファイルパス = 3;

            var syochiDbs = new List<SyochiDb>();

            //UNDONE debug 処置DBリスト読み込 ハードコード
            SeitokuDB _syoKikanDb = new SeitokuDB(Path.Combine(
                global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, "S9X0401X.XXX"));

            string[,] rows = _syoKikanDb.Rows;

            for (int row = 0, len = rows.GetLength(0); row < len; ++row)
            {
                string startDateStr = rows[row, COL_開始期日];
                string endDateStr = rows[row, COL_失効期日];

                if (DateTime.TryParse(startDateStr, out DateTime startDate) &&
                    DateTime.TryParse(endDateStr, out DateTime endDate))
                {
                    string path = rows[row, COL_ファイルパス];
                    path = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, $"{Path.GetFileName(path)}t");

                    var syochiDb = new SyochiDb(startDate, endDate, path);
                    syochiDbs.Add(syochiDb);
                }
            }

            return syochiDbs;
        }
    }

    public sealed class SyochiDb
    {
        public readonly DateTime startDate;
        public readonly DateTime endDate;
        public readonly string path;

        private SeitokuDB _db = null;
        public SeitokuDB Db
        {
            get { return _db ?? (_db = new SeitokuDB(path)); }
        }

        public SyochiDb(DateTime startDate, DateTime endDate, string path)
        {
            this.startDate = startDate;
            this.endDate = endDate;
            this.path = path;
        }

        public bool IsIn(DateTime dt)
        {
            return (startDate <= dt) && (dt <= endDate);
        }
    }
}