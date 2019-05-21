using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class MyQueue<T>
    {
        private int noItems = 0; //Number of items
        T[] queue = new T[5];

        public void Enqueue(T item)
        {
            if (noItems == queue.Length)
                Expand();
            queue[noItems] = item;
            noItems++;
        }

        public T Dequeue()
        {
            if(noItems < (float)queue.Length / 2 && noItems > 5)
            {
                Decrease();
            }
            noItems--;
            T item = queue[noItems];
            queue[noItems] = default(T);
            return item;
        }

        public T Peek()
        {
            return queue[noItems - 1];
        }

        public int Count
        {
            get { return noItems; }
        }

        public MyQueue()
        {

        }

        private void Expand()
        {
            T[] temp = new T[queue.Length];
            for (int i = 0; i < noItems; i++)
            {
                temp[i] = queue[i];
            }
            queue = new T[queue.Length * 2];
            for (int i = 0; i < temp.Length; i++)
            {
                queue[i] = temp[i];
            }
        }

        private void Decrease()
        {
            T[] temp = new T[noItems];
            for (int i = 0; i < noItems; i++)
            {
                temp[i] = queue[i];
            }
            queue = new T[queue.Length / 2];
            for (int i = 0; i < noItems; i++)
            {
                queue[i] = temp[i];
            }
        }
    }
}
