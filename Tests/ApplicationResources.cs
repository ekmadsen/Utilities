using NUnit.Framework;


namespace ErikTheCoder.Utilities.Tests
{
    [SetUpFixture]
    public class ApplicationResources
    {
        public static IThreadsafeRandom Random;
        public static IThreadsafeRandom CryptoRandom;


        [OneTimeSetUp]
        public void SetUp()
        {
            Random = new ThreadsafeRandom();
            CryptoRandom = new ThreadsafeCryptoRandom();
        }


        [OneTimeTearDown]
        public static void TearDown()
        {
            CryptoRandom?.Dispose();
            CryptoRandom = null;
            Random?.Dispose();
            Random = null;
        }
    }
}