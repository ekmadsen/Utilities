using System;
using JetBrains.Annotations;


namespace ErikTheCoder.Utilities
{
    public class ThreadsafeRandom : IThreadsafeRandom
    {
        private Random _random;
        private object _lock;
        private bool _disposed;


        public ThreadsafeRandom() : this(null)
        {
        }


        [UsedImplicitly]
        public ThreadsafeRandom(int Seed) : this((int?)Seed)
        {
        }


        private ThreadsafeRandom(int? Seed)
        {
            _random = Seed.HasValue ? new Random(Seed.Value) : new Random();
            _lock = new object();
        }


        ~ThreadsafeRandom() => Dispose(false);


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool Disposing)
        {
            if (_disposed) { return; }
            // No unmanaged resources to release.
            if (Disposing)
            {
                // Release managed resources.
                lock (_lock)
                {
                    _random = null;
                }
                _lock = null;
            }
            _disposed = true;
        }


        public int Next()
        {
            lock (_lock)
            {
                return _random.Next();
            }
        }


        public int Next(int ExclusiveMax)
        {
            lock (_lock)
            {
                return _random.Next(ExclusiveMax);
            }
        }


        public int Next(int InclusiveMin, int ExclusiveMax)
        {
            lock (_lock)
            {
                return _random.Next(InclusiveMin, ExclusiveMax);
            }
        }


        public double NextDouble()
        {
            lock (_lock)
            {
                return _random.NextDouble();
            }
        }


        public double NextDouble(double Max) => NextDouble(0, Max);


        public double NextDouble(double Min, double Max)
        {
            var range = Max - Min;
            if (range < -double.Epsilon) { throw new ArgumentOutOfRangeException(); }
            lock (_lock)
            {
                return Min + (_random.NextDouble() * range);
            }
        }


        public void NextBytes(byte[] Bytes)
        {
            lock (_lock)
            {
                _random.NextBytes(Bytes);
            }
        }
    }
}