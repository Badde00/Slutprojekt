using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slutprojekt
{
    class MyQueue<T>
    {
        private bool IsEmpty;
        private int noItems = 0;
        T[] queue = new T[5];

        public void EnQueue(T item)
        {
            if (noItems == queue.GetLength())
                Expand();
            queue[noItems] = item;
            noItems++;
        }

        public T DeQueue()
        {
            return default(T);
        }

        public T Peek()
        {
            return default(T);
        }

        public int Count
        {
            get { return noItems; }
        }

        private void Expand()
        {

        }

        private void Decrease()
        {

        }
    }
}
