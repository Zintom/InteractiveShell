# ShellHelper
Provides an easy mechanism for displaying interactive menus in the CLI.

### Installation
Nuget `PM> Install-Package InteractiveShell`

## Example
Here's how to setup a basic menu (see [DemoProject](https://github.com/Zintom/ShellHelper/tree/master/ShellHelper/DemoProject) for more)
```
// Create a new instance of the ShellMenu class.
ShellMenu menu = new ShellMenu();

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
menu.DrawTitle(title: "Sample Menu", subtitle: "Select an option:", displayOptions: titleDisplayOptions, clear: true);

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
