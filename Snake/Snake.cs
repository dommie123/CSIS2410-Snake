using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// Class <c>Snake</c> moves based on a speed interval, changes direction, dies, and eats eggs
    /// on the grid.
    /// </summary>
    public class Snake
    {
        public KeyValuePair<int, int> head;

        public CircularQueue<KeyValuePair<int, int>> Segments { get; set; }
        public int Length { get; set; } 
        public int Speed { get; set; }
        public enum Direction { RIGHT, UP, LEFT, DOWN }
        public bool IsDead { get; set; }
        public Direction Compass { get; set; }

        public Snake(int length, int speed)
        {
            Length = length;
            Speed = speed;
            Compass = Direction.RIGHT;

            Segments = new CircularQueue<KeyValuePair<int, int>>(Length);
            for (int i = 0; i < Length; i++)
            {
                Segments.Enqueue(new KeyValuePair<int, int>(i + 2, (Grid.Size - 1) / 2));
                Grid.Tiles[i + 2, (Grid.Size - 1) / 2] = 1;
            }
            // Grid.InitializeGrid();
            head = Segments.Last();
            IsDead = false;
        }
        /// <summary>
        /// Method <c>Move</c> "moves" the snake
        /// </summary>
        public void Move()
        {
            Console.SetCursorPosition((head.Key + 4) * 2, head.Value + 2);
            CircularQueue<KeyValuePair<int, int>> segments2 = new CircularQueue<KeyValuePair<int, int>>(Length);
            KeyValuePair<int, int> tempSeg = head;

            if (Segments.Size >= Length)
            {
                KeyValuePair<int, int> del = Segments.Dequeue();

                DeleteLeftovers(del.Key + 1, del.Value);
                DeleteLeftovers(del.Key - 1, del.Value);
                DeleteLeftovers(del.Key, del.Value + 1);
                DeleteLeftovers(del.Key, del.Value - 1);
            }
            else Segments.Size++;

            for (int i = 0; i < Segments.Count;)
            {
                segments2.Enqueue(Segments.Dequeue());
            }
            

            switch (Compass)
            {
                case Direction.UP:
                    if (HitEgg())
                    {
                        EatEgg();
                        Grid.Tiles[tempSeg.Key, tempSeg.Value - 1] = 1;
                    }
                    else if (HitObstacle() || HitSelf())
                    {
                        Speed = 0;
                        IsDead = true;
                    }
                    segments2.Enqueue(new KeyValuePair<int, int>(tempSeg.Key, tempSeg.Value - 1));
                    break;
                case Direction.LEFT:
                    if (HitEgg())
                    {
                        EatEgg();
                        Grid.Tiles[tempSeg.Key - 1, tempSeg.Value] = 1;
                    }
                    else if (HitObstacle() || HitSelf())
                    {
                        Speed = 0;
                        IsDead = true;
                    }
                    segments2.Enqueue(new KeyValuePair<int, int>(tempSeg.Key - 1, tempSeg.Value));
                    break;
                case Direction.RIGHT:
                    if (HitEgg())
                    {
                        EatEgg();
                        Grid.Tiles[tempSeg.Key + 1, tempSeg.Value] = 1;
                    }
                    else if (HitObstacle() || HitSelf())
                    {
                        Speed = 0;
                        IsDead = true;
                    }
                    segments2.Enqueue(new KeyValuePair<int, int>(tempSeg.Key + 1, tempSeg.Value));
                    break;
                case Direction.DOWN:
                    if (HitEgg())
                    {
                        EatEgg();
                        Grid.Tiles[tempSeg.Key, tempSeg.Value + 1] = 1;
                    }
                    else if (HitObstacle() || HitSelf())
                    {
                        Speed = 0;
                        IsDead = true;
                    }
                    segments2.Enqueue(new KeyValuePair<int, int>(tempSeg.Key, tempSeg.Value + 1));
                    break;
            }

            if (Length > 0) Segments = segments2;
            head = Segments.Last();
        }
        /// <summary>
        /// Method <c>ChangeDirection</c> changes the direction of the snake.
        /// </summary>
        /// <param name="newDirection">The snake's new direction</param>
        public void ChangeDirection(Direction newDirection)
        {
            switch (Compass)
            {
                case Direction.UP:
                    if (!newDirection.Equals(Direction.DOWN)) Compass = newDirection;
                    break;
                case Direction.RIGHT:
                    if (!newDirection.Equals(Direction.LEFT)) Compass = newDirection;
                    break;
                case Direction.LEFT:
                    if (!newDirection.Equals(Direction.RIGHT)) Compass = newDirection;
                    break;
                case Direction.DOWN:
                    if (!newDirection.Equals(Direction.UP)) Compass = newDirection;
                    break;

            }
        }
        /// <summary>
        /// Method <c>DrawSnake</c> draws the snake on the console.
        /// </summary>
        public void DrawSnake()
        {
            Console.BackgroundColor = ConsoleColor.Green;
            foreach (KeyValuePair<int, int> el in Segments)
            {
                if (el.Equals(head)) Console.BackgroundColor = ConsoleColor.Green;
                else Console.BackgroundColor = ConsoleColor.DarkGreen;

                Console.SetCursorPosition((el.Key + 4) * 2, el.Value + 2);
                Console.Write("XX");
            }
            Console.SetCursorPosition((head.Key + 4) * 2 + 1, head.Value + 2);
        }

        private void EatEgg()
        {
            Length++;
            if (Length > 15) Speed = 2;
            if (Length > 20) Speed = 3;
            if (Length > 25) Speed = 4;
            if (Length > 30) Speed = 5;
            if (Length > 35) Speed = 6;
            if (Length > 40) Speed = 7;
            if (Length > 45) Speed = 8;
            Random r = new Random();
            Grid.SetEgg(r.Next(0, Grid.Size - 1), r.Next(0, Grid.Size - 1));
            Grid.SetObstacle(r.Next(0, Grid.Size - 1), r.Next(0, Grid.Size - 1));
            Grid.DrawScore();
        }

        //*************** Sensory Methods *********************
        private bool HitObstacle()
        {
            try
            {
                switch (Compass)
                {
                    case Direction.RIGHT:
                        if (Grid.Tiles[head.Key + 1, head.Value] == -1)
                            return true;
                        break;
                    case Direction.UP:
                        if (Grid.Tiles[head.Key, head.Value - 1] == -1)
                            return true;
                        break;
                    case Direction.LEFT:
                        if (Grid.Tiles[head.Key - 1, head.Value] == -1)
                            return true;
                        break;
                    case Direction.DOWN:
                        if (Grid.Tiles[head.Key, head.Value + 1] == -1)
                            return true;
                        break;
                }
            }
            catch (IndexOutOfRangeException)
            { return false; }

            return false;
        }

        private bool HitEgg()
        {
            try
            {
                switch (Compass)
                {
                    case Direction.RIGHT:
                        if (Grid.Tiles[head.Key + 1, head.Value] == 2)
                            return true;
                        break;
                    case Direction.UP:
                        if (Grid.Tiles[head.Key, head.Value - 1] == 2)
                            return true;
                        break;
                    case Direction.LEFT:
                        if (Grid.Tiles[head.Key - 1, head.Value] == 2)
                            return true;
                        break;
                    case Direction.DOWN:
                        if (Grid.Tiles[head.Key, head.Value + 1] == 2)
                            return true;
                        break;
                }
            }
            catch (IndexOutOfRangeException)
            { return false; }

            return false;
        }

        private bool HitSelf()
        {
            try
            {
                switch (Compass)
                {
                    case Direction.RIGHT:
                        if (Grid.Tiles[head.Key + 1, head.Value] == 1)
                            return true;
                        break;
                    case Direction.UP:
                        if (Grid.Tiles[head.Key, head.Value - 1] == 1)
                            return true;
                        break;
                    case Direction.LEFT:
                        if (Grid.Tiles[head.Key - 1, head.Value] == 1)
                            return true;
                        break;
                    case Direction.DOWN:
                        if (Grid.Tiles[head.Key, head.Value + 1] == 1)
                            return true;
                        break;
                }
            }
            catch (IndexOutOfRangeException)
            { return false; }

            return false;
        }
        //************ End of Sensory Methods ******************

        // Prevents the snake from increasing in length by removing any leftover
        // segments.
        private void DeleteLeftovers(int x, int y)
        {
            try
            {
                if (Grid.Tiles[x, y] == 1 && !ContainsSegment(x, y))
                {
                    Grid.Tiles[x, y] = 0;
                    Console.SetCursorPosition((x + 4) * 2, y + 2);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("  ");
                }
                Console.SetCursorPosition((head.Key + 4) * 2, head.Value + 2);
            } catch (IndexOutOfRangeException)
            {
                // Do nothing
            }
        }
        // Checks if the snake contains a segment at the given corrdinates.
        private bool ContainsSegment(int x, int y)
        {
            foreach(KeyValuePair<int, int> el in Segments)
            {
                if (el.Key == x && el.Value == y) return true;
            }
            return false;
        }
    }
}