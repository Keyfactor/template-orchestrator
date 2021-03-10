using System;
using FluentAssertions;
using Keyfactor.Integrations.Orchestrator.Template.Jobs;
using Keyfactor.Integrations.Orchestrator.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mocks = Keyfactor.Integrations.Orchestrator.Test.Mocks;

namespace Keyfactor.OrchestratorTemplate.Test
{
    [TestClass]
    public class ReenrollmentTests
    {
        [TestMethod]
        public void ReturnsTheCorrectJobClassAndStoreType()
        {
            var enrollment = new Reenrollment();
            enrollment.GetJobClass().Should().Equals("Enrollment");
            enrollment.GetStoreType().Should().Equals("<STORE TYPE NAME>"); // TODO: fill in the store type
        }

        [TestMethod]
        public void JobReturnsFailureResponseAfterError()
        {
            var enrollment = new Mock<Reenrollment>() { CallBase = true };
            var config = Mocks.GetMockConfig();


            // Mock and configure helper method or client class to throw an exception or return a bad result.
            // example:


            //config.Store.Properties = JsonConvert.SerializeObject(new
            //{
            //    VaultUrl = "https://test.vault",
            //    TenantId = "8b74a908-b153-41dc-bfe5-3ea7b22b9678",
            //    ClientSecret = "testClientSecret",
            //    ApplicationId = Guid.NewGuid().ToString(),
            //    SubscriptionId = Guid.NewGuid().ToString(),
            //    VaultName = "wrongVaultName",
            //    ResourceGroupName = "testResourceGroupName",
            //    APIObjectId = Guid.NewGuid().ToString(),
            //});

            var result = enrollment.Object.processJob(config, Mocks.GetSubmitInventoryDelegateMock().Object, Mocks.GetSubmitEnrollmentDelegateMock().Object, Mocks.GetSubmitDiscoveryDelegateMock().Object);
            result.Status.Should().Be(4);
            result.Message.Should().Contain("<Custom message you want to show to show up as the error message in Job History in KF Command>"); // TODO: this should match the message defined in the job.
            // mockAzClient.Verify(az => az.CreateVault(), Times.Once());
        }
    }
}
