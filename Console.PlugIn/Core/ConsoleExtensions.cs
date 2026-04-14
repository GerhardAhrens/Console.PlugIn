//-----------------------------------------------------------------------
// <copyright file="ConsoleExtensions.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2026
// </copyright>
// <Template>
// 	Version 3.0.2026.1, 08.1.2026
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.03.2026 14:26:39</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    internal static class ConsoleExtensions
    {
        // Erstelle Extension für den Typ String
        extension(Console)
        {
            public static ConsoleKeyInfo Wait(string text = "")
            {
                ConsoleKeyInfo result;
                ConsoleColor defaultColor = Console.ForegroundColor;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.CursorVisible = false;
                if (string.IsNullOrEmpty(text) == true)
                {
                    Console.Write('\n');
                    Console.WriteLine("Eine Taste drücken, um zum Menü zurück zukehren!");
                }
                else
                {
                    Console.Write('\n');
                    Console.WriteLine($"{text}");
                }

                Console.ForegroundColor = defaultColor;
                result = Console.ReadKey();
                Console.CursorVisible = true;

                return result;
            }

            public static void WriteText(string text, ConsoleColor setColor = ConsoleColor.White)
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                bool defaultCursor = Console.CursorVisible;

                Console.ForegroundColor = setColor;
                Console.CursorVisible = false;

                Console.WriteLine(text);

                Console.ForegroundColor = defaultColor;
                Console.CursorVisible = defaultCursor;
            }

            public static void Line(char lineSymbol = '-', ConsoleColor setColor = ConsoleColor.White)
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                bool defaultCursor = Console.CursorVisible;

                Console.ForegroundColor = setColor;
                Console.CursorVisible = false;

                Console.WriteLine(new string(lineSymbol, Console.WindowWidth));

                Console.ForegroundColor = defaultColor;
                Console.CursorVisible = defaultCursor;
            }
        }
    }
}
