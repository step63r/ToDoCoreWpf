using Prism.Events;

namespace MinatoProject.Apps.ToDoCoreWpf.Content.Events
{
    /// <summary>
    /// TasksPageViewModel→MainWindowViewModelへのタスク変更イベント
    /// </summary>
    /// <remarks>パラメータは (期限超過, 本日期限, 期限前) のタスク数</remarks>
    public sealed class TaskEvent : PubSubEvent<(int Overdue, int Deadline, int Future)>
    {
    }
}
