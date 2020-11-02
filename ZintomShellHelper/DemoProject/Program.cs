using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZintomShellHelper;

namespace DemoProject
{
    class Program
    {
        static void Main(string[] args)
        {

            int menuResult = MenuManager.CreateMenu(new string[] { "Option 1", "Option 2", "Option 3", "Option 4", "Option 5" }, false, 2);

        }
    }
}