using NUnit.Framework;
using PlainElastic.Net.Mappings;

namespace PlainElastic.Net.Tests._NunitTests
{
    public class MappingTest
    {
        [Test]
        public void MultiFields_ES1X()
        {
            var mappings = new MapBuilder<Dto>().RootObject("_default_", mapping => mapping.Properties(p => p.NotAnalyzedStringWithAnalazedCopyForEs1X(x => x.StringField))).Build();
            Assert.That(mappings, Is.EqualTo("{ \"_default_\": { \"properties\": { \"StringField\": { \"type\": \"multi_field\",\"fields\": { \"StringField\": { \"type\": \"string\",\"index\": \"not_analyzed\" },\"StringField_analyzed\": { \"type\": \"string\",\"index\": \"analyzed\" } } } } } }"));
        }

        [Test]
        public void MultiFields_ES5X()
        {
            var mappings = new MapBuilder<Dto>().RootObject("_default_", mapping => mapping.Properties(p => p.NotAnalyzedStringWithAnalazedCopyForEs5X(x => x.StringField))).Build();
            Assert.That(mappings, Is.EqualTo("{ \"_default_\": { \"properties\": { \"StringField\": { \"type\": \"string\",\"index\": \"not_analyzed\",\"fields\": { \"StringField_analyzed\": { \"type\": \"string\",\"index\": \"analyzed\" } } } } } }"));
        }

        public class Dto
        {
            public string StringField { get; set; }
        }
    }
}