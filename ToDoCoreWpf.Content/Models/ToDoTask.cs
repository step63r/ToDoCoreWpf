using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Models
{
    /// <summary>
    /// タスク
    /// </summary>
    [DataContract]
    public class ToDoTask : INotifyPropertyChanged, IEquatable<ToDoTask>, IComparable<ToDoTask>
    {
        #region プロパティ
        private Guid _guid = Guid.NewGuid();
        /// <summary>
        /// Guid
        /// </summary>
        [DataMember]
        public Guid Guid
        {
            get => _guid;
            private set
            {
                if (value == _guid)
                {
                    return;
                }
                _guid = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Guid));
            }
        }

        private ToDoCategory _category = new();
        /// <summary>
        /// 区分
        /// </summary>
        [DataMember]
        public virtual ToDoCategory Category
        {
            get => _category;
            set
            {
                if (value == _category)
                {
                    return;
                }
                _category = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Category));
            }
        }

        private ToDoStatus _status = new();
        /// <summary>
        /// 状況
        /// </summary>
        [DataMember]
        public virtual ToDoStatus Status
        {
            get => _status;
            set
            {
                if (value == _status)
                {
                    return;
                }
                _status = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Status));
            }
        }

        private ToDoPriority _priority = ToDoPriority.Medium;
        /// <summary>
        /// 優先度
        /// </summary>
        [DataMember]
        public ToDoPriority Priority
        {
            get => _priority;
            set
            {
                if (value == _priority)
                {
                    return;
                }
                _priority = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Priority));
            }
        }

        private DateTime _created = DateTime.Now;
        /// <summary>
        /// 作成日時
        /// </summary>
        [DataMember]
        public DateTime Created
        {
            get => _created;
            private set
            {
                if (value == _created)
                {
                    return;
                }
                _created = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Created));
            }
        }

        private DateTime _updated;
        /// <summary>
        /// 更新日時
        /// </summary>
        [DataMember]
        public DateTime Updated
        {
            get => _updated;
            set
            {
                if (value == _updated)
                {
                    return;
                }
                _updated = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Updated));
            }
        }

        private DateTime _dueDate = DateTime.Now.AddDays(1);
        /// <summary>
        /// 期限
        /// </summary>
        [DataMember]
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                if (value == _dueDate)
                {
                    return;
                }
                _dueDate = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(DueDate));
            }
        }

        private string _title = string.Empty;
        /// <summary>
        /// タイトル
        /// </summary>
        [DataMember]
        public string Title
        {
            get => _title;
            set
            {
                if (value == _title)
                {
                    return;
                }
                _title = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Title));
            }
        }

        private string _detail = string.Empty;
        /// <summary>
        /// 詳細
        /// </summary>
        [DataMember]
        public string Detail
        {
            get => _detail;
            set
            {
                if (value == _detail)
                {
                    return;
                }
                _detail = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Detail));
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ToDoTask() { }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="source">コピー元オブジェクト</param>
        public ToDoTask(ToDoTask source)
        {
            Guid = source.Guid;
            Category = source.Category;
            Status = source.Status;
            Priority = source.Priority;
            Created = source.Created;
            Updated = source.Updated;
            DueDate = source.DueDate;
            Title = source.Title;
            Detail = source.Detail;
        }
        #endregion

        #region INotifyPropertyChanged
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region IEquatable<T>
        /// <summary>
        /// このクラスのオブジェクト同士が等価かどうか判定する
        /// </summary>
        /// <param name="other">別のインスタンス</param>
        /// <returns></returns>
        public bool Equals(ToDoTask other)
        {
            return Guid.Equals(other.Guid);
        }
        #endregion

        #region IComparable<T>
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
        #endregion
    }
}
