## InteractiveShell
Provides an easy mechanism for displaying interactive menus in the CLI.

[![NuGet](https://img.shields.io/nuget/v/InteractiveShell?color=%2327ae60)](https://www.nuget.org/packages/InteractiveShell/2.0.1)
[![License](https://img.shields.io/github/license/Zintom/InteractiveShell)](https://github.com/Zintom/InteractiveShell/blob/master/LICENSE.txt)

### Installation
NuGet `PM> Install-Package InteractiveShell`

### Example
Here's how to setup a basic menu (see [DemoProject](https://github.com/Zintom/InteractiveShell/blob/master/ShellHelper/DemoProject/Program.cs) for more)
```
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
switch(result)
{
  case 0:
    // Do Option 1
    break;
  case 1:
    // Do Option 2
    break;
}
```
