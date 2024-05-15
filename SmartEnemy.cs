using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static ConfinementDemo.Enemy;

namespace ConfinementDemo
{
    internal class SmartEnemy(Field field) : IEnemy
    {
        #region Init
        private class DistanceField
        {
            public int this[Point point] => _distances[point.X, point.Y];

            private readonly int[,] _distances;
            private readonly Field _field;

            private int[,] GetDistances(Point start)
            {
                var distances = new int[Field.FieldSize, Field.FieldSize];
                for (var i = 0; i < Field.FieldSize; i++)
                for (var j = 0; j < Field.FieldSize; j++)
                    distances[i, j] = int.MaxValue;
                distances[start.X, start.Y] = 0;
                var queue = new Queue<Point>();
                queue.Enqueue(start);
                while (queue.Count != 0)
                {
                    var current = queue.Dequeue();
                    foreach (var neighbour in GetNeighbours(current, _field))
                    {
                        if (distances[neighbour.X, neighbour.Y] != int.MaxValue) continue;
                        distances[neighbour.X, neighbour.Y] = distances[current.X, current.Y] + 1;
                        queue.Enqueue(neighbour);
                    }
                }
                return distances;
            }

            public DistanceField(Point start, Field field)
            {
                _field = field;
                _distances = GetDistances(start);
            }
        }

        private class NodeData(Point? previous, double price)
        {
            public Point? Previous { get; } = previous;
            public double Price { get; } = price;
        }

        private static List<Point> GetObstacles(Field field)
        {
            var result = new List<Point>();
            for (var i = 0; i < Field.FieldSize; i++)
            for (var j = 0; j < Field.FieldSize; j++)
            {
                if (field[i, j] == FieldElements.Obstacle)
                    result.Add(new Point(i, j));
            }
            return result;
        }

        private class AlgorithmInfo(Point start, Field field)
        {
            public Field Field => field;
            public DistanceField Distances { get; } = new(start, field);
            public Dictionary<Point, NodeData> Track { get; } = new() { [start] = new NodeData(null, 0) };
            public HashSet<Point> Visited { get; } = [];
            public HashSet<Point> NotVisited { get; } = GetNeighbours(start, field).ToHashSet();
            public List<Point> Obstacles { get; } = GetObstacles(field);
            public HashSet<Point> Targets { get; } = field.GetVoidPoints().ToHashSet();
        }
        #endregion
        #region NeighboursFinder
        private static IEnumerable<Point> GetDeltas()
        {
            for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
            {
                if (dx != 0 && dy != 0) continue;
                yield return new Point(dx, dy);
            }
        }

        private static void GetNeighbours(Point point, Field field, List<Point> neighbours, HashSet<Point> doubleMove)
        {
            foreach (var delta in GetDeltas())
            {
                var neighbour = delta + point;
                if (!Field.InBounds(neighbour) ||
                    field[neighbour.X, neighbour.Y] == FieldElements.Obstacle ||
                    field[neighbour.X, neighbour.Y] == FieldElements.Enemy)
                    continue;
                if (field[neighbour.X, neighbour.Y] == FieldElements.DoubleMove &&
                    doubleMove.Add(neighbour))
                    GetNeighbours(neighbour, field, neighbours, doubleMove);
                else
                    neighbours.Add(neighbour);
            }
        }

        private static IEnumerable<Point> GetNeighbours(Point point, Field field)
        {
            var result = new List<Point>();
            GetNeighbours(point, field, result, new HashSet<Point>());
            return result;
        }
            

        private static IEnumerable<Point> GetNeighbours(Point point, AlgorithmInfo info) =>
            GetNeighbours(point, info.Field);


        #endregion

        private static List<Point> GetPath(Point end,
            AlgorithmInfo info)
        {
            var current = end;
            var path = new List<Point>();
            while (current is not null)
            {
                path.Add(current);
                current = info.Track[current].Previous;
            }

            path.Reverse();
            return path;
        }

        private static double GetDistance(Point first, Point second) =>
            Math.Sqrt((first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y));

        private static double GetPrice(Point point, AlgorithmInfo infos) =>
        
            infos.Obstacles
                .Select(obstacle => ((double)Field.FieldSize / 2 - GetDistance(point, obstacle)) * infos.Distances[point])
                .Where(counted => counted > 0).Sum();
        
        private static Point? GetNodeToOpen(AlgorithmInfo infos)
        {
            Point? result = null;
            var bestPrice = double.PositiveInfinity;
            foreach (var point in infos.NotVisited.Where(infos.Track.ContainsKey))
            {
                var currentPrice = infos.Track[point].Price;
                if (currentPrice >= bestPrice ||
                    infos.Visited.Contains(point)) continue;
                bestPrice = currentPrice;
                result = point;
            }
            return result;
        }

        private static void OpenNode(Point point, AlgorithmInfo info)
        {
            foreach (var neighbour in GetNeighbours(point, info))
            {
                info.NotVisited.Add(neighbour);
                var currentPrice = info.Track[point].Price + GetPrice(neighbour, info);
                if (!info.Track.TryGetValue(neighbour, out var neighbourData) || neighbourData.Price > currentPrice)
                    info.Track[neighbour] = new NodeData(point, currentPrice);
            }
        }


        private IEnumerable<List<Point>> GetPath(Point start)
        {
            var info = new AlgorithmInfo(start, field);
            info.NotVisited.Add(start);
            while (info.NotVisited.Count != 0)
            {
                var toOpen = GetNodeToOpen(info);
                if (toOpen is null) yield break;
                if (info.Targets.Remove(toOpen))
                    yield return GetPath(toOpen, info);
                OpenNode(toOpen, info);
                info.Visited.Add(toOpen);
            }
        }

        private void Move(Point point)
        {
            field.Mark(field.GetEnemyPosition(), FieldElements.Empty);
            field.Mark(point, FieldElements.Enemy);
        }

        public MoveResult MakeMove()
        {
            var path = GetPath(field.GetEnemyPosition()).FirstOrDefault();
            if (path is null)
                return MoveResult.NoPoints;
            var move = path[1];
            var result = field[move.X, move.Y];
            Move(move);
            return result == FieldElements.Void ? MoveResult.Win : MoveResult.Move;
        }
    }
}
