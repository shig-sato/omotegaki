/*
 * kanaxs C# 拡張StringBuilder版 1.0
 * Copyright (c) 2014,
 * All rights reserved.
 *
 * このクラスは、以下のソースを参考にして作成しました。
 *
 * kanaxs C# 拡張版 1.0.2
 * Copyright (c) 2011, DOBON! <http://dobon.net>
 * All rights reserved.
 *
 * New BSD License（修正BSDライセンス）
 * http://wiki.dobon.net/index.php?free%2FkanaxsCSharp%2Flicense
 *
 * このクラスは、以下のソースを参考にして作成しました。
 * kanaxs Kana.JS
 * shogo4405 <shogo4405 at gmail.com>
 * http://code.google.com/p/kanaxs/
*/

using System;
using System.Text;

namespace CSharp.Japanese.Kanaxs
{
  /// <summary>
  /// ひらがなとカタカナ、半角と全角の文字変換を行うメソッドを提供します。
  /// Kanaクラスの拡張版です。
  /// </summary>
  public sealed class KanaExSb
  {
    private KanaExSb()
    {
    }

    /// <summary>
    /// 全角カタカナを全角ひらがなに変換します。
    /// </summary>
    /// <param name="str">変換する StringBuiler。</param>
    /// <remarks>
    /// Kana.ToHiraganaメソッドと違い、「ヽヾヷヸヹヺ」も変換します。
    /// </remarks>
    public static void ToHiragana(StringBuilder str)
    {
      int len = 0;
      int f = str.Length;

      str.EnsureCapacity(f * 2);

      for (int i = 0; i < f; i++)
      {
        char c = str[i];
        // ァ(0x30A1) ～ ン(0x30F3)
        // ヴ(0xu30F4)
        // ヵ(0x30F5) ～ ヶ(0x30F6)
        // ヽ(0x30FD) ヾ(0x30FE)
        if (('ァ' <= c && c <= 'ヶ') ||
            ('ヽ' <= c && c <= 'ヾ'))
        {
          str[len++] = (char)(c - 0x0060);
        }
        // ヷ(0x30F7) ～ ヺ(0x30FA)
        else if ('ヷ' <= c && c <= 'ヺ')
        {
          str[len++] = (char)(c - 0x0068);
          str.Insert(len++, '゛');
        }
        else
        {
          str[len++] = c;
        }
      }
    }
    public static string ToHiragana(string str)
    {
      var sb = new StringBuilder(str);
      ToHiragana(sb);
      return sb.ToString();
    }

    /// <summary>
    /// 全角ひらがなを全角カタカナに変換します。
    /// </summary>
    /// <param name="str">変換する StringBuiler。</param>
    /// <remarks>
    /// Kana.ToKatakanaメソッドと違い、「ゝゞ」も変換します。
    /// </remarks>
    public static void ToKatakana(StringBuilder str)
    {
      int f = str.Length;

      for (int i = 0; i < f; i++)
      {
        char c = str[i];
        // ぁ(0x3041) ～ ゖ(0x3096)
        // ゝ(0x309D) ゞ(0x309E)
        if (('ぁ' <= c && c <= 'ゖ') ||
            ('ゝ' <= c && c <= 'ゞ'))
        {
          str[i] = (char)(c + 0x0060);
        }
      }
    }
    public static string ToKatakana(string str)
    {
      var sb = new StringBuilder(str);
      ToKatakana(sb);
      return sb.ToString();
    }

    /// <summary>
    /// 全角英数字および記号を半角英数字および記号に変換します。
    /// </summary>
    /// <param name="str">変換する StringBuiler。</param>
    /// <remarks>
    /// Kana.ToHankakuメソッドと違い、「￥”’」も変換します。
    /// </remarks>
    public static void ToHankaku(StringBuilder str)
    {
      int f = str.Length;

      for (int i = 0; i < f; i++)
      {
        char c = str[i];
        // ！(0xFF01) ～ ～(0xFF5E)
        if ('！' <= c && c <= '～')
        {
          str[i] = (char)(c - 0xFEE0);
        }
        // 全角スペース(0x3000) -> 半角スペース(0x0020)
        else if (c == '　')
        {
          str[i] = ' ';
        }
        else if (c == '￥')
        {
          str[i] = '\\';
        }
        else if (c == '”' || c == '“')
        {
          str[i] = '"';
        }
        else if (c == '’' || c == '‘')
        {
          str[i] = '\'';
        }
      }
    }
    public static string ToHankaku(string str)
    {
      var sb = new StringBuilder(str);
      ToHankaku(sb);
      return sb.ToString();
    }

    /// <summary>
    /// 半角英数字および記号を全角に変換します。
    /// </summary>
    /// <param name="str">変換する StringBuiler。</param>
    /// <remarks>
    /// Kana.ToZenkakuメソッドと違い、「\"'」を「￥”’」に変換します。
    /// </remarks>
    public static void ToZenkaku(StringBuilder str)
    {
      int f = str.Length;

      for (int i = 0; i < f; i++)
      {
        char c = str[i];
        if (c == '\\')
        {
          str[i] = '￥';
        }
        else if (c == '"')
        {
          str[i] = '”';
        }
        else if (c == '\'')
        {
          str[i] = '’';
        }
        // !(0x0021) ～ ~(0x007E)
        else if ('!' <= c && c <= '~')
        {
          str[i] = (char)(c + 0xFEE0);
        }
        // 半角スペース(0x0020) -> 全角スペース(0x3000)
        else if (c == ' ')
        {
          str[i] = '　';
        }
      }
    }
    public static string ToZenkaku(string str)
    {
      var sb = new StringBuilder(str);
      ToZenkaku(sb);
      return sb.ToString();
    }

    /// <summary>
    /// 全角カタカナを半角カタカナに変換します。
    /// </summary>
    /// <param name="str">変換する StringBuiler。</param>
    /// <remarks>
    /// Kana.ToHankakuKanaメソッドと違い、
    /// 『、。「」・゛゜U+3099（濁点）U+309A（半濁点）ヴヷヺ』も変換します。
    /// </remarks>
    public static void ToHankakuKana(StringBuilder str)
    {
      int len = 0;
      int f = str.Length;

      str.EnsureCapacity(f * 2);

      for (int i = 0; i < f; i++)
      {
        char c = str[i];
        // 、(0x3001) ～ ー(0x30FC)
        if ('、' <= c && c <= 'ー')
        {
          char m = ConvertToHankakuKanaChar(c);
          if (m != '\0')
          {
            str[len++] = m;
          }
          // カ(0x30AB) ～ ド(0x30C9)
          else if ('カ' <= c && c <= 'ド')
          {
            str[len++] = ConvertToHankakuKanaChar((char)(c - 1));
            str.Insert(len++, 'ﾞ');
          }
          // ハ(0x30CF) ～ ポ(0x30DD)
          else if ('ハ' <= c && c <= 'ポ')
          {
            int mod3 = c % 3;
            str[len++] = ConvertToHankakuKanaChar((char)(c - mod3));
            str.Insert(len++, (mod3 == 1 ? 'ﾞ' : 'ﾟ'));
          }
          // ヴ(0x30F4)
          else if (c == 'ヴ')
          {
            str[len++] = 'ｳ';
            str.Insert(len++, 'ﾞ');
          }
          // ヷ(0x30F7)
          else if (c == 'ヷ')
          {
            str[len++] = 'ﾜ';
            str.Insert(len++, 'ﾞ');
          }
          // ヺ(0x30FA)
          else if (c == 'ヺ')
          {
            str[len++] = 'ｦ';
            str.Insert(len++, 'ﾞ');
          }
          else
          {
            str[len++] = c;
          }
        }
        else
        {
          str[len++] = c;
        }
      }
    }
    public static string ToHankakuKana(string str)
    {
      var sb = new StringBuilder(str);
      ToHankakuKana(sb);
      return sb.ToString();
    }

    /// <summary>
    /// 半角カタカナを全角カタカナに変換します。
    /// </summary>
    /// <param name="str">変換する StringBuiler。</param>
    /// <remarks>
    /// Kana.ToZenkakuKanaと違い、「｡｢｣､･」も変換します。
    /// また、濁点、半濁点がその前の文字と合体できる時は合体させて1文字にします。
    /// </remarks>
    public static void ToZenkakuKana(StringBuilder str)
    {
      int len = str.Length;

      for (int i = len - 1; 0 <= i; i--)
      {
        char c = str[i];

        // 濁点(0xFF9E)
        if (c == 'ﾞ' && 0 < i)
        {
          char c2 = str[i - 1];
          // ｶ(0xFF76) ～ ﾁ(0xFF81)
          if ('ｶ' <= c2 && c2 <= 'ﾁ')
          {
            str.Remove(i--, 1)[i] = (char)((c2 - 0xFF76) * 2 + 0x30AC);
          }
          // ﾂ(0xFF82) ～ ﾄ(0xFF84)
          else if ('ﾂ' <= c2 && c2 <= 'ﾄ')
          {
            str.Remove(i--, 1)[i] = (char)((c2 - 0xFF82) * 2 + 0x30C5);
          }
          // ﾊ(0xFF8A) ～ ﾎ(0xFF8E)
          else if ('ﾊ' <= c2 && c2 <= 'ﾎ')
          {
            str.Remove(i--, 1)[i] = (char)((c2 - 0xFF8A) * 3 + 0x30D0);
          }
          // ｳ(0xFF73)
          else if (c2 == 'ｳ')
          {
            str.Remove(i--, 1)[i] = 'ヴ';
          }
          // ﾜ(0xFF9C)
          else if (c2 == 'ﾜ')
          {
            str.Remove(i--, 1)[i] = 'ヷ';
          }
          // ｦ(0xFF66)
          else if (c2 == 'ｦ')
          {
            str.Remove(i--, 1)[i] = 'ヺ';
          }
          // 全角濁点
          else
          {
            str[i] = '゛';
          }
        }
        // 半濁点(0xFF9F)
        else if (c == 'ﾟ' && 0 < i)
        {
          char c2 = str[i - 1];
          // ﾊ(0xFF8A) ～ ﾎ(0xFF8E)
          if ('ﾊ' <= c2 && c2 <= 'ﾎ')
          {
            str.Remove(i--, 1)[i] = (char)((c2 - 0xFF8A) * 3 + 0x30D1);
          }
          // 全角半濁点
          else
          {
            str[i] = '゜';
          }
        }
        // ｡(0xFF61) ～ ﾟ(0xFF9F)
        else if ('｡' <= c && c <= 'ﾟ')
        {
          char m = ConvertToZenkakuKanaChar(c);
          if (m != '\0')
          {
            str[i] = m;
          }
          else
          {
            str[i] = c;
          }
        }
        else
        {
          str[i] = c;
        }
      }
    }
    public static string ToZenkakuKana(string str)
    {
      var sb = new StringBuilder(str);
      ToZenkakuKana(sb);
      return sb.ToString();
    }

    /// <summary>
    /// 「は゛」を「ば」のように、濁点や半濁点を前の文字と合わせて1つの文字に変換します。
    /// </summary>
    /// <param name="str">変換する StringBuiler。</param>
    /// <remarks>
    /// Kana.ToPaddingと違い、「ゔゞヴヷヸヹヺヾ」への変換も行います。
    /// また、U+3099（濁点）とU+309A（半濁点）も前の文字と合体させて1文字にします。
    /// </remarks>
    public static void ToPadding(StringBuilder str)
    {
      int f = str.Length - 1;

      for (int i = f; 0 <= i; i--)
      {
        char c = str[i];

        // 濁点
        if ((c == '゛' || c == '\u3099') && 0 < i)
        {
          char c2 = str[i - 1];
          int mod2 = c2 % 2;
          int mod3 = c2 % 3;

          // か(0x304B) ～ ち(0x3061)
          // カ(0x30AB) ～ チ(0x30C1)
          // つ(0x3064) ～ と(0x3068)
          // ツ(0x30C4) ～ ト(0x30C8)
          // は(0x306F) ～ ほ(0x307B)
          // ハ(0x30CF) ～ ホ(0x30DB)
          // ゝ(0x309D) ヽ(0x30FD)
          if (('か' <= c2 && c2 <= 'ち' && mod2 == 1) ||
              ('カ' <= c2 && c2 <= 'チ' && mod2 == 1) ||
              ('つ' <= c2 && c2 <= 'と' && mod2 == 0) ||
              ('ツ' <= c2 && c2 <= 'ト' && mod2 == 0) ||
              ('は' <= c2 && c2 <= 'ほ' && mod3 == 0) ||
              ('ハ' <= c2 && c2 <= 'ホ' && mod3 == 0) ||
              c2 == 'ゝ' || c2 == 'ヽ')
          {
            str.Remove(i--, 1)[i] = (char)(c2 + 1);
          }
          // う(0x3046) ウ(0x30A6) -> ゔヴ
          else if (c2 == 'う' || c2 == 'ウ')
          {
            str.Remove(i--, 1)[i] = (char)(c2 + 0x004E);
          }
          // ワ(0x30EF)ヰヱヲ(0x30F2) -> ヷヸヹヺ
          else if ('ワ' <= c2 && c2 <= 'ヲ')
          {
            str.Remove(i--, 1)[i] = (char)(c2 + 8);
          }
          else
          {
            str[i] = c;
          }
        }
        // ゜(0x309C)
        else if ((c == '゜' || c == '\u309A') && 0 < i)
        {
          char c2 = str[i - 1];
          int mod3 = c2 % 3;

          // は(0x306F) ～ ほ(0x307B)
          // ハ(0x30CF) ～ ホ(0x30DB)
          if (('は' <= c2 && c2 <= 'ほ' && mod3 == 0) ||
              ('ハ' <= c2 && c2 <= 'ホ' && mod3 == 0))
          {
            str.Remove(i--, 1)[i] = (char)(c2 + 2);
          }
          else
          {
            str[i] = c;
          }
        }
        else
        {
          str[i] = c;
        }
      }
    }
    public static string ToPadding(string str)
    {
      var sb = new StringBuilder(str);
      ToPadding(sb);
      return sb.ToString();
    }

    private static char ConvertToHankakuKanaChar(char zenkakuChar)
    {
      switch (zenkakuChar)
      {
        case 'ァ':
          return 'ｧ';
        case 'ィ':
          return 'ｨ';
        case 'ゥ':
          return 'ｩ';
        case 'ェ':
          return 'ｪ';
        case 'ォ':
          return 'ｫ';
        case 'ー':
          return 'ｰ';
        case 'ア':
          return 'ｱ';
        case 'イ':
          return 'ｲ';
        case 'ウ':
          return 'ｳ';
        case 'エ':
          return 'ｴ';
        case 'オ':
          return 'ｵ';
        case 'カ':
          return 'ｶ';
        case 'キ':
          return 'ｷ';
        case 'ク':
          return 'ｸ';
        case 'ケ':
          return 'ｹ';
        case 'コ':
          return 'ｺ';
        case 'サ':
          return 'ｻ';
        case 'シ':
          return 'ｼ';
        case 'ス':
          return 'ｽ';
        case 'セ':
          return 'ｾ';
        case 'ソ':
          return 'ｿ';
        case 'タ':
          return 'ﾀ';
        case 'チ':
          return 'ﾁ';
        case 'ツ':
          return 'ﾂ';
        case 'テ':
          return 'ﾃ';
        case 'ト':
          return 'ﾄ';
        case 'ナ':
          return 'ﾅ';
        case 'ニ':
          return 'ﾆ';
        case 'ヌ':
          return 'ﾇ';
        case 'ネ':
          return 'ﾈ';
        case 'ノ':
          return 'ﾉ';
        case 'ハ':
          return 'ﾊ';
        case 'ヒ':
          return 'ﾋ';
        case 'フ':
          return 'ﾌ';
        case 'ヘ':
          return 'ﾍ';
        case 'ホ':
          return 'ﾎ';
        case 'マ':
          return 'ﾏ';
        case 'ミ':
          return 'ﾐ';
        case 'ム':
          return 'ﾑ';
        case 'メ':
          return 'ﾒ';
        case 'モ':
          return 'ﾓ';
        case 'ヤ':
          return 'ﾔ';
        case 'ユ':
          return 'ﾕ';
        case 'ヨ':
          return 'ﾖ';
        case 'ラ':
          return 'ﾗ';
        case 'リ':
          return 'ﾘ';
        case 'ル':
          return 'ﾙ';
        case 'レ':
          return 'ﾚ';
        case 'ロ':
          return 'ﾛ';
        case 'ワ':
          return 'ﾜ';
        case 'ヲ':
          return 'ｦ';
        case 'ン':
          return 'ﾝ';
        case 'ッ':
          return 'ｯ';

        //ャュョ を追加
        case 'ャ':
          return 'ｬ';
        case 'ュ':
          return 'ｭ';
        case 'ョ':
          return 'ｮ';

        // 、。「」・ を追加
        case '、':
          return '､';
        case '。':
          return '｡';
        case '「':
          return '｢';
        case '」':
          return '｣';
        case '・':
          return '･';

        //゛゜ を追加
        case '゛':
          return 'ﾞ';
        case '゜':
          return 'ﾟ';

        //U+3099とU+309Aの濁点と半濁点を追加
        case '\u3099':
          return 'ﾞ';
        case '\u309A':
          return 'ﾟ';

        default:
          return '\0';
      }
    }

    private static char ConvertToZenkakuKanaChar(char hankakuChar)
    {
      switch (hankakuChar)
      {
        case 'ｦ':
          return 'ヲ';
        case 'ｧ':
          return 'ァ';
        case 'ｨ':
          return 'ィ';
        case 'ｩ':
          return 'ゥ';
        case 'ｪ':
          return 'ェ';
        case 'ｫ':
          return 'ォ';
        case 'ｰ':
          return 'ー';
        case 'ｱ':
          return 'ア';
        case 'ｲ':
          return 'イ';
        case 'ｳ':
          return 'ウ';
        case 'ｴ':
          return 'エ';
        case 'ｵ':
          return 'オ';
        case 'ｶ':
          return 'カ';
        case 'ｷ':
          return 'キ';
        case 'ｸ':
          return 'ク';
        case 'ｹ':
          return 'ケ';
        case 'ｺ':
          return 'コ';
        case 'ｻ':
          return 'サ';
        case 'ｼ':
          return 'シ';
        case 'ｽ':
          return 'ス';
        case 'ｾ':
          return 'セ';
        case 'ｿ':
          return 'ソ';
        case 'ﾀ':
          return 'タ';
        case 'ﾁ':
          return 'チ';
        case 'ﾂ':
          return 'ツ';
        case 'ﾃ':
          return 'テ';
        case 'ﾄ':
          return 'ト';
        case 'ﾅ':
          return 'ナ';
        case 'ﾆ':
          return 'ニ';
        case 'ﾇ':
          return 'ヌ';
        case 'ﾈ':
          return 'ネ';
        case 'ﾉ':
          return 'ノ';
        case 'ﾊ':
          return 'ハ';
        case 'ﾋ':
          return 'ヒ';
        case 'ﾌ':
          return 'フ';
        case 'ﾍ':
          return 'ヘ';
        case 'ﾎ':
          return 'ホ';
        case 'ﾏ':
          return 'マ';
        case 'ﾐ':
          return 'ミ';
        case 'ﾑ':
          return 'ム';
        case 'ﾒ':
          return 'メ';
        case 'ﾓ':
          return 'モ';
        case 'ﾔ':
          return 'ヤ';
        case 'ﾕ':
          return 'ユ';
        case 'ﾖ':
          return 'ヨ';
        case 'ﾗ':
          return 'ラ';
        case 'ﾘ':
          return 'リ';
        case 'ﾙ':
          return 'ル';
        case 'ﾚ':
          return 'レ';
        case 'ﾛ':
          return 'ロ';
        case 'ﾜ':
          return 'ワ';
        case 'ﾝ':
          return 'ン';
        case 'ﾞ':
          return '゛';
        case 'ﾟ':
          return '゜';

        // ｬｭｮｯ を追加
        case 'ｬ':
          return 'ャ';
        case 'ｭ':
          return 'ュ';
        case 'ｮ':
          return 'ョ';
        case 'ｯ':
          return 'ッ';

        // ｡｢｣､･ を追加
        case '｡':
          return '。';
        case '｢':
          return '「';
        case '｣':
          return '」';
        case '､':
          return '、';
        case '･':
          return '・';

        default:
          return '\0';
      }
    }
  }
}
