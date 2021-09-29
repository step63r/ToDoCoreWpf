using System;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Models
{
    /// <summary>
    /// 状況
    /// </summary>
    public record ToDoStatus
    {
        /// <summary>
        /// Guid
        /// </summary>
        public Guid Guid => Guid.NewGuid();
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
    }
}
