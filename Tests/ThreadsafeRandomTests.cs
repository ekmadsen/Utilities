using System;
using ErikTheCoder.Utilities;
using NUnit.Framework;


namespace ErikTheCoder.Tests
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
        private IThreadsafeRandom _random;
        private IThreadsafeRandom _cryptoRandom;


        [OneTimeSetUp]
        public void SetUp()
        {
            _random = new ThreadsafeRandom();
            _cryptoRandom = new ThreadsafeCryptoRandom();
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            // Release unmanaged resources.
            _random.Dispose();
            _random = null;
            _cryptoRandom.Dispose();
            _cryptoRandom = null;
        }

        // +---------------------+
        // |                     |                
        // |    Integer Tests    |
        // |                     |
        // +---------------------+


        // Test no parameters.
        [Test]
        public void TestRandomInt() => TestRandomInt(_random);


        [Test]
        public void TestCryptoRandomInt() => TestRandomInt(_cryptoRandom);


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
        public void TestRandomIntNegativeMax() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomIntNegativeMax(_random));


        [Test]
        public void TestCryptoRandomIntNegativeMax() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomIntNegativeMax(_cryptoRandom));


        private static void TestRandomIntNegativeMax(IThreadsafeRandom Random) => Random.Next(-13);


        // Test max zero.
        [Test]
        public void TestRandomIntMaxZero() => TestRandomIntMaxZero(_random);


        [Test]
        public void TestCryptoRandomIntMaxZero() => TestRandomIntMaxZero(_cryptoRandom);


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
        public void TestRandomIntMax() => TestRandomIntMax(_random);


        [Test]
        public void TestCryptoRandomIntMax() => TestRandomIntMax(_cryptoRandom);


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
        public void TestRandomIntInvalidRange() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomIntInvalidRange(_random));


        [Test]
        public void TestCryptoRandomIntInvalidRange() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomIntInvalidRange(_cryptoRandom));


        private static void TestRandomIntInvalidRange(IThreadsafeRandom Random) => Random.Next(9, 8);


        // Test same value for min and max.
        [Test]
        public void TestRandomIntMinMaxSame() => TestRandomIntMinMaxSame(_random);


        [Test]
        public void TestCryptoRandomIntMinMaxSame() => TestRandomIntMinMaxSame(_cryptoRandom);


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
        public void TestRandomIntMinMax() => TestRandomIntMinMax(_random);


        [Test]
        public void TestCryptoRandomIntMinMax() => TestRandomIntMinMax(_cryptoRandom);


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
        public void TestRandomDouble() => TestRandomDouble(_random);


        [Test]
        public void TestCryptoRandomDouble() => TestRandomDouble(_cryptoRandom);


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
        public void TestRandomDoubleMax() => TestRandomDoubleMax(_random);


        [Test]
        public void TestCryptoRandomDoubleMax() => TestRandomDoubleMax(_cryptoRandom);


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
        public void TestRandomDoubleInvalidRange() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomDoubleInvalidRange(_random));


        [Test]
        public void TestCryptoRandomDoubleInvalidRange() => Assert.Throws<ArgumentOutOfRangeException>(() => TestRandomDoubleInvalidRange(_cryptoRandom));


        private static void TestRandomDoubleInvalidRange(IThreadsafeRandom Random) => Random.NextDouble(99d, 88d);


        // Test min and max parameters.
        [Test]
        public void TestRandomDoubleMinMax() => TestRandomDoubleMinMax(_random);


        [Test]
        public void TestCryptoRandomDoubleMinMax() => TestRandomDoubleMinMax(_cryptoRandom);


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
