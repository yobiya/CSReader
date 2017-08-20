using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSReader.Command.Print
{
    public static class PrintConverter
    {
        // コンソールにオブジェクトを出力する
        public static string Convert(object info)
        {
            var valueList = new List<string>();

            var properties = info.GetType().GetProperties();
            var propertyTexts = properties.Select(p => ConvertValue(p, () => p.GetValue(info))).Where(v => v != null);
            valueList.AddRange(propertyTexts);

            var fields = info.GetType().GetFields();
            var fieldTexts = fields.Select(f => ConvertValue(f, () => f.GetValue(info))).Where(v => v != null);
            valueList.AddRange(fieldTexts);

            return valueList.Aggregate((text, value) => text + Environment.NewLine + Environment.NewLine + value);
        }

        private static string ConvertValue(MemberInfo memberInfo, Func<object> getValue)
        {
            var attribute = (ValueAttribute)memberInfo.GetCustomAttributes(typeof(ValueAttribute), false).First();

            var value = getValue();
            var array = value as string[];

            if (array.IsNullOrEmpty())
            {
                return attribute.Name + Environment.NewLine + "  " + value.ToString();
            }
            else
            {
                return
                    attribute.Name
                    + Environment.NewLine
                    + "  "
                    + array.Aggregate((a, b) => a + Environment.NewLine + "  " + b);
            }
        }
    }
}
