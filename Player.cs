using static ConfinementDemo.Field;

namespace ConfinementDemo
{
    internal class Player(Field field)
    {
        public Point Cursor { get; private set; } = new(FieldSize / 2, FieldSize / 2);

        public Point MakeMove()
        {
            GetPointFromConsole();
            return Cursor;
        }

        private void GetClosestEmptyPoint(Point offset)
        {
            var result = Cursor.Copy();
            while (result.X + offset.X is >= 0 and < FieldSize &&
                   result.Y + offset.Y is >= 0 and < FieldSize)
            {
                result += offset;
                if (field[result.X, result.Y] != FieldElements.Empty) continue;
                Cursor = result;
                break;
            }
        }

        private void GetPointFromConsole()
        {
            var key = Console.ReadKey().Key;
            while (key != ConsoleKey.Spacebar ||
                   field[Cursor.X, Cursor.Y] != FieldElements.Empty)
            {
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        GetClosestEmptyPoint(Point.EmptyX(-1));
                        break;
                    case ConsoleKey.DownArrow:
                        GetClosestEmptyPoint(Point.EmptyX(1));
                        break;
                    case ConsoleKey.LeftArrow:
                        GetClosestEmptyPoint(Point.EmptyY(-1));
                        break;
                    case ConsoleKey.RightArrow:
                        GetClosestEmptyPoint(Point.EmptyY(1));
                        break;
                }
                Painter.PaintField(field, Console.WindowHeight / 2 - 10, Console.WindowWidth / 2 - 10, [],
                    Cursor);
                key = Console.ReadKey().Key;
            }
        }
    }
}
