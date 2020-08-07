using System.Linq;

using Cogito.Autofac;

using Newtonsoft.Json.Linq;

using Serilog.Core;
using Serilog.Events;

namespace Cogito.Serilog.Autofac.Destructuring
{

    [RegisterAs(typeof(IDestructuringPolicy))]
    public class JArrayDestructuringPolicy : IDestructuringPolicy
    {

        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            if (value is JArray json)
            {
                result = propertyValueFactory.CreatePropertyValue(json.ToArray(), true);
                return true;
            }

            result = null;
            return false;
        }

    }

}
