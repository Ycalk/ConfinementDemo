namespace ConfinementDemo
{
    internal enum FieldElements
    {
        Void,
        Empty,
        Enemy,
        Obstacle,
        DoubleMove
    }

    internal class Field
    {
        public const int FieldSize = 11;

        private readonly (int x, int y, FieldElements element)[] _templatePositions =
        [
            // Center enemy position
            (FieldSize / 2, FieldSize / 2, FieldElements.Enemy),
    
            // Corner and border positions
            
            (1, 1, FieldElements.Void),
            (1, 2, FieldElements.Void),
            (2, 1, FieldElements.Void),

            
            (FieldSize - 2, 1, FieldElements.Void),
            (FieldSize - 2, 2, FieldElements.Void),
            (FieldSize - 3, 1, FieldElements.Void),

            (1, FieldSize - 2, FieldElements.Void),
            (2, FieldSize - 2, FieldElements.Void),
            (1, FieldSize - 3, FieldElements.Void),

            (FieldSize - 2, FieldSize - 2, FieldElements.Void),
            (FieldSize - 3, FieldSize - 2, FieldElements.Void),
            (FieldSize - 2, FieldSize - 3, FieldElements.Void),
        ];

        private readonly FieldElements[,] _elements = new FieldElements[FieldSize, FieldSize];

        public FieldElements this[int x, int y] => _elements[x, y];

        public static Field GetSquaredField()
        {
            var result = new Field();
            for (var i = 0; i < FieldSize; i++)
            for (var j = 0; j < FieldSize; j++)
            {
                result._elements[i, j] = FieldElements.Empty;
                    if (i == 0 || j == 0 || i == FieldSize - 1 || j == FieldSize - 1)
                        result._elements[i, j] = FieldElements.Void;
            }
            result._elements[FieldSize / 2, FieldSize / 2] = FieldElements.Enemy;
            return result;
        }


        public Field()
        {
            var random = new Random();
            for (var i = 0; i < FieldSize; i++)
            for (var j = 0; j < FieldSize; j++)
            {
                var randomNum = random.Next(100);
                _elements[i, j] = FieldElements.Empty;
                if (randomNum < 5)
                    _elements[i, j] = FieldElements.Obstacle;
                else if (randomNum is > 5 and < 10)
                    _elements[i, j] = FieldElements.DoubleMove;
                
                if (i == 0 || j == 0 || i == FieldSize - 1 || j == FieldSize - 1)
                    _elements[i, j] = FieldElements.Void;
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

        public List<Point> GetEmptyPoints()
        {
            var result = new List<Point>();
            for (var i = 0; i < FieldSize; i++)
            for (var j = 0; j < FieldSize; j++)
            {
                if (_elements[i, j] == FieldElements.Empty)
                    result.Add(new Point(i, j));
            }
            return result;
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
