using System.Collections.Generic;
using FluentAssertions;
using Keyfactor.Integrations.Orchestrator.Template.Jobs;
using Keyfactor.Platform.Extensions.Agents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Keyfactor.Integrations.Orchestrator.Test
{
    [TestClass]
    public class InventoryTests
    {
        [TestMethod]
        public void ReturnsTheCorrectJobClassAndStoreType()
        {
            var inventory = new Inventory();
            inventory.GetJobClass().Should().Be("Inventory");
            inventory.GetStoreType().Should().Be("<STORE TYPE NAME>");
        }

        [TestMethod]
        public void JobInvokesCorrectDelegate()
        {
            var inventory = new Mock<Inventory>() { CallBase = true };
            var mockInventoryDelegate = Mocks.GetSubmitInventoryDelegateMock();

            var result = inventory.Object.processJob(Mocks.GetMockConfig(), mockInventoryDelegate.Object, Mocks.GetSubmitEnrollmentDelegateMock().Object, Mocks.GetSubmitDiscoveryDelegateMock().Object);

            mockInventoryDelegate.Verify(m => m(It.IsAny<List<AgentCertStoreInventoryItem>>()));
        }

        [TestMethod]
        public void JobReturnsCorrectCertificates()
        {

        }

        [TestMethod]
        public void JobReturnsFailureResponseAfterError()
        {
            var inventory = new Mock<Inventory>() { CallBase = true };

            // set mock client or helper method up to throw an exception.
            // var mockClient = Mocks.GetMockClient();
            // mockClient.Setup(m => m.GetCertificatesAsync()).ThrowsAsync(new Exception("FAIL"));
            // inventory.Protected().Setup<Client>("MyClient").Returns(mockClient.Object);

            var mockInventoryDelegate = Mocks.GetSubmitInventoryDelegateMock();
            var result = inventory.Object.processJob(Mocks.GetMockConfig(), mockInventoryDelegate.Object, Mocks.GetSubmitEnrollmentDelegateMock().Object, Mocks.GetSubmitDiscoveryDelegateMock().Object);

            mockInventoryDelegate.Verify(m => m(It.IsAny<List<AgentCertStoreInventoryItem>>()), Times.Never()); // should not be invoked

            result.Status.Should().Be(4);
            result.Message.Should().Contain("FAIL");
            // mockClient.Verify(az => az.GetCertificatesAsync(), Times.Once());
        }
    }
}
