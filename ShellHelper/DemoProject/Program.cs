using Zintom.ShellHelper;

namespace DemoProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ShellMenu menu = new ShellMenu();

            ShellDisplayOptions displayOptions = new ShellDisplayOptions()
            {
                LeftOffset = 2,
                DisplayHorizontally = false
            };

            ShellTitleDisplayOptions titleDisplayOptions = new ShellTitleDisplayOptions()
            {
                LeftOffset = 2
            };

            while (true)
            {
                menu.DrawTitle("Title", "Line1\nLine2\nLine3", "Content content content", titleDisplayOptions, true);
                int menuResult = menu.DisplayMenu(new string[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" }, displayOptions);
            }
        }
    }
}
