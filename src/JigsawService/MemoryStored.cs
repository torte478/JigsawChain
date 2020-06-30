using System;
using System.Collections.Generic;

namespace JigsawService
{
    internal sealed class MemoryStored<TKey, TValue> : IStored<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> stored = new Dictionary<TKey, TValue>();
        private readonly Func<TKey> generateId;

        public MemoryStored(Func<TKey> generateId)
        {
            this.generateId = generateId;
        }

        public TValue Extract(TKey id)
        {
            var extracted = stored[id];
            stored.Remove(id);
            return extracted;
        }

        public bool IsStored(TKey id)
        {
            return stored.ContainsKey(id);
        }

        public TValue Read(TKey id)
        {
            return stored[id];
        }

        public bool Remove(TKey id)
        {
            return stored.Remove(id);
        }

        public TKey Store(TValue value)
        {
            var id = generateId();
            stored.Add(id, value);
            return id;
        }

        public TKey Store(TKey key, TValue value)
        {
            stored.Add(key, value);
            return key;
        }
    }
}