using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JsRT_UWP_Sample
{
    public sealed partial class MainPage : Page
    {
        ChakraHost.ChakraHost host = new ChakraHost.ChakraHost();

        public MainPage()
        {
            this.InitializeComponent();
            string msg = host.init();
            if (msg != "NoError")
                JsOutput.Text = msg;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string result = host.runScript(JsInput.Text);
            JsOutput.Text = JsOutput.Text + "\n> " + JsInput.Text + "\n" + result;
            JsOutput.UpdateLayout();
            JsOutputScroll.ChangeView(null, double.MaxValue, null);
        }
    }
}
