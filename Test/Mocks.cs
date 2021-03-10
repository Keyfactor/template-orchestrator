using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Keyfactor.Platform.Extensions.Agents;
using Keyfactor.Platform.Extensions.Agents.Delegates;
using Moq;

[assembly: InternalsVisibleTo("OrchestratorStarter")]
namespace Keyfactor.Integrations.Orchestrator.Test
{
    public static class Mocks
    {
        public static AnyJobConfigInfo GetMockConfig()
        {
            var ajStore = new AnyJobStoreInfo()
            {
                // below is an example of how to Mock custom parameters when the Job requires them.

                //Properties = JsonConvert.SerializeObject(new
                //{
                //    Prop1 = "abc", 
                //    Prop2 = "123",                
                //}),

                Inventory = new List<AnyJobInventoryItem>(),
                StorePath = "http://test.vault",
                Storetype = 1,
            };
            var ajJob = new AnyJobJobInfo { OperationType = Platform.Extensions.Agents.Enums.AnyJobOperationType.Create, Alias = "testJob", JobId = Guid.NewGuid(), JobTypeId = Guid.NewGuid(), };
            var ajServer = new AnyJobServerInfo { Username = "testUsername", Password = "testPassword", UseSSL = true };
            var ajc = new AnyJobConfigInfo()
            {
                Store = ajStore,
                Job = ajJob,
                Server = ajServer
            };

            return ajc;
        }

        public static Mock<SubmitInventoryUpdate> GetSubmitInventoryDelegateMock() => new Mock<SubmitInventoryUpdate>();

        public static Mock<SubmitEnrollmentRequest> GetSubmitEnrollmentDelegateMock() => new Mock<SubmitEnrollmentRequest>();

        public static Mock<SubmitDiscoveryResults> GetSubmitDiscoveryDelegateMock() => new Mock<SubmitDiscoveryResults>();
    }
}
