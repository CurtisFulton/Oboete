using System;

namespace MigrationProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Pyramid(75));

            Console.ReadKey();
        }

        private static string Pyramid(int n)
        {
            // Programming Challenge
            string result = "";
            int charPerRow = ((n - 1) * 2) + 1;
            for (int i = n - 1; i >= 0; i--) {
                string row = new String('#', charPerRow - (2 * i));
                row = row.PadLeft(charPerRow - i);
                row = row.PadRight(charPerRow);
                result += $"{row}\r\n";
            }
            return result;
        }
    }
}
