using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    class TextBox
    {
        int x = 0;
        int y = 0;
        int width = 0;
        int InnerWidth
        {
            get
            {
                return width - 1;
            }
        }
        string text = "";
        int position = 0;

        public string Text{
            set
            {
                if (value.Length > width - 1)
                {
                    throw new IndexOutOfRangeException("Text is too long for textbox");
                }
                else
                {
                    Clear();
                    Console.CursorTop = y;
                    Console.CursorLeft = x;
                    text = value;
                    Console.Write(text);
                    position = text.Length;                 
                }
                
            }
            get
            {
                return text;
            }
        }

        public TextBox(int x, int y, int width)
        {
            this.x = x + 1;
            this.y = y + 1;
            this.width = width - 2;
            string horizontalEdge = new string('─', width);

            Console.CursorLeft = x;
            Console.CursorTop = y;
            Console.Write("┌" + horizontalEdge + "┐");
            Console.CursorLeft = x;
            Console.CursorTop = y + 1;
            Console.Write("│" + new string(' ', width) + "│");
            Console.CursorLeft = x;
            Console.CursorTop = y + 2;
            Console.Write("└" + horizontalEdge + "┘");
        }

        public void Clear()
        {
            Console.CursorTop = y;
            Console.CursorLeft = x;
            Console.Write(new string(' ', width + 1));
            Console.CursorLeft = x;
            text = "";
            position = 0;
        }

        public bool Empty()
        {
            return text == "";
        }

        public ConsoleKey Focus()
        {
            Console.CursorVisible = true;
            Console.CursorLeft = x + text.Length;
            Console.CursorTop = y;
            ConsoleKeyInfo keyInfo;
            ConsoleKey key = ConsoleKey.A;
            char keyChar;
            position = text.Length;
            while (
                   key != ConsoleKey.Enter 
                && key != ConsoleKey.UpArrow 
                && key != ConsoleKey.DownArrow 
                && key != ConsoleKey.Escape 
                && key != ConsoleKey.Tab
            )
            {
                keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;
                keyChar = keyInfo.KeyChar;
                if (key == ConsoleKey.Backspace)
                {
                    // Clear textbox
                    if (keyInfo.Modifiers.HasFlag(ConsoleModifiers.Shift))
                    {
                        Clear();
                    }
                    // Remove charachter left of cursor
                    else
                    {
                        if (position > 0)
                        {
                            string beginning = text.Substring(0, position - 1);
                            string end = text.Substring(position);
                            text = beginning + end;
                            Console.CursorLeft--;
                            Console.Write(end + new string(' ', width - text.Length + 1));
                            position--;
                            Console.CursorLeft = x + position;
                        }
                    }

                }
                else if (key == ConsoleKey.LeftArrow)
                {
                    if (position > 0)
                    {
                        position--;
                        Console.CursorLeft--;
                    }
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    if (position < text.Length)
                    {
                        position++;
                        Console.CursorLeft++;
                    }
                }
                else if (
                        text.Length < width + 1
                     && key != ConsoleKey.Enter 
                     && key != ConsoleKey.UpArrow 
                     && key != ConsoleKey.DownArrow 
                     && key != ConsoleKey.Escape 
                     && key != ConsoleKey.Tab
                )
                {
                    string beginning = text.Substring(0, position);
                    string end = text.Substring(position);
                    text = beginning + keyChar + end;

                    Console.Write(keyChar + end + new string(' ', width - text.Length + 1));
                    position++;
                    Console.CursorLeft = x + position;
                }
                
            }
            Console.CursorVisible = false;
            position = 0;
            return key;
        }
    }
}
