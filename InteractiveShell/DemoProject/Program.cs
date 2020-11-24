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

            menu.FallbackDisplayOptions = displayOptions;
            menu.FallbackTitleDisplayOptions = titleDisplayOptions;

            //
            // Single choice menu
            //

            // Draw the title, clearing the screen beforehand
            menu.DrawTitle(title: "Sample Menu", subtitle: "Select a single option:", "Content test", displayOptions: null, clearScreen: true);

            // Present the user with the interactive menu
            int result = menu.DisplayMenu(options: new string[] { "Option 1", "Option 2" }, displayOptions: null);
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
            menu.DrawTitle(title: "Sample Menu", subtitle: "Select multiple options with Control + Enter:", displayOptions: null, clearScreen: true);

            // Present the user with the interactive menu
            int[] results = menu.DisplayMultiMenu(options: new string[] { "Option 1", "Option 2", "Option 3" }, displayOptions: null);

            menu.DrawContentText("\nSelected indexes:", false);
            foreach (var index in results)
            {
                menu.DrawContentText(index.ToString(), false);
            }

            menu.Reset();
            Console.ReadKey();
        }
    }
}
