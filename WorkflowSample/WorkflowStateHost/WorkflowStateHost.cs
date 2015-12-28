using WorkflowStateHost.Interfaces;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowStateHost
{
    /// <remarks>
    /// Each ActorID maps to an instance of this class.
    /// The IProjName  interface (in a separate DLL that client code can
    /// reference) defines the operations exposed by ProjName objects.
    /// </remarks>
    internal class WorkflowStateHost : StatefulActor<WorkflowState>, IWorkflowStateHost
    {
        /// <summary>
        /// This class contains each actor's replicated state.
        /// Each instance of this class is serialized and replicated every time an actor's state is saved.
        /// For more information, see http://aka.ms/servicefabricactorsstateserialization
        /// </summary>


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

        Task<int> IWorkflowStateHost.SetName(string text)
        {
            State.Name = text;
            State.CurrentState = eState.eSetSurname;
            return Task.FromResult(1);
        }
        Task<int> IWorkflowStateHost.SetSurname(string text)
        {
            State.Surname = text;
            State.CurrentState = eState.eAddComment;
            return Task.FromResult(2);
        }

        Task<int> IWorkflowStateHost.AddNewComment(string text)
        {
            State.Comments.Add(text);
            State.CurrentState = eState.eIsMoreComments;
            return Task.FromResult(3);
        }

        Task<int> IWorkflowStateHost.IsMoreComments(bool finished)
        {
            if (!finished)
            {
                State.CurrentState = eState.eAddComment;
                return Task.FromResult(4);
            }
            else
            {
                State.CurrentState = eState.eFinished; //End of "workflow"
                return Task.FromResult(5);
            }
        }
        [Readonly]
        Task<IList<string>> IWorkflowStateHost.GetAllComments()
        {
            return Task.FromResult(this.State.Comments);
        }

        [Readonly]
        Task<eState> IWorkflowStateHost.GetCurrentState()
        {
            return Task.FromResult(this.State.CurrentState);
        }
    }
}
