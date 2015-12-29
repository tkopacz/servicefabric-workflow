using Microsoft.ServiceFabric.Actors;
using Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using WorkflowStateHost.Interfaces;

namespace DemoConsoleClient
{
    public class Program
    {
        const string SERVICE_URI = "fabric:/WorkflowSample";
        static readonly ActorId m_actorIdStatistics = new ActorId(0);

        public static void Main(string[] args)
        {

            //var astatictics = ActorProxy.Create<IStatistics>(m_actorIdStatistics, SERVICE_URI);
            //var wf1 = ActorProxy.Create<ITKWorkflow>(ActorId.NewId(), SERVICE_URI); 
            //var wf2 = ActorProxy.Create<ITKWorkflow>(ActorId.NewId(), SERVICE_URI);
            //wf1.SetName("A");
            //wf1.SetSurname("B");
            //wf1.AddNewComment("C");
            //wf1.IsMoreComments(true);
            //wf1.AddNewComment("C1");
            //wf1.IsMoreComments(true);
            //wf1.AddNewComment("C2");
            //wf1.IsMoreComments(false);


        }
    }
}
