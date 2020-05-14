using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// Class <c>Game</c> is the main class in the Snake game. It always plays for as long as
    /// the user wants.
    /// </summary>
    class Game
    {
        private static bool cont = true;

        public static void Main(string[] args)
        {
            Introduction();
        }
        // Plays the game.
        private static void Play()
        {
            while (!Grid.snake.IsDead)
            {
                Grid.Update();
            }

            if (Grid.snake.IsDead)
            {
                Console.SetCursorPosition(Grid.Size, Grid.Size + 2);
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Game Over! Continue? (Y/N)");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Beep(250, 500);
                
                cont = Console.ReadLine().Equals("Y") || Console.ReadLine().Equals("y");
            }

            if (cont)
            {
                NewGame();
                Play();
            }
        }
        // Starts a new game.
        private static void NewGame()
        {
            Console.Clear();
            Grid.InitializeGrid();
        }
        // Displays the Introduction
        private static void Introduction()
        {
            Console.SetCursorPosition(4, 2);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("       Welcone to Snake!       ");
            Console.CursorLeft = 4;
            Console.WriteLine("                               ");
            Console.CursorLeft = 4;
            Console.WriteLine("Enter - Play Game              ");
            Console.CursorLeft = 4;
            Console.WriteLine("I - Instructions               ");
            Console.CursorLeft = 4;
            Console.WriteLine("Escape - Quit Game             ");

            ConsoleKey key = Console.ReadKey().Key;
            if (key.Equals(ConsoleKey.I)) DisplayInstructions();
            else if (key.Equals(ConsoleKey.Enter))
            {
                Console.BackgroundColor = ConsoleColor.Black;
                NewGame();

                while (cont)
                {
                    Play();
                }
            }
            else if (!key.Equals(ConsoleKey.Escape))
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Introduction();
            }
        }
        // Displays the Instructions
        private static void DisplayInstructions()
        {
            Console.Clear();
            Console.ResetColor();
            Console.SetCursorPosition(10, 10);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("            Instructions              ");
            Console.SetCursorPosition(10, Console.CursorTop);
            Console.WriteLine("                                      ");
            Console.SetCursorPosition(10, Console.CursorTop);
            Console.WriteLine("       Use Arrow Keys to Move.        ");
            Console.SetCursorPosition(10, Console.CursorTop);
            Console.WriteLine("     Eat as many eggs as you can!     ");
            Console.SetCursorPosition(10, Console.CursorTop);
            Console.WriteLine("         Dodge the Obstacles!         ");
            Console.SetCursorPosition(10, Console.CursorTop);
            Console.WriteLine("The Snake gets faster as you eat eggs!");
            Console.ReadKey();

            Console.ResetColor();
            Console.Clear();
            Introduction();
        }
    }
}