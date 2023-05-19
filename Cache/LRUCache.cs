using System.Collections.Concurrent;
using TextEditor.Configuration;

namespace TextEditor.Cache
{
    class LRUCache<KeyT, ValueT> : ICache<KeyT, ValueT> where ValueT: class
    {
        private readonly int _maxSize;
        private ConcurrentDictionary<KeyT, LinkedListNode<CacheItem>> _nodes;
        private LinkedList<CacheItem> _itemsList;
        private ReaderWriterLockSlim _rwLock;
        private static Conf _configuration;
        private static readonly LRUCache<KeyT, ValueT> _instance; 

        static LRUCache()
        {
            _configuration = Hosting.Instance.Config;
            _instance = new LRUCache<KeyT, ValueT>(_configuration.MaxTextGenCacheSize, _configuration.MaxTextGenCacheWorkers);
        }

        public static LRUCache<KeyT, ValueT> Instance
        {
            get
            {
                return _instance;
            }
        }


        private LRUCache(int maxSize, int numWorkingThreads)
        {
            _maxSize = maxSize;
            _nodes = new ConcurrentDictionary<KeyT, LinkedListNode<CacheItem>>(numWorkingThreads, maxSize);
            _itemsList = new LinkedList<CacheItem>();
            _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        public void Clear()
        {
            _rwLock.EnterWriteLock();
            try
            {
                _nodes.Clear();
                _itemsList.Clear();
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public ValueT Get(KeyT key)
        {
            _rwLock.EnterReadLock();
            try
            {
                var hit = _nodes.TryGetValue(key, out var node);
                if(!hit)
                {
                    return null;
                }
                else
                {
                    _itemsList.Remove(node);
                    _itemsList.AddFirst(node);
                    return node.Value.Value;
                }
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }

        public bool IsEmpty()
        {
            return Size() == 0;
        }

        public bool Set(KeyT key, ValueT value)
        {
            _rwLock.EnterWriteLock();
            try
            {
                bool hit = _nodes.TryGetValue(key, out var oldNode);
                if(hit)
                {
                    LinkedListNode<CacheItem> updatedNode = new LinkedListNode<CacheItem>(oldNode.Value);
                    updatedNode.Value.Value = value;
                    _itemsList.Remove(oldNode);
                    _itemsList.AddFirst(updatedNode);
                    return _nodes.TryUpdate(key, updatedNode, oldNode);
                }
                if(Size() >= _maxSize)
                {
                    removeOneEntry();
                }
                var item = new CacheItem() { Key = key, Value = value };
                var newNode = new LinkedListNode<CacheItem>(item);
                _itemsList.AddFirst(newNode);
                _ = _nodes.GetOrAdd(key, newNode);
                return true;
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        private void removeOneEntry()
        {
            _rwLock.EnterWriteLock();
            try
            {
                var removedNode = _itemsList.Last;
                if(removedNode != null)
                {
                    _itemsList.RemoveLast();
                    _nodes.TryRemove(removedNode.Value.Key, out var _);
                }
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        public int Size()
        {
            _rwLock.EnterReadLock();
            try
            {
                return _itemsList.Count;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }


        private class CacheItem
        {
            public KeyT Key { get; set; }
            public ValueT Value { get; set; }
        }
    }
}
