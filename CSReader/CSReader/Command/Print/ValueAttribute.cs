using System;

namespace CSReader.Command.Print
{
    /// <summary>
    /// 値をコンソールに表示するための属性
    /// </summary>
    public class ValueAttribute : Attribute
    {
        public string Name { get; }

        public ValueAttribute(string name)
        {
            Name = name;
        }
    }
}
