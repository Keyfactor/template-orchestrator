using System;

namespace Keyfactor.Integrations.Orchestrator.Template
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class JobAttribute : Attribute
    {
        private string jobClass { get; set; }

        public JobAttribute(string jobClass)
        {
            this.jobClass = jobClass;
        }

        public virtual string JobClass
        {
            get { return jobClass; }
        }
    }
}
