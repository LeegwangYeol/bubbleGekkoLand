namespace adviser.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private string _title = "Adviser - MVVM Setup";
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
        }
    }
}
