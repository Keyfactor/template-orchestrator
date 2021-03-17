// Copyright 2021 Keyfactor
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions
// and limitations under the License.

using FluentAssertions;
using $ext_safeprojectname$.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mocks = $ext_safeprojectname$.Test.Mocks;

namespace $ext_safeprojectname$.Test
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
