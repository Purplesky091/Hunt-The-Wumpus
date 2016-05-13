using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunt_the_Wumpus_Text_based
{
    static class WarningPrinter
    {
        private static StringBuilder warnings = new StringBuilder(4);

        public static void Add(string message)
        {
            warnings.Append(message);
            warnings.Append("\n");
        }

        public static bool HasWarnings()
        {
            if (warnings.Length == 0)
                return false;
            else
                return true;
        }

        public static void printMessages()
        {
            Console.WriteLine();
            Console.WriteLine(warnings.ToString());
            Console.WriteLine();

            warnings.Clear();
        }
    }
}
