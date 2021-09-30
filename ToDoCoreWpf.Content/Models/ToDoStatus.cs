using System;
using System.Runtime.Serialization;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Models
{
    /// <summary>
    /// 状況
    /// </summary>
    [DataContract]
    public record ToDoStatus
    {
        /// <summary>
        /// Guid
        /// </summary>
        [DataMember]
        public Guid Guid { get; private set; } = new Guid();
        /// <summary>
        /// 並び順
        /// </summary>
        [DataMember]
        public int Order { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
