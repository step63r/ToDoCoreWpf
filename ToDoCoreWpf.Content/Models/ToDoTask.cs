using System;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Models
{
    /// <summary>
    /// タスク
    /// </summary>
    public record ToDoTask
    {
        /// <summary>
        /// Guid
        /// </summary>
        public Guid Guid => Guid.NewGuid();
        /// <summary>
        /// 区分
        /// </summary>
        public ToDoCategory Category { get; set; }
        /// <summary>
        /// 状況
        /// </summary>
        public ToDoStatus Status { get; set; }
        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// 期限
        /// </summary>
        public DateTime DueDate { get; set; }
        /// <summary>
        /// タイトル
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 詳細
        /// </summary>
        public string Detail { get; set; }
    }
}
