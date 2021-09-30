using System;
using System.Runtime.Serialization;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Models
{
    /// <summary>
    /// タスク
    /// </summary>
    [DataContract]
    public record ToDoTask : IComparable<ToDoTask>
    {
        /// <summary>
        /// Guid
        /// </summary>
        [DataMember]
        public Guid Guid { get; private set; } = new Guid();
        /// <summary>
        /// 区分
        /// </summary>
        [DataMember]
        public ToDoCategory Category { get; set; }
        /// <summary>
        /// 状況
        /// </summary>
        [DataMember]
        public ToDoStatus Status { get; set; }
        /// <summary>
        /// 優先度
        /// </summary>
        [DataMember]
        public ToDoPriority Priority { get; set; } = ToDoPriority.Medium;
        /// <summary>
        /// 作成日時
        /// </summary>
        [DataMember]
        public DateTime Created { get; private set; } = DateTime.Now;
        /// <summary>
        /// 更新日時
        /// </summary>
        [DataMember]
        public DateTime Updated { get; set; }
        /// <summary>
        /// 期限
        /// </summary>
        [DataMember]
        public DateTime DueDate { get; set; }
        /// <summary>
        /// タイトル
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// 詳細
        /// </summary>
        [DataMember]
        public string Detail { get; set; }

        /// <summary>
        /// インスタンスを比較する
        /// </summary>
        /// <param name="other">別のインスタンス</param>
        /// <returns></returns>
        public int CompareTo(ToDoTask other)
        {
            // 期限で比較
            int ret = DueDate.CompareTo(other.DueDate);
            if (ret != 0)
            {
                return ret;
            }

            // 期限が同じ場合、優先度で比較
            ret = Priority.CompareTo(other.Priority);
            if (ret != 0)
            {
                return ret;
            }

            // 優先度が同じ場合、状況で比較
            ret = Status.Order.CompareTo(other.Status.Order);
            if (ret != 0)
            {
                return ret;
            }

            // 状況が同じ場合、区分で比較
            ret = Category.Order.CompareTo(other.Category.Order);
            if (ret != 0)
            {
                return ret;
            }

            // 区分が同じ場合、タイトルで比較
            return string.Compare(Title, other.Title, StringComparison.Ordinal);
        }
    }
}
