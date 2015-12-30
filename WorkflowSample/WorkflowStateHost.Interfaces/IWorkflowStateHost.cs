using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using System.Runtime.Serialization;

namespace WorkflowStateHost.Interfaces
{
    /// <summary>
    /// States for workflow
    /// </summary>
    /// <remarks>
    ///     eSetName
    ///     eSetSurname
    ///     eNextComment -> ?eIsMoreComments => No -> eFinished
    ///                                      => Yes -> eNextComment
    ///     ...
    /// </remarks>
    [DataContract(Name = "eTKState")]
    public enum eTKState
    {
        [EnumMember]
        eSetName,
        [EnumMember]
        eSetSurname,
        [EnumMember]
        eAddComment,
        [EnumMember]
        eIsMoreComments,
        [EnumMember]
        eFinished   
    }

    [DataContract]
    public sealed class TKWorkflowState
    {
        [DataMember]
        public eTKState CurrentState { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public IDictionary<long,string> Comments { get; set; }
    }
    /// <summary>
    /// This interface represents the actions a client app can perform on an actor.
    /// It MUST derive from IActor and all methods MUST return a Task.
    /// </summary>
    public interface ITKWorkflow : IActor
    {
        /// <summary>
        /// Get current step (eTKState)
        /// </summary>
        /// <returns></returns>
        Task<eTKState> GetCurrentStep();
        /// <summary>
        /// Return full state
        /// </summary>
        /// <returns></returns>
        Task<TKWorkflowState> GetCurrentState();
        Task<int> SetName(string text);
        Task<int> SetSurname(string text);
        Task<int> AddNewComment(string text, DateTime timeStamp);
        Task<int> IsMoreComments(bool finished);
        Task<IDictionary<long, string>> GetAllComments();
    }
}
