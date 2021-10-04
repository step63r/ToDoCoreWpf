using System.ComponentModel;
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
        [Description("高")]
        High,
        [EnumMember]
        [Description("中")]
        Medium,
        [EnumMember]
        [Description("低")]
        Low,
    }
}
