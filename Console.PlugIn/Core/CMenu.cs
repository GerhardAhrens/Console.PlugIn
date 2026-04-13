//-----------------------------------------------------------------------
// <copyright file="ConsoleMenu.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>07.05.2025 14:27:39</date>
//
// <summary>
// Klasse zum Erstellen und Auswählen von Menüpunkten
// </summary>
//-----------------------------------------------------------------------

namespace Console.PlugIn
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CMenu
    {
        private readonly List<MenuItem> _items = new();
        private int _selectedIndex = 0;
        private readonly string _title;
        private CMenu _parent;

        private const string ShortcutChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public CMenu(string title, CMenu parent = null)
        {
            this._title = title;
            this._parent = parent;

            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
        }

        public void AddItem(string text, Action action, string icon = "")
        {
            char shortcut = GenerateShortcut();
            this._items.Add(new MenuItem(text, action, icon, shortcut));
        }

        public void AddSubMenu(string text, CMenu submenu, string icon = "")
        {
            char shortcut = GenerateShortcut();

            submenu.SetParent(this);

            this._items.Add(new MenuItem(text, () => submenu.Show(), icon, shortcut));
        }

        private char GenerateShortcut()
        {
            if (this._items.Count >= ShortcutChars.Length)
            {
                throw new InvalidOperationException("Maximale Anzahl von Menüeinträgen erreicht.");
            }

            return ShortcutChars[this._items.Count];
        }

        private void SetParent(CMenu parent)
        {
            this._parent = parent;
        }

        public void Show()
        {
            ConsoleKey key;

            do
            {
                this.Draw();
                var keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;

                if (key == ConsoleKey.UpArrow)
                {
                    this._selectedIndex = (this._selectedIndex - 1 + this._items.Count) % this._items.Count;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    this._selectedIndex = (this._selectedIndex + 1) % this._items.Count;
                }
                else if (key == ConsoleKey.Enter)
                {
                    ExecuteSelected();
                }
                else if (key == ConsoleKey.Backspace && this._parent != null)
                {
                    return;
                }
                else
                {
                    char input = char.ToUpper(keyInfo.KeyChar);

                    for (int i = 0; i < this._items.Count; i++)
                    {
                        if (this._items[i].Shortcut == input)
                        {
                            this._selectedIndex = i;
                            this.ExecuteSelected();
                            break;
                        }
                    }
                }

            } while (true);
        }

        private void Draw()
        {
            Console.Clear();
            Console.WriteLine($"=== {this._title} ===\n");

            for (int i = 0; i < _items.Count; i++)
            {
                var item = this._items[i];

                if (i == this._selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{(i == this._selectedIndex ? ">" : " ")} {item.Shortcut}) {item.Icon}\u00A0 {item.Text}");
                Console.ResetColor();
            }

            if (this._parent != null)
            {
                Console.WriteLine("\n[Backspace] ← Zurück");
            }
        }

        private void ExecuteSelected()
        {
            Console.Clear();
            this._items[this._selectedIndex].Action.Invoke();
        }

        private class MenuItem
        {
            public string Text { get; }
            public Action Action { get; }
            public string Icon { get; }
            public char Shortcut { get; }

            public MenuItem(string text, Action action, string icon, char shortcut)
            {
                this.Text = text;
                this.Action = action;
                this.Icon = icon;
                this.Shortcut = shortcut;
            }
        }

        public static void Wait(string text = "")
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.CursorVisible = false;
            if (string.IsNullOrEmpty(text) == true)
            {
                Console.Write('\n');
                Console.WriteLine("Eine Taste drücken, um zum Menü zurück zukehren!");
            }
            else
            {
                Console.Write('\n');
                Console.WriteLine($"Eine Taste drücken, um zum Menü zurück zukehren! {text}");
            }

            Console.ForegroundColor = defaultColor;
            Console.ReadKey();
            Console.CursorVisible = true;
        }

        public static void Print(string text, ConsoleColor setColor = ConsoleColor.White)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = setColor;
            Console.CursorVisible = false;

            Console.WriteLine(text);

            Console.ForegroundColor = defaultColor;
            Console.CursorVisible = true;
        }
    }
}
