// Copyright 2021 Keyfactor
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions
// and limitations under the License.


using System;
using Keyfactor.Platform.Extensions.Agents;
using Keyfactor.Platform.Extensions.Agents.Delegates;
using Keyfactor.Platform.Extensions.Agents.Interfaces;

namespace $ext_safeprojectname$.Jobs
{
    // The Reenrollment class implementes IAgentJobExtension and is meant to:
    //  1) Generate a new public/private keypair locally
    //  2) Generate a CSR from the keypair,
    //  3) Submit the CSR to KF Command to enroll the certificate and retrieve the certificate back
    //  4) Deploy the newly re-enrolled certificate to a certificate store

    [Job(JobTypes.REENROLLMENT)]  //Setting to "Management" makes this the entry point for all Management jobs
    public class Reenrollment : OrchestratorJob, IAgentJobExtension
    {
        //Job Entry Point
        public override AnyJobCompleteInfo processJob(AnyJobConfigInfo config, SubmitInventoryUpdate submitInventory, SubmitEnrollmentRequest submitEnrollmentRequest, SubmitDiscoveryResults sdr)
        {
            //METHOD ARGUMENTS...
            //config - contains context information passed from KF Command to this job run:
            //
            // config.Server.Username, config.Server.Password - credentials for orchestrated server - use to authenticate to certificate store server.
            //
            // config.Store.ClientMachine - server name or IP address of orchestrated server
            // config.Store.StorePath - location path of certificate store on orchestrated server
            // config.Store.StorePassword - if the certificate store has a password, it would be passed here
            //
            // config.Store.Properties - JSON object containing certain reserved values for Discovery or custom properties set up in the Certificate Store Type
            // config.Store.Properties.dirs.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) - Certificate Store Discovery Job Scheduler - Directories to search
            // config.Store.Properties.extensions.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) - Certificate Store Discovery Job Scheduler - Extensions
            // config.Store.Properties.ignoreddirs.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) - Certificate Store Discovery Job Scheduler - Directories to ignore
            // config.Store.Properties.patterns.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) - Certificate Store Discovery Job Scheduler - File name patterns to match
            //
            // config.Job.EntryContents - PKCS12 or PEM representation of certificate being added (Management job only)
            // config.Job.Alias - optional string value of certificate alias (used in java keystores and some other store types)
            // config.Job.OpeerationType - enumeration representing function with job type.  Used only with Management jobs where this value determines whether the Management job is a CREATE/ADD/REMOVE job.
            // config.Job.Overwrite - Boolean value telling the AnyAgent whether to overwrite an existing certificate in a store.  How you determine whether a certificate is "the same" as the one provided is AnyAgent implementation dependent
            // config.Job.PfxPassword - For a Management Add job, if the certificate being added includes the private key (therefore, a pfx is passed in config.Job.EntryContents), this will be the password for the pfx.


            //NLog Logging to c:\CMS\Logs\CMS_Agent_Log.txt
            Logger.Debug($"Begin Reenrollment...");

            try
            {

                //Code logic to:
                //  1) Generate a new public/private keypair locally from any config.Store.Properties passed
                //  2) Generate a CSR from the keypair (PKCS10),
                //  3) Submit the CSR to KF Command to enroll the certificate using:
                //      string resp = (string)submitEnrollmentRequest(Convert.ToBase64String(PKCS10_bytes);
                //      X509Certificate2 cert = new X509Certificate2(Convert.FromBase64String(resp));
                //  4) Deploy the newly re-enrolled certificate (cert in #3) to a certificate store
            }
            catch (Exception ex)
            {
                //Status: 2=Success, 3=Warning, 4=Error
                return new AnyJobCompleteInfo() { Status = 4, Message = "<Custom message you want to show to show up as the error message in Job History in KF Command>" };
            }

            //Status: 2=Success, 3=Warning, 4=Error
            return new AnyJobCompleteInfo() { Status = 2, Message = "Successful" };
        }
    }
}
