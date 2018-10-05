using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace Cogito.Serilog.Tests
{

    [TestClass]
    public class ExceptionLogContextDataEnricherTests
    {

        [TestMethod]
        public void Should_capture_log_context_and_add_property()
        {
            var events = new List<LogEvent>();
            var logger = new LoggerConfiguration()
                .Enrich.WithExceptionLogContext()
                .WriteTo.Sink(new DelegateLogEventSink(e => events.Add(e)))
                .CreateLogger();

            try
            {
                using (LogContext.PushProperty("PROPERTY", "VALUE"))
                {
                    throw new Exception("FAILED!");
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "EXCEPTION");
            }

            events[0].Exception.Should().NotBeNull();
            events[0].Properties["PROPERTY"].Should().BeEquivalentTo(new ScalarValue("VALUE"));

            try
            {
                throw new Exception();
            }
            catch (Exception e)
            {
                // good
            }
        }

    }

}
