using System;
using System.Text.RegularExpressions;

namespace Zintom.ShellHelper
{

    /// <summary>
    /// Provides theming options for displaying menus etc.
    /// </summary>
    public class BaseDisplayOptions
    {
        /// <summary>
        /// The whitespace padding to add to the left of drawn text.
        /// </summary>
        public int LeftOffset { get; set; }
    }

    /// <inheritdoc cref="BaseDisplayOptions"/>
    public class ShellDisplayOptions : BaseDisplayOptions
    {
        /// <summary>
        /// Menu should display horizontally rather than the default which is vertical.
        /// </summary>
        public bool DisplayHorizontally { get; set; }
    }

    /// <inheritdoc cref="BaseDisplayOptions"/>
    public class ShellTitleDisplayOptions : BaseDisplayOptions
    {
        /// <summary>
        /// The newline padding applied underneath the text.
        /// </summary>
        public int TitleVerticalPadding { get; set; } = 1;

        /// <inheritdoc cref="TitleVerticalPadding"/>
        public int SubtitleVerticalPadding { get; set; } = 1;

        /// <inheritdoc cref="TitleVerticalPadding"/>
        public int ContentVerticalPadding { get; set; } = 1;
    }

    /// <summary>
    /// Provides an easy mechanism for displaying interactive menus in the CLI.
    /// </summary>
    public class InteractiveShell
    {

        private readonly ConsoleColor DefaultBackColor;
        private readonly ConsoleColor DefaultForeColor;

        private bool _menuDrawnOnce;

        /// <summary>
        /// Creates a new instance of the <see cref="InteractiveShell"/> class.
        /// </summary>
        /// <remarks>
        /// The <see cref="Console.BackgroundColor"/> and <see cref="Console.ForegroundColor"/> are
        /// retained by the object and can be restored at any time with a call to <see cref="ResetColours"/>.
        /// </remarks>
        public InteractiveShell()
        {
            DefaultBackColor = Console.BackgroundColor;
            DefaultForeColor = Console.ForegroundColor;
        }

        /// <inheritdoc cref="DisplayMenu(string[], ShellDisplayOptions)"/>
        public int DisplayMenu(string option, ShellDisplayOptions displayOptions)
        {
            return DisplayMenu(new string[] { option }, displayOptions);
        }

        /// <summary>
        /// Displays a menu to the user with the given choice of <paramref name="options"/> and
        /// themed as per the <paramref name="displayOptions"/>.
        /// </summary>
        /// <param name="options">A list of options that the user can select from.</param>
        /// <param name="displayOptions">The theming options for the menu.</param>
        /// <remarks>
        /// <b>Warning:</b> This will take over control of the console keyboard, to return control to the user (allowing them to input text etc), call <see cref="Reset"/>.
        /// </remarks>
        /// <returns>The selected item, or if <b>Esc</b> was pressed, will return <see langword="-1"/>.</returns>
        public int DisplayMenu(string[] options, ShellDisplayOptions displayOptions)
        {
            _menuDrawnOnce = false;
            Console.CursorVisible = false;
            int selectedOption = 0;

            while (true)
            {
                DrawMenu(options, selectedOption, displayOptions);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Reset();
                    return selectedOption;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Reset();
                    return -1;
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (selectedOption > 0)
                        selectedOption -= 1;
                    else
                        selectedOption = options.Length - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (selectedOption < options.Length - 1)
                        selectedOption += 1;
                    else
                        selectedOption = 0;
                }
            }
        }

        private void DrawMenu(string[] options, int selected, ShellDisplayOptions displayOptions)
        {
            if (_menuDrawnOnce && !displayOptions.DisplayHorizontally)
                Console.CursorTop -= options.Length;
            else if (_menuDrawnOnce && displayOptions.DisplayHorizontally)
                Console.CursorTop -= 1;

            if (displayOptions.DisplayHorizontally)
            {
                if (displayOptions.LeftOffset > 0)
                {
                    ResetColours();
                    DrawLeftOffset(displayOptions.LeftOffset);
                }

                for (int o = 0; o < options.Length; o++)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("<");

                    if (o == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.Write(options[o]);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("> ");
                }
                Console.WriteLine("");
            }
            else
            {
                for (int o = 0; o < options.Length; o++)
                {
                    if (displayOptions.LeftOffset > 0)
                    {
                        ResetColours();
                        DrawLeftOffset(displayOptions.LeftOffset);
                    }

                    if (o == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine(options[o]);
                }
            }

            ResetColours();

            _menuDrawnOnce = true;
        }

        /// <inheritdoc cref="DrawTitle(string, string?, string?, ShellTitleDisplayOptions, bool)"/>
        public void DrawTitle(string title, ShellTitleDisplayOptions displayOptions, bool clearScreen = true) => DrawTitle(title, null, null, displayOptions, clearScreen);
        /// <inheritdoc cref="DrawTitle(string, string?, string?, ShellTitleDisplayOptions, bool)"/>
        public void DrawTitle(string title, string? subtitle, ShellTitleDisplayOptions displayOptions, bool clearScreen = true) => DrawTitle(title, subtitle, null, displayOptions, clearScreen);

        /// <summary>
        /// Draws a fancy title screen, with optionally included 'subtitle' and 'content' text; themed by the given <paramref name="displayOptions"/>
        /// </summary>
        /// <param name="title">The title text.</param>
        /// <param name="subtitle">The subtitle text.</param>
        /// <param name="content">The content text.</param>
        /// <param name="displayOptions">The theming options.</param>
        /// <param name="clearScreen">Clear the screen prior to drawing.</param>
        public void DrawTitle(string title, string? subtitle, string? content, ShellTitleDisplayOptions displayOptions, bool clearScreen = true)
        {
            if (clearScreen)
                Console.Clear();

            // Draw title text
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(ApplyLeftOffset("".PadRight(title.Length + 2, '='), displayOptions.LeftOffset));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" " + ApplyLeftOffset(title, displayOptions.LeftOffset));
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(ApplyLeftOffset("".PadRight(title.Length + 2, '='), displayOptions.LeftOffset));
            
            // Draw title padding
            Console.Write("".PadRight(displayOptions.TitleVerticalPadding, '\n'));
            
            if (subtitle != null)
            {
                // Draw subtitle text
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(ApplyLeftOffset(subtitle, displayOptions.LeftOffset));

                // Draw subtitle padding
                Console.Write("".PadRight(displayOptions.SubtitleVerticalPadding, '\n'));
            }
            if (content != null)
            {
                // Draw content text
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(ApplyLeftOffset(content, displayOptions.LeftOffset));

                // Draw content text padding
                Console.Write("".PadRight(displayOptions.ContentVerticalPadding, '\n'));
            }

            ResetColours();
        }

        /// <summary>
        /// Applies a line terminator aware left offset to the given <paramref name="input"/> string.
        /// </summary>
        private static string ApplyLeftOffset(string input, int leftOffset)
        {
            // Replace all instances of a newline
            // with a newline but with padding on the right
            // also padding the first line manually.
            return "".PadLeft(leftOffset) + Regex.Replace(input, "(\n|\r|\r\n)", Environment.NewLine + "".PadLeft(leftOffset));
        }

        /// <summary>
        /// Draws the left offset padding of whitespace.
        /// </summary>
        private static void DrawLeftOffset(int offset)
        {
            Console.Write("".PadLeft(offset));
        }

        /// <summary>
        /// Returns control back to the user, turns theming off and makes the cursor visible again.
        /// </summary>
        public void Reset()
        {
            Console.TreatControlCAsInput = false;
            Console.CursorVisible = true;

            ResetColours();
        }

        /// <summary>
        /// Sets the foreground and background colours to their default values.
        /// </summary>
        public void ResetColours()
        {
            Console.BackgroundColor = DefaultBackColor;
            Console.ForegroundColor = DefaultForeColor;
        }

    }
}