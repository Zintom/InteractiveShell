## InteractiveShell
Provides an easy mechanism for displaying interactive menus in the CLI.

[![NuGet](https://img.shields.io/nuget/v/InteractiveShell?color=%2327ae60)](https://www.nuget.org/packages/InteractiveShell)
[![License](https://img.shields.io/github/license/Zintom/InteractiveShell)](https://github.com/Zintom/InteractiveShell/blob/master/LICENSE.txt)

### Installation
NuGet `PM> Install-Package InteractiveShell`

### Example
Here's how to setup a basic menu (see [DemoProject](https://github.com/Zintom/InteractiveShell/tree/master/InteractiveShell/DemoProject/Program.cs) for more)
```
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

// We will use these fallbacks, notice how we specify `null` for the displayOptions parameters,
// these can be omitted as they are optional parameters but have been explicitly set to null to make the intent clearer.
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

// Print out the selected indexes
menu.DrawContentText("\nSelected indexes:", false);
foreach (var index in results)
{
    menu.DrawContentText(index.ToString(), false);
}

menu.Reset();
Console.ReadKey();
```
