using System;

namespace ZintomShellHelper
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("Please select an option:");

            string[] options = new string[] { " Login ", " Sign-up ", " Cancel " };
            int selection = CreateMenu(options);

            Console.WriteLine(string.Format("You selected {0}", options[selection]));

            Console.ReadLine();
        }

        /// <summary>
        /// Creates an option menu in text form. Returns the selected option.
        /// </summary>
        /// <returns>Selected item.</returns>
        static int CreateMenu(string[] options)
        {
            bool DrawnOnce = false;
            int selectedOption = 0;

            DrawMenu(options, selectedOption, ref DrawnOnce);

            while (true)
            {
                while (!Console.KeyAvailable)
                {
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    return selectedOption;
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (selectedOption > 0)
                        selectedOption -= 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (selectedOption < options.Length - 1)
                        selectedOption += 1;
                }

                DrawMenu(options, selectedOption, ref DrawnOnce);
            }
        }

        static void DrawMenu(string[] options, int selected, ref bool DrawnOnce)
        {
            if (DrawnOnce)
                Console.CursorTop -= options.Length;

            for (int o = 0; o < options.Length; o++)
            {
                if (o == selected)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine(options[o]);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(options[o]);
                }
            }

            Console.TreatControlCAsInput = true;
            Console.CursorVisible = false;

            DrawnOnce = true;
        }
    }
}