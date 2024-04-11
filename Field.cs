namespace ConfinementDemo
{
    internal enum FieldElements
    {
        Void,
        Lose,
        Empty,
        Enemy,
        Obstacle,
    }

    internal class Field
    {
        public const int FieldSize = 9;

        private readonly (int x, int y, FieldElements element)[] _templatePositions =
        [
            // Center enemy position
            (FieldSize / 2, FieldSize / 2, FieldElements.Enemy),
    
            // Corner and border positions
            (0, 0, FieldElements.Void),
            (0, 1, FieldElements.Void),
            (1, 0, FieldElements.Void),
            (1, 1, FieldElements.Lose),

            (FieldSize - 1, 0, FieldElements.Void),
            (FieldSize - 1, 1, FieldElements.Void),
            (FieldSize - 2, 0, FieldElements.Void),
            (FieldSize - 2, 1, FieldElements.Lose),

            (0, FieldSize - 1, FieldElements.Void),
            (1, FieldSize - 1, FieldElements.Void),
            (0, FieldSize - 2, FieldElements.Void),
            (1, FieldSize - 2, FieldElements.Lose),

            (FieldSize - 1, FieldSize - 1, FieldElements.Void),
            (FieldSize - 2, FieldSize - 1, FieldElements.Void),
            (FieldSize - 1, FieldSize - 2, FieldElements.Void),
            (FieldSize - 2, FieldSize - 2, FieldElements.Lose)
        ];

        private readonly FieldElements[,] _elements = new FieldElements[FieldSize, FieldSize];

        public FieldElements this[int x, int y] => _elements[x, y];

        public Field()
        {
            for (var i = 0; i < FieldSize; i++)
            for (var j = 0; j < FieldSize; j++)
            {
                _elements[i, j] = FieldElements.Empty;
                if (i == 0 || j == 0 || i == FieldSize - 1 || j == FieldSize - 1)
                    _elements[i, j] = FieldElements.Lose;
            }

            foreach (var (x, y, element) in _templatePositions)
                _elements[x, y] = element;
        }

        public IEnumerable<Point> GetVoidPoints()
        {
            for (var i = 0; i < FieldSize; i++)
            for (var j = 0; j < FieldSize; j++)
            {
                if (_elements[i, j] == FieldElements.Void)
                    yield return new Point(i, j);
            }
        }

        public static bool InBounds(Point point) =>
            point is { X: >= 0 and < FieldSize, Y: >= 0 and < FieldSize };

        public void Mark(Point point, FieldElements element) =>
            _elements[point.X, point.Y] = element;

        public Point GetEnemyPosition()
        {
            for(var i = 0; i < FieldSize; i++)
            for (var j = 0; j < FieldSize; j++)
            {
                if (_elements[i, j] == FieldElements.Enemy)
                    return new Point(i, j);
            }
            throw new Exception("Enemy not found");
        }
    }
}
