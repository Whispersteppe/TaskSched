using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskSched.Common.Interfaces;
using TaskSched.ExecutionEngine;
using TaskSched.Test.XUnit;
using Xunit.Abstractions;

namespace TaskSched.Test
{
    public class ExecutionEngineTests : XUnitTestClassBase
    {
        public ExecutionEngineTests(XUnitClassFixture fixture, XUnitCollectionFixture collectionFixture, ITestOutputHelper output) : base(fixture, collectionFixture, output)
        {
        }

        [Fact]
        public async Task EngineTest()
        {
            ILogger logger = this.GetLogger(memberName: nameof(ExecutionEngineTests));
            IExecutionStore executionStore = new ExecutionStore.ExecutionStore(logger);

            IExecutionEngine engine = new ActivityEngine(logger, executionStore);
        }
    }
}
