using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// Class <c>CircularQueue</c> is a data structure implemented in the Snake game.
    /// In this case, it stores the segments of the snake.
    /// </summary>
    /// <typeparam name="E">The type of item to be stored.</typeparam>
    public class CircularQueue<E> : IList<E>
    {
        public int Size { get; set; }
        public int Count { get; private set; }

        public bool IsReadOnly { get; }
        private Queue<E> queue;

        public E this[int index] 
        { 
            get
            {
                return queue.ElementAt(index);
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        // Default Constructor
        public CircularQueue(int size)
        {
            Size = size;
            Count = 0;
            queue = new Queue<E>(size);
        }
        // Empty Constructor
        public CircularQueue()
        {
            Count = 0;
            Size = 0;
            queue = new Queue<E>();
        }

        /// <summary>
        /// Method <c>IndexOf</c> gets the index of a specified element.
        /// </summary>
        /// <param name="item">the element</param>
        /// <returns>the index of the specified element</returns>
        public int IndexOf(E item)
        {
            return queue.ToList().IndexOf(item);
        }

        // Deprecated. Use Enqueue(E item) instead!
        public void Add(E item)
        {
            Enqueue(item);
        }
        /// <summary>
        /// Method <c>Enqueue</c> adds a new element to the last index of the queue. This method
        /// dequeues the first element if the queue is full.
        /// </summary>
        /// <param name="item">the element to add</param>
        public void Enqueue(E item)
        {
            queue.Enqueue(item);

            if (Count > Size) queue.Dequeue();
            else Count++;
        }

        /// <summary>
        /// Method <c>Dequeue</c> removes the first element in the queue.
        /// </summary>
        /// <returns>the first element in the queue</returns>
        public E Dequeue()
        {
            Count--;
            return queue.Dequeue();
        }
        /// <summary>
        /// Method <c>Peek</c> returns the first element without removing it.
        /// </summary>
        /// <returns>the first element in the queue</returns>
        public E Peek()
        {
            return queue.Peek();
        }
        /// <summary>
        /// Method <c>Clear</c> removes all of the elements in the queue.
        /// </summary>
        public void Clear()
        {
            foreach (E el in queue) queue.Dequeue();
        }
        /// <summary>
        /// Method <c>Contains</c> checks the queue for the specified item
        /// </summary>
        /// <param name="item">the item to compare</param>
        /// <returns><c>true</c> if the queue contains the item or <c>false</c> if it does not.</returns>
        public bool Contains(E item)
        {
            return queue.Contains(item);
        }
        /// <summary>
        /// Method <c>GetEnumerator</c> gets the queue's enumerator.
        /// </summary>
        /// <returns>the queue's enumerator</returns>
        public IEnumerator<E> GetEnumerator()
        {
            return queue.GetEnumerator();
        }
        /// <summary>
        /// This is the same method as <c>GetEnumerator</c>.
        /// </summary>
        /// <returns>the queue's enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return queue.GetEnumerator();
        }

        // Unimplemented Methods
        public void CopyTo(E[] array, int arrayIndex)
        {
            throw new NotImplementedException("This has not been implemented yet!");
        }

        public bool Remove(E item)
        {
            throw new NotImplementedException("This is not implemented in a CircularQueue!");
        }

        public void Insert(int index, E item)
        {
            throw new NotImplementedException("This is not implemented in a CircularQueue!");
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException("This is not implemented in a CircularQueue!");
        }
    }
}