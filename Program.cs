using ConfinementDemo;

Painter.PaintCenteredMessage("Confinement", ConsoleColor.DarkGray, ConsoleColor.White);
Thread.Sleep(500);

while (true)
{
    Console.Clear();
    var game = new Game();
    game.Start();
    Console.ReadKey();
}
