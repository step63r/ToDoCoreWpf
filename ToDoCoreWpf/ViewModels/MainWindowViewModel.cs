using Prism.Mvvm;

namespace MinatoProject.Apps.ToDoCoreWpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "ToDo Core Wpf";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
