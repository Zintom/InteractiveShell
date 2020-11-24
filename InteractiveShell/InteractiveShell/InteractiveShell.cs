using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Zintom.InteractiveShell
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
        /// The fallback theme for when a theme is not provided to one of the 'DisplayX' methods.
        /// </summary>
        public ShellDisplayOptions FallbackDisplayOptions { get; set; }

        /// <summary>
        /// The fallback theme for when a theme is not provided to one of the 'DrawX' methods.
        /// </summary>
        public ShellTitleDisplayOptions FallbackTitleDisplayOptions { get; set; }

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

            FallbackDisplayOptions = new ShellDisplayOptions();
            FallbackTitleDisplayOptions = new ShellTitleDisplayOptions();
        }

        /// <inheritdoc cref="DisplayMenu(string[], ShellDisplayOptions)"/>
        public int DisplayMenu(string option, ShellDisplayOptions? displayOptions = null)
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
        public int DisplayMenu(string[] options, ShellDisplayOptions? displayOptions = null)
        {
            int[] result = DisplayMultiMenu(options, displayOptions, 1, true);

            if (result.Length == 0) return -1;
            else return result[0];
        }

        /// <summary>
        /// Displays a multiple-choice menu to the user with the given <paramref name="options"/> to select from and
        /// themed as per the <paramref name="displayOptions"/>.
        /// <para>
        /// Select multiple options with <b>Control</b> + <b>Enter</b>.
        /// </para>
        /// </summary>
        /// <param name="options">A list of options that the user can select from.</param>
        /// <param name="displayOptions">The theming options for the menu.</param>
        /// <param name="maxSelectableItems">Defines the maximum amount of items the user will be able to multi-select.</param>
        /// <param name="singleResult">If <see langword="true"/>, removes the multi-select functionality, pressing enter on an option will both select it and return it.</param>
        /// <remarks>
        /// <b>Warning:</b> This will take over control of the console keyboard, to return control to the user (allowing them to input text etc), call <see cref="Reset"/>.
        /// </remarks>
        /// <returns>The selected items, or if <b>Esc</b> was pressed, will return <see cref="Array.Empty{int}"/>.</returns>
        public int[] DisplayMultiMenu(string[] options, ShellDisplayOptions? displayOptions = null, int maxSelectableItems = int.MaxValue, bool singleResult = false)
        {
            _menuDrawnOnce = false;
            Console.CursorVisible = false;
            List<int> selectedOptions = new List<int>();
            int currentOptionPosition = 0;

            while (true)
            {
                DrawMenu(options, selectedOptions, currentOptionPosition, displayOptions);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (singleResult)
                    {
                        //Reset();
                        return new int[] { currentOptionPosition };
                    }

                    if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                    {
                        // If the option is already selected then deselect it.
                        if (selectedOptions.Contains(currentOptionPosition))
                        {
                            selectedOptions.Remove(currentOptionPosition);
                        }
                        // If the option is not already selected and
                        // we are below the max selectable amount of items, then add it.
                        else if (selectedOptions.Count < maxSelectableItems)
                        {
                            selectedOptions.Add(currentOptionPosition);
                        }
                    }
                    else
                    {
                        //Reset();
                        
                        if (!selectedOptions.Contains(currentOptionPosition))
                            selectedOptions.Add(currentOptionPosition);

                        // Convert the list to an array and sort it in numerical order.
                        int[] output = selectedOptions.ToArray();
                        Array.Sort(output);

                        return output;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    //Reset();
                    return Array.Empty<int>();
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (currentOptionPosition > 0)
                        currentOptionPosition -= 1;
                    else
                        currentOptionPosition = options.Length - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (currentOptionPosition < options.Length - 1)
                        currentOptionPosition += 1;
                    else
                        currentOptionPosition = 0;
                }
            }
        }

        private void DrawMenu(string[] options, List<int> selectedOptions, int currentPosition, ShellDisplayOptions? displayOptions)
        {
            if (displayOptions == null)
                displayOptions = FallbackDisplayOptions;

            if (_menuDrawnOnce)
            {
                if (displayOptions.DisplayHorizontally)
                    Console.CursorTop -= 1;
                else
                    Console.CursorTop -= options.Length;
            }

            Console.CursorLeft = displayOptions.LeftOffset;

            for (int o = 0; o < options.Length; o++)
            {
                if (!displayOptions.DisplayHorizontally)
                    Console.CursorLeft = displayOptions.LeftOffset;

                if (displayOptions.DisplayHorizontally)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("<");
                }

                // Set correct display colour for the given option.
                if (o == currentPosition)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else if (selectedOptions.Contains(o))
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (displayOptions.DisplayHorizontally)
                {
                    Console.Write(options[o]);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("> ");

                    // If we are the last item, then add a newline.
                    if (o == options.Length - 1)
                        Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(options[o]);
                }
            }

            ResetColours();

            _menuDrawnOnce = true;
        }

        /// <inheritdoc cref="DrawTitle(string, string?, string?, ShellTitleDisplayOptions, bool)"/>
        public void DrawTitle(string title, ShellTitleDisplayOptions? displayOptions = null, bool clearScreen = true) => DrawTitle(title, null, null, displayOptions, clearScreen);
        /// <inheritdoc cref="DrawTitle(string, string?, string?, ShellTitleDisplayOptions, bool)"/>
        public void DrawTitle(string title, string? subtitle, ShellTitleDisplayOptions? displayOptions = null, bool clearScreen = true) => DrawTitle(title, subtitle, null, displayOptions, clearScreen);

        /// <summary>
        /// Draws a fancy title screen, with optionally included <paramref name="subtitle"/> and <paramref name="content"/> text; themed by the given <paramref name="displayOptions"/>,
        /// if <paramref name="displayOptions"/> is <see langword="null"/> the <see cref="FallbackTitleDisplayOptions"/> is used instead.
        /// </summary>
        /// <param name="title">The title text.</param>
        /// <param name="subtitle">The subtitle text.</param>
        /// <param name="content">The content text.</param>
        /// <param name="displayOptions">The theming options.</param>
        /// <param name="clearScreen">Clear the screen prior to drawing.</param>
        public void DrawTitle(string title, string? subtitle, string? content, ShellTitleDisplayOptions? displayOptions = null, bool clearScreen = true)
        {
            if (clearScreen)
                Console.Clear();

            if (displayOptions == null)
                displayOptions = FallbackTitleDisplayOptions;

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
                DrawSubtitleText(subtitle, true, displayOptions);
            }
            if (content != null)
            {
                DrawContentText(content, true, displayOptions);
            }

            ResetColours();
        }

        /// <summary>
        /// Draws fancy text, conforming to the given <paramref name="displayOptions"/> theme,
        /// if the theme passed is null then the <see cref="FallbackTitleDisplayOptions"/> will be used.
        /// </summary>
        /// <param name="appendNewLine">Whether a '<see langword="\n"/>' character should be appended to the end of your given string.</param>
        public void DrawSubtitleText(string subtitle, bool appendNewLine = true, ShellTitleDisplayOptions? displayOptions = null)
        {
            DrawTextWithThemeing(subtitle, ConsoleColor.Yellow, appendNewLine, displayOptions);
        }

        /// <inheritdoc cref="DrawSubtitleText(string, bool, ShellTitleDisplayOptions?)"/>
        public void DrawContentText(string content, bool appendNewLine = true, ShellTitleDisplayOptions? displayOptions = null)
        {
            DrawTextWithThemeing(content, ConsoleColor.Gray, appendNewLine, displayOptions);
        }

        private void DrawTextWithThemeing(string text, ConsoleColor foreColour, bool appendNewLine = false, ShellTitleDisplayOptions? displayOptions = null)
        {
            if (displayOptions == null)
                displayOptions = FallbackTitleDisplayOptions;

            // Draw text
            Console.ForegroundColor = foreColour;
            Console.Write(ApplyLeftOffset(text, displayOptions.LeftOffset) + (appendNewLine ? "\n" : ""));

            // Draw text vertical padding
            Console.Write("".PadRight(displayOptions.ContentVerticalPadding, '\n'));

            // Reset the console colours back to what they were.
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
        [Obsolete("Use `ApplyLeftOffset` to apply a left offset.")]
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