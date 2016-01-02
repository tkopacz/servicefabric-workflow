using TKStatistics.Interfaces;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace TKStatistics
{
    /// <remarks>
    /// Each ActorID maps to an instance of this class.
    /// The IProjName  interface (in a separate DLL that client code can
    /// reference) defines the operations exposed by ProjName objects.
    /// </remarks>
    internal class TKStatistics : StatefulActor<TKStatisticsState>, ITKStatistics
    {


        /// <summary>
        /// This method is called whenever an actor is activated.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            if (this.State == null)
            {
                // This is the first time this actor has ever been activated.
                // Set the actor's initial state values.
                this.State = new TKStatisticsState { CountCalls = 0, CountFinished = 0, CountStarted = 0 };
            }

            ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            return Task.FromResult(true);
        }


        [Readonly]
        Task<TKStatisticsState> ITKStatistics.GetCountAsync()
        {
            // For methods that do not change the actor's state,
            // [Readonly] improves performance by not performing serialization and replication of the actor's state.
            ActorEventSource.Current.ActorMessage(this, "Getting current count value as {0}", this.State);
            return Task.FromResult(this.State);
        }

        Task ITKStatistics.IncCalls()
        {
            this.State.CountCalls++;
            return Task.FromResult(true);
        }

        Task ITKStatistics.IncFinished()
        {
            this.State.CountFinished++;
            return Task.FromResult(true);
        }

        Task ITKStatistics.IncStarted()
        {
            this.State.CountStarted++;
            return Task.FromResult(true);
        }
    }
}
