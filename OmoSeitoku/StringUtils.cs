namespace OmoSeitoku
{
    public static class StringUtils
    {
        public static string ZenToHanNum(string value)
        {
            // 全角数字が含まれない場合は早期リターン
            bool hasZen = false;
            for (int i = 0; i < value.Length; i++)
            {
                if ((uint)(value[i] - '０') <= 9u) { hasZen = true; break; }
            }
            if (!hasZen) return value; // アロケーションなしで元のインスタンスを返す

            char[] ar = value.ToCharArray();
            for (int i = 0; i < ar.Length; i++)
            {
                char c = ar[i];
                if ((uint)(c - '０') <= 9u)
                    ar[i] = (char)(c - '０' + '0');
            }
            return new string(ar);
        }
    }
}
