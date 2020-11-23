using System;
using Zintom.InteractiveShell;

namespace DemoProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new instance of the ShellMenu class.
            InteractiveShell menu = new InteractiveShell();

            // Create the options for theming.
            ShellDisplayOptions displayOptions = new ShellDisplayOptions()
            {
                LeftOffset = 2,
                DisplayHorizontally = false
            };
            ShellTitleDisplayOptions titleDisplayOptions = new ShellTitleDisplayOptions()
            {
                LeftOffset = 2
            };

            //
            // Single choice menu
            //

            // Draw the title, clearing the screen beforehand
            menu.DrawTitle(title: "Sample Menu", subtitle: "Select a single option:", displayOptions: titleDisplayOptions, clearScreen: true);

            // Present the user with the interactive menu
            int result = menu.DisplayMenu(options: new string[] { "Option 1", "Option 2" }, displayOptions: displayOptions);
            switch (result)
            {
                case 0:
                    // Do Option 1
                    break;
                case 1:
                    // Do Option 2
                    break;
            }

            //
            // Multi-choice menu
            //

            // Draw the title, clearing the screen beforehand
            menu.DrawTitle(title: "Sample Menu", subtitle: "Select multiple options with Control + Enter:", displayOptions: titleDisplayOptions, clearScreen: true);

            // Present the user with the interactive menu
            int[] results = menu.DisplayMultiMenu(options: new string[] { "Option 1", "Option 2", "Option 3" }, displayOptions: displayOptions);

            Console.WriteLine("Selected indexes:");
            foreach (var index in results)
            {
                Console.WriteLine(index);
            }

            menu.Reset();
            Console.ReadKey();
        }
    }
}
