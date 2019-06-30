using System;
using NUnit.Framework;


namespace ErikTheCoder.Utilities.Tests
{
    [TestFixture]
    public class ThreadsafeRandomTests
    {
        private const int _repeatTests = 997;
        private const int _integerInterval = 9_973;
        private const int _integerComboInterval = 999_983;
        private const double _doubleComboInterval = 9_999_991d + (1d / 7d);
        private const double _minDoubleValue = -1_000_000_000_000d;
        private const double _maxDoubleValue = 1_000_000_000_000d;


        // +---------------------+
        // |                     |                
        // |    Integer Tests    |
        // |                     |
        // +---------------------+


        // Test no parameters.
        [Test]
        public void TestRandomInt() => TestRandomInt(ApplicationResources.Random);


        [Test]
        public void TestCryptoRandomInt() => TestRandomInt(ApplicationResources.CryptoRandom);


        private static void TestRandomInt(IThreadsafeRandom Random)
        {
            for (int i = 0; i < _repeatTests; i++)
            {
                int value = Random.Next();
                Assert.That(value, Is.GreaterThanOrEqualTo(0));
                Assert.That(value, Is.LessThan(int.MaxValue));
            }
        }


        // Test negative max exception.
        [Test]
        public void TestRandomIntNegativeMax() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomIntNegativeMax(ApplicationResources.Random));


        [Test]
        public void TestCryptoRandomIntNegativeMax() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomIntNegativeMax(ApplicationResources.CryptoRandom));


        private static void TestRandomIntNegativeMax(IThreadsafeRandom Random) => Random.Next(-13);


        // Test max zero.
        [Test]
        public void TestRandomIntMaxZero() => TestRandomIntMaxZero(ApplicationResources.Random);


        [Test]
        public void TestCryptoRandomIntMaxZero() => TestRandomIntMaxZero(ApplicationResources.CryptoRandom);


        private static void TestRandomIntMaxZero(IThreadsafeRandom Random)
        {
            for (int i = 0; i < _repeatTests; i++)
            {
                int value = Random.Next(0);
                Assert.That(value, Is.EqualTo(0));
            }
        }


        // Test max.
        [Test]
        public void TestRandomIntMax() => TestRandomIntMax(ApplicationResources.Random);


        [Test]
        public void TestCryptoRandomIntMax() => TestRandomIntMax(ApplicationResources.CryptoRandom);


        private static void TestRandomIntMax(IThreadsafeRandom Random)
        {
            const int maxValue = int.MaxValue - _integerInterval;
            // Don't test zero.  That's covered by TestRandomIntMaxZero.
            for (int max = 1; max <= maxValue; max += _integerInterval)
            {
                int value = Random.Next(max);
                Assert.That(value, Is.GreaterThanOrEqualTo(0));
                Assert.That(value, Is.LessThan(max));
            }
        }


        // Test invalid range exception.
        [Test]
        public void TestRandomIntInvalidRange() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomIntInvalidRange(ApplicationResources.Random));


        [Test]
        public void TestCryptoRandomIntInvalidRange() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomIntInvalidRange(ApplicationResources.CryptoRandom));


        private static void TestRandomIntInvalidRange(IThreadsafeRandom Random) => Random.Next(9, 8);


        // Test same value for min and max.
        [Test]
        public void TestRandomIntMinMaxSame() => TestRandomIntMinMaxSame(ApplicationResources.Random);


        [Test]
        public void TestCryptoRandomIntMinMaxSame() => TestRandomIntMinMaxSame(ApplicationResources.CryptoRandom);


        private static void TestRandomIntMinMaxSame(IThreadsafeRandom Random)
        {
            const int maxValue = int.MaxValue - _integerInterval;
            for (int minMax = 1; minMax <= maxValue; minMax += _integerInterval)
            {
                int value = Random.Next(minMax, minMax);
                Assert.That(value, Is.EqualTo(minMax));
            }
        }


        // Test min and max parameters.
        [Test]
        public void TestRandomIntMinMax() => TestRandomIntMinMax(ApplicationResources.Random);


        [Test]
        public void TestCryptoRandomIntMinMax() => TestRandomIntMinMax(ApplicationResources.CryptoRandom);


        private static void TestRandomIntMinMax(IThreadsafeRandom Random)
        {
            const int maxValue = int.MaxValue - (2 * _integerComboInterval);
            for (int min = int.MinValue; min <= maxValue; min += _integerComboInterval)
            {
                // Don't test max < min.   That's covered by TestRandomIntInvalidRange.
                // Don't test max == min.  That's covered by TestRandomIntMinMaxSame.
                for (int max = min + _integerComboInterval; max <= maxValue; max += _integerComboInterval)
                {
                    int value = Random.Next(min, max);
                    Assert.That(value, Is.GreaterThanOrEqualTo(min));
                    Assert.That(value, Is.LessThan(max));
                }
            }
        }


        // +--------------------+
        // |                    |                
        // |    Double Tests    |
        // |                    |
        // +--------------------+


        // Test no parameters.
        [Test]
        public void TestRandomDouble() => TestRandomDouble(ApplicationResources.Random);


        [Test]
        public void TestCryptoRandomDouble() => TestRandomDouble(ApplicationResources.CryptoRandom);


        private static void TestRandomDouble(IThreadsafeRandom Random)
        {
            for (int i = 0; i < _repeatTests; i++)
            {
                double value = Random.NextDouble();
                Assert.That(value, Is.GreaterThanOrEqualTo(-double.Epsilon));
                Assert.That(value, Is.LessThanOrEqualTo((1d + double.Epsilon)));
            }
        }


        // Test max.
        [Test]
        public void TestRandomDoubleMax() => TestRandomDoubleMax(ApplicationResources.Random);


        [Test]
        public void TestCryptoRandomDoubleMax() => TestRandomDoubleMax(ApplicationResources.CryptoRandom);


        private static void TestRandomDoubleMax(IThreadsafeRandom Random)
        {
            double max = 0;
            do
            {
                double value = Random.NextDouble(max);
                Assert.That(value, Is.GreaterThanOrEqualTo(-double.Epsilon));
                Assert.That(value, Is.LessThanOrEqualTo(max + double.Epsilon));
                max += _doubleComboInterval;
            } while (max <= _maxDoubleValue);
        }


        // Test invalid range exception.
        [Test]
        public void TestRandomDoubleInvalidRange() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomDoubleInvalidRange(ApplicationResources.Random));


        [Test]
        public void TestCryptoRandomDoubleInvalidRange() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomDoubleInvalidRange(ApplicationResources.CryptoRandom));


        private static void TestRandomDoubleInvalidRange(IThreadsafeRandom Random) => Random.NextDouble(99d, 88d);


        // Test min and max parameters.
        [Test]
        public void TestRandomDoubleMinMax() => TestRandomDoubleMinMax(ApplicationResources.Random);


        [Test]
        public void TestCryptoRandomDoubleMinMax() => TestRandomDoubleMinMax(ApplicationResources.CryptoRandom);


        private static void TestRandomDoubleMinMax(IThreadsafeRandom Random)
        {

            double min = _minDoubleValue;
            double max = min;
            do
            {
                do
                {
                    // Don't test max < min.  That's covered by TestRandomDoubleInvalidRange.
                    max += _doubleComboInterval;
                    double value = Random.NextDouble(min, max);
                    Assert.That(value, Is.GreaterThanOrEqualTo(min - double.Epsilon));
                    Assert.That(value, Is.LessThanOrEqualTo(max + double.Epsilon));
                } while (max <= _maxDoubleValue);
                min += _doubleComboInterval;
            } while (min <= _maxDoubleValue);
        }
    }
}
