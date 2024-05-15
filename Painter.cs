using static ConfinementDemo.Field;
namespace ConfinementDemo
{
    internal static class Painter
    {
        public static void PaintBackground(char back)
        {
            for (var i = 0; i < Console.WindowWidth; i++)
                for (var j = 0; j < Console.WindowHeight; j++)
                    Console.Write(back);
        }

        public static void PaintCenteredMessage(string message, ConsoleColor background, ConsoleColor foreground)
        {
            Console.BackgroundColor = background;
            PaintBackground(' ');
            Console.ForegroundColor = foreground;
            Console.SetCursorPosition(
                Console.WindowWidth / 2 - message.Length / 2,
                Console.WindowHeight / 2);
            Console.Write(message);
            Console.ResetColor();
        }

        public static void PaintField(Field field, int topIndent,
            int leftIndent, string[] messages, Point dedicated)
        {
            for (var i = 0; i < FieldSize; i++)
            for (var j = 0; j < FieldSize; j++)
            {
                if (i == dedicated.X && j == dedicated.Y)
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                switch (field[i, j])
                {
                    case FieldElements.Empty:
                        Console.SetCursorPosition(leftIndent + i * 2,
                            topIndent + j);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("·" + " ");
                        break;
                    case FieldElements.Obstacle:
                        Console.SetCursorPosition(leftIndent + i * 2,
                            topIndent + j);
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write(" " + " ");
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;
                    case FieldElements.Enemy:
                        Console.SetCursorPosition(leftIndent + i * 2,
                                                       topIndent + j);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\u00b0" + " ");
                        break;
                    case FieldElements.DoubleMove:
                        Console.SetCursorPosition(leftIndent + i * 2,
                            topIndent + j);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("0" + " ");
                        break;
                    }
                Console.BackgroundColor = ConsoleColor.Black;

                
            }
            Console.WriteLine();
            Console.ResetColor();
            for (var i = 0; i < messages.Length; i++)
            {
                Console.SetCursorPosition(leftIndent - messages[i].Length / 2 + 10, topIndent + 10 + 2 + i);
                Console.Write(messages[i]);
            }
        }
    }
    

}
