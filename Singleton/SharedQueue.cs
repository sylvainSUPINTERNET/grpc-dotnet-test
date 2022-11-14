using System;
using System.Collections;

namespace Singleton 
{
    class SharedQueue : ISharedQueue
    {
        private Dictionary<Guid, Queue> _queueList = new Dictionary<Guid, Queue>();

        public void AddQueue(Guid id)
        {
            _queueList.Add(id, new Queue());
        }

        public void RemoveQueue(Guid id)
        {
            _queueList.Remove(id);
        }

        public void Enqueue(Guid id, object obj)
        {
            _queueList[id].Enqueue(obj);
        }

        public object Dequeue(Guid id)
        {
            return _queueList[id].Dequeue();
        }

        public bool IsEmpty(Guid id)
        {
            return _queueList[id].Count == 0;
        }

        public int Count(Guid id)
        {
            return _queueList[id].Count;
        }

        public void Clear(Guid id)
        {
            _queueList[id].Clear();
        }

        public Dictionary<Guid, Queue> GetQueueList()
        {
            return _queueList;
        }

        public Queue GetQueue(Guid id)
        {
            return _queueList[id];
        }

        
    }
}