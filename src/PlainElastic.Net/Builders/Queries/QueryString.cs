using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PlainElastic.Net.Builders;
using PlainElastic.Net.Utils;


namespace PlainElastic.Net.Queries
{
    /// <summary>
    /// A query that uses a query parser in order to parse its content
    /// see http://www.elasticsearch.org/guide/reference/query-dsl/query-string-query.html
    /// </summary>    
    public class QueryString<T> : IJsonConvertible
    {

        private readonly List<string> queryFields = new List<string>();

        private readonly List<string> queryParts = new List<string>();

        private bool hasValue;



        public QueryString<T> DefaultField(Expression<Func<T, object>> field)
        {
            var defaultField = field.GetQuotatedPropertyPath();
            var defaultPart = " 'default_field': {0}".AltQuoteF(defaultField);

            queryParts.Add(defaultPart);

            return this;
        }

        public QueryString<T> Fields(params Expression<Func<T, object>> [] fields)
        {
            foreach (var field in fields)
                queryFields.Add(field.GetQuotatedPropertyPath());

            return this;
        }

        public QueryString<T> FieldsOfCollection<TProp>(Expression<Func<T, IEnumerable<TProp>>> collectionField, params Expression<Func<TProp, object>>[] fields)
        {
            var collectionProperty = collectionField.GetPropertyPath();

            foreach (var field in fields)
            {
                var fieldName = collectionProperty + "." + field.GetPropertyPath();
                fieldName = fieldName.Quotate();
                queryFields.Add(fieldName);
            }

            return this;
        }

        public QueryString<T> Query(string value, bool wrapInWildcard = false)
        {
            if (value.IsNullOrEmpty())
                return this;

            var values = value.SplitByCommaAndSpaces();

            if (wrapInWildcard)
                values = values.Select(v => "*" + v + "*").ToArray();

            var fullTextQuery = values.JoinWithSeparator(" ").LowerAndQuotate();

            var queryValue = " 'query': {0}".AltQuoteF(fullTextQuery);

            queryParts.Add(queryValue);

            hasValue = true;

            return this;
        }

        public QueryString<T> Boost(double boost)
        {
            var boostPart = " 'boost': {0}".AltQuoteF(boost.AsString());
            queryParts.Add(boostPart);

            return this;
        }

        public QueryString<T> Rewrite(Rewrite rewrite, int n = 0)
        {
            var rewriteValue = GetRewriteValue(rewrite, n).Quotate();
            var rewritePart = " 'rewrite': {0}".AltQuoteF(rewriteValue);

            queryParts.Add(rewritePart);

            return this;
        }


        public QueryString<T> Custom(string queryFormat, params string[] args)
        {
            var query = queryFormat.AltQuoteF(args);
            queryParts.Add(query);
            hasValue = true;

            return this;
        }


        private static string GetRewriteValue(Rewrite rewrite, int n)
        {
            switch(rewrite)
            {
                case Queries.Rewrite.top_terms_boost_n:
                    return "top_terms_boost_" + n;

                case Queries.Rewrite.top_terms_n:
                    return "top_terms_" + n;
            }
            return rewrite.ToString();
        }


        private string GenerateFieldsQueryPart()
        {
            if (!queryFields.Any())
                return "";

            var fields = queryFields.JoinWithComma();
            var fieldPart = " 'fields': [{0}]".AltQuoteF(fields);
            return fieldPart;
        }


        string IJsonConvertible.ToJson()
        {
            if (!hasValue)
                return "";

            string fields = GenerateFieldsQueryPart();
            if(!fields.IsNullOrEmpty())
                queryParts.Insert(0, fields);

            var body = queryParts.JoinWithComma();
            var result = "{{ 'query_string': {{ {0} }} }}".AltQuoteF(body);

            return result;
        }
    }
}