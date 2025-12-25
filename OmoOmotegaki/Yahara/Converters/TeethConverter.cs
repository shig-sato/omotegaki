#nullable enable

using OmoEReceLib;
using OmoEReceLib.ERObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace omotegaki_xml.Libs.Yahara.Entities.KarteEntities
{
    public static class TeethConverter
    {
        public static string Convert(ERŽ•Ž® shishiki)
        {
            string str;
            {
                var sb = new StringBuilder("- - -");

                foreach (var tanni in shishiki.GetYaharaEnumerator())
                {
                    AddHa(sb, tanni);
                }
                str = sb.ToString();
            }

            // •”‹ßSŒ„‚Ì‡”Ô“™‚ðC³
            foreach (var tanni in shishiki.Where(p => p.ó‘Ô == ER_ó‘Ô.•”‹ßSŒ„))
            {
                // •”‹ßSŒ„‚Ì‘ÎÛ (—á:" „£#5") ‚ð’T‚·
                var m = Regex.Match(str, GetBuiStr(tanni) + @"[^\?]" + tanni.Ž•Ží.Substring(3, 1) + "[^ ]*");
                if (m.Success)
                {
                    // " „£#5~4"
                    var t = new StringBuilder(m.Value).Append("~4").ToString();

                    // C³Œã: " „£#5~4 „£?5~1"
                    if (tanni.•”ˆÊ == ERŽ•Ž®.•”ˆÊ.‰E‘¤ãŠ{ || tanni.•”ˆÊ == ERŽ•Ž®.•”ˆÊ.‰E‘¤‰ºŠ{)
                    {
                        // " „£?5~1"
                        var r = AddHa(new StringBuilder(), tanni).ToString();

                        str = str
                            .Replace(r, string.Empty)
                            .Replace(m.Value, tanni.•”ˆÊ == ERŽ•Ž®.•”ˆÊ.‰E‘¤‰ºŠ{ ? r + t : t + r);
                    }
                    else
                    {
                        str = str.Replace(m.Value, t);
                    }

                }
            }

            return str;
        }

        private static string GetBuiStr(ERŽ•Ž®’PˆÊ tanni)
        {
            return tanni.•”ˆÊ switch
            {
                ERŽ•Ž®.•”ˆÊ.‰E‘¤ãŠ{ => " „£",
                ERŽ•Ž®.•”ˆÊ.¶‘¤ãŠ{ => " „¤",
                ERŽ•Ž®.•”ˆÊ.¶‘¤‰ºŠ{ => " „¡",
                ERŽ•Ž®.•”ˆÊ.‰E‘¤‰ºŠ{ => " „¢",
                _ => throw new Exception("[ewriolu092p90i20oersg]"),
            };
        }

        private static StringBuilder AddHa(StringBuilder sb, ERŽ•Ž®’PˆÊ tanni)
        {
            sb.Append(GetBuiStr(tanni));

            sb.Append((tanni.ó‘Ô == ER_ó‘Ô.•”‹ßSŒ„) ? '?' : (tanni.Is“ûŽ• ? '$' : '#'));

            sb.Append(tanni.Ž•Ží.Substring(3, 1));

            /*
                ? ŠÔŒ„
                ! ‘Ž•
                ~1 •ªŠ„‹ßS
                ~4 •ªŠ„‰“S
            */

            sb.Append(tanni.ó‘Ô switch
            {
                ER_ó‘Ô.Œ»‘¶Ž• => string.Empty,
                ER_ó‘Ô.•” => "a",
                ER_ó‘Ô.Œ‡‘¹Ž• => throw new Exception("[io345wguih123]"),
                ER_ó‘Ô.Žx‘äŽ• => "@",
                ER_ó‘Ô.•ªŠ„”²Ž•Žx‘ä => throw new Exception("[3:lkik34ef]"),
                ER_ó‘Ô.•Ö‹X”²‘Žx‘äŽ• => "*",
                ER_ó‘Ô.Žcª => throw new Exception("[0t345sjbnwaer1asd]"),
                ER_ó‘Ô.•”ƒCƒ“ƒvƒ‰ƒ“ƒg => throw new Exception("[23qf56hen6um]"),
                ER_ó‘Ô.•”‹ßSŒ„ => "~1",
                ER_ó‘Ô.‹ßSˆÊ‚É‘¶Ý => throw new Exception("[h45o923v2alrlrg]"),
                _ => throw new Exception("[4eys532dfgwa42352rsf]"),
            });

            //sb.Append(tanni.•”•ª switch
            //{
            //    ER_•”•ª.•”•ªŽw’è‚È‚µ => string.Empty,
            //    ER_•”•ª.‰“S–j‘¤ª => "",
            //    ER_•”•ª.‹ßS–j‘¤ª => "",
            //    ER_•”•ª.‹ßS–j‘¤ª‹y‚Ñ‰“S–j‘¤ª => "",
            //    ER_•”•ª.ã‘¤_ŒûŠW_ª => "",
            //    ER_•”•ª.ã‘¤_ŒûŠW_ª‹y‚Ñ‰“S–j‘¤ª => "",
            //    ER_•”•ª.ã‘¤_ŒûŠW_ª‹y‚Ñ‹ßS–j‘¤ª => "",
            //    ER_•”•ª.‰“Sª => "",
            //    ER_•”•ª.‹ßSª => "",
            //    _ => throw new Exception("[ygdfvw46766f2412q]"),
            //});

            return sb;
        }

    }
}
