using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Keyfactor.Platform.Extensions.Agents.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Keyfactor.Integrations.Orchestrator.Test
{
    [TestClass]
    public class AssemblyTests
    {
        /// <summary>
        /// These tests verify that the compiled binary has the appropriate entry points in order to be imported into the Orchestrator
        /// This also verifies that the job types are named correctly.
        /// Add more job types to test as appropriate
        /// </summary>

        private const string folderPath = @"../../../../AzureKeyVault/bin/Debug/";
        private const string assemblyName = @"AzureKeyVault.dll";
        private const string STORE_TYPE = "<Store Type Name>";
        private const string INVENTORY_JOB_TYPE = "Inventory";
        private const string DISCOVERY_JOB_TYPE = "Discovery";
        private const string MANAGEMENT_JOB_TYPE = "Management";
        private const string ENROLLMENT_JOB_TYPE = "Enrollment";

        [TestMethod]
        public void AssemblyReflectsIncludedJobTypes()
        {
            var workingPath = Assembly.GetExecutingAssembly().Location;
            var dllFile = new FileInfo($"{folderPath}{assemblyName}");
            var assembly = Assembly.LoadFile(dllFile.FullName);

            var jobExtensionTypes = new List<AgentJobExtensionType>();

            if (assembly == null)
            {
                throw new ArgumentNullException("Assembly cannot be null");
            }
            else
            {
                Type extensionType = typeof(IAgentJobExtension);

                List<Type> extensionClasses = GetLoadableTypes(assembly)
                    .Where(type => type.GetInterfaces().Any(i => i.FullName == extensionType.FullName) // Checking just the fully-qualified namespace/class, no assembly version
                        && type.IsClass
                        && !type.IsAbstract)
                    .ToList();

                if (extensionClasses.Count != 0)
                {
                    foreach (var type in extensionClasses)
                    {
                        extensionType.IsAssignableFrom(type).Should().BeTrue();

                        IAgentJobExtension instance = (IAgentJobExtension)Activator.CreateInstance(type);

                        var jobExtensionType = new AgentJobExtensionType()
                        {
                            Assembly = assembly.GetName().Name,
                            Class = type.FullName,
                            JobType = $"{instance.GetStoreType()}{instance.GetJobClass()}", // <----- bug?
                            ShortName = instance.GetStoreType(),
                            Directory = folderPath
                        };

                        var dependecies = assembly.GetReferencedAssemblies();
                        foreach (var dependency in dependecies)
                        {
                            string localPath = $"{folderPath}\\{dependency.Name}.dll";
                            if (File.Exists(localPath))
                            {
                                jobExtensionType.LocalDependecies.Add(new AssemblyFileInfo()
                                {
                                    Name = dependency.Name,
                                    Directory = folderPath
                                });
                            }
                        }

                        jobExtensionTypes.Add(jobExtensionType);

                    }
                }

                jobExtensionTypes.Any(j => j.JobType == STORE_TYPE + INVENTORY_JOB_TYPE).Should().BeTrue();
                jobExtensionTypes.Any(j => j.JobType == STORE_TYPE + MANAGEMENT_JOB_TYPE).Should().BeTrue();
                jobExtensionTypes.Any(j => j.JobType == STORE_TYPE + DISCOVERY_JOB_TYPE).Should().BeTrue();
                jobExtensionTypes.Any(j => j.JobType == STORE_TYPE + ENROLLMENT_JOB_TYPE).Should().BeTrue();
            }
        }

        private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(type => type != null);
            }
        }
    }

    public class AgentJobExtensionType : MarshalByRefObject
    {
        public string Assembly { get; set; }
        public string Class { get; set; }
        public string FullName => $"{Class}, {Assembly}";
        public string ShortName { get; set; }
        public string Directory { get; set; }
        public string JobType { get; set; }
        public List<AssemblyFileInfo> LocalDependecies { get; set; } = new List<AssemblyFileInfo>();
    }

    public class AssemblyFileInfo : MarshalByRefObject
    {
        public string Name { get; set; }
        public string Directory { get; set; }
        public string FullPath => $"{Directory}\\{Name}.dll";
    }
}
