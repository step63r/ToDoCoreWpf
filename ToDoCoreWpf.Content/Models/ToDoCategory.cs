using System;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Models
{
    /// <summary>
    /// 区分
    /// </summary>
    public record ToDoCategory
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
