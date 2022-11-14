using System.Collections;

namespace Singleton 

{
    public interface ISharedQueue {

        Queue GetQueue(Guid id);
        Dictionary<Guid, Queue> GetQueueList(); 
        void AddQueue(Guid id);
        void RemoveQueue(Guid id);
        void Enqueue(Guid id, object obj);
        object Dequeue(Guid id);
        bool IsEmpty(Guid id);
        int Count(Guid id);
        void Clear(Guid id);

    }
}