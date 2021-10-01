using System.Runtime.Serialization;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Models
{
    /// <summary>
    /// 優先度
    /// </summary>
    [DataContract]
    public enum ToDoPriority
    {
        [EnumMember]
        High,
        [EnumMember]
        Medium,
        [EnumMember]
        Low,
    }
}
