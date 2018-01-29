using System;
using System.Text;

namespace EmptyEngine
{
    public static class Debugs
    {
        public static void Log(string msg)
        {
            string.Concat(msg, "\n", Environment.StackTrace);
            Console.WriteLine(msg);
        }

        public static void Log(params object[] objectList)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object obj in objectList)
            {
                builder.Append(obj.ToString());
            }
            Log(builder.ToString());
        }
    }
}
