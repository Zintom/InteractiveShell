# ShellHelper
A set of methods to make console-based GUI as easy as possible.

## Basics
Creating a menu is as easy as calling MenuManager.CreateMenu(string[] options, ...).
You can specify whether you want the menu to display horizontally or vertically, and what space 'offset' you want each menu item to have.

A few helper methods are provided to make displaying menu titles/subtitles easier, DrawTitle(...) being the main method with optional parameters to fit your need.

### Installation
#### Option 1
In Visual Studio goto 'Project > Add Reference > Browse' and select the DLL you've downloaded from this repo.
#### Option 2
Compile the .cs file yourself and reference it like in Option 1

## Example
Here is how a basic menu would work.
```
while(true){
  MenuManager.DrawTitle("Sample Menu", "Select an option:", true);
  switch(MenuManager.CreateMenu(new string[]{ "Option1", "Option2"}, false, 0){
    case 0:
      // Do Option 1
      break;
    case 1:
      // Do Option 2
      break;
  }
}
```
