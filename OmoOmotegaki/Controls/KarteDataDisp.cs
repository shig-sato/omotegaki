using OmoEReceLib;
using OmoSeitokuEreceipt.SER;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OmoOmotegaki.Controls
{
    /// <summary>
    /// FormにAcceptButtonを指定することでテキストボックスの改行入力を無効にできる。
    /// </summary>
    public partial class KarteDataDisp : UserControl
    {
        private KarteData _karteData = KarteData.Empty;

        public KarteDataDisp()
        {
            InitializeComponent();
        }

        public KarteData KarteData
        {
            get => _karteData;
            set
            {
                _karteData = (value is null) ? KarteData.Empty : new KarteData(value);
                RefreshControlsByKarteData();
            }
        }

        public int? 公費負担者番号 => _karteData.公費負担者番号;
        public int? 公費受給者番号 => _karteData.公費受給者番号;
        public string 受診者_氏名 => _karteData.氏名;
        public DateTime? 受診者_生年月日 => _karteData.生年月日;
        public ER_男女区分? 受診者_性別 => _karteData.性別;
        public int? 郵便番号 => _karteData.郵便番号;
        public string 住所 => _karteData.住所;
        public string 電話番号 => _karteData.電話番号;
        public string 職業 => _karteData.職業;
        public string 世帯主との続柄 => _karteData.世帯主との続柄;
        public int? 保険者番号 => _karteData.保険者番号;
        public string 被保険者証_記号 => _karteData.被保険者証_記号;
        public string 被保険者証_番号 => _karteData.被保険者証_番号;
        public DateTime? 被保険者証_有効期限 => _karteData.保険有効期限;
        public string 世帯主氏名 => _karteData.世帯主氏名;
        public DateTime? 資格取得 => _karteData.資格取得;
        public string 特別区名 => _karteData.特別区名;
        public string 市町村名 => _karteData.市町村名;
        public string 国民健康保険組合名 => _karteData.国民健康保険組合名;

        public void Clear()
        {
            _karteData = KarteData.Empty;
            KarteData = _karteData;

            ResizeKigouBangou();
        }

        private void ResizeKigouBangou()
        {
            lblKigouBangouSplit.Left =
                lblKigou.Left + lblKigou.Width;
            lblBangou.Left =
                lblKigouBangouSplit.Left + lblKigouBangouSplit.Width;
        }

        private void KanjaDataDisp_Load(object sender, EventArgs e)
        {
            lblKigou.Resize += delegate { ResizeKigouBangou(); };
            lblBangou.Resize += delegate { ResizeKigouBangou(); };

            // テキストボックスの改行文字キャンセル
            static void textChanged(object sender, EventArgs e)
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Text.Contains(Environment.NewLine))
                {
                    // キャレット位置復帰用に現在位置を取得
                    int selectionStart = textBox.SelectionStart;
                    textBox.Text = textBox.Text.Replace(Environment.NewLine, string.Empty);
                    // 復帰
                    textBox.Select(selectionStart - 2, 0);
                }
            }
            txtSimei.TextChanged += textChanged;
            txtJyuusyo.TextChanged += textChanged;
            txtSyokugyou.TextChanged += textChanged;
            txtZokugara.TextChanged += textChanged;
            txtSetainusi.TextChanged += textChanged;
            txtTokubetukumei.TextChanged += textChanged;
            txtSityousonmei.TextChanged += textChanged;
            txtKokuminkenkou.TextChanged += textChanged;
        }

        private void RefreshControlsByKarteData()
        {
            KarteData karteData = _karteData;

            // 公費負担者番号
            txtKouhihutan.Text = Create番号String(karteData.公費負担者番号, 8, new[] { 4, 6, 8 });

            // 公費受給者番号
            txtKouhiJukyuu.Text = Create番号String(karteData.公費受給者番号, 7, 7);

            // 氏名
            txtSimei.Text = karteData.氏名?.Trim() ?? string.Empty;

            // 生年月日, 性別
            lblSeinengappi.Text = string.Concat(
                karteData.生年月日.HasValue
                    ? string.Concat(
                        ERDateTime.GetEraYear(karteData.生年月日.Value, true),
                        karteData.生年月日.Value.ToString(" 年 MM 月 dd 日生     "))
                    : string.Empty,
                karteData.性別.HasValue
                    ? karteData.性別.Value.ToString()
                    : string.Empty);

            // 郵便番号
            lblYuubinBangou.Text = Create郵便番号(karteData);
            static string Create郵便番号(KarteData karteData)
            {
                string s = (!karteData.郵便番号.HasValue || karteData.郵便番号 <= 0)
                                ? string.Empty
                                : karteData.郵便番号.Value.ToString();
                return 4 < s.Length ? s.Insert(3, "-") : s;
            }

            // 住所
            txtJyuusyo.Text = karteData.住所?.Trim() ?? string.Empty;

            // 電話番号
            lblDenwa.Text = karteData.電話番号?.Trim() ?? string.Empty;

            // 職業
            txtSyokugyou.Text = karteData.職業?.Trim() ?? string.Empty;

            // 世帯主との続柄
            txtZokugara.Text = karteData.世帯主との続柄?.Trim() ?? string.Empty;

            // 保険者番号
            txtHokensya.Text = Create番号String(karteData.保険者番号, 8, new[] { 4, 6, 8 });

            // 被保険者証_記号
            lblKigou.Text = karteData.被保険者証_記号?.Trim() ?? string.Empty;
            // 被保険者証_番号
            lblBangou.Text = karteData.被保険者証_番号?.Trim() ?? string.Empty;

            // 保険有効期限
            lblYuukoukigen.Text = karteData.保険有効期限.HasValue
                ? ERDateTime.GetEraYear(karteData.保険有効期限.Value).Replace(" ", "  ") + karteData.保険有効期限.Value.ToString(" 年  MM 月  dd 日")
                : string.Empty;

            // 世帯主氏名
            txtSetainusi.Text = karteData.世帯主氏名?.Trim() ?? string.Empty;

            // 資格取得
            lblSikakuSyutoku.Text = karteData.資格取得.HasValue
                ? ERDateTime.GetEraYear(karteData.資格取得.Value).Replace(" ", "  ") + karteData.資格取得.Value.ToString(" 年  MM 月  dd 日")
                : string.Empty;

            // 特別区名
            txtTokubetukumei.Text = karteData.特別区名?.Trim() ?? string.Empty;

            // 市町村名
            txtSityousonmei.Text = karteData.市町村名?.Trim() ?? string.Empty;

            // 国民健康保険組合名
            txtKokuminkenkou.Text = karteData.国民健康保険組合名?.Trim() ?? string.Empty;

            return;


            // value の桁数が allowLengths のいずれかの長さに合致する場合は paddingLength の長さにして文字列化する。
            static string Create番号String(int? value, int paddingLength, params int[] allowLengths)
            {
                if (value.HasValue && value.Value > 0)
                {
                    string numStr = value.Value.ToString();
                    int length = numStr.Length;
                    if (allowLengths.Any(p => p == length))
                    {
                        return numStr.PadLeft(paddingLength);
                    }
                }

                return string.Empty;
            }
        }
    }
}
