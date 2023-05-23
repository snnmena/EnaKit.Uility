using System;
using System.Collections;
using System.Collections.Generic;

namespace Yoziya
{
    public class OnEventQueue<T> where T : IEquatable<T>
    {
        private Queue<T> queue = new Queue<T>();
        private event Action<T> OnEnqueue;

        public void Enqueue(T item)
        {
            queue.Enqueue(item);
            OnEnqueue?.Invoke(item);
        }

        public T Dequeue()
        {
            return queue.Dequeue();
        }

        public T Peek()
        {
            return queue.Peek();
        }

        public int Count
        {
            get { return queue.Count; }
        }
        public void AddListener(Action<T> action)
        {
            OnEnqueue += action;
        }
        public void RemoveListener(Action<T> action)
        {
            OnEnqueue -= action;
        }
    }
}