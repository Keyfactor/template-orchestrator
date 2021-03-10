using System;
using System.Collections.Generic;
using Keyfactor.Platform.Extensions.Agents;
using Keyfactor.Platform.Extensions.Agents.Delegates;
using Keyfactor.Platform.Extensions.Agents.Interfaces;

namespace Keyfactor.Integrations.Orchestrator.Template.Jobs
{
    [Job(JobTypes.DISCOVERY)]  //Setting to "Discovery" makes this the entry point for all Discovery jobs
    public class Discovery : OrchestratorJob, IAgentJobExtension
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
            Logger.Debug($"Begin Discovery...");

            //Instantiate collection of certificate store locations to pass back
            List<string> locations = new List<string>();

            try
            {
                //Code logic to:
                // 1) Connect to the orchestrated server (config.Store.ClientMachine)
                // 2) Custom logic to search for valid certificate stores based on passed in:
                //      a) Directories to search
                //      b) Extensions
                //      c) Directories to ignore
                //      d) File name patterns to match
                // 3) Place found and validated store locations (path and file name) in "locations" collection instantiated above
            }
            catch (Exception ex)
            {
                //Status: 2=Success, 3=Warning, 4=Error
                return new AnyJobCompleteInfo() { Status = 4, Message = "<Custom message you want to show to show up as the error message in Job History in KF Command>" };
            }

            try
            {
                //Sends store locations back to KF command where they can be approved or rejected
                sdr.Invoke(locations);
                //Status: 2=Success, 3=Warning, 4=Error
                return new AnyJobCompleteInfo() { Status = 2, Message = "Successful" };
            }
            catch (Exception ex)
            {
                // NOTE: if the cause of the submitInventory.Invoke exception is a communication issue between the Orchestrator server and the Command server, the job status returned here
                //  may not be reflected in Keyfactor Command.
                return new AnyJobCompleteInfo() { Status = 4, Message = "<Custom message you want to show to show up as the error message in Job History in KF Command>" };
            }
        }
    }
}

