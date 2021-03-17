// Copyright 2021 Keyfactor
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions
// and limitations under the License.

using FluentAssertions;
using $ext_safeprojectname$.Jobs;
using Keyfactor.Platform.Extensions.Agents.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;

namespace $ext_safeprojectname$.Test
{
    [TestClass]
    public class ManagementTests
    {
        /// <summary>
        /// Job level tests : 
        ///     verify the correct job class and store type are being returned.
        ///     verify the correct delegates are called and others are not
        ///     verify the job handles client errors gracefully
        ///     ? verify appropriate event logging
        /// </summary>


        [TestMethod]
        public void ReturnsTheCorrectJobClassAndStoreType()
        {
            var manage = new Management();
            manage.GetJobClass().Should().Be("Management");
            manage.GetStoreType().Should().Be("<STORE TYPE NAME>");
        }

        #region Create

        [TestMethod]
        public void JobForCreateOnlyCallsPerformCreate()
        {
            var managementMock = new Mock<Management>() { CallBase = true };
            managementMock.Protected().Setup("PerformCreateVault").Verifiable();
            managementMock.Protected().Setup("PerformRemoval", ItExpr.IsAny<string>()).Verifiable();
            managementMock.Protected().Setup("PerformAddition", ItExpr.IsAny<string>(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>()).Verifiable();

            var config = Mocks.GetMockConfig();
            config.Job.OperationType = AnyJobOperationType.Create;

            var result = managementMock.Object.processJob(config, Mocks.GetSubmitInventoryDelegateMock().Object, Mocks.GetSubmitEnrollmentDelegateMock().Object, Mocks.GetSubmitDiscoveryDelegateMock().Object);
            managementMock.Protected().Verify("PerformCreateVault", Times.Once());
            managementMock.Protected().Verify("PerformRemoval", Times.Never(), ItExpr.IsAny<string>());
            managementMock.Protected().Verify("PerformAddition", Times.Never(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>());
        }


        [TestMethod]
        public void JobForCreateVaultHandlesClientError()
        {
            var managementMock = new Mock<Management>() { CallBase = true };

            var config = Mocks.GetMockConfig();
            config.Job.OperationType = AnyJobOperationType.Create;
            // mockClient.Setup(m => m.CreateVault()).Throws(new Exception("FAILURE"));
            // 
            var result = managementMock.Object.processJob(config, Mocks.GetSubmitInventoryDelegateMock().Object, Mocks.GetSubmitEnrollmentDelegateMock().Object, Mocks.GetSubmitDiscoveryDelegateMock().Object);

            // mockClient.Verify(az => az.CreateVault(), Times.Once());
            result.Status.Should().Equals(4);
            result.Message.Should().Be("FAILURE");
        }

        #endregion

        #region Add

        [TestMethod]
        public void JobForAddOnlyCallsPerformAddition()
        {
            var managementMock = new Mock<Management>() { CallBase = true };
            managementMock.Protected().Setup("PerformCreateVault").Verifiable();
            managementMock.Protected().Setup("PerformRemoval", ItExpr.IsAny<string>()).Verifiable();
            managementMock.Protected().Setup("PerformAddition", ItExpr.IsAny<string>(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>()).Verifiable();

            var config = Mocks.GetMockConfig();
            config.Job.OperationType = AnyJobOperationType.Add;

            var result = managementMock.Object.processJob(config, Mocks.GetSubmitInventoryDelegateMock().Object, Mocks.GetSubmitEnrollmentDelegateMock().Object, Mocks.GetSubmitDiscoveryDelegateMock().Object);
            managementMock.Protected().Verify("PerformCreateVault", Times.Never());
            managementMock.Protected().Verify("PerformRemoval", Times.Never(), ItExpr.IsAny<string>());
            managementMock.Protected().Verify("PerformAddition", Times.Once(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>());
        }

        #endregion

        #region Remove

        [TestMethod]
        public void JobForRemoveOnlyCallsPerformRemove()
        {
            var managementMock = new Mock<Management>() { CallBase = true };
            managementMock.Protected().Setup("PerformCreateVault").Verifiable();
            managementMock.Protected().Setup("PerformRemoval", ItExpr.IsAny<string>()).Verifiable();
            managementMock.Protected().Setup("PerformAddition", ItExpr.IsAny<string>(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>()).Verifiable();

            var config = Mocks.GetMockConfig();
            config.Job.OperationType = AnyJobOperationType.Remove;

            var result = managementMock.Object.processJob(config, Mocks.GetSubmitInventoryDelegateMock().Object, Mocks.GetSubmitEnrollmentDelegateMock().Object, Mocks.GetSubmitDiscoveryDelegateMock().Object);
            managementMock.Protected().Verify("PerformCreateVault", Times.Never());
            managementMock.Protected().Verify("PerformRemoval", Times.Once(), ItExpr.IsAny<string>());
            managementMock.Protected().Verify("PerformAddition", Times.Never(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>());
        }

        [TestMethod]
        public void JobForRemoveThrowsIfAliasMissing() { }

        [TestMethod]
        public void JobForRemoveHandlesClientError() { }

        #endregion
    }
}
