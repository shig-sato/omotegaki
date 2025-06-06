using OmoOmotegaki.Forms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OmoOmotegaki
{
    public sealed class MyApplicationContext : ApplicationContext
    {
        public int FormCount
        {
            get { return _forms.Count; }
        }


        private List<OmotegakiForm> _forms = new List<OmotegakiForm>();


        internal MyApplicationContext()
        {
            OmotegakiForm frm = NewForm();

            frm.Show();
        }

        public OmotegakiForm NewForm()
        {
            OmotegakiForm newForm = new OmotegakiForm();

            _forms.Add(newForm);

            newForm.Closed += OnFormClosed;

            if (MainForm == null)
            {
                MainForm = newForm;
            }

            return newForm;
        }

        public void ShowNewForm()
        {
            // _forms の最初には必ずフォームがあるので そのスレッドで開始
            _forms[0].Invoke(new Action(delegate
            {
                NewForm().Show();
            }));
        }

        /// <summary>
        /// すべてのフォームを閉じる
        /// </summary>
        public void CloseAll()
        {
            var closeForm = new Action<OmotegakiForm>(frm => frm.Close());

            // 逆順で閉じる
            for (int i = _forms.Count - 1; 0 <= i; --i)
            {
                _forms[i].Invoke(closeForm, _forms[i]);
            }
        }

        private void OnFormClosed(object sender, EventArgs e)
        {
            // フォームが閉じたらリストから削除
            var frm = (OmotegakiForm)sender;

            _forms.Remove(frm);

            if (_forms.Count == 0)
            {
                // フォームリストが空になったらアプリ終了
                MainForm = null;
                ExitThread();
            }
            else
            {
                // 閉じられたのがメインフォームだったら
                // リストの最初のフォームをメインフォームに設定
                if (MainForm == frm)
                {
                    MainForm = _forms[0];
                }
            }
        }
    }
}
