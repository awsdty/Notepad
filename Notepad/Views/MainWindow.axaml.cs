using Avalonia.Controls;
using Avalonia.Interactivity;
using Notepad.ViewModels;

namespace Notepad.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        public void DoubleTap(object sender, RoutedEventArgs e)
        {
            var mwvm = (MainWindowViewModel?)DataContext;
            if (mwvm == null) return;

            var src = e.Source;
            if(src == null) return;

            var name = src.GetType().Name;
            if (name == "Image" || name == "ContentPresenter" || name == "TextBlock") mwvm.DoubleTap();
        }
    }
}
