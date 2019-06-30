using System;
using System.Security.Cryptography;


namespace ErikTheCoder.Utilities
{
    public class ThreadsafeCryptoRandom : IThreadsafeRandom
    {
        private RNGCryptoServiceProvider _random;
        private object _lock;
        private bool _disposed;


        // Note that RNGCryptoServiceProvider constructor doesn't accept a seed parameter.
        // The only parameter it accepts and doesn't ignore is CspParameters (not implemented here).
        // This is because it seeds its internal algorithm with entropy from mouse movements, keyboard presses, system clock, memory status, etc.
        // See https://docs.microsoft.com/en-us/windows/desktop/api/wincrypt/nf-wincrypt-cryptgenrandom#remarks.
        public ThreadsafeCryptoRandom()
        {
            _random = new RNGCryptoServiceProvider();
            _lock = new object();
        }


        ~ThreadsafeCryptoRandom()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool Disposing)
        {
            if (_disposed) { return; }
            // Release unmanaged resources.
            lock (_lock)
            {
                _random.Dispose();
                _random = null;
            }
            if (Disposing)
            {
                // Release managed resources.
                _lock = null;
            }
            _disposed = true;
        }


        public int Next() => Next(0, int.MaxValue);


        public int Next(int ExclusiveMax) => Next(0, ExclusiveMax);


        public int Next(int InclusiveMin, int ExclusiveMax)
        {
            if (ExclusiveMax < InclusiveMin) { throw new ArgumentOutOfRangeException(); }
            int range = ExclusiveMax - InclusiveMin;
            if (range == 0) { return InclusiveMin; }
            byte[] randomBytes = new byte[sizeof(int)];
            NextBytes(randomBytes);
            uint random = BitConverter.ToUInt32(randomBytes, 0);
            return InclusiveMin + (int)(random % range);
        }


        public double NextDouble() => NextDouble(0, 1d);


        public double NextDouble(double Max) => NextDouble(0, Max);


        public double NextDouble(double Min, double Max)
        {
            double range = Max - Min;
            if (range < -double.Epsilon) { throw new ArgumentOutOfRangeException(); }
            byte[] randomBytes = new byte[sizeof(ulong)];
            NextBytes(randomBytes);
            // Bit-shift 11 and 53 based on double's mantissa bits.
            // See https://stackoverflow.com/a/2854635/8992299.
            ulong randomLong = BitConverter.ToUInt64(randomBytes, 0) / (1ul << 11);
            double randomDouble = randomLong / (double)(1ul << 53);
            return Min + (randomDouble * range);
        }


        public void NextBytes(byte[] Bytes)
        {
            lock (_lock)
            {
                _random.GetBytes(Bytes);
            }
        }
    }
}
