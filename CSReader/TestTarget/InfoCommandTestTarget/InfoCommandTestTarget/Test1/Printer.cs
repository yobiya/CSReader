using System;

namespace InfoCommandTestTarget.Test1
{
    public class Printer
    {
        public class Message
        {
            private readonly string _m1;
            private readonly string _m2;

            public Message(string m1, string m2)
            {
                _m1 = m1;
                _m2 = m2;
            }

            public string Get()
            {
                return _m1 + _m2;
            }
        }

        public virtual void Print(Message message)
        {
            PrintImpl(message);
        }

        public void PrintImpl(Message message)
        {
            Console.WriteLine(message.Get());
        }
    }
}
