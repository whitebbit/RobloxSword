using System;
using System.Collections.Generic;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class SaveHandler<T>
    {
        public T current;
        public List<T> unlocked = new();

        public bool IsCurrent(T obj) => EqualityComparer<T>.Default.Equals(current, obj);
        public void SetCurrent(T obj) => current = obj;
        
        public void Unlock(T obj)
        {
            if (Unlocked(obj)) return;
            
            unlocked.Add(obj);
        }

        public bool Unlocked(T obj) => unlocked.Contains(obj);
    }
}