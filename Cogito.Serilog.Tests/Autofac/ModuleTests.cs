using Autofac;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Serilog;

namespace Cogito.Serilog.Tests.Autofac
{

    [TestClass]
    public class ModuleTests
    {

        [TestMethod]
        public void Can_resolve_logger()
        {
            var b = new ContainerBuilder();
            b.RegisterModule<Cogito.Serilog.Autofac.AssemblyModule>();
            var c = b.Build();
            var l = c.Resolve<ILogger>();
        }

    }

}
