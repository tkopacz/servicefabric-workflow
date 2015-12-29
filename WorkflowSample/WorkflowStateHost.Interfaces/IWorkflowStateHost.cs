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
    public enum eState
    {
        eSetName=0x1,
        eSetSurname=0x2,
        eAddComment=0x3,
        eIsMoreComments=0x4,
        eFinished=0x5   
    }

    [DataContract]
    public sealed class WorkflowState
    {
        public eState CurrentState { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public IList<string> Comments { get; set; }
    }
    /// <summary>
    /// This interface represents the actions a client app can perform on an actor.
    /// It MUST derive from IActor and all methods MUST return a Task.
    /// </summary>
    public interface IWorkflow : IActor
    {
        /// <summary>
        /// Get current state
        /// </summary>
        /// <returns></returns>
        Task<eState> GetCurrentState();
        Task<int> SetName(string text);
        Task<int> SetSurname(string text);
        Task<int> AddNewComment(string text);
        Task<int> IsMoreComments(bool finished);
        Task<IList<string>> GetAllComments();
    }
}
