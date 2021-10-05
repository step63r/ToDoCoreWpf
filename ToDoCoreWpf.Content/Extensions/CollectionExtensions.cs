using System.Collections.ObjectModel;
using System.Linq;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Extensions
{
    /// <summary>
    /// コレクションの拡張メソッド
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// コレクションをソートする
        /// </summary>
        /// <typeparam name="T">型</typeparam>
        /// <param name="source">ソート前のコレクション</param>
        /// <returns>ソート後のコレクション</returns>
        public static ObservableCollection<T> SortCollection<T>(this ObservableCollection<T> source) where T : class
        {
            var list = source.ToList();
            list.Sort();
            return new ObservableCollection<T>(list);
        }
    }
}
