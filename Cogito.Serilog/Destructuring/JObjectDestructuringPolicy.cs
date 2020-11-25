using System.Linq;

using Newtonsoft.Json.Linq;

using Serilog.Core;
using Serilog.Events;

namespace Cogito.Serilog.Destructuring
{

    public class JObjectDestructuringPolicy : IDestructuringPolicy
    {

        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            if (value is JObject json)
            {
                result = propertyValueFactory.CreatePropertyValue(json.Properties().ToDictionary(i => i.Name, i => i.Value), true);
                return true;
            }

            result = null;
            return false;
        }

    }

}
