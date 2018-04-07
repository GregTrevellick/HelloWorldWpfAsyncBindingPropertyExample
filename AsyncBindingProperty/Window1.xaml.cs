using System.Windows;

namespace AsyncBindingProperty
{
    public partial class Window1 : Window
    {
        SlowSource source = new SlowSource();

        public Window1()
        {
            InitializeComponent();

            this.DataContext = source;

            //Upon start up do the slow thing without locking up the UI...
            source.FetchNewData();
        }

        private void getNewDataButton_Click(object sender, RoutedEventArgs e)
        {
            source.FetchNewData();
        }
    }
}
