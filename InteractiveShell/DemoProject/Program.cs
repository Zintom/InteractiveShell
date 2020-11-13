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
                LeftOffset = 2
            };
            ShellTitleDisplayOptions titleDisplayOptions = new ShellTitleDisplayOptions()
            {
                LeftOffset = 2
            };

            // Draw the title, clearing the screen beforehand
            menu.DrawTitle(title: "Sample Menu", subtitle: "Select an option:", displayOptions: titleDisplayOptions, clearScreen: true);

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
        }
    }
}
