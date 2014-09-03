using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PlainElastic.Net.Utils;

namespace PlainElastic.Net.Queries
{
    /// <summary>
    /// A multi-value metrics aggregation that computes stats over numeric values extracted from the aggregated documents. These values can be extracted either from specific numeric fields in the documents, or be generated by a provided script.
	/// The extended_stats aggregations is an extended version of the stats aggregation, where additional metrics are added such as sum_of_squares, variance and std_deviation.
	/// see http://www.elasticsearch.org/guide/en/elasticsearch/reference/current/search-aggregations-metrics-extendedstats-aggregation.html
    /// </summary>
	public class ExtendedStatsAggregation<T> : AggregationBase<ExtendedStatsAggregation<T>, T>
    {
        private readonly List<string> aggregationFields = new List<string>();

        /// <summary>
		/// The field to execute extended stats aggregation against.
        /// </summary>
		public ExtendedStatsAggregation<T> Field(string fieldName)
        {
            RegisterJsonPart("'field': {0}", fieldName.Quotate());            
            return this;
        }

        /// <summary>
		/// The field to execute extended stats aggregation against.
        /// </summary>
		public ExtendedStatsAggregation<T> Field(Expression<Func<T, object>> field)
        {
            return Field(field.GetPropertyPath());
        }

        /// <summary>
		/// The field to execute extended stats aggregation against.
        /// </summary>
		public ExtendedStatsAggregation<T> FieldOfCollection<TProp>(Expression<Func<T, IEnumerable<TProp>>> collectionField, Expression<Func<TProp, object>> field)
        {
            var collectionProperty = collectionField.GetPropertyPath();
            var fieldName = collectionProperty + "." + field.GetPropertyPath();

            return Field(fieldName);
        }


        /// <summary>
		/// Allow to define a script to evaluate, with its value used to compute the extended stats information.
        /// </summary>
		public ExtendedStatsAggregation<T> Script(string script)
        {
            RegisterJsonPart("'script': {0}", script.Quotate());
            return this;
        }


        /// <summary>
        /// Sets a scripting language used for scripts.
        /// By default used mvel language.
        /// see: http://www.elasticsearch.org/guide/reference/modules/scripting.html
        /// </summary>
		public ExtendedStatsAggregation<T> Lang(string lang)
        {
            RegisterJsonPart("'lang': {0}", lang.Quotate());
            return this;
        }

        /// <summary>
        /// Sets a scripting language used for scripts.
        /// By default used mvel language.
        /// see: http://www.elasticsearch.org/guide/reference/modules/scripting.html
        /// </summary>
		public ExtendedStatsAggregation<T> Lang(ScriptLangs lang)
        {
            return Lang(lang.AsString());
        }

        /// <summary>
        /// Sets parameters used for scripts.
        /// </summary>
		public ExtendedStatsAggregation<T> Params(string paramsBody)
        {
            RegisterJsonPart("'params': {0}", paramsBody);
            return this;
        }

		protected override string ApplyAggregationBodyJsonTemplate(string body)
		{
			return "'extended_stats': {{ {0} }}".AltQuoteF(body);
		}
    }
}