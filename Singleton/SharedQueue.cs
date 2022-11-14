using System;
using System.Collections;

namespace Singleton 
{
    class SharedQueue : ISharedQueue
    {
        private Dictionary<Guid, Queue> _queueList = new Dictionary<Guid, Queue>();
    }
}