namespace ConfinementDemo
{
    internal class Point(int x, int y)
    {
        public override bool Equals(object? obj) =>
            obj is Point other && X == other.X && Y == other.Y;

        public override int GetHashCode() =>
            unchecked((17 * 23 + X) * 23 + Y);

        public readonly int X = x;
        public readonly int Y = y;

        public Point Copy() => new(X, Y);
        public static Point EmptyX(int y) => new(0, y);
        public static Point EmptyY(int x) => new(x, 0);
        public Point WithX(int x) => new(x, Y);
        public Point WithY(int y) => new(X, y);
        public override string ToString() => "(" + X + ", " + Y + ")";
        public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
        public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
        public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Point a, Point b) => a.X != b.X && a.Y != b.Y;
    }
}
