using System.Collections.Generic;


namespace ErikTheCoder.Utilities
{
    public class ListDiff<T>
    {
        public HashSet<T> Added { get; } = new HashSet<T>();
        public HashSet<T> Removed { get; } = new HashSet<T>();
        public HashSet<T> Remaining { get; } = new HashSet<T>();
    }
}
