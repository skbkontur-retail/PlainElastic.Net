using PlainElastic.Net.Utils;

namespace PlainElastic.Net.Mappings
{
    public class FieldNorms : MappingBase<FieldNorms>
    {

        /// <summary>
        /// Defines if norms should be omitted or not. 
        /// </summary>
        public FieldNorms Enbaled(bool enabled = true)
        {
            RegisterCustomJsonMap("'enabled': {0}", enabled.AsString());
            return this;
        }

        /// <summary>
        /// Defines if norms should be omitted or not. 
        /// </summary>
        public FieldNorms Loading(NormsLoading loading)
        {
            RegisterCustomJsonMap("'loading': {0}", loading.AsString().ToLower());
            return this;
        }

        protected override string ApplyMappingTemplate(string mappingBody)
        {
            return "'norms': {{ {0} }}".AltQuoteF(mappingBody);
        }
    }
}