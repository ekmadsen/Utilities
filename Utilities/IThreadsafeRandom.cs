using System;
using JetBrains.Annotations;


namespace ErikTheCoder.Utilities
{
    public interface IThreadsafeRandom : IDisposable
    {
        /// <summary>
        /// Returns a nonnegative random integer.
        /// </summary>
        int Next();


        /// <summary>
        /// Returns a nonnegative random integer that is less than the specified maximum.
        /// </summary>
        int Next(int ExclusiveMax);


        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        int Next(int InclusiveMin, int ExclusiveMax);


        /// <summary>
        /// Returns a random floating-point number between 0.0 and 1.0.
        /// </summary>
        double NextDouble();


        /// <summary>
        /// Returns a random floating-point number between 0.0 and Max.
        /// </summary>
        double NextDouble(double Max);


        /// <summary>
        /// Returns a random floating-point number between Min and Max.
        /// </summary>
        double NextDouble(double Min, double Max);


        /// <summary>
        /// Fills the elements of the specified array of bytes with random numbers.
        /// </summary>
        [UsedImplicitly]
        void NextBytes(byte[] Bytes);
    }
}