using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace Snake
{
    /// <summary>
    /// Class <c>Grid</c> lays out the grid, and sets the eggs/obstacles. It also keeps
    /// updating as long as the snake does not "die".
    /// </summary>
    public class Grid
    {
        public static int[,] Tiles { get; set; }
        public static int Size { get; private set; }
        public static Random random = new Random();
        public static Snake snake;
        private static int initialLength;

        private static ConsoleKey key;

        /// <summary>
        /// Method <c>SetEgg</c> sets an egg anywhere on the grid.
        /// </summary>
        /// <param name="x">The Left/Right Position</param>
        /// <param name="y">The Down/Up Position</param>
        public static void SetEgg(int x, int y)
        {
            if (Tiles[x, y] != 2)
            {
                while (Tiles[x, y] > 0)
                {
                    x = random.Next(0, Size - 1);
                    y = random.Next(0, Size - 1);
                }

                Tiles[x, y] = 2;
                Console.SetCursorPosition((x + 4) * 2, y + 2);
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("()");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        /// <summary>
        /// Method <c>SetObstacle</c> sets an obstacle anywhere on the grid.
        /// </summary>
        /// <param name="x">The Left/Right Position</param>
        /// <param name="y">The Down/Up Position</param>
        public static void SetObstacle(int x, int y)
        {
            if (Tiles[x, y] != -1)
            {
                while (Tiles[x, y] > 0)
                {
                    x = random.Next(0, Size - 1);
                    y = random.Next(0, Size - 1);
                }

                Tiles[x, y] = -1;
                Console.SetCursorPosition((x + 4) * 2, y + 2);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("[]");
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
        /// <summary>
        /// Method <c>InitializeGrid</c> initializes the grid.
        /// </summary>
        public static void InitializeGrid()
        {
            Size = 25;
            Tiles = new int[Size,Size];
            snake = new Snake(10, 0);
            initialLength = snake.Length;

            for (int i = 1; i <= 5; i++)
                SetObstacle(random.Next(0, Size - 1), random.Next(0, Size - 1));

            SetEgg(random.Next(0, Size - 1), random.Next(0, Size - 1));
            snake.DrawSnake();
            DrawGrid();
            DrawTitle();
        }
        /// <summary>
        /// Method <c>Update</c> updates the grid with new information.
        /// </summary>
        public static void Update()
        {
            //snake.Speed = 1;

            // Do this while the user has not pressed a key yet.
            while (!Console.KeyAvailable) 
            {
                if (snake.Speed > 0)
                {
                    snake.Move();       // Here, the snake's speed could change to 0 within this condition,
                                        // hence the redundancy.
                    if (snake.Speed > 0) Thread.Sleep(500 / snake.Speed);
                    snake.DrawSnake();
                }

                try
                { 
                    foreach (KeyValuePair<int, int> el in snake.Segments)
                    {
                        Tiles[el.Key, el.Value] = 1;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    snake.Speed = 0;
                    snake.IsDead = true;
                }

                if (snake.IsDead) return;        // If the snake dies, return from this method.
            }

            key = Console.ReadKey(true).Key;        // The key the user has pressed.

            if (key.Equals(ConsoleKey.RightArrow))
            {
                snake.ChangeDirection(Snake.Direction.RIGHT);
                if (snake.Speed == 0) snake.Speed = 1;
            }
            else if (key.Equals(ConsoleKey.UpArrow))
            {
                snake.ChangeDirection(Snake.Direction.UP);
                if (snake.Speed == 0) snake.Speed = 1;
            }
            else if (key.Equals(ConsoleKey.LeftArrow))
            {
                snake.ChangeDirection(Snake.Direction.LEFT);
                if (snake.Speed == 0) snake.Speed = 1;
            }
            else if (key.Equals(ConsoleKey.DownArrow))
            {
                snake.ChangeDirection(Snake.Direction.DOWN);
                if (snake.Speed == 0) snake.Speed = 1;
            }
        }
        /// <summary>
        /// Method <c>DrawScore</c> draws the score, which is the snake's current length
        /// minus its initial length.
        /// </summary>
        public static void DrawScore()
        {
            Console.CursorTop = Size + 4;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.CursorLeft = (Size * 2) - 10;
            Console.Write($"Score: {snake.Length - initialLength}");

            Console.SetCursorPosition((snake.head.Key + 4) * 2, snake.head.Value + 2);
        }
        // Draws the Grid 
        private static void DrawGrid()
        {
            int bounds = Size;
            Console.CursorTop = bounds + 2;
            
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            
            // Draws bottom wall
            for (Console.CursorLeft = 6; Console.CursorLeft < (Size + 4) * 2;)
                Console.Write("##");
            // Draws Right Wall
            for (Console.CursorTop = Console.CursorTop; Console.CursorTop >= 2; Console.CursorTop--)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write("##");
                Console.CursorLeft -= 2;
            }
            //Draws Top Wall
            for (Console.CursorLeft = Console.CursorLeft; Console.CursorLeft > 6; Console.CursorLeft -= 4)
                Console.Write("##");
            
            // Draws Left Wall
            for (Console.CursorTop = Console.CursorTop; Console.CursorTop <= bounds + 2; Console.CursorTop++)
            {
                Console.Write("##");
                Console.CursorLeft -= 2;
            }

            Console.ResetColor();
            Console.SetCursorPosition((snake.head.Key + 4) * 2, snake.head.Value + 2);
        }
        // Draws the Title
        private static void DrawTitle()
        {
            
            Console.CursorLeft = (Size / 2) + 4;
            Console.CursorTop = Size + 4;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  Welcome to Snake!  ");
            DrawScore();
        }
    }
}