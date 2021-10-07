using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Models
{
    /// <summary>
    /// 区分
    /// </summary>
    [DataContract]
    public class ToDoCategory : INotifyPropertyChanged, IEquatable<ToDoCategory>, IComparable<ToDoCategory>
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
            set
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

        private int _order;
        /// <summary>
        /// 並び順
        /// </summary>
        [DataMember]
        public int Order
        {
            get => _order;
            set
            {
                if (value == _order)
                {
                    return;
                }
                _order = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Order));
            }
        }

        private string _name = string.Empty;
        /// <summary>
        /// 名前
        /// </summary>
        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Name));
            }
        }

        private string _foregroundColorHex = "#FF000000";
        /// <summary>
        /// 前景色
        /// </summary>
        [DataMember]
        public string ForegroundColorHex
        {
            get => _foregroundColorHex;
            set
            {
                if (value == _foregroundColorHex)
                {
                    return;
                }
                _foregroundColorHex = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ForegroundColorHex));
            }
        }

        private string _backgroundColorHex = "#00000000";
        /// <summary>
        /// 背景色
        /// </summary>
        [DataMember]
        public string BackgroundColorHex
        {
            get => _backgroundColorHex;
            set
            {
                if (value == _backgroundColorHex)
                {
                    return;
                }
                _backgroundColorHex = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(BackgroundColorHex));
            }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ToDoCategory() { }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="source"></param>
        public ToDoCategory(ToDoCategory source)
        {
            Guid = source.Guid;
            Order = source.Order;
            Name = source.Name;
            ForegroundColorHex = source.ForegroundColorHex;
            BackgroundColorHex = source.BackgroundColorHex;
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
        public bool Equals(ToDoCategory other)
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
        public int CompareTo(ToDoCategory other)
        {
            int ret = Order.CompareTo(other.Order);
            if (ret != 0)
            {
                return ret;
            }

            ret = Name.CompareTo(other.Name);
            return ret;
        }
        #endregion
    }
}
