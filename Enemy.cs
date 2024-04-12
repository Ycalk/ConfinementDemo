namespace ConfinementDemo
{
    internal class Enemy(Field field)
    {
        public enum MoveResult
        {
            Move,
            NoPoints,
            Win
        }

        private readonly HashSet<Point> _targets = field.GetVoidPoints().ToHashSet();

        private static IEnumerable<Point> GetRandomDeltas()
        {
            var deltas = new List<Point>();
            for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
            {
                if (dx != 0 && dy != 0) continue;
                deltas.Add(new Point(dx, dy));
            }
            var random = new Random();
            while (deltas.Count != 0)
            {
                var randomIndex = random.Next(deltas.Count);
                yield return deltas[randomIndex];
                deltas.RemoveAt(randomIndex);
            }
                
        }

        private IEnumerable<Point> GetNeighbours(Point point) =>
            GetRandomDeltas()
                .Select(delta => delta + point)
                .Where(neighbour => 
                    Field.InBounds(neighbour) &&
                    field[neighbour.X, neighbour.Y] != FieldElements.Obstacle &&
                    field[neighbour.X, neighbour.Y] != FieldElements.Enemy);


        private static List<Point> GetPath(Point end, IReadOnlyDictionary<Point, Point?> paths)
        {
            var current = end;
            var result = new List<Point>();
            while (true)
            {
                result.Add(current);
                current = paths[current];
                if (current is null) break;
            }

            result.Reverse();
            return result;
        } 

        private IEnumerable<List<Point>> GetPath(Point start)
        {
            var queue = new Queue<Point>();
            var paths = new Dictionary<Point, Point?>();
            queue.Enqueue(start);
            paths[start] = null;
            while (queue.Count != 0)
            {
                var point = queue.Dequeue();
                if (_targets.Contains(point))
                    yield return GetPath(point, paths);

                foreach (var neighbour in GetNeighbours(point))
                    if (paths.TryAdd(neighbour, point))
                        queue.Enqueue(neighbour);
            }
        }

        private void Move(Point point)
        {
            field.Mark(field.GetEnemyPosition(), FieldElements.Empty);
            field.Mark(point, FieldElements.Enemy);
        }

        public MoveResult MakeMove()
        {
            var path = GetPath(field.GetEnemyPosition()).MinBy(p => p.Count);
            if (path is null)
                return MoveResult.NoPoints;
            var move = path[1];
            var result = field[move.X, move.Y];
            Move(move);
            return result == FieldElements.Lose ? MoveResult.Win : MoveResult.Move;
        }
    }
}
