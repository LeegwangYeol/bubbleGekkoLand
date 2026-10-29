using adviser.ViewModels;

namespace adviser.Models
{
    public class HoneybunButtonModel : ObservableObject
    {
        private int _q;
        public int Q
        {
            get => _q;
            set => SetProperty(ref _q, value);
        } // Axial coordinate

        private int _r;
        public int R
        {
            get => _r;
            set => SetProperty(ref _r, value);
        } // Axial coordinate

        private double _x;
        public double X
        {
            get => _x;
            set => SetProperty(ref _x, value);
        }

        private double _y;
        public double Y
        {
            get => _y;
            set => SetProperty(ref _y, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private double _scale = 1.0;
        public double Scale
        {
            get => _scale;
            set => SetProperty(ref _scale, value);
        }
    }
}
