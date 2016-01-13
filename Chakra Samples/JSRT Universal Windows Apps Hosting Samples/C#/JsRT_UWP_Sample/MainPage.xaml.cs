using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

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

