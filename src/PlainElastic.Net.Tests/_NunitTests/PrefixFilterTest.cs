using NUnit.Framework;
using PlainElastic.Net.Queries;

namespace PlainElastic.Net.Tests._NunitTests
{
    public class PrefixFilterTest
    {
        [Test]
        public void TestCompatibilityWithEs14()
        {
            Assert.AreEqual(
                @"{ ""prefix"": { ""Field"": ""val"", ""_cache"": true,""_cache_key"": ""key"",""_name"": ""n"" } }",
                new PrefixFilter<int>().Cache(true).Field("Field").Value("val").CacheKey("key").Name("n").Build());
            Assert.AreEqual(@"{ ""prefix"": { ""Field"": ""val"" } }",
                new PrefixFilter<int>().Field("Field").Value("val").Build());

            Assert.AreEqual(@"{ ""prefix"": { ""z\""x"": ""val\""q'"" } }",
                new PrefixFilter<int>().Field("z\"x").Value("val\"q'").Build());
            Assert.AreEqual(@"{ ""prefix"": { ""Field"": ""val"", ""_name"": ""n\""m"" } }",
                new PrefixFilter<int>().Field("Field").Value("val").Name("n\"m").Build());

            Assert.AreEqual("", new PrefixFilter<int>().Field("a").Build());
            Assert.AreEqual("", new PrefixFilter<int>().Value("a").Build());
        }
    }
}