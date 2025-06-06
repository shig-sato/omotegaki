namespace OmoEReceLib.ERObjects
{
    public sealed class ER歯式省略単位 : IER歯式表示単位
    {
        #region Const

        private const string 歯式単位省略記号 = "～";

        #endregion


        #region Property

        public bool Is乳歯
        {
            get;
            private set;
        }

        public ER_状態 状態
        {
            get { return ER_状態.現存歯; }
        }

        public string 歯席表示
        {
            get { return 歯式単位省略記号; }
        }

        #endregion


        #region Constructor

        public ER歯式省略単位(bool isNyuusi)
        {
            this.Is乳歯 = isNyuusi;
        }

        #endregion
    }
}
