using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SequenceNoElements
{
    public class WorkflowJson
    {
        public Guid Id { get; set; }
        public Guid WorkflowDefinitionVersionId { get; set; }
        public DateTime TimeCreated { get; set; }

        public Workflow Workflow { get; set; }


    }

    public class Workflow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<WorkflowActivity> Activities { get; set; }
    }

    public class WorkflowActivity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
