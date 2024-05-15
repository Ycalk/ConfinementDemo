namespace ConfinementDemo
{
    internal class Game
    {
        private readonly IEnemy _enemy;
        private readonly Field _field;
        private readonly Player _player;

        public Game()
        {
            _field = new Field();
            _player = new Player(_field);
            _enemy = new SmartEnemy(_field);
        }

        public void Start()
        {
            Enemy.MoveResult result;
            for (var i = 0; ; i++)
            {
                Painter.PaintField(
                    _field, Console.WindowHeight / 2 - 10, 
                    Console.WindowWidth / 2 - 10, [], _player.Cursor);
                if (i % 2 == 0)
                    _field.Mark(_player.MakeMove(), FieldElements.Obstacle);
                else
                {
                    result = _enemy.MakeMove();
                    if (result != Enemy.MoveResult.Move)
                        break;
                }
            }

            GameEnd(result);
        }

        private static void GameEnd(Enemy.MoveResult gameResult)
        {
            Console.Clear();
            if (gameResult == Enemy.MoveResult.NoPoints)
                Painter.PaintCenteredMessage("You win!", ConsoleColor.DarkGreen, ConsoleColor.White);
            else
                Painter.PaintCenteredMessage("You lose!", ConsoleColor.DarkRed, ConsoleColor.White);
        }
    }
}
