using System;
using Zintom.ShellHelper;

namespace DemoProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ShellMenu.Init();
            ShellMenu.DrawTitle("Title", "Subtitle", "Content", true);
            int menuResult = ShellMenu.CreateMenu(new string[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" }, false, 2);
        }
    }
}
