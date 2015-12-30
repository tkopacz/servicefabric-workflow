using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowStateHost.Interfaces;

namespace WebUI.Model
{
    public class WFText
    {
        public string Text { get; set; }
    }

    public class WFComment:WFText
    {
        public bool NextComment { get; set; }
    }
}
