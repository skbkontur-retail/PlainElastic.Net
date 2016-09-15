using PlainElastic.Net.Utils;

namespace PlainElastic.Net.Queries
{
    /// <summary>
    ///     Filters documents that have fields containing terms with a specified prefix (not analyzed).
    ///     Similar to phrase query, except that it acts as a filter.
    ///     Can be placed within queries that accept a filter.
    ///     see http://www.elasticsearch.org/guide/reference/query-dsl/prefix-filter.html
    /// </summary>
    public class PrefixFilter<T> : FieldQueryBase<T, PrefixFilter<T>>
    {
        private string value;

        public PrefixFilter<T> Value(string prefix)
        {
            value = prefix;
            return this;
        }

        /// <summary>
        ///     Allows to name filter, so the search response will include for each hit the matched_filters
        ///     it matched on (note, this feature make sense for or / bool filters).
        ///     http://www.elasticsearch.org/guide/reference/api/search/named-filters.html
        /// </summary>
        public PrefixFilter<T> Name(string filterName)
        {
            RegisterJsonPart("'_name': {0}", filterName.Quotate());
            return this;
        }

        /// <summary>
        ///     Allows to specify Cache Key that will be used as the caching key for that filter.
        /// </summary>
        public PrefixFilter<T> CacheKey(string cacheKey)
        {
            RegisterJsonPart("'_cache_key': {0}", cacheKey.Quotate());
            return this;
        }

        /// <summary>
        ///     Controls whether the filter will be cached.
        /// </summary>
        public PrefixFilter<T> Cache(bool cache)
        {
            RegisterJsonPart("'_cache': {0}", cache.AsString());
            return this;
        }

        protected override bool ForceJsonBuild()
        {
            return true;
        }


        protected override bool HasRequiredParts()
        {
            return true;
        }

        protected override string ApplyJsonTemplate(string body)
        {
            if (string.IsNullOrEmpty(RegisteredField) || string.IsNullOrEmpty(value))
                return "";
            if (string.IsNullOrEmpty(body))
                return "{{ 'prefix': {{ {0}: {1} }} }}".AltQuoteF(RegisteredField, value.Quotate());
            return "{{ 'prefix': {{ {0}: {1}, {2} }} }}".AltQuoteF(RegisteredField, value.Quotate(), body);
        }
    }
}