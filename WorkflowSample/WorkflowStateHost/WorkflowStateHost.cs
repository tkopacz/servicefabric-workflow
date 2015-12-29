using WorkflowStateHost.Interfaces;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Statistics.Interfaces;

namespace WorkflowStateHost
{
    /// <remarks>
    /// Each ActorID maps to an instance of this class.
    /// The IProjName  interface (in a separate DLL that client code can
    /// reference) defines the operations exposed by ProjName objects.
    /// </remarks>
    internal class TKWorkflow : StatefulActor<TKWorkflowState>, ITKWorkflow
    {
        const string SERVICE_URI = "fabric:/WorkflowSample";
        //static readonly ActorId m_actorIdStatistics = new ActorId(0);

        static IStatistics m_astatictics;// = ActorProxy.Create<IStatistics>(m_actorIdStatistics, SERVICE_URI);

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            if (this.State == null)
            {
                // This is the first time this actor has ever been activated.
                // Set the actor's initial state values.
                this.State = new WorkflowState();
                this.State.Comments = new List<string>();
                this.State.CurrentState = eState.eSetName;
            }

            ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            return Task.FromResult(true);
        }

        Task<int> ITKWorkflow.SetName(string text)
        {
            m_astatictics.IncCalls();
            m_astatictics.IncStarted();
            State.Name = text;
            State.CurrentState = eState.eSetSurname;
            return Task.FromResult(1);
        }
        Task<int> ITKWorkflow.SetSurname(string text)
        {
            m_astatictics.IncCalls();
            State.Surname = text;
            State.CurrentState = eState.eAddComment;
            return Task.FromResult(2);
        }

        Task<int> ITKWorkflow.AddNewComment(string text)
        {
            m_astatictics.IncCalls();
            State.Comments.Add(text);
            State.CurrentState = eState.eIsMoreComments;
            return Task.FromResult(3);
        }

        Task<int> ITKWorkflow.IsMoreComments(bool finished)
        {
            m_astatictics.IncCalls();
            if (!finished)
            {
                State.CurrentState = eState.eAddComment;
                return Task.FromResult(4);
            }
            else
            {
                m_astatictics.IncFinished();
                State.CurrentState = eState.eFinished; //End of "workflow"
                return Task.FromResult(5);
            }
        }
        [Readonly]
        Task<IList<string>> ITKWorkflow.GetAllComments()
        {
            m_astatictics.IncCalls();
            return Task.FromResult(this.State.Comments);
        }

        [Readonly]
        Task<eState> ITKWorkflow.GetCurrentState()
        {
            m_astatictics.IncCalls();
            return Task.FromResult(this.State.CurrentState);
        }
    }
}
