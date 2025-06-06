using OmoSeitoku;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace OmoOmotegaki.Data
{
    public sealed class TermConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType == typeof(Term)) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            return (destinationType == typeof(string)) && (value is Term term)
                ? string.Concat(
                    term.Years.ToString(), ", ",
                    term.Months.ToString(), ", ",
                    term.Days.ToString(), ", ",
                    term.Hours.ToString(), ", ",
                    term.Minutes.ToString(), ", ",
                    term.Seconds.ToString())
                : base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
            CultureInfo culture, object value)
        {
            if (
                //!(context is null) &&
                //context.PropertyDescriptor.PropertyType == typeof(Term) &&
                (value is string strValue))
            {
                try
                {
                    // 正しい文字列であれば、item[0]=Left値,item[1]=Top値,
                    // item[2]=Right値,item[3]=Bottom値の文字列が入る。
                    string[] item = strValue.Split(',');

                    Term rs;

                    if (item.Length == 0)
                        rs = default;
                    else if (item.Length == 3)
                        rs = new Term(int.Parse(item[0]), int.Parse(item[1]), int.Parse(item[2]));
                    else if (item.Length == 6)
                        rs = new Term(int.Parse(item[0]), int.Parse(item[1]), int.Parse(item[2]),
                                      int.Parse(item[3]), int.Parse(item[4]), int.Parse(item[5]));
                    else
                        throw new Exception();

                    return rs;
                }
                catch
                {
                    throw new ArgumentException($"文字列 '{strValue}' を Term 型に変換できません");
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        // プロパティグリッドのプロパティの並びを定義順にソート
        public override PropertyDescriptorCollection GetProperties(
            ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            if (value is Term)
            {
                var cache = PropertyDescriptorCollectionCache;
                if (cache is null)
                {
                    // プロパティ一覧をリフレクションから取得
                    string[] names = typeof(Term).GetProperties()
                                        .Select(propertyInfo => propertyInfo.Name)
                                        .ToArray();
                    // TypeDescriptorを使用してプロパティ一覧を取得する
                    PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(value, attributes);
                    // リフレクションから取得した順でソート
                    cache = pdc.Sort(names);
                    PropertyDescriptorCollectionCache = cache;
                }
                return cache;
            }

            return TypeDescriptor.GetProperties(value, attributes);
        }
        private static PropertyDescriptorCollection PropertyDescriptorCollectionCache;

        /// <summary>
        /// GetPropertiesをサポートしていることを表明する。
        /// </summary>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
