// Copyright 2021 Keyfactor
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions
// and limitations under the License.

using System.Collections.Generic;
using FluentAssertions;
using $ext_safeprojectname$.Jobs;
using Keyfactor.Platform.Extensions.Agents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace $ext_safeprojectname$.Test
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
