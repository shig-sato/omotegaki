using OmoSeitoku.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace OmoOmotegaki.Forms
{
    public partial class KarteRuleForm : Form
    {
        private DataListView _listView;
        private DataTable _table;
        private string _dataSource;

        private FileInfo _outputFile;

        public KarteRuleForm(FileInfo outputFile)
        {
            InitializeComponent();

            _outputFile = outputFile;

            // 一時ファイルをデータソースとして使用する。
            _dataSource = Path.GetTempFileName();

            if (outputFile.Exists)
            {
                // 保存先ファイルをデータソース（一時ファイル）にコピー
                File.Copy(_outputFile.FullName, _dataSource, true);
            }
            else if (File.Exists(_dataSource))
            {
                // Path.GetTempFileName() で空ファイルが作成されてしまうので削除する。
                File.Delete(_dataSource);
            }

            _listView = new DataListView();
            _listView.Dock = DockStyle.Fill;
            _listView.SelectedIndexChanged += new EventHandler(_listView_SelectedIndexChanged);

            panel1.Controls.Add(_listView);


            _listView.BringToFront();

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            _table = new DataTable();

            _table.Columns.Add("id");
            _table.Columns.Add("type");
            _table.Columns.Add("data1");
            _table.Columns.Add("data2");

            UpdateView();
        }


        private void UpdateView()
        {
            _listView.DataSource = null;
            _table.Clear();

            using (var cnn = new SQLiteConnection("Data Source=" + this._dataSource))
            using (SQLiteCommand cmd = cnn.CreateCommand())
            {
                cnn.Open();

                CreateTableIfNotExists(cmd);

                // SELECT
                cmd.CommandText = "SELECT * FROM karte_rule_table WHERE 1";

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        DataRow row = _table.NewRow();
                        row["id"] = reader["id"];
                        row["type"] = reader["type"];
                        row["data1"] = reader["data1"];
                        row["data2"] = reader["data2"];

                        _table.Rows.Add(row);
                    }
                }
            }

            _listView.DataSource = _table;
        }

        private void DeleteRows(int[] rows)
        {
            var where = new StringBuilder();

            foreach (var row in rows)
            {
                where.Append(" OR id = ").Append(row);
            }


            using (var cnn = new SQLiteConnection("Data Source=" + this._dataSource))
            using (SQLiteCommand cmd = cnn.CreateCommand())
            {
                cnn.Open();

                // DELETE
                cmd.CommandText = "DELETE FROM karte_rule_table WHERE 0 " + where;

                cmd.ExecuteNonQuery();
            }

            // 更新前に選択しているアイテムのうち最後のもののインデックスを取得しておく
            int selected = -1;
            var indices = _listView.SelectedIndices;
            if (0 < indices.Count)
            {
                selected = indices[indices.Count - 1];
            }

            // 更新
            UpdateView();

            // 更新前の選択アイテムの次のアイテムを選択
            if (-1 < selected)
            {
                try
                {
                    _listView.SelectedIndices.Add(selected);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    try
                    {
                        _listView.SelectedIndices.Add(selected - 1);
                        Console.WriteLine(ex.Message);
                    }
                    catch (ArgumentOutOfRangeException ex2)
                    {
                        Console.WriteLine(ex2.Message);
                    }
                }
            }
        }



        private static void CreateTableIfNotExists(SQLiteCommand cmd)
        {
            // CREATE TABLE
            cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS karte_rule_table (" +
                "    id INTEGER PRIMARY KEY," +
                "    type TEXT," +
                "    data1 TEXT," +
                "    data2 TEXT" +
                ")";
            cmd.ExecuteNonQuery();
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (File.Exists(_dataSource))
            {
                File.Delete(_dataSource);
            }
        }

        private void 追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string input = null;
            using (var dlg = new InputDialog())
            {
                dlg.Value = "act1," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",data2";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    input = dlg.Value;
                }
            }

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            string[] data = input.Split(',');
            if (data.Length < 2 || 3 < data.Length)
            {
                MessageBox.Show("データ入力はカンマ（,）で区切り、 2つまたは3つの項目を入力してください。（例： type,data1,data2）");
                return;
            }


            using (var cnn = new SQLiteConnection("Data Source=" + this._dataSource))
            using (SQLiteCommand cmd = cnn.CreateCommand())
            {
                cnn.Open();

                // INSERT OR REPLACE
                cmd.CommandText =
                    "INSERT OR REPLACE INTO karte_rule_table (" +
                    "    type," +
                    "    data1," +
                    "    data2" +
                    ") VALUES(" +
                    "    '" + data[0] + "'," +
                    "    '" + data[1] + "'," +
                    "    '" + (data.Length == 3 ? data[2] : "") + "'" +
                    ")";

                cmd.ExecuteNonQuery();
            }

            UpdateView();
        }

        private void 削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rows = new List<int>();

            foreach (ListViewItem item in _listView.SelectedItems)
            {
                rows.Add(int.Parse(item.Text));
            }

            DeleteRows(rows.ToArray());
        }

        private void 最新の情報に更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateView();
        }

        // リストビューのアイテム選択状態が変更された際に発生
        private void _listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            削除ToolStripMenuItem.Enabled = 0 < _listView.SelectedItems.Count;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // データソース（一時ファイル）を実際のパスにコピー
            File.Copy(_dataSource, _outputFile.FullName, true);

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }
    }
}
