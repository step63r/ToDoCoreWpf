using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Models
{
    /// <summary>
    /// 状況
    /// </summary>
    [DataContract]
    public class ToDoStatus : INotifyPropertyChanged
    {
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
    }
}
