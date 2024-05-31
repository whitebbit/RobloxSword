using System;
using System.Collections.Generic;

namespace _3._Scripts.Saves
{
    [Serializable]
    public class SaveManyHandler<T>
    {
        public List<T> current = new();
        public List<T> unlocked = new();

        public bool IsCurrent(T obj) => current.Contains(obj);

        public void SetCurrent(T obj, int maxObjects = 3)
        {
            if(current.Count >= maxObjects) return;
            
            current.Add(obj);
        }

        public void RemoveCurrent(T obj)
        {
            current.Remove(obj);
        }

        public bool FilledIn(int maxObjects)
        {
            return current.Count >= maxObjects;
        }
        public void Unlock(T obj)
        {
            if (Unlocked(obj)) return;

            unlocked.Add(obj);
        }

        public bool Unlocked(T obj) => unlocked.Contains(obj);
    }
}