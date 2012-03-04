﻿using Machine.Specifications;
using PlainElastic.Net.Queries;
using PlainElastic.Net.Utils;

namespace PlainElastic.Net.Tests.Buildres.Queries
{
    [Subject(typeof(TermQuery<>))]
    class When_TermQuery_with_custom_part_built
    {
        private Because of = () => result = new TermQuery<FieldsTestClass>()
                                                .Field(f => f.Property1)
                                                .Value("One")
                                                .Custom("'boost': {0}", "5")
                                                .ToString();

        It should_contain_custom_part = () => result.ShouldContain(@"'boost': 5".AltQuote());

        It should_return_correct_query = () => result.ShouldEqual(@"{ 'term': { 'Property1': { 'value': 'one','boost': 5 } } }".AltQuote());

        private static string result;
    }
}
