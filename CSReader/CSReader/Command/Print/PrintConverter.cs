using System.Collections.Generic;
using System.Linq;

namespace CSReader.Command.Print
{
    public static class PrintConverter
    {
        // コンソールにオブジェクトを出力する
        public static string Convert(object info)
        {
            var valueList = new List<string>();

            var properties = info.GetType().GetProperties();
            foreach (var property in properties)
            {
                var attribute = (ValueAttribute)property.GetCustomAttributes(typeof(ValueAttribute), false).First();

                var text
                    = attribute.Name + System.Environment.NewLine
                    + "  " + property.GetValue(info).ToString() + System.Environment.NewLine;

                valueList.Add(text);
            }

            var fields = info.GetType().GetFields();
            foreach (var field in fields)
            {
                var attribute = (ValueAttribute)field.GetCustomAttributes(typeof(ValueAttribute), false).First();

                var text
                    = attribute.Name + System.Environment.NewLine
                    + "  " + field.GetValue(info).ToString() + System.Environment.NewLine;

                valueList.Add(text);
            }

            return valueList.Aggregate((text, value) => text + System.Environment.NewLine + value);
        }
    }
}
