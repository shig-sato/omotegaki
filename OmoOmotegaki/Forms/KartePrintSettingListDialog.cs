using OmoSeitokuEreceipt.SER;
using System;
using System.Data;
using System.Windows.Forms;

namespace OmoOmotegaki.Forms
{
    public partial class KartePrintSettingListDialog : Form
    {
        /// <summary>
        /// リストから選択された設定。
        /// 新規印刷設定ダイアログ表示時のデフォルト設定。
        /// </summary>
        public KartePrintDesign.Settings Settings
        {
            get
            {
                return __prop__Settings;
            }
            set
            {
                __prop__Settings = new KartePrintDesign.Settings(value);
            }
        }

        private KartePrintDesign.Settings __prop__Settings;



        public KartePrintSettingListDialog()
        {
            InitializeComponent();

            contextMenuStrip1.Items.Add(" - MENU - ");
            contextMenuStrip1.Closed += delegate
            {
                menuStrip1.Items.AddRange(new[] {
                    行編集ToolStripMenuItem,
                    行削除ToolStripMenuItem
                });
            };
            contextMenuStrip1.Opening += delegate
            {
                contextMenuStrip1.Items.AddRange(new[] {
                    行編集ToolStripMenuItem,
                    行削除ToolStripMenuItem
                });
            };
        }


        #region メソッド


        /// <summary>
        /// 印刷設定ダイアログを表示する。
        /// </summary>
        private KartePrintDesign.Settings ShowSettingDialog(KartePrintDesign.Settings defaultSettings)
        {
            using (var dlg = new KartePrintSettingDialog())
            {
                if (defaultSettings != null)
                {
                    dlg.Settings = defaultSettings;
                }

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    return dlg.Settings;
                }
                else
                {
                    // キャンセル
                    return null;
                }
            }
        }

        private void InsertRow(KartePrintDesign.Settings settings, int position)
        {
            var tbl = (DataTable)dataGridView1.DataSource;

            DataRow row;

            row = tbl.NewRow();

            row["保険診療"] = (settings.Hoken.HasValue ? settings.Hoken.Value.ToString() : "");

            row["未来院請求"] = (settings.Miraiin.HasValue ? settings.Miraiin.Value.ToString() : "");

            row["カルテ番号を表示"] = settings.ShowKarteNumber.ToString();

            row["患者名を表示"] = settings.ShowKarteName.ToString();

            row["カルテ期間を表示"] = settings.ShowKarteKikan.ToString();

            row["病名を表示"] = settings.ShowByoumei.ToString();

            row["診療録ソフトの処置名"] = settings.SSSSyochiName.ToString();

            row["診療録ソフトのチェック印刷"] = settings.SSSCheckPrint.ToString();


            tbl.Rows.InsertAt(row, position);
        }

        private void DeleteRows(int[] indices)
        {
            dataGridView1.SuspendLayout();
            try
            {
                var rows = ((DataTable)dataGridView1.DataSource).Rows;

                // 降順に削除する。
                Array.Sort<int>(indices);
                for (int i = indices.Length - 1; 0 <= i; --i)
                {
                    rows.RemoveAt(indices[i]);
                }

                dataGridView1.ClearSelection();
            }
            finally
            {
                dataGridView1.ResumeLayout();
            }
        }

        private void UpdateMenuStatus()
        {
            int selectedRowCount = dataGridView1.SelectedRows.Count;

            行追加ToolStripMenuItem.Enabled = selectedRowCount <= 1;
            行編集ToolStripMenuItem.Enabled = selectedRowCount == 1;
            行削除ToolStripMenuItem.Enabled = 0 < selectedRowCount;
        }

        private KartePrintDesign.Settings RowToSettings(int index)
        {
            var settings = this.Settings ?? new KartePrintDesign.Settings();
            var row = dataGridView1.Rows[index];

            string val;

            val = (string)row.Cells["保険診療"].Value;
            settings.Hoken = (SER_保険診療)Enum.Parse(typeof(SER_保険診療), val);

            val = (string)row.Cells["未来院請求"].Value;
            settings.Miraiin = (SER_未来院請求)Enum.Parse(typeof(SER_未来院請求), val);

            val = (string)row.Cells["カルテ番号を表示"].Value;
            settings.ShowKarteNumber = bool.Parse(val);

            val = (string)row.Cells["患者名を表示"].Value;
            settings.ShowKarteName = bool.Parse(val);

            val = (string)row.Cells["カルテ期間を表示"].Value;
            settings.ShowKarteKikan = bool.Parse(val);

            val = (string)row.Cells["病名を表示"].Value;
            settings.ShowByoumei = bool.Parse(val);

            val = (string)row.Cells["診療録ソフトの処置名"].Value;
            settings.SSSSyochiName = bool.Parse(val);

            val = (string)row.Cells["診療録ソフトのチェック印刷"].Value;
            settings.SSSCheckPrint = bool.Parse(val);


            return settings;
        }

        #endregion


        #region ヘルパーメソッド

        private void OnAddRowCommand()
        {
            dataGridView1.ClearSelection();

            var settings = ShowSettingDialog(this.Settings);
            if (settings != null)
            {
                int insertPos = dataGridView1.Rows.Count;

                InsertRow(settings, insertPos);

                dataGridView1.Rows[insertPos].Selected = true;
            }
        }

        private void OnEditRowCommand()
        {
            if (dataGridView1.SelectedRows?.Count == 1)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                KartePrintDesign.Settings settings = RowToSettings(index);

                settings = ShowSettingDialog(settings);

                if (settings != null)
                {
                    InsertRow(settings, index);
                    DeleteRows(new int[] { index + 1 });
                }
            }
        }

        private void OnDeleteRowCommand()
        {
            var sels = dataGridView1.SelectedRows;

            if (0 < sels.Count)
            {
                if (MessageBox.Show("選択中の行を削除します。", "選択行削除",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk,
                                    MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
                {
                    int[] ar = new int[sels.Count];

                    for (int i = sels.Count - 1; 0 <= i; --i)
                    {
                        ar[i] = sels[i].Index;
                    }

                    try
                    {
                        DeleteRows(ar);
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        #endregion


        // <イベントハンドラ>

        #region KartePrintSettingDialog

        private void KartePrintSettingDialog_Load(object sender, EventArgs e)
        {
            var tbl = new DataTable();

            tbl.Columns.AddRange(new[] {
                new DataColumn("保険診療", typeof(string)),
                new DataColumn("未来院請求", typeof(string)),
                new DataColumn("カルテ番号を表示", typeof(string)),
                new DataColumn("患者名を表示", typeof(string)),
                new DataColumn("カルテ期間を表示", typeof(string)),
                new DataColumn("病名を表示", typeof(string)),
                new DataColumn("診療録ソフトの処置名", typeof(string)),
                new DataColumn("診療録ソフトのチェック印刷", typeof(string))
                });

            dataGridView1.DataSource = tbl;


            UpdateMenuStatus();
        }

        private void KartePrintSettingDialog_Shown(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                OnAddRowCommand();
            }
        }

        #endregion

        #region Menu

        private void 行追加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnAddRowCommand();
        }

        private void 行編集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnEditRowCommand();
        }

        private void 行削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnDeleteRowCommand();
        }

        #endregion

        #region dataGridView1

        // 選択状態変更時
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateMenuStatus();
        }

        // セル ダブルクリック時
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            OnEditRowCommand();
        }

        // 行のコンテキストメニュー要求時
        private void dataGridView1_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            e.ContextMenuStrip = contextMenuStrip1;

            // 選択行が複数ではない場合、コンテキストメニューを要求した行のみを選択する。
            if (dataGridView1.SelectedRows.Count <= 1)
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[e.RowIndex].Selected = true;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK.PerformClick();
            }
        }

        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
            var sels = dataGridView1.SelectedRows;

            if (sels.Count != 1)
            {
                MessageBox.Show("印刷に使用する設定を1つ選択してください。");
                return;
            }

            this.Settings = RowToSettings(sels[0].Index);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.Close();
        }

        // </イベントハンドラ>
    }
}
