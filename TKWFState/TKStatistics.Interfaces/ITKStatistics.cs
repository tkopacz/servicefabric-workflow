using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using System.Runtime.Serialization;

namespace TKStatistics.Interfaces
{
    /// <summary>
    /// This class contains each actor's replicated state.
    /// Each instance of this class is serialized and replicated every time an actor's state is saved.
    /// For more information, see http://aka.ms/servicefabricactorsstateserialization
    /// </summary>
    [DataContract]
    public sealed class TKStatisticsState
    {
        /// <summary>
        /// Total number of calls to another actors
        /// </summary>
        [DataMember]
        public int CountCalls { get; set; }
        /// <summary>
        /// Total number of Workflow - started
        /// </summary>
        [DataMember]
        public int CountStarted { get; set; }
        /// <summary>
        /// Total number of Workflow - finished
        /// </summary>
        [DataMember]
        public int CountFinished { get; set; }

        public override string ToString()
        {
            return $"CountCalls: {CountCalls}, CountStarted:{CountStarted}, CountFinished:{CountFinished}";
        }
    }
    /// <summary>
    /// This interface represents the actions a client app can perform on an actor.
    /// It MUST derive from IActor and all methods MUST return a Task.
    /// </summary>
    public interface ITKStatistics : IActor
    {
        Task<TKStatisticsState> GetCountAsync();

        Task IncCalls();
        Task IncStarted();
        Task IncFinished();
    }

}
