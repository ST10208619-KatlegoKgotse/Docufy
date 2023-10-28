using System.Windows;

namespace Docufy
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();

            Loaded += (sender, args) =>
            {
                Wpf.Ui.Appearance.Watcher.Watch(this,
                    Wpf.Ui.Appearance.BackgroundType.Mica,
                    true);
            };
        }
    }
}
