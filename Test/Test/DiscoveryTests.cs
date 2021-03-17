// Copyright 2021 Keyfactor
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions
// and limitations under the License.

using System.Collections.Generic;
using FluentAssertions;
using $ext_safeprojectname$.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace $ext_safeprojectname$.Test
{
    [TestClass]
    public class DiscoveryTests
    {
        [TestMethod]
        public void ReturnsTheCorrectJobClassAndStoreType()
        {
            var discovery = new Discovery();
            discovery.GetJobClass().Should().Be("Discovery");
            discovery.GetStoreType().Should().Be("<STORE TYPE NAME>"); //replace this with the store type name
        }

        [TestMethod]
        public void JobInvokesCorrectDelegate()
        {
            var discovery = new Mock<Discovery>() { CallBase = true };
            var mockSdr = Mocks.GetSubmitDiscoveryDelegateMock();

            var result = discovery.Object.processJob(Mocks.GetMockConfig(), Mocks.GetSubmitInventoryDelegateMock().Object, Mocks.GetSubmitEnrollmentDelegateMock().Object, mockSdr.Object);

            mockSdr.Verify(m => m(It.IsAny<List<string>>()));
        }

        [TestMethod]
        public void JobReturnsCorrectVaultNames()
        {
            var discovery = new Mock<Discovery>() { CallBase = true };

            var v1 = "TestVault1";
            var v2 = "TestVault2";
            var v3 = "TestVAult3";

            // Here is where you should mock out the return values from the operation to test that 
            // they are parsed and processed correctly.

            // example: 
            // mockAzureClient.Setup(c => c.GetVaults()).ReturnsAsync(() => new _DiscoveryResult()
            // {
            //   Vaults = new List<_AKV_Location>() {
            //         new _AKV_Location(){Name = v1},
            //         new _AKV_Location(){Name = v2 },
            //         new _AKV_Location(){Name = v3 } }
            // });

            // discovery.Protected().Setup<AzureClient>("AzClient").Returns(mockAzClient.Object);

            var mockSdr = Mocks.GetSubmitDiscoveryDelegateMock();

            var result = discovery.Object.processJob(Mocks.GetMockConfig(), Mocks.GetSubmitInventoryDelegateMock().Object, Mocks.GetSubmitEnrollmentDelegateMock().Object, mockSdr.Object);

            mockSdr.Verify(m => m(It.Is<List<string>>(l => l.Contains(v1) && l.Contains(v2) && l.Contains(v3))));
        }

        [TestMethod]
        public void JobReturnsFailureResponseAfterError()
        {
            var discovery = new Mock<Discovery>() { CallBase = true };
            // simulate a thrown exception by mocking a helper method or client and configure the return value.
            // example: 
            // mockAzureClient.Setup(m => m.GetVaults()).ThrowsAsync(new Exception("FAIL"));
            // discovery.Protected().Setup<AzureClient>("AzClient").Returns(mockAzClient.Object);

            var mockSdr = Mocks.GetSubmitDiscoveryDelegateMock();
            var result = discovery.Object.processJob(Mocks.GetMockConfig(), Mocks.GetSubmitInventoryDelegateMock().Object, Mocks.GetSubmitEnrollmentDelegateMock().Object, mockSdr.Object);

            mockSdr.Verify(m => m(It.IsAny<List<string>>()), Times.Never()); // sdr should not be invoked

            result.Status.Should().Be(4);
            result.Message.Should().Contain("FAIL");
            // mockAzureClient.Verify(az => az.GetVaults(), Times.Once());
        }
    }
}
