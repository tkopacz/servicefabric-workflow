using Microsoft.ServiceFabric.Actors;
using Statistics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowStateHost.Interfaces;

namespace DemoConsoleClient
{
    public class Program
    {
        const string SERVICE_URI = "fabric:/WorkflowSample";
        static readonly ActorId m_actorIdStatistics = new ActorId(0);

        public static void Main(string[] args)
        {
            try {
                var astatictics = ActorProxy.Create<IStatistics>(m_actorIdStatistics, SERVICE_URI);
                var wf1 = ActorProxy.Create<ITKWorkflow>(ActorId.NewId(), SERVICE_URI);
                var wf2 = ActorProxy.Create<ITKWorkflow>(ActorId.NewId(), SERVICE_URI);
                int i;
                TKWorkflowState state;
                i = wf1.SetName("A").Result;
                i = wf1.SetSurname("B").Result;
                state = wf1.GetCurrentState().Result;
                var ts = DateTime.Now;
                i = wf1.AddNewComment("C", ts).Result;
                state = wf1.GetCurrentState().Result;
                i = wf1.AddNewComment("C", ts).Result; //Test for failure - redelivery for that message
                state = wf1.GetCurrentState().Result;
                i = wf1.IsMoreComments(true).Result;
                state = wf1.GetCurrentState().Result;
                i = wf1.AddNewComment("C1", DateTime.Now).Result;
                state = wf1.GetCurrentState().Result;
                i = wf1.IsMoreComments(true).Result;
                state = wf1.GetCurrentState().Result;
                i = wf1.AddNewComment("C2", DateTime.Now).Result;
                state = wf1.GetCurrentState().Result;
                i = wf1.IsMoreComments(false).Result;
                state = wf1.GetCurrentState().Result;
                StatisticsState ss;
                ss = astatictics.GetCountAsync().Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();


        }
    }
}
