using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OmoSeitokuEreceipt.SER
{
    /// <summary>
    /// 診療録作成ソフトのチェック印刷
    /// </summary>
    public sealed class CheckPrint
    {
        public const string FILE_NAME = "CHDATRCD.900";
        public static Encoding ENCODING = Encoding.GetEncoding("Shift_JIS");

        private static Regex ITEM_REGEX = new Regex("^\"([^\"]+)\",(\\d+),\"([^\"]+)\",\"([^\"]+)\"[^$]*?$", RegexOptions.Multiline);

        public List<Item> List = new List<Item>(2000);


        public CheckPrint()
        {
            Load();
        }


        public void Reload()
        {
            Load();
        }

        private void Load()
        {
            List<Item> list = this.List;

            list.Clear();

            string path = Path.Combine(global::OmoSeitokuEreceipt.Properties.Settings.Default.DataFolder, FILE_NAME);
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path, ENCODING);
                var ms = ITEM_REGEX.Matches(text);
                foreach (Match m in ms)
                {
                    if (!int.TryParse(m.Groups[2].Value, out int karteNumber))
                    {
                        continue;
                    }

                    var shinryoujo = new Shinryoujo(m.Groups[1].Value);

                    list.Add(new Item(
                        new KarteId(shinryoujo, karteNumber),
                        m.Groups[3].Value,
                        m.Groups[4].Value));
                }

                //using (var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                //using (var reader = new StreamReader(fs, ENCODING))
                //{
                //    Regex reg = ITEM_REGEX;

                //    while (!reader.EndOfStream)
                //    {
                //        var m = reg.Match(reader.ReadLine());
                //        if (m.Success)
                //        {
                //            int karteNumber;
                //            if (!int.TryParse(m.Groups[2].Value, out karteNumber)) karteNumber = 0;

                //            list.Add(new Item(
                //                m.Groups[1].Value,
                //                karteNumber,
                //                m.Groups[3].Value,
                //                m.Groups[4].Value));
                //        }
                //    }
                //}
            }
        }

        public Item[] GetItems(KarteId karteId)
        {
            return this.List
                .Where(p => p.KarteId == karteId)
                .ToArray();
        }


        public sealed class Item
        {
            public readonly KarteId KarteId;
            public readonly string Nenrei;
            public readonly string Comment;

            public Item(KarteId karteId, string nenrei, string comment)
            {
                this.KarteId = karteId;
                this.Nenrei = nenrei;
                this.Comment = comment;
            }
        }
    }
}
