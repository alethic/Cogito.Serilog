using Cogito.Autofac;

using Newtonsoft.Json.Linq;

using Serilog.Core;
using Serilog.Events;

namespace Cogito.Serilog.Autofac.Destructuring
{

    [RegisterAs(typeof(IDestructuringPolicy))]
    public class JValueDestructuringPolicy :
        IDestructuringPolicy
    {

        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            if (value is JValue json)
            {
                result = propertyValueFactory.CreatePropertyValue(json.Value, true);
                return true;
            }

            result = null;
            return false;
        }

    }

}
